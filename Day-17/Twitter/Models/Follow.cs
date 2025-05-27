using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Twitter.Models
{
    public class Follow
    {
        [Key]
        public int Id { get; set; }
        public int FollowerId { get; set; }

        public int FollowingId { get; set; }

        [ForeignKey("FollowerId")]
        [InverseProperty("Following")]
        public User? Follower { get; set; }

        [ForeignKey("FollowingId")]
        [InverseProperty("Followers")]
        public User? Following { get; set; }
    }
}
