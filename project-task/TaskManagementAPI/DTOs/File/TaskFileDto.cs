using System;

namespace TaskManagementAPI.DTOs.TaskFiles
{
    public class TaskFileDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
    }
}
