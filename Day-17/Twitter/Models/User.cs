using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Twitter.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public ICollection<Tweet>? Tweets { get; set; }
        public ICollection<Like>? Likes { get; set; }
        
        [InverseProperty("Follower")]
        public ICollection<Follow>? Following { get; set; }

        [InverseProperty("Following")]
        public ICollection<Follow>? Followers { get; set; }   
    }
}
