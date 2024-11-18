# Ce qu'il doit etre fait AVANT mise en prodution (ou présentation du POC)

---

## Architecture

### logiciel : Clean architecture avec NX

1. @scope/application (si nécessaire avec redux pour les états de l'application)
2. @scope/domain : les entités metier (et les entités du back : DTO)
3. @scope/infra : gestion de données entre l'exterieur et le front avec apollo/client
4. @scope/react-infra : couche d'adaptation entre react et apollo/client ou redux

### UI

1. techno : Material-UI (mui)
2. display: flexbox
3. type de visuel :
   1. responsive
   2. scroll sur le contenu de la page uniquement

### connectivité

   apollo/client (REST)

## US.0 - [HORS POC] Authentification

- ajout d'une redirection SSO (implicit flow)
- ajout de la page de login (si authorization flow)

## US.1 - Page "Liste des projets"

- type: treeview
- permetttre l'affichage sur la partie gauche de l'ecran et retractable
- une liste de projet (id, nom)
  - `api: [GET]/project`
  - la suppression du projet
  - la navigation vers la page du projet
- l'ajout d'un projet
  - `(api: [POST]/project)`
  - navigation vers la page de "projet" en mode creation

## US.2 - Page "Projet"

- `(api: [GET]/project/:id)`
- une barre d'action (horizontal pour se simplifier la vie - alignement par la droite):
  - [si non créé] un bouton "Créer" permettant la création du projet `(api: [POST]/project)`
    - envoi des documents et du prompt au back pour nouveau chiffrage
  - [si créé] un bouton "Sauver" remplacant le bouton "Créer" `(api: [PUT]/project)`
    - envoi des documents et du prompt au back pour chiffrage
  - **[HORS POC]** un bouton "Chiffrage" pour faire estimer à des humains les userstories
    - navigation vers l'écran de chiffrage
  - **[HORS POC]** [si mise à jour des documents] le chiffrage fait par les humains disparait
- contenu de la page (avec scroll bar - la barre d'action doit toujours être visible)
  - un champ nom (mode: read/write)
  - une collection des documents du projet présentant : `(api: [GET]/project/:projectId/document/:id)`
    - l'id du document
    - le nom du document
    - la date de dépôt (date de creation)
    - un bouton "Ajout" pour ajouter un document `(api: [POST]/project/:projectId/document)`
    - un bouton "Supprimer" pour supprimer un document `(api: [DELETE]/project/:projectId/document/:id)`
    - **[HORS POC]** un bouton "Download" pour télécharger un document `(api: [GET]/project/:projectId/document/:id)`
  - un champ pour le prompt (modifiable)
  - une collection de userstories généré lors de la phase de creation `(api: [GET]/project/:id/userstory)`

## US.3 - [HORS POC] Page "Chiffrage humains"

- un champs (non editable) pour le chiffrage de toutes les userstories
- une liste des userstories à chiffrer
  - composant de types accordéon pour les user stories avec le cout total estimé des taches
  - un composant de type tableau pour les taches de chaque user story (id, nom, cout estimé)
- un bouton "Valider" pour sauvegarder le chiffrages et l'envoyer au back pour sauvegarde `(api: [POST]/project/:id/userstory)`

## US.4 - [HORS POC] Page "Comparaison des chiffrages"

- une liste des userstories à comparer (=> meme écran que la page de chiffrage pour les humains avec quelques nuances)
  - le cout des userstories change en fonction de la selection des chiffrages actifs
  - à chaque taches 3 inputs :
    - bouton de gauche : selection du chiffrage de l'IA
    - au mileu, un input de saisie de type number pour un chiffrage manuel
    - bouton de droite : selection du chiffrage de l'humain
    - un seul des trois inputs ne peut etre selectionné
    - couleur des inputs selectionnés en fonction de la palette du design system
- un bouton "Valider" pour sauvegarder la comparaison `(api: [POST]/project/:id/userstory)`
