using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TweetsAPI.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Email { get; set; }

    }

    public class UserCredential
    {
        public string UserName { get; set; }

        public string Password { get; set; }
        public string Email { get; set; }
    }
}
