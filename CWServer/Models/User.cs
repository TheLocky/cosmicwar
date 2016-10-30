using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Nancy.Security;

namespace CWServer.Models {
    public class User : IUserIdentity {
        public enum Claim {
            User,
            Admin
        }

        public int Id { get; set; }

        [Required]
        public string PassHash { get; set; }

        [Required]
        public string Guid { get; set; }

        [Required]
        public Claim Role { get; set; } = Claim.User;

        [Required]
        public string UserName { get; set; }

        [NotMapped]
        public IEnumerable<string> Claims => new[] {Role.ToString()};
    }
}
