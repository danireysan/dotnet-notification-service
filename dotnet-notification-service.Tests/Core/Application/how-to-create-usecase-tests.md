This README explains how to test the **Use Case** (Application Layer) of the application. Testing Use Cases is critical because they contain the actual business logic, orchestration, and domain rules.

---

# Testing Use Cases

Use Case tests ensure that the business logic behaves correctly by orchestrating Domain Entities and Repositories. Unlike Controller tests, which focus on HTTP responses, Use Case tests focus on **Domain Logic** and **Data Flow**.

### The Testing Strategy

We test Use Cases by mocking their external dependencies (Repositories, Services, or Identity Providers) and verifying the `Either<Failure, T>` result.

---

## 1. Setup & Tools

* **xUnit**: Test Runner.
* **Moq**: To mock Repositories and Domain Services.
* **FluentAssertions** (Optional but recommended): For more readable assertions.
* **Funcky.Monads**: To handle the `Either` result type.

---

## 2. Standard Test Structure

### A. Arrange

1. **Mock Dependencies**: Identify what the Use Case needs (e.g., `IUserRepository`).
2. **Define Success/Failure Scenarios**: Set up the repository to return a specific entity or throw a custom domain exception.
3. **Instantiate Use Case**: Inject the mocked objects into the Use Case constructor.

### B. Act

Call the `CallAsync` (or `Execute`) method with a Command or Request object.

### C. Assert

1. **Verify the Monad**: Use `.Match` or xUnit asserts to check if the result is `Right` (Success) or `Left` (Failure).
2. **Verify Side Effects**: Check if the Repository's `Save` or `Add` methods were called with the correct data.

---

## 3. Example: CreateUserUseCase Test

Here is how you test a Use Case that saves a user and returns a result.

```csharp
public class CreateUserUseCaseTests
{
    private readonly Mock<IUserRepository> _repositoryMock;
    private readonly CreateUserUseCase _useCase;

    public CreateUserUseCaseTests()
    {
        _repositoryMock = new Mock<IUserRepository>();
        _useCase = new CreateUserUseCase(_repositoryMock.Object);
    }

    [Fact]
    public async Task CallAsync_ShouldReturnSuccess_WhenUserIsSaved()
    {
        // Arrange
        var command = new CreateUserCommand("new@user.com", "SecurePass123");
        
        // Mocking the repository to return success
        _repositoryMock
            .Setup(repo => repo.SaveAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.CallAsync(command);

        // Assert
        // 1. Verify it is a success (Right)
        Assert.True(result.IsRight);
        
        // 2. Verify repository was called exactly once
        _repositoryMock.Verify(repo => repo.SaveAsync(It.Is<User>(u => u.Email == command.Email)), Times.Once);
    }

    [Fact]
    public async Task CallAsync_ShouldReturnFailure_WhenEmailAlreadyExists()
    {
        // Arrange
        var command = new CreateUserCommand("exists@user.com", "pass");
        var domainFailure = new Failure { Message = "User already exists" };

        _repositoryMock
            .Setup(repo => repo.SaveAsync(It.IsAny<User>()))
            .ThrowsAsync(new DomainException("User already exists"));

        // Act
        var result = await _useCase.CallAsync(command);

        // Assert
        // Verify it is a Failure (Left)
        Assert.True(result.IsLeft);
        result.AndThen(f => Assert.Equal("User already exists", f.Message));
    }
}

```

---

## 4. Best Practices

### Use `It.IsAny<T>()` and `It.Is<T>(...)`

* Use `It.IsAny<User>()` when the specific data doesn't matter for the test.
* Use `It.Is<User>(u => u.Email == "...")` to verify that the Use Case correctly mapped the Command data to the Domain Entity before saving.

### Testing Domain Logic

If your Use Case calculates values (e.g., password hashing or tax calculation), your assertions should verify that the output of the Use Case reflects those calculations.

### Mocking Behavior

Avoid mocking logic. If a helper class has complex logic, it might be better to use a real instance or move that logic into a Domain Service that is also tested independently.

---

## 5. Summary Checklist

* [ ] Does the test cover the "Happy Path"?
* [ ] Does the test cover validation failures?
* [ ] Does the test cover infrastructure failures (e.g., Database down)?
* [ ] Are we verifying that the Repository was called with the correct arguments?

Would you like me to show you how to test a Use Case that involves **multiple** repository calls or external API integrations?