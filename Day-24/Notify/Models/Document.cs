namespace Notify.Models
{
    public class Document
    {
        public int Id { get; set; }
        
        public string FileName { get; set; }
        
        public string FilePath { get; set; }
        
        public DateTime UploadedAt { get; set; }
        
        public int UploadedById { get; set; }
        
        public User UploadedBy { get; set; } 
    }
}