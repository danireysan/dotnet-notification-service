# dotnet-notification-service

## How to run the project

### Prerequisites
- Docker is required to run the project
- Docker Compose is used to run the project
- Firebase json credentials file is required to send push notifications, you can create a Firebase project and generate the necessary credentials.
- Twilio credentials are required to send SMS notifications, you can create a Twilio account and get the necessary credentials.
- Smtp credentials are required to send email notifications, you can use any SMTP server and get the necessary credentials.
- The credentials should be added to the .env file in the root of the project, you can use the .env.example file as a template.


### Running the project
```
chmod 711 up_dev.sh
./up_dev.sh
```


## Decisions Taken for:
### Setup, Architecture and Design
- The project is made with a feature folder based TDD clean architecture monolith to have a foundation for making a project that is easy to test and easy to refactor to microservices if needed
- The project uses slnx instead of sln because it is encouraged for new projects for its better readability.
- I used controllers instead of minimal API's because despite minimal API's being very attractive for its conciseness, controllers are more mature and widespread in this environment for their better structure and organization.
- The project uses Funcky a lightweight functional programming library to create more explicit contracts and declarative pipelines

### Implementation
- The project stores user Ids as ULID so we can have faster indexes
- I chose ASP.NET Core Identity with token auth to keep the project self-contained and easy to run for reviewers. In a production system, this could be replaced with an external identity provider such as Microsoft Entra ID.
- I created a JwtOptions POCO to keep the JWT configuration in one place and make it easier to change in the future.
- I did not use a Datasource layer in the auth feature because ASP.NET Core Identity already abstracts the data access layer.
- I modeled the Notification entity as a record because it is an immutable data structure that represents a notification.
- I used MailKit and used 587 port for SMTP because it is the latest port for secure email submission.
- I added a metadata field to the Notification entity to allow for extensibility in the future, as it can store additional information about the notification without changing the database schema.
- I added a small health check endpoint to allow for monitoring the health of the service, we could have used the built-in health checks in ASP.NET Core, but I wanted to keep it simple for this exercise.

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
- Metadata field in the Notification entity is currently a string, it could be changed to a more structured format such as JSON or a separate table for better querying and flexibility in the future.

## Possible improvements
- The project could benefit from implementing cancellation tokens
- The project could benefit from implementing rate limiting to prevent abuse
- The project could benefit from a CI/CD pipeline for automated testing and deployment
- The project could benefit from using Swagger for API documentation

## Non Goals
- The Get Notifications endpoint does not have pagination or filtering implemented because it is out of scope for this exercise, but it can be easily added in the future if needed.



