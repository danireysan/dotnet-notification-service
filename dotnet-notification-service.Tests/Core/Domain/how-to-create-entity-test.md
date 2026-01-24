Testing **Domain Entities** is the most important part of Unit Testing. Since Entities contain the core business rules (invariants), these tests ensure that your system remains consistent regardless of the database or UI being used.

In a Clean Architecture setup, Entity tests focus on **behavior** and **validation** rather than just properties.

---

# Testing Domain Entities

Domain Entities should encapsulate their own logic. We test them to ensure that an entity can never enter an "invalid state."

### Key Areas to Test

* **Constructors:** Does the entity initialize correctly with valid data?
* **Invariants/Validations:** Does it throw an exception or return a failure if rules are broken (e.g., empty email, negative price)?
* **Business Methods:** Do methods that change state (e.g., `Deactivate()`, `UpdateEmail()`) work as expected?
* **Equality:** Does the entity correctly identify itself based on its Unique ID?

---

## 1. Testing Valid Initialization

The first test should always ensure that a valid set of inputs creates a valid object.

```csharp
[Fact]
public void User_ShouldInitializeCorrectly_WhenDataIsValid()
{
    // Arrange
    var email = "test@example.com";
    var password = "SecurePassword123";

    // Act
    var user = new User(email, password);

    // Assert
    Assert.Equal(email, user.Email);
    Assert.True(user.IsActive);
    Assert.NotEqual(Guid.Empty, user.Id);
}

```

---

## 2. Testing Business Rules (Invariants)

Entities should protect their state. If your `User` entity requires a valid email, the test should prove that it's impossible to create one without it.

```csharp
[Theory]
[InlineData("")]
[InlineData("invalid-email")]
[InlineData(null)]
public void User_ShouldThrowException_WhenEmailIsInvalid(string invalidEmail)
{
    // Act & Assert
    Assert.Throws<DomainException>(() => new User(invalidEmail, "password123"));
}

```

---

## 3. Testing State Transitions

If an entity has logic to change its state, you must test the transitions.

```csharp
[Fact]
public void Deactivate_ShouldChangeStatusToInactive_WhenUserIsActive()
{
    // Arrange
    var user = new User("test@mail.com", "password");

    // Act
    user.Deactivate();

    // Assert
    Assert.False(user.IsActive);
    Assert.NotNull(user.DeactivatedAt); // Ensuring audit fields are set
}

```

---

## 4. Testing Entity Equality

In Domain-Driven Design (DDD), two entities are considered equal if their **IDs** match, even if their other properties differ.

```csharp
[Fact]
public void Entities_ShouldBeEqual_WhenIdsAreSame()
{
    // Arrange
    var id = Guid.NewGuid();
    var user1 = new User(id, "one@mail.com");
    var user2 = new User(id, "two@mail.com");

    // Assert
    Assert.Equal(user1, user2);
    Assert.True(user1 == user2);
}

```

---

## Best Practices for Entity Tests

| Rule | Description |
| --- | --- |
| **No Mocks** | Entities should never have dependencies on services or repositories. Do not use Moq here. |
| **Test Behavior, Not Setters** | Avoid testing simple `get; set;` properties. Focus on methods that perform logic. |
| **Use Theory/InlineData** | Use `[Theory]` to test multiple edge cases (like different types of invalid strings) in a single test method. |
| **Domain Exceptions** | Ensure your entities throw specific **Domain Exceptions** so the Application layer can catch and map them to `Failure` types. |

---

## Summary Checklist

* [ ] Is the entity always in a valid state after the constructor runs?
* [ ] Are all business rules (if/else logic inside the entity) covered by a test?
* [ ] Does the entity handle nulls or empty strings gracefully?
* [ ] Are date-time values (like `CreatedAt`) assigned correctly?