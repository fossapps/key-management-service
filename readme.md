## KeyStore

The goal of this project is to have service that can be used as a simple public key storage for auth services (or any other kind of services that you might want)

Consider a authentication system which uses JWT, and uses asymmetric keys to sign keys, public keys can be stored on this storage with some retention policy.

Once a key expires, it's simply removed after being sent to a long term storage for historical purposes.

## Getting Started
You can start by cloning this repository, or better yet, just use docker to run this on your own.

## Contribution
Please follow the existing coding standards which is being followed, no trailing whitespaces, edge cases goes to if conditions,
follow line of sight rule. Happy path is always straight down, only short circuit (early exits) the error path unless there's a strong reason not to.

## Structure

This repository contains backend API for KeyStore project. The problem "KeyStorage" is solved using 4 different projects:
- Startup
- API
- Business
- Storage

### Startup
Startup is what hooks things together, and this is the entry point of the entire project.

### API
This is the API layer of this project. Currently it implements GraphQL protocol. But it could in theory be changed to whatever we'd want to.

### Business
This houses all the business logic behind the backend API. This is a transport agnostic layer which means if you wanted to switch from GraphQL to something else, you wouldn't have to touch this project.

### Storage
This contains everything that is storage specific. If we ever wanted to switch from normal RDBMS to a different paradigm, we could simply change this and everything else should be working exactly the same way.

### Creating Migrations
Entity Framework helps you create migrations by looking at what's in the database vs what you're trying to get to.

To create a migration, first change your models as you wish. If you're creating a new model, simply create the new model and register in application context.

Once You're happy with the changes, simply enter into the Startup project and run: `dotnet ef migrations add InitialCreate --project ../Storage/Storage.csproj` This should create migrations inside Storage project.

### Running a migration
To run a migration, simply run use the migrate command
