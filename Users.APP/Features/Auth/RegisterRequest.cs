using CORE.APP.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;
using Users.APP.Domain;

namespace Users.APP.Features.Auth
{
    /// <summary>
    /// Register request for creating a new User.
    /// GroupId will be assigned automatically based on BirthDate (child/adult).
    /// </summary>
    public class RegisterRequest : IRequest<RegisterResponse>
    {
        [Required, StringLength(30)]
        public string UserName { get; set; }

        [Required, StringLength(15)]
        public string Password { get; set; }

        // BirthDate is required in registration (even though entity allows null),
        // because we need it to compute child/adult group.
        [Required]
        public DateOnly BirthDate { get; set; }

        // Entity requires a non-null Gender (enum), so request should provide it too.
        [Required]
        public Genders Gender { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(250)]
        public string Address { get; set; }
    }
}