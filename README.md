# dotnet-notification-service

## Decisions Taken for:
### Setup, Architecture and Design
- The project is made with a feature folder based TDD clean architecture monolith to have a foundation for making a project that is easy to test and easy to refactor to microservices if needed
- The project uses slnx instead of sln because it is encouraged for new projects for its better readability.
- I used controllers instead of minimal API's because despite minimal API's being very attractive for its conciseness, controllers are more mature and widespread in this environment for their better structure and organization.
- I did not use Native AOT because despite its better performance, the speed of development in this project is important, and hot reload speeds up the iterative process.
- The project uses Funcky a lightweight FP library to create more explicit contracts and declarative pipelines

### Implementation
- The project stores user Ids as ULID so we can have faster indexes
- I chose ASP.NET Core Identity with token auth to keep the project self-contained and easy to run for reviewers. In a production system, this could be replaced with an external identity provider such as Microsoft Entra ID.
- I created a JwtOptions POCO to keep the JWT configuration in one place and make it easier to change in the future.
- I did not use a Datasource layer in the auth feature because ASP.NET Core Identity already abstracts the data access layer.
- I modeled the Notification entity as a record because it is an immutable data structure that represents a notification.

### Testing
- I used the Arrange-Act-Assert pattern in tests to improve readability and maintainability by clearly separating the setup, execution, and verification phases of each test case.
- I used Testcontainers to create a disposable SQL Server instance for integration tests to ensure that tests are isolated and do not depend on external resources.
- I decided to do Black Box testing from the Notification Controller feature forward due that I consider this strategy will offer the most value, due that this feature is very CRUD centric.
- I just tested the Create Notification endpoint for authorization because the authorization is set for the entire controller, and testing one endpoint is sufficient to ensure that the authorization mechanism works as expected.


## Known issues
- Changing the hashing algorithm is not as striaghforward as changing the implementation of IPasswordHasher, it requires a migration to update old passwords to the new format.
- The project does not have automatic DI registration so every new service needs to be registered manually
- Tests could use some refactoring to reduce duplication and improve readability
- The Programs.cs file is a bit long because of all the service registrations and middleware configurations, it could be split into extension methods for better readability.
- The project does not have logging implemented, which is crucial for monitoring and debugging in a production environment.

## Possible improvements
- The project could benefit from implementing cancellation tokens
- The project could benefit from implementing rate limiting to prevent abuse
- The project could benefit from a CI/CD pipeline for automated testing and deployment
- The project could benefit from using Swagger for API documentation

## Non Goals
- The Get Notifications endpoint does not have pagination or filtering implemented because it is out of scope for this exercise, but it can be easily added in the future if needed.



