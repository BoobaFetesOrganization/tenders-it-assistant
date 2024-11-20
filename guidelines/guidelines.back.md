fichier généré avec *ChatGpt-4o*

# Guidelines pour les intervenants d'un projet WebAPI en C #

## Bonnes pratiques des API

Tout non respect de règles ci-dessous fera l'objet d'un refus catégorique de PR.

### APIs RestFull

[article expliquant ce que sont les API RestFULL](https://www.redhat.com/fr/topics/api/what-is-a-rest-api?form=MG0AV3)

[article vers les verbes HTTP à utiliser pour les API](https://developer.mozilla.org/fr/docs/Web/HTTP/Methods?form=MG0AV3)

De plus, toutes les APIs doivent :
- retourner l'objet du domain sur lequel il a travaillé (incluant les DELETE et les POST) 
- être implementé sur le contoller représentant ce domain

### Endpoints

Les endpoints ou point de terminaisons doivent toujours refleter le domaine.

Voici les règles de présentation de nos APIs :

- chaque segment représente soit un domain soit l'identifiant d'un element du domain
- tout segment est un mot au singulier

- un segment d'url intermediaire peut etre insérer pour des actions qui ne sont pas de l'ordre du CRUD
  - bien souvent il s'agit d'action modifiant l'état du domain donc le verbe PUT doit etre utilisé
voici quelques exemples :

|Endpoint|Explication|
|-|-|
|/project| liste les projets|
|/project/:id| fourni les details d'un projet|
|/projet/:id/generate/userstory| génère les user stories du projet cible|
|/project/:projectId/document|liste les documents du projet cible|
|/project/:projectId/document/:id| fourni les détails d'un document du projet cible|
|/project/:projectId/userstory|liste les userstories du projet cible|
|/project/:projectId/userstory/:id| fourni les détails d'une userstory du projet cible|
|/project/:projectId/userstory/:id/task| fourni les détails d'une tache d'une userstory d'un projet|
|/project/:projectId/userstory/:userstoryId/task/:id| fourni les détails d'une tache d'une userstory d'un projet|

### Code de retour

| Code | Description |
|------|-------------|
| 200 OK | La requête a réussi. |
| 201 Created | La requête POST a réussi et une nouvelle ressource a été créée. |
| 204 No Content | La requête a réussi, mais il n'y a pas de contenu à retourner. |
| 400 Bad Request | La requête est mal formée ou invalide. |
| 401 Unauthorized | L'utilisateur n'est pas authentifié. |
| 403 Forbidden | L'utilisateur n'a pas les permissions nécessaires. |
| 404 Not Found | La ressource demandée n'existe pas. |
| 409 Conflict | Le contenu de la requête est en conflit avec l'état actuel du serveur. |
| 500 Internal Server Error | Une erreur interne du serveur s'est produite. |

### Code de retour par Verbe HTTP

| Méthode | Réussite                       | Échec                                     |
|---------|--------------------------------|-------------------------------------------|
| **GET** | 200 OK                         | 400 Bad Request                           |
|         |                                | 401 Unauthorized                          |
|         |                                | 403 Forbidden                             |
|         |                                | 404 Not Found                             |
|         |                                | 500 Internal Server Error                 |
| **POST**| 201 Created                    | 400 Bad Request                           |
|         | 200 OK (si aucune ressource n'est créée) | 401 Unauthorized                          |
|         |                                | 403 Forbidden                             |
|         |                                | 404 Not Found                             |
|         |                                | 409 Conflict (si le contenu est en conflit avec l'état actuel du serveur) |
|         |                                | 500 Internal Server Error                 |
| **PUT** | 200 OK                         | 400 Bad Request                           |
|         | 204 No Content (si aucune réponse n'est nécessaire) | 401 Unauthorized                          |
|         |                                | 403 Forbidden                             |
|         |                                | 404 Not Found                             |
|         |                                | 409 Conflict (si le contenu est en conflit avec l'état actuel du serveur) |
|         |                                | 500 Internal Server Error                 |
| **DELETE**| 200 OK                       | 400 Bad Request                           |
|         | 204 No Content (si l'opération est réussie mais aucune réponse n'est nécessaire) | 401 Unauthorized                          |
|         |                                | 403 Forbidden                             |
|         |                                | 404 Not Found                             |
|         |                                | 500 Internal Server Error                 |

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
