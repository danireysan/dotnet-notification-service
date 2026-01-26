# dotnet-notification-service

## Decisions Taken 
- The project is made with a feature folder based TDD clean architecture monolith to have a foundation for making a project that is easy to test and easy to refactor to microservices if needed
- The project uses slnx instead of sln because it is encouraged for new projects for its better readability.
- I used controllers instead of minimal API's because despite minimal API's being very attractive for its conciseness, controllers are more mature in this environment
- I did not use Native AOT because despite its better performance, the speed of development in this project is important, and hot reload speeds up the iterative process.
- The project uses Funcky a lightweight FP library to create more explicit contracts and declarative pipelines
- The project stores user Ids as ULID so we can have faster indexes
- I chose ASP.NET Core Identity with cookie authentication to keep the project self-contained and easy to run for reviewers. In a production system, this could be replaced with an external identity provider such as Microsoft Entra ID.

## Know issues


