This README guide explains how to implement unit tests for API Controllers that follow the **Clean Architecture** pattern, where business logic is encapsulated in **Use Cases** (Interactors) and results are handled using the **Result Pattern** (via the `Funcky.Monads` library).

---

# Testing Controllers with Use Cases

In our architecture, Controllers are thin wrappers. They don't contain logic; they simply receive a request, call a Use Case, and map the functional result (`Either<Failure, Success>`) into an HTTP response.

### Core Testing Workflow

The goal is to verify that the Controller correctly handles both successful and failed outcomes from the application layer.

---

## 1. Prerequisites

To follow this pattern, your test project needs:

* **xUnit**: The testing framework.
* **Moq**: To create mock instances of your Use Case interfaces.
* **Funcky**: To handle the `Either` monad results.

---

## 2. Test Structure: Arrange, Act, Assert

### Arrange: Mocking the Use Case

Since we are unit testing the **Controller**, we must isolate it from the actual business logic. We use `Moq` to define how the Use Case should behave for a specific test case.

**For Success:**

```csharp
MockUseCase
    .Setup(uc => uc.CallAsync(It.IsAny<CreateUserCommand>()))
    .ReturnsAsync(Either<Failure, CreateUserResult>.Right(expectedResult));

```

**For Failure:**

```csharp
MockUseCase
    .Setup(uc => uc.CallAsync(It.IsAny<CreateUserCommand>()))
    .ReturnsAsync(Either<Failure, CreateUserResult>.Left(failure));

```

### Act: Execute the Endpoint

Simply call the controller method directly. Since most Use Cases are I/O bound, these calls are typically `async`.

```csharp
var result = await controller.CreateUser(MockCreateUserRequest);

```

### Assert: Verify HTTP Responses

We use `Assert.IsType<T>` to verify that the returned `ActionResult` matches the expected HTTP status code.

| Use Case Result | Expected HTTP Action | ASP.NET Core Type |
| --- | --- | --- |
| **Right** (Success) | `201 Created` or `200 OK` | `CreatedResult` / `OkObjectResult` |
| **Left** (Failure) | `400 Bad Request` | `BadRequestObjectResult` |

---

## 3. Example Implementation

The following example demonstrates testing an `AuthController` that consumes an `ICreateUserUseCase`.

```csharp
[Fact]
public async Task CreateUser_ShouldReturnCreated_WhenUseCaseSucceeds()
{
    // 1. ARRANGE
    var expectedResult = new CreateUserResult("token", "userId");
    
    MockUseCase
        .Setup(uc => uc.CallAsync(It.IsAny<CreateUserCommand>()))
        .ReturnsAsync(Either<Failure, CreateUserResult>.Right(expectedResult));

    var controller = new AuthController(MockUseCase.Object);

    // 2. ACT
    var result = await controller.CreateUser(MockCreateUserRequest);

    // 3. ASSERT
    var createdResult = Assert.IsType<CreatedResult>(result.Result);
    var value = Assert.IsType<CreateUserResponse>(createdResult.Value);
    Assert.Equal("token", value.Token);
}

```

---

## 4. Key Takeaways

* **Mock Interfaces, not Classes**: Always inject `IMock<IUseCase>` to ensure your tests are decoupled from implementation.
* **Test the Mapping**: Ensure that the data inside the `Either.Right` is correctly transformed into the DTO returned by the API.
* **Handle Failures**: Always write a test case for `Either.Left` to ensure your API returns the correct error object and status code to the client.

Would you like me to create a base test class to help reduce the boilerplate code for mocking these Use Cases?