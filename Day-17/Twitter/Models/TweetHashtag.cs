using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Twitter.Models
{
    public class TweetHashtag
    {
        [Key]
        public int Id { get; set; }
        public int TweetId { get; set; }
        public int HashtagId { get; set; }

        [ForeignKey("TweetId")]
        public Tweet? Tweet { get; set; }

        [ForeignKey("HashtagId")]
        public Hashtag? Hashtag { get; set; }
    }
}
