# dotnet-notification-service

## Decisions Taken 
- The project is made with a feature folder based TDD clean architecture monolith to have a foundation for making a project that is easy to test and easy to refactor to microservices if needed
- The project uses slnx instead of sln because it is encouraged for new projects for its better readability.
- I used controllers instead of minimal API's because despite minimal API's being very attractive for its conciseness, controllers are more mature in this environment
- I did not use Native AOT because despite its better performance, the speed of development in this project is important, and hot reload speeds up the iterative process.
- The project uses Funcky a lightweight FP library to create more explicit contracts and declarative pipelines
- The project stores user Ids as ULID so we can have faster indexes
- I chose ASP.NET Core Identity with token auth to keep the project self-contained and easy to run for reviewers. In a production system, this could be replaced with an external identity provider such as Microsoft Entra ID.
- I created a JwtOptions POCO to keep the JWT configuration in one place and make it easier to change in the future.
- I did not use a Datasource layer in the auth feature because ASP.NET Core Identity already abstracts the data access layer.
- I modeled the Notification entity as a record because it is an immutable data structure that represents a notification.
- I use Testcontainers to create a disposable SQL Server instance for integration tests to ensure that tests are isolated and do not depend on external resources.

## Know issues
- The project does not have automatic DI registration so every new service needs to be registered manually
- Tests could use some refactoring to reduce duplication and improve readability
- The Programs.cs file is a bit long because of all the service registrations and middleware configurations, it could be split into extension methods for better readability.



