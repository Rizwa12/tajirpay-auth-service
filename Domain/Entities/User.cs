using Domain.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Email { get; set; }
        public string? Password { get;  set; }
        public string? Name { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        public string? EmailVerificationToken { get; set; }
        public DateTime? EmailVerificationTokenExpiry { get; set; }
        public User()
        {
            
        }
        public User(string email, string password, string name)
        {
            Email = email;
            Password = password;
            Name = name;
        }
        public void UpdateName(string newName)
        {
            Name = newName;
        }

        public void GenerateEmailVerificationToken()
        {
            EmailVerificationToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(24); // expires in 24 hours
        }

        public void VerifyEmail(string token)
        {
            if (EmailVerificationToken != token || EmailVerificationTokenExpiry < DateTime.UtcNow)
                throw new AuthException("Invalid or expired email verification token.");

            IsEmailVerified = true;
            EmailVerificationToken = null;
            EmailVerificationTokenExpiry = null;
        }
    }
}
