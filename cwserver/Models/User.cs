using System.Collections.Generic;
using Nancy.Security;
using ServiceStack.DataAnnotations;

namespace cwserver.Models {
    public class User : IUserIdentity {
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Pass { get; set; }

        [Required]
        [StringLength(100)]
        public string Guid { get; set; }

        [Required]
        [Index(Unique = true)]
        [StringLength(100)]
        public string UserName { get; set; }

        public IEnumerable<string> Claims { get; set; } = new List<string>();
    }
}
