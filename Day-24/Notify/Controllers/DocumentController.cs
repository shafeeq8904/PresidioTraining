using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Notify.DTOs;
using Notify.Hubs;
using Notify.Interfaces;
using Notify.Models;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Notify.Controllers
{
    [Authorize(Roles = "HRAdmin")]
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentRepository _documentRepo;
        private readonly IUserRepository _userRepo;
        private readonly IHubContext<NotificationHub> _hub;

        public DocumentController(IDocumentRepository documentRepo, IUserRepository userRepo, IHubContext<NotificationHub> hub)
        {
            _documentRepo = documentRepo;
            _userRepo = userRepo;
            _hub = hub;
        }

        [HttpPost("upload")]
        [Authorize(Roles = "HRAdmin,User")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var username = User.FindFirstValue(ClaimTypes.Name);
            var uploader = await _userRepo.GetByUsernameAsync(username);

            var document = new Document
            {
                FileName = fileName,
                FilePath = $"uploads/{fileName}",
                UploadedById = uploader.Id,
                UploadedAt = DateTime.UtcNow
            };

            await _documentRepo.Add(document);

            await _hub.Clients.All.SendAsync("ReceiveNotification", $"New document uploaded: {file.FileName}");

            return Ok("File uploaded and notification sent.");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllDocuments()
        {
            var documents = await _documentRepo.GetAll();
            return Ok(documents);
        }
    }
}
