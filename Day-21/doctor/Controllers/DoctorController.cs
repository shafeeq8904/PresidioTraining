
using System.Threading.Tasks;
using doctor.Interfaces;
using doctor.Models;
using doctor.Models.DTOs.DoctorSpecialities;
using doctor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace doctor.Controllers
{


    [ApiController]
    [Route("/api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<IEnumerable<DoctorsBySpecialityResponseDto>>> GetDoctors(string speciality)
        {
            var result = await _doctorService.GetDoctorsBySpeciality(speciality);
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<Doctor>> PostDoctor([FromBody] DoctorAddRequestDto doctor)
        {
            try
            {
                var newDoctor = await _doctorService.AddDoctor(doctor);
                if (newDoctor != null)
                    return Created("", newDoctor);
                return BadRequest("Unable to process request at this moment");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}