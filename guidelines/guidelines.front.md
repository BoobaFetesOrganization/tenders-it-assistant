ecrit moi un fichier guidelines.md pour tous les intervenants d'un projet react avec nx qui possède les couches suivante :

je veux un rappel des bonne pratique de developpements, de testing pour une application react avec es exemple concret utilisant des hook react

tu décrira aussi brievement comment fonctionne apoolo/client pour une developpeur qui ne connaitrait pas

tu donnera des exemples de code Typescript , react et appolo/client

Concernant la partie "Bonne pratiques de developpement"
pour la modularité : je veux que tu donne une exemple avec les hooks reacts
pour les principes solid: je veux que tu donne une exemple avec un function component react, pareil pour la documentation

je veux pouvoir copier/coller l'ensemble de ta réponse dans un fichier MD

fichier généré avec *ChatGpt-4o*

# Guidelines pour les intervenants d'un projet React avec Nx

## Clean Architecture de Martin Fowler

La Clean Architecture de Martin Fowler est une approche de conception logicielle qui vise à séparer les préoccupations et à créer des systèmes modulaires et maintenables. Elle est composée de plusieurs couches, chacune ayant une responsabilité distincte :

1. **Couche de Présentation (Presentation Layer)** : Contient les composants d'interface utilisateur et la logique de présentation. Cette couche interagit avec les utilisateurs et affiche les données.
2. **Couche d'Application (Application Layer)** : Contient la logique métier spécifique à l'application. Elle orchestre les opérations et les flux de données entre les différentes couches.
3. **Couche de Domaine (Domain Layer)** : Contient les entités métier et les règles de domaine. Cette couche est indépendante des détails techniques et des frameworks.
4. **Couche d'Infrastructure (Infrastructure Layer)** : Contient les implémentations concrètes des interfaces et les services externes. Elle gère les interactions avec les bases de données, les API externes, etc.

Chaque couche dépend uniquement des couches inférieures, ce qui permet de maintenir une séparation claire des préoccupations et de faciliter les tests et la maintenance.

## Couches du projet

1. **@scope/application** : Contient la logique de l'application et les composants React.
2. **@scope/domain** : Contient les entités métier et les règles de domaine. *(contient donc les objets de retour du back !!!)*
3. **@scope/infrastructure** : Gère les interactions avec les services externes et les API.
4. **@scope/react-infrastructure** : Encapsule les dépendances liées à l'infrastructure, évitant ainsi l'import de package n'ayant pas trait à la logique de l'application.

### Couche @scope/react-infrastructure

Cette couche permet de maintenir une séparation claire des préoccupations entre l'application et l'infrastructure et de faciliter les tests.

En claire, elle encapsule les packages pour communiquer ou pour intergir avec l'environnement pour éviter que la couche application ne dépende de package technique.

ex: encapsule les requêtes et commandes Apollo Client, de ce fait @scope/application ne connait pas appollo/client et donc permet de changer sans danger la connectivité (ex on change pour redux-saga)

## Utilisation de NX

je vous prie de trouver la documentation officielle : <https://nx.dev/getting-started/intro>

> executez les commande suivante depuis le repertoire root, cad celui qui contient le fichier package.json

```powersehell

# installer les package NPM, comme d'habitude
yarn

# lancez l'application 
yarn nx serve client
```

>notez que le nom de la SPA n'est pas obligatoire
> ex : yarn nx serve

normalement un navigateur se lance avec la bonne url.

**Vous pourrez debugger votre application en ajoutant des breakpoints, ce que je vous recommande vivement**

## Bonnes pratiques de développement

### Performance

Utiliser les hook et les HOC permettant de mémoizer vos composant ou vos callback afin de permettre au moteur de React d'améliorer les performance

exemple de code :

```typescript
// préférez l'import suivant plutot que [import React from 'react']
import { memo, useCallback } from 'react';

interface MyComponentProps {
  count: string;
}

export const MyComponent: React.FC<MyComponentProps> = memo(({ count }) => {
    const [value, setValue] = useState(count);
    
    const increment = useCallback(() => {
        setCount(prevCount => prevCount + 1);
    }, []);

    const expensiveValue = useMemo(() => {
        console.log('Calculating expensive value');
        return count * 2;
    }, [count]);

    return (
        <div>
            <p>Count: {count}</p>
            <p>Expensive Value: {expensiveValue}</p>
            <button onClick={increment}>Increment</button>
        </div>
    );
});
```

### Modularité

Utilisez des hooks React pour modulariser votre code.

> Les composant (comme les Function Components ou les Class Components) ne devrait pas avoir d'état, sauf si l'ajout d'un hook complique la lisibilité.

Le hook permet la séparation de responsabilité entre la vue (le composant React) et son comportement (le hook)

exemple de code :

```typescript
// User.tsx
interface IUserProps{
    id: number;
}

const User: FC<IUserProps>=memo(props => {
    // théoriquement le composant ne devrait pas avoir cet etat 
    // mais dans ce cas précis le placer dans un hook complique inutilement la vue.    
    const [value, setValue] = useState(`${props.id}`);

    // par contre dans ce cas, il est obligatoire de séparer la logique (la communication) et la vue (le visuel) 
    // car cela nous permet de séparer les comportements et faciliter leur testabilité
    const { user, setUser, fetch } = useUserQuery(props);

    return (
        <div>
            <div>
                <div>user id to fetch :</div>
                <input value={value} onChange={evt => setValue(evt.target.value)} />
            </div>
            <div>            
                <h1>{user.name}</h1>
                <p>{user.email}</p>
                <button onClick={() => refetch(+value)}>Refresh</button>
            </div>
        </div>
    );
});

// useUserQuery.ts
import { useState, useEffect } from 'react';

interface IUserUserQueryProps{
    id: number;
}
interface IUserUserQueryResult{
    user: IUser;
    setUser(state: IUser):void;
    refetch(id: number):void;
}

export function useUserQuery({ id }: IUserUserQueryProps) {
  const [user, setUser] = useState<IUser>(null);
  const [error, setError] = useError<string>(null);

  const callApi = useCallback(async (id: number) => {
    // Fetch user data
    try {
        setError(null);
        return await fetch(`/api/users/${id}`);
    }
    catch(ex){
        setError(ex.message);
    }
  }, []);

  useEffect(() => { callApi(id); }, [id]);

  return useMemo(() => { user, setUser, refetch:(userId:number) => callApi(userId) });
}
```

### Principes SOLID

Respectez les principes SOLID avec des composants fonctionnels React.

```typescript
// UserProfile.tsx
import React from 'react';
import { useUser } from './useUser';

interface UserProfileProps {
  userId: string;
}

const UserProfile: React.FC<UserProfileProps> = ({ userId }) => {
  const user = useUser(userId);

  if (!user) return <div>Loading...</div>;

  return (
    <div>
      <h1>{user.name}</h1>
      <p>{user.email}</p>
    </div>
  );
};

export default UserProfile;
```

### Documentation

Commentez votre code pour maintenir une documentation à jour.

```typescript
/**
 * Hook to fetch user data.
 * @param userId - The ID of the user to fetch.
 * @returns The user data.
 */
export function useUser(userId: string) {
  // ...
}
```

### Bonnes pratiques de testing

#### Tests unitaires

Écrivez des tests unitaires pour chaque hook et composant

```typescript
// useUser.test.ts
import { renderHook } from '@testing-library/react-hooks';
import { useUser } from './useUser';

test('should fetch user data', async () => {
  const { result, waitForNextUpdate } = renderHook(() => useUser('1'));

  await waitForNextUpdate();

  expect(result.current).toEqual({ name: 'John Doe', email: 'john@example.com' });
});
```

## Fonctionnement d'Apollo Client

Apollo Client est une bibliothèque de gestion de l'état pour JavaScript qui permet de gérer les requêtes GraphQL (et REST). Il fournit des outils pour interagir avec une API GraphQL (ou des APIs REST) et gérer les données dans votre application.

En d'autres termes, elle permet la gestion des états distants (les états lié à vos APIs) et locaux (états propre à votre applications) sans code additionnel comme le nécessiterait redux.

> Cette bibliothèque utilise le concept de variables réactive afin de gérer les états locaux et permet aussi de les inclure dans vos communications avec vos APIs de façon très efficace sans ajout de code supplémentaire !
> (voici ici : <https://www.apollographql.com/docs/react/local-state/reactive-variables>)

Exemple de code avec Apollo Client

```typescript
// queries.ts
import { gql } from '@apollo/client';

export const GET_USER = gql`
  query GetUser($id: ID!) {
    user(id: $id) {
      id
      name
      email
    }
  }
`;

// UserProfile.tsx
import React from 'react';
import { useQuery } from '@apollo/client';
import { GET_USER } from './queries';

interface UserProfileProps {
  userId: string;
}

const UserProfile: React.FC<UserProfileProps> = ({ userId }) => {
  const { loading, error, data } = useQuery(GET_USER, {
    variables: { id: userId },
  });

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error.message}</div>;

  return (
    <div>
      <h1>{data.user.name}</h1>
      <p>{data.user.email}</p>
    </div>
  );
};

export default UserProfile;
```
