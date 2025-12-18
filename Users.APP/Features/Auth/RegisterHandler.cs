using CORE.APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.Authentication;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Users.APP.Domain;

namespace Users.APP.Features.Auth
{
    public class RegisterHandler : Service<User>, IRequestHandler<RegisterRequest, RegisterResponse>
    {
        private readonly DbContext _db;
        private readonly ITokenAuthService _tokenAuthService;
        private readonly IConfiguration _configuration;
        
        public RegisterHandler(
            DbContext db,
            ITokenAuthService tokenAuthService,
            IConfiguration configuration
        ) : base(db)
        {
            _db = db;
            _tokenAuthService = tokenAuthService;
            _configuration = configuration;
        }

        public async Task<RegisterResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            // 1) Username unique check
            var usernameExists = await _db.Set<User>()
                .AnyAsync(u => u.UserName == request.UserName, cancellationToken);

            if (usernameExists)
                return new RegisterResponse(false, "UserName already exists.");

            var age = CalculateAge(request.BirthDate);
            var groupTitle = age < 18 ? "Child" : "Adult";

            var group = await _db.Set<Group>()
                .SingleOrDefaultAsync(g => g.Title == groupTitle, cancellationToken);

            if (group is null)
                return new RegisterResponse(false, $"Group '{groupTitle}' not found. Seed Groups first (Child/Adult).");


            var role = await _db.Set<Role>()
                .SingleOrDefaultAsync(r => r.Name == "Customer", cancellationToken)
                ?? await _db.Set<Role>().SingleOrDefaultAsync(r => r.Name == "User", cancellationToken);

            if (role is null)
                return new RegisterResponse(false, "Default role not found. Seed Roles first (Customer/User).");

            // 4) Create user with default fields
            var user = new User
            {
                UserName = request.UserName,
                Password = request.Password, 
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,

                Gender = request.Gender,
                BirthDate = request.BirthDate,

                RegistrationDate = DateTime.Now,
                Score = 0m,
                IsActive = true,

                GroupId = group.Id
            };

            // 5) Assign default role
            user.UserRoles.Add(new UserRole
            {
                RoleId = role.Id
            });
            user.RefreshToken = _tokenAuthService.GetRefreshToken();
            user.RefreshTokenExpiration = DateTime.Now.AddDays(7);

            // 6) Save
            await Create(user, cancellationToken);
            
            var expiration = DateTime.Now.AddMinutes(5);

            var tokenResponse = _tokenAuthService.GetTokenResponse(
                user.Id,
                user.UserName,
                user.UserRoles.Select(ur => ur.Role.Name).ToArray(),
                expiration,
                _configuration["SecurityKey"],
                _configuration["Issuer"],
                _configuration["Audience"],
                _tokenAuthService.GetRefreshToken()
            );

            return new RegisterResponse(true, "User registered and authorized successfully.")
            {
                Token = tokenResponse.Token,
                RefreshToken = tokenResponse.RefreshToken
            };
        }

        private static int CalculateAge(DateOnly birthDate)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age))
                age--;
            return age;
        }
    }
}