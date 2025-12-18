using CORE.APP.Models;

namespace Users.APP.Features.Auth
{
    public class RegisterResponse : CommandResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        
        public RegisterResponse(bool isSuccessful, string message) : base(isSuccessful, message)
        {
        }
    }
}