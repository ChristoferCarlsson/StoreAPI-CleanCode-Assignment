using Application.Authentication.Commands;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Authentication.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IConfiguration _configuration;

        public LoginCommandHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Dummy logic for login - no real user validation
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            // Proceed to generate a token for testing
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.Role, "User"), // Or any role you'd like to assign
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), // Token expiration time
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Task.FromResult(tokenString);
        }
    }
}
