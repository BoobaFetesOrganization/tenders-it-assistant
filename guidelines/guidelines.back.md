fichier généré avec *ChatGpt-4o*

# Guidelines pour les intervenants d'un projet WebAPI en C #

## Bonnes pratiques de développement

1. **Respecter les conventions de nommage** : Utilisez des noms de variables, de méthodes et de classes clairs et significatifs.

    ```csharp
    // Mauvais exemple
    int a = 10;

    // Bon exemple
    int age = 10;
    ```

2. **Modularité** : Divisez le code en modules réutilisables et maintenables.

    ```csharp
    // Mauvais exemple
    public class UserService
    {
        public void RegisterUser(string username, string password)
        {
            // Logique d'enregistrement de l'utilisateur
        }

        public void DeleteUser(int userId)
        {
            // Logique de suppression de l'utilisateur
        }
    }

    // Bon exemple
    public class UserRegistrationService
    {
        public void RegisterUser(string username, string password)
        {
            // Logique d'enregistrement de l'utilisateur
        }
    }

    public class UserDeletionService
    {
        public void DeleteUser(int userId)
        {
            // Logique de suppression de l'utilisateur
        }
    }
    ```

3. **SOLID Principles** : Suivez les principes SOLID pour une conception orientée objet robuste.

    ```csharp
    // Exemple de principe de responsabilité unique (Single Responsibility Principle)
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserRepository
    {
        public void Save(User user)
        {
            // Logique de sauvegarde de l'utilisateur
        }
    }

    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void RegisterUser(User user)
        {
            // Logique d'enregistrement de l'utilisateur
            _userRepository.Save(user);
        }
    }
    ```

4. **Documentation** : Commentez votre code et maintenez une documentation à jour.

    ```csharp
    /// <summary>
    /// Service pour gérer les utilisateurs.
    /// </summary>
    public class UserService
    {
        /// <summary>
        /// Enregistre un nouvel utilisateur.
        /// </summary>
        /// <param name="user">L'utilisateur à enregistrer.</param>
        public void RegisterUser(User user)
        {
            // Logique d'enregistrement de l'utilisateur
        }
    }
    ```

5. **Gestion des exceptions** : Gérez les exceptions de manière appropriée pour éviter les plantages inattendus.

    ```csharp
    public class UserService
    {
        public void RegisterUser(User user)
        {
            try
            {
                // Logique d'enregistrement de l'utilisateur
            }
            catch (Exception ex)
            {
                // Gestion de l'exception
                Console.WriteLine($"Erreur lors de l'enregistrement de l'utilisateur : {ex.Message}");
            }
        }
    }
    ```

## Bonnes pratiques de testing

1. **Tests unitaires** : Écrivez des tests unitaires pour chaque méthode publique.

    ```csharp
    using Xunit;

    public class UserServiceTests
    {
        [Fact]
        public void RegisterUser_ShouldSaveUser()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var userService = new UserService(userRepository.Object);
            var user = new User { Username = "testuser", Password = "password" };

            // Act
            userService.RegisterUser(user);

            // Assert
            userRepository.Verify(repo => repo.Save(user), Times.Once);
        }
    }
    ```

2. **Tests d'intégration** : Testez l'intégration entre différents modules.

    ```csharp
    using Xunit;
    using Microsoft.EntityFrameworkCore;

    public class UserServiceIntegrationTests
    {
        [Fact]
        public void RegisterUser_ShouldSaveUserToDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new AppDbContext(options);
            var userRepository = new UserRepository(context);
            var userService = new UserService(userRepository);
            var user = new User { Username = "testuser", Password = "password" };

            // Act
            userService.RegisterUser(user);

            // Assert
            var savedUser = context.Users.FirstOrDefault(u => u.Username == "testuser");
            Assert.NotNull(savedUser);
        }
    }
    ```

