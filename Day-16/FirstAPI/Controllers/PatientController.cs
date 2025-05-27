using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/[controller]")]
public class PatientController : ControllerBase
{
    static List<Patient> patients = new List<Patient>
    {
        new Patient{PatientId = 201, Name = "Harish", Age = 30, diagnosis = "Flu"},
        new Patient{PatientId = 202, Name = "Vijay", Age = 25, diagnosis = "Fever"},
        new Patient{PatientId = 203, Name = "Suresh", Age = 40, diagnosis = "Cold"},
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

        if (patients.Any(p => p.PatientId == patient.PatientId))
        {
            return Conflict($"Patient with ID {patient.PatientId} already exists.");
        }

        patients.Add(patient);
        return Created("", patient);
    }

    [HttpPut("{id}")]
    public ActionResult<Patient> PutPatient(int id, [FromBody] Patient patient)
    {
        var existingPatient = patients.FirstOrDefault(p => p.PatientId == id);
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
        existingPatient.diagnosis = patient.diagnosis;

        return Ok(existingPatient);
    }

    [HttpDelete("{id}")]
    public ActionResult DeletePatient(int id)
    {
        var patient = patients.FirstOrDefault(p => p.PatientId == id);
        if (patient == null)
        {
            return NotFound($"Patient with ID {id} not found.");
        }
        patients.Remove(patient);
        return Ok("Patient deleted successfully");
    }
}
