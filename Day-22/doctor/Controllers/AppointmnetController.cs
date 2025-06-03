using doctor.Interfaces;
using doctor.Models;
using doctor.Repositories;
using doctor.Models.DTOs;
using doctor.Services;
using doctor.Models.DTOs.AppointmnetAddRequestDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace doctor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmnetController : ControllerBase
    {
        private readonly IAppointmnetService _appointmnetService;
        private readonly IRepository<string, Appointmnet> _appointmnetRepository;
        private readonly IRepository<int, Doctor> _doctorRepository;
        private readonly AppointmnetMapper _appointmnetMapper = new AppointmnetMapper();
        private readonly ILogger<AppointmnetController> _logger;

        public AppointmnetController(
            IAppointmnetService appointmnetService,
            IRepository<string, Appointmnet> appointmnetRepository,
            IRepository<int, Doctor> doctorRepository,
            ILogger<AppointmnetController> logger)
        {
            _appointmnetService = appointmnetService;
            _appointmnetRepository = appointmnetRepository;
            _doctorRepository = doctorRepository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddAppointment([FromBody] AppointmnetAddRequestDto dto)
        {
            try
            {
                var result = await _appointmnetService.AddAppointmnet(dto);
                var response = _appointmnetMapper.MapToResponseDto(result);
                return CreatedAtAction(nameof(GetAppointmentsByPatientId), new { patientId = result.PatientId }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding appointment: {ex.Message}");
                return StatusCode(500, "Failed to add appointment");
            }
        }


        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetAppointmentsByPatientId(int patientId)
        {
            try
            {
                var appointments = await _appointmnetService.GetAppointmnetsByPatientId(patientId);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving appointments: {ex.Message}");
                return StatusCode(500, "Failed to retrieve appointments");
            }
        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetAppointmentsByDoctorId(int doctorId)
        {
            try
            {
                var appointments = await _appointmnetService.GetAppointmnetsByDoctorId(doctorId);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving appointments: {ex.Message}");
                return StatusCode(500, "Failed to retrieve appointments");
            }
        }

        [HttpPut("cancel/{appointmentNumber}")]
        public async Task<IActionResult> CancelAppointment(
            string appointmentNumber,
            [FromServices] IAuthorizationService authorizationService)
        {
            try
            {
                var appointment = await _appointmnetRepository.Get(appointmentNumber);
                if (appointment == null)
                    return NotFound("Appointment not found");

                // Resource-based authorization
                var authResult = await authorizationService.AuthorizeAsync(User, appointment, "ExperiencedDoctorOnly");
                if (!authResult.Succeeded)
                    return Forbid();

                appointment.Status = "Cancelled";
                await _appointmnetRepository.Update(appointmentNumber, appointment);

                return Ok("Appointment cancelled successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error cancelling appointment: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
