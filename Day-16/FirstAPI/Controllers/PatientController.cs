using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/[controller]")]

public class PatientController : ControllerBase
{
    static List<Patient> patients = new List<Patient>
    {
        new Patient{Id=201, Name="Harish", Age=30},
        new Patient{Id=202, Name="vijay", Age=25},
        new Patient{Id=203, Name="Suresh", Age=40},
    };

    [HttpGet]
    public ActionResult<IEnumerable<Patient>> GetPatients()
    {
        return Ok(patients);
    }

    [HttpPost]
    public ActionResult<Patient> PostPatient([FromBody] Patient patient)
    {
        if (patient.Name.Length < 2)
        {
            return BadRequest("Name must be at least 2 characters.");
        }
        if (patient.Age <= 0 || patient.Age > 120)
        {
            return BadRequest("Age must be between 1 and 120.");
        }

        if (patients.Any(p => p.Id == patient.Id))
        {
            return Conflict($"Patient with ID {patient.Id} already exists.");
        }

        patients.Add(patient);
            return Created("", patient);
    }

    [HttpPut("{id}")]
    public ActionResult<Patient> PutPatient(int id, [FromBody] Patient patient)
    {
        var existingPatient = patients.FirstOrDefault(p => p.Id == id);
        if (existingPatient == null)
        {
            return NotFound($"Patient with ID {id} not found.");
        }

        if (string.IsNullOrWhiteSpace(patient.Name))
        {
            return BadRequest("Name is required.");
        }
        if (patient.Age <= 0 || patient.Age > 120)
        {
            return BadRequest("Age must be between 1 and 120.");
        }

        existingPatient.Name = patient.Name;
        existingPatient.Age = patient.Age;

        return Ok(existingPatient);
    }

    [HttpDelete("{id}")]
    public ActionResult DeletePatient(int id)
    {
        var patient = patients.FirstOrDefault(p => p.Id == id);
        if (patient == null)
        {
            return NotFound($"Patient with ID {id} not found.");
        }
        patients.Remove(patient);
        return Ok("Patient deleted successfully");
    }
}