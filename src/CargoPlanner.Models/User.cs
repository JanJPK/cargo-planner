using System;

namespace CargoPlanner.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public string PasswordEncryption { get; set; }
    }
}