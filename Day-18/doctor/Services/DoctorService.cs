using doctor.Interfaces;
using doctor.Models;
using doctor.Models.DTOs.DoctorSpecialities;

namespace doctor.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IRepository<int, Doctor> _doctorRepository;
        private readonly IRepository<int, Speciality> _specialityRepository;
        private readonly IRepository<int, DoctorSpeciality> _doctorSpecialityRepository;

        public DoctorService(IRepository<int, Doctor> doctorRepository,
                             IRepository<int, Speciality> specialityRepository,
                             IRepository<int, DoctorSpeciality> doctorSpecialityRepository)
        {
            _doctorRepository = doctorRepository;
            _specialityRepository = specialityRepository;
            _doctorSpecialityRepository = doctorSpecialityRepository;
        }

        public async Task<Doctor> AddDoctor(DoctorAddRequestDto doctorDto)
        {
            var doctor = new Doctor
            {
                Name = doctorDto.Name,
                YearsOfExperience = doctorDto.YearsOfExperience
            };

            var addedDoctor = await _doctorRepository.Add(doctor);

            if (doctorDto.Specialities != null)
            {
                foreach (var specDto in doctorDto.Specialities)
                {
                    var speciality = (await _specialityRepository.GetAll())
                        .FirstOrDefault(s => s.Name.ToLower() == specDto.Name.ToLower());

                    if (speciality == null)
                    {
                        speciality = await _specialityRepository.Add(new Speciality
                        {
                            Name = specDto.Name,
                            Status = "Active"
                        });
                    }

                    await _doctorSpecialityRepository.Add(new DoctorSpeciality
                    {
                        DoctorId = addedDoctor.Id,
                        SpecialityId = speciality.Id
                    });
                }
            }

            return addedDoctor;
        }

        public async Task<Doctor> GetDoctByName(string name)
        {
            var doctors = await _doctorRepository.GetAll();
            var doctor = doctors.FirstOrDefault(d => d.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            return doctor ?? throw new Exception("Doctor not found with the given name");
        }

        public async Task<ICollection<Doctor>> GetDoctorsBySpeciality(string specialityName)
        {
            var allSpecialities = await _specialityRepository.GetAll();
            var speciality = allSpecialities.FirstOrDefault(s => s.Name.Equals(specialityName, StringComparison.OrdinalIgnoreCase));

            if (speciality == null)
                throw new Exception("Speciality not found");

            var doctorSpecialities = await _doctorSpecialityRepository.GetAll();
            var matchingDoctors = doctorSpecialities
                .Where(ds => ds.SpecialityId == speciality.Id)
                .Select(ds => ds.Doctor)
                .Where(d => d != null)
                .Distinct()
                .ToList();

            return matchingDoctors!;
        }
    }
}
