using System.Security.Claims;
using System.Text;
using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.Domain.Entities.User;
using dotnet_notification_service.Features.Auth.Domain.Repositories;
using Funcky.Monads;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace dotnet_notification_service.Features.Auth.Infra.Repositories;

using TokenOrFailure = Either<Failure, string>;

public class TokenRepository(IOptions<JwtOptions> options) : ITokenRepository
{
    public Task<TokenOrFailure> Generate(UserEntity user)
    {
        try
        {
            string secretKey = options.Value.Key;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    [
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email.Value),
                        new Claim(JwtRegisteredClaimNames.EmailVerified, "true")
                    ]
                ),
                Expires = DateTime.UtcNow.AddMinutes(options.Value.ExpiresMinutes),
                SigningCredentials = credentials,
                Issuer = options.Value.Issuer,
                Audience = options.Value.Audience,
            };

            var handler = new JsonWebTokenHandler();

            string token = handler.CreateToken(tokenDescriptor);
            
            return Task.FromResult(TokenOrFailure.Right(token));
        }
        catch (Exception e)
        {
            var failure = new ServerFailure
            {
                Message = "Failed to generate token: " + e.Message,
            };
            return Task.FromResult( TokenOrFailure.Left(failure));
        }
        
        
    }
}