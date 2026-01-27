using dotnet_notification_service.Features.Auth.Domain.Entities.User.ValueObjects;
using JetBrains.Annotations;

namespace dotnet_notification_service.Tests.Features.Auth.Domain.Entities.User.ValueObjects;

[TestSubject(typeof(EmailAddress))]
public class EmailAddressTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void EmailAddress_ShouldReturnFailure_WhenValueIsEmpty(string? value)
    {
        // Arrange and Act
        var emailAddress = EmailAddress.Create(value);
        // Assert
        emailAddress.Switch(
            left: Assert.NotNull,
            right: _ => { Assert.Fail("Expected a failure"); }
        );
    }
    
    [Theory]
    [InlineData("mail")]
    [InlineData("mail@")]
    [InlineData("@mail.com")]
    [InlineData("1431324")]
    public void EmailAddress_ShouldReturnFailure_WhenValueIsInvalid(string value)
    {
        // Arrange and Act
        var emailAddress = EmailAddress.Create(value);

        emailAddress.Switch(
            left: Assert.NotNull,
            right: _ => { Assert.Fail("Expected a failure"); }
        );
    }


    [Theory]
    [InlineData("daniel@gmail.com")]
    [InlineData("thing@mail.mx")]
    public void EmailAddress_ShouldReturnSameString_WhenStringIsValid(string email)
    {
        // Arrange and Act
        var emailAddress = EmailAddress.Create(email);

        emailAddress.Switch(
            left: _ => Assert.Fail("Expected a String"),
            right: mail =>
            {
                Assert.Equal(email, mail.Value);
            }
        );
    }
}