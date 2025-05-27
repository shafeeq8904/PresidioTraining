using System.ComponentModel.DataAnnotations;

namespace Twitter.Models
{
    public class Hashtag
    {
        [Key]
        public int HashtagId { get; set; }
        public string Tag { get; set; } = string.Empty;

        public ICollection<TweetHashtag>? TweetHashtags { get; set; }
    }
}
