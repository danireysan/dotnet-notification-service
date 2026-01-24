Testing **Repositories** is different from testing Use Cases or Entities. Since Repositories interact with an external dependency (the Database), you should **not** use Moq for these tests. Instead, you should use an **In-Memory Database** or **TestContainers** to verify that your SQL/LINQ queries actually work.

---

# Testing Repositories

Repository tests verify that your Data Access Layer correctly translates Domain Entity actions into Database state changes.

### The Strategy: Integration Testing

We use **SQLite In-Memory** or **EF Core In-Memory** to run tests against a real database schema. This ensures that:

1. Your LINQ queries are valid.
2. Database constraints (Unique, Not Null) are respected.
3. Relationships/Joins are loaded correctly.

---

## 1. Setting up the Test Context

To keep tests isolated, every test should start with a **clean, freshly created database**.

```csharp
public abstract class BaseRepositoryTest : IDisposable
{
    protected readonly NotificationDbContext Context;

    protected BaseRepositoryTest()
    {
        // Use SQLite In-Memory for high fidelity (supports relations/constraints)
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<NotificationDbContext>()
            .UseSqlite(connection)
            .Options;

        Context = new NotificationDbContext(options);
        Context.Database.EnsureCreated(); // Creates the schema
    }

    public void Dispose() => Context.Dispose();
}

```

---

## 2. Writing Repository Tests

### A. Testing Persistence (Save)

Verify that when you save an entity, it is correctly assigned an ID and persisted.

```csharp
public class UserRepositoryTests : BaseRepositoryTest
{
    [Fact]
    public async Task SaveAsync_ShouldPersistUserToDatabase()
    {
        // Arrange
        var repository = new UserRepository(Context);
        var user = new User(Email.Create("test@mail.com"), "hashed_pass");

        // Act
        await repository.SaveAsync(user);
        
        // Assert
        var savedUser = await Context.Users.FirstOrDefaultAsync(u => u.Email == "test@mail.com");
        Assert.NotNull(savedUser);
        Assert.Equal(user.Id, savedUser.Id);
    }
}

```

### B. Testing Queries (Exists/Get)

Verify that your query logic (like `Where` or `Any`) works as expected.

```csharp
[Fact]
public async Task ExistsByEmailAsync_ShouldReturnTrue_WhenUserExists()
{
    // Arrange
    var email = "check@mail.com";
    Context.Users.Add(new User(Email.Create(email), "pass"));
    await Context.SaveChangesAsync();

    var repository = new UserRepository(Context);

    // Act
    var exists = await repository.ExistsByEmailAsync(email);

    // Assert
    Assert.True(exists);
}

```

---

## 3. Testing Database Constraints

Repository tests are the perfect place to ensure your **Database Configuration** (Fluent API) matches your Domain rules.

```csharp
[Fact]
public async Task SaveAsync_ShouldThrowException_WhenEmailIsDuplicate()
{
    // Arrange
    var email = Email.Create("duplicate@mail.com");
    Context.Users.Add(new User(email, "pass1"));
    await Context.SaveChangesAsync();

    var repository = new UserRepository(Context);
    var duplicateUser = new User(email, "pass2");

    // Act & Assert
    await Assert.ThrowsAsync<DbUpdateException>(() => repository.SaveAsync(duplicateUser));
}

```

---

## 4. Best Practices for Repository Tests

| Rule | Description |
| --- | --- |
| **No Mocking DBContext** | Never use `Mock<DbContext>`. It is notoriously difficult to mock correctly and doesn't test the actual SQL generation. |
| **Avoid EF In-Memory** | Use **SQLite In-Memory** instead. EF's default "InMemory" provider behaves like a List, ignoring constraints (like `Unique`) and Foreign Keys. |
| **Cleanup** | Always ensure the database is deleted/disposed between tests to avoid data leaking from one test to another. |
| **Test "Includes"** | If your repository method is supposed to return a User with their Notifications, verify that the `Notifications` collection is actually loaded (not null). |

---

## Summary Checklist

* [ ] Are you using a real (in-memory) database instead of mocks?
* [ ] Does each test run in an isolated environment (fresh DB)?
* [ ] Are you testing the "Find" logic with different edge cases (lowercase/uppercase)?
* [ ] Are you verifying that aggregate roots save their child entities correctly?

Would you like me to show you how to use **Respawn** or **TestContainers** to run these tests against a real **Dockerized PostgreSQL/SQL Server** for even higher accuracy?