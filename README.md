# Mini Projet — Gestion Commerciale

Application de gestion commerciale permettant de gérer les clients, les produits et les commandes, avec calcul automatique des totaux et gestion du stock.

Projet réalisé dans le cadre d'un test technique, selon le cahier des charges fourni.

Une vidéo de démonstration présentant le fonctionnement complet de l'application est jointe à ce livrable.

## Stack technique

- **Back-end** : .NET 8 (ASP.NET Core Web API)
- **Front-end** : Angular 18 (standalone components)
- **Base de données** : SQLite via Entity Framework Core

> **Note sur SQLite** : le cahier des charges demandait SQL Server. J'ai choisi SQLite pour ce livrable, une option explicitement acceptée par le cahier des charges (section 6 : "ou une base SQLite prête à l'emploi"). Grâce à Entity Framework Core, le code est écrit de façon totalement indépendante du moteur de base de données : passer à SQL Server ne demande que de changer le package NuGet (`Microsoft.EntityFrameworkCore.SqlServer` au lieu de `.Sqlite`) et la chaîne de connexion dans `appsettings.json` — aucune ligne de logique métier, de modèle ou de service ne change.

## Prérequis

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) version 18 ou supérieure
- [Angular CLI](https://angular.dev/tools/cli) : `npm install -g @angular/cli`

Aucune installation de base de données n'est nécessaire : SQLite fonctionne comme un simple fichier, créé automatiquement au premier lancement.

## Structure du projet

```
GestionCommerciale/
├── GestionCommerciale.Api/       → Back-end .NET 8 (API REST)
└── gestion-commerciale-ui/       → Front-end Angular
```

## Lancer le back-end

```bash
cd GestionCommerciale.Api
dotnet restore
dotnet run
```

Au premier lancement, l'application crée automatiquement le fichier de base de données `GestionCommercialeDb.sqlite` et applique les migrations Entity Framework Core (création des tables Clients, Products, Orders, OrderLines). La base est vide au démarrage — voir la section "Scénario de test" ci-dessous pour la peupler.

L'API est accessible sur l'URL affichée dans le terminal (ex: `http://localhost:5089`).

La documentation interactive des endpoints (Swagger) est disponible sur :
```
http://localhost:5163/swagger
```

## Lancer le front-end

Dans un **second terminal** :

```bash
cd gestion-commerciale-ui
npm install
```

Puis :

```bash
ng serve
```

L'application est accessible sur :
```
http://localhost:4200
```

## Scénario de test

La base de données est vide au premier lancement. Pour tester l'application de bout en bout  :

1. Aller dans **Clients** → créer un client
2. Aller dans **Produits** → créer plusieurs produits avec stock et prix
3. Aller dans **Commandes** → créer une nouvelle commande pour ce client, ajouter plusieurs produits
4. Constater le calcul automatique des totaux (HT / TTC avec TVA à 19%)
5. Ouvrir le détail de la commande et cliquer sur **Valider la commande**
6. Retourner dans **Produits** pour constater la mise à jour automatique du stock

Ce scénario complet est également visible dans la vidéo de démonstration jointe.

## Fonctionnalités

- **Clients** : liste, création, modification, suppression
- **Produits** : liste, création, modification, suppression, indicateur visuel de stock faible
- **Commandes** :
  - Création avec sélection d'un client et ajout dynamique de plusieurs lignes de produits
  - Calcul automatique du total de chaque ligne et du total global (HT / TTC avec TVA à 19%)
  - Modification d'une commande en statut "Brouillon"
  - Validation d'une commande (met à jour le stock des produits automatiquement)
  - Page de détail avec récapitulatif complet

## Règles de gestion implémentées

- Une commande ne peut pas être créée sans client
- Une ligne de commande ne peut pas avoir une quantité ≤ 0
- Il n'est pas possible de commander une quantité supérieure au stock disponible
- Le stock est décrémenté uniquement à la validation de la commande (pas à la création)
- Le prix unitaire est figé sur la ligne de commande au moment de sa création (indépendant d'une modification ultérieure du prix catalogue)
- Seule une commande en statut "Brouillon" peut être modifiée ou validée

## Architecture back-end

Projet organisé en couches, dans un projet ASP.NET Core :

```
GestionCommerciale.Api/
├── Controllers/     → Points d'entrée HTTP 
├── Services/        → Logique métier et règles de gestion
├── Data/            → DbContext EF Core
├── Models/          → Entités (tables de la base de données)
├── DTOs/            → Objets d'échange avec le front-end
└── Migrations/      → Historique des migrations EF Core
```

Chaque service est défini par une interface (`IClientService`, `IProductService`, `IOrderService`) injectée via l'injection de dépendances native d'ASP.NET Core, ce qui permet de découpler les Controllers de leur implémentation concrète.

## Choix techniques assumés

- **Architecture en un seul projet** plutôt qu'une séparation en plusieurs projets (Clean Architecture à la Domain/Application/Infrastructure) : pour un projet de cette taille et ce délai, une organisation claire en dossiers avec interfaces et injection de dépendances apporte les mêmes bénéfices de séparation des responsabilités, avec une prise en main plus rapide.
- **Mapping manuel** entre entités et DTOs plutôt qu'une librairie comme AutoMapper : plus explicite et suffisant à cette échelle.
- **Adresses client** stockées en colonne JSON (`List<string>` convertie automatiquement par EF Core) plutôt qu'une table séparée : les adresses sont de simples chaînes de texte sans sous-structure (pas de ville/code postal séparés dans le cahier des charges), une table dédiée aurait ajouté de la complexité sans bénéfice ici.
- **Base de données SQLite** plutôt que SQL Server : option explicitement acceptée par le cahier des charges (section 6), retenue pour la simplicité d'installation. EF Core garantit une migration triviale vers SQL Server si nécessaire.
- **Authentification** : non implémentée dans cette version (bonus optionnel du cahier des charges, section 7).

## Auteur

## Wael Bouatay
