# Ce qu'il doit etre fait AVANT mise en prodution (ou présentation du POC)

## Architecture

diagramme à voir plus tard

environment :

- Azure pour production/staging
- local pour developpement

### Codes de Statut HTTP à respecter pour les Opérations GET, POST, PUT et DELETE

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

#### Détails des Codes de Statut

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

## US.0 - [HORS POC] Authentification

stqndby

## US.1 - Page "Liste des projets"

liste des apis : voir la document du front pour leur fonctionnalité :

### `api: [GET]/project`

retour https status OK
récupération d'une collection de projet : id, name uniquement

### `api: [POST]/project`

Création d'un projet et génération des userstory par l'IA

retour https status 201 Created

## US.2 - Page "Projet"

liste des apis : voir la document du front pour leur fonctionnalité :

### `api: [GET]/project/:id`

 récupération du projet : id, name, prompt, documents, userstories (document et userstories : id, name uniquement)

### `api: [POST]/project`

creation du projet avec au minima :prompt généraliste, nom du projet et sa liste de documents

1. génère des usterstories et leur chiffrage

### `api: [PUT]/project`

 mise à jour du projet avec au minima : nom et liste de documents

1. génère des usterstories et leur chiffrage
2. **[HORS POC]** supprime le chiffrage fait par les humains

### `[DELETE]/project/:id`

suppression du projet

### `api: [GET]/project/:projectId/document/:id`

récupération du "Content" stocké en base du document à fin de téléchargement (donc le front se charge de le transformer en un fichier physique sur le DD du client)

### `api: [POST]/project/:projectId/document`

ajout d'un document au projet

### `api: [DELETE]/project/:projectId/document/:id`

suppression d'un document du projet

### `api: [GET]/project/:id/userstory`

récupération de la liste des userstories du projet: id, name, tasks (tous les champs)

## US.3 - [HORS POC] Page "Chiffrage humains"

liste des apis : voir la document du front pour leur fonctionnalité :

### `(api: [POST]/project/:id/userstory)`

## US.4 - [HORS POC] Page "Comparaison des chiffrages"

liste des apis : voir la document du front pour leur fonctionnalité :

### `api: [GET]/project/:id/userstory`

récupération de la liste des userstories du projet: id, name, tasks (tous les champs)

### `api: [PUT]/project/:id/userstory`

transmission des chiffrage humain pour chaque tache de la liste des userstories du projet: id, name, tasks (tous les champs)