3. **Mocking** : Utilisez des mocks pour isoler les tests unitaires.

    ```csharp
    using Moq;
    using Xunit;

    public class UserServiceTests
    {
        [Fact]
        public void RegisterUser_ShouldSaveUser()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>();
            var userService = new UserService(userRepository.Object);
            var user = new User { Username = "testuser", Password = "password" };

            // Act
            userService.RegisterUser(user);

            // Assert
            userRepository.Verify(repo => repo.Save(user), Times.Once);
        }
    }
    ```

## Clean Architecture de Martin Fowler

La Clean Architecture se compose de plusieurs couches, chacune ayant un rôle spécifique. Voici une description de chaque couche et ce qu'un développeur peut y trouver :

### 1. Couche de Présentation (Presentation Layer)

Cette couche contient tout ce qui concerne l'interface utilisateur.

- **Exemple de code** :

    ```csharp
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }
    }
    ```

### 2. Couche d'Application (Application Layer)

Cette couche contient la logique métier spécifique à l'application.

- **Exemple de code** :

    ```csharp
    public class ProjectApplication : IProjectService
    {
        private readonly IGenAiApiAdapter _genAiAdapter;
        private readonly IGenAiUnitOfWorkAdapter _unitOfWork;

        public ProjectApplication(IGenAiApiAdapter genAiAdapter, IGenAiUnitOfWorkAdapter unitOfWork)
        {
            _genAiAdapter = genAiAdapter;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProjectDomain> CreateAsync(string name, string prompt, IEnumerable<DocumentDomain>? documents = null)
        {
            var isExisting = (await _unitOfWork.Projects.GetAllAsync(p => p.Name.ToLower().Equals(name.ToLower()))).Data?.Any() ?? false;
            if (isExisting)
            {
                throw new Exception("Project with the same name already exists");
            }
            var project = new ProjectDomain(name, prompt);
            if (documents is not null)
            {
                project.SetDocuments(documents);
                await _genAiAdapter.SendFilesAsync(documents);
            }
            project.PromptResponse = await _genAiAdapter.SendPromptAsync(prompt, documents);
            var userstories = LoadUsersStoriesFromProject(project);
            project.SetUserStories(userstories);
            await _unitOfWork.Projects.AddAsync(project);
            await _unitOfWork.SaveChangesAsync();
            return project;
        }
    }
    ```

### 3. Couche de Domaine (Domain Layer)

Cette couche contient les entités métier et les règles de domaine.

- **Exemple de code** :

    ```csharp
    public class ProjectDomain
    {
        public string Name { get; private set; }
        public string Prompt { get; private set; }
        public string PromptResponse { get; set; }
        public List<DocumentDomain> Documents { get; private set; }
        public List<UserStoryDomain> UserStories { get; private set; }

        public ProjectDomain(string name, string prompt)
        {
            Name = name;
            Prompt = prompt;
            Documents = new List<DocumentDomain>();
            UserStories = new List<UserStoryDomain>();
        }

        public void SetDocuments(IEnumerable<DocumentDomain> documents)
        {
            Documents.AddRange(documents);
        }

        public void SetUserStories(IEnumerable<UserStoryDomain> userStories)
        {
            UserStories.AddRange(userStories);
        }
    }
    ```

### 4. Couche d'Infrastructure (Infrastructure Layer)

Cette couche contient les implémentations concrètes des interfaces et les services externes.

- **Exemple de code** :

    ```csharp
    public class GenAiApiAdapter : IGenAiApiAdapter
    {
        public async Task SendFilesAsync(IEnumerable<DocumentDomain> documents)
        {
            // Implémentation pour envoyer des fichiers à l'API GenAI
        }

        public async Task<string> SendPromptAsync(string prompt, IEnumerable<DocumentDomain>? documents)
        {
            // Implémentation pour envoyer un prompt à l'API GenAI et obtenir une réponse
        }
    }
    ```

En suivant ces lignes directrices et en respectant la Clean Architecture, vous pouvez créer une application WebAPI en C# robuste, maintenable et évolutive.
