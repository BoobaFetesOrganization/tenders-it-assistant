using CsvHelper;
using CsvHelper.Configuration;
using GenAIChat.Application.Adapter;
using GenAIChat.Application.Entity;
using GenAIChat.Domain;
using GenAIChat.Domain.Common;
using GenAIChat.Domain.Project;
using System.Globalization;
using System.Linq.Expressions;

namespace GenAIChat.Application
{
    public class ProjectApplication(IGenAiApiAdapter genAiAdapter, IGenAiUnitOfWorkAdapter unitOfWork)
    {
        private const string CsvStartTag = "```csv";
        private const string CsvEndTag = "```";
        private static readonly CsvConfiguration CsvConfiguration = new(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ";"
        };

        private readonly IGenAiApiAdapter _genAiAdapter = genAiAdapter;
        public async Task<Paged<ProjectDomain>> GetAllAsync(PaginationOptions options, Expression<Func<ProjectDomain, bool>>? filter = null)
        {
            return await unitOfWork.Projects.GetAllAsync(options, filter);
        }

        public async Task<ProjectDomain> CreateAsync(string name, string prompt, IEnumerable<DocumentDomain>? documents = null)
        {
            var isExisting = (await unitOfWork.Projects.GetAllAsync(p => p.Name.ToLower().Equals(name.ToLower()))).Data?.Any() ?? false;
            if (isExisting)
            {
                throw new Exception("Project with the same name already exists");
            }

            var project = new ProjectDomain(name, prompt);

            // upload files to the GenAI
            if (documents is not null)
            {
                project.SetDocuments(documents);
                await _genAiAdapter.SendFilesAsync(documents);
            }

            // send prompt to the GenAI
            project.PromptResponse = await _genAiAdapter.SendPromptAsync(prompt, documents);

            // load user stories from the GenAI result

            IEnumerable<UserStoryDomain> userstories = LoadUsersStoriesFromProject(project);
            project.SetUserStories(userstories);

            await unitOfWork.Projects.AddAsync(project);
            return project;
        }

        public async Task<ProjectDomain?> GetByIdAsync(int id)
        {
            return await unitOfWork.Projects.GetByIdAsync(id);
        }

        public static IEnumerable<UserStoryDomain> LoadUsersStoriesFromProject(ProjectDomain project)
        {
            // get content after the start tag and finish before end tag
            var startIndex = project.PromptResponse.Text.IndexOf(CsvStartTag) + CsvStartTag.Length;
            var endIndex = project.PromptResponse.Text.IndexOf(CsvEndTag);
            var csvContent = project.PromptResponse.Text[startIndex..endIndex].Trim();

            // arrange the csv reader
            using var reader = new StringReader(csvContent);
            using var csv = new CsvReader(reader, CsvConfiguration);

            // parse csv content
            var records = csv.GetRecords<CsvUserStory>().ToList();

            // convert records to UserStoryDomain
            var userStories = records
                .GroupBy(r => r.UserStoryId)
                .Select(g => new UserStoryDomain
                {
                    Name = g.First().UserStoryName,
                    Tasks = g.Select(t => new UserStoryTaskDomain
                    {
                        Name = t.TaskName,
                        Cost = t.Cost
                    }).ToList()
                }).ToList();

            return userStories;
        }
    }
}
