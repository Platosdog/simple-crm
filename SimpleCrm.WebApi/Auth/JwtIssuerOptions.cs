using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace SimpleCrm.WebApi.Auth
{
    public class JwtIssuerOptions
    {
        public DateTime IssuedAt { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
        public DateTime ExpirationTime => IssuedAt.AddMinutes(ValidFor);
        public int ValidFor { get; set; }
        public Func<Task<string>> JtiGenerator => () => Task.FromResult(Guid.NewGuid().ToString());
    }
}

