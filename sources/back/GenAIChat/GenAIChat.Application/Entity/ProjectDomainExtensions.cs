using CsvHelper;
using CsvHelper.Configuration;
using GenAIChat.Domain.Project;
using System.Globalization;

namespace GenAIChat.Application.Entity
{
    internal static class ProjectDomainExtensions
    {
        private const string CsvStartTag = "```csv";
        private const string CsvEndTag = "```";
        private static readonly CsvConfiguration CsvConfiguration = new(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ";"
        };

        public static IEnumerable<UserStoryDomain> LoadUsersStoriesFromProject(this ProjectDomain project)
        {
            // get content after the start tag and finish before end tag
            var startIndex = project.PromptResponse.Text.IndexOf(CsvStartTag) + CsvStartTag.Length;
            var endIndex = project.PromptResponse.Text.IndexOf(CsvEndTag, startIndex);
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
