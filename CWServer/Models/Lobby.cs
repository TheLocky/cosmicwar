using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CWServer.Models {
    public class Lobby {
        public enum State {
            Wait,
            Active,
            Inactive
        }

        public int Id { get; set; }

        [Required]
        public User Owner { get; set; }

        [Required]
        public List<User> Players { get; set; } = new List<User>();

        [Required]
        public State Status { get; set; }
    }
}
