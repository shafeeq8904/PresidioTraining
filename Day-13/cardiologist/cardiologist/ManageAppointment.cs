using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cardiologist.Interfaces;
using cardiologist.Models;

namespace cardiologist
{
    public class ManageAppointment
    {
        private readonly IAppointmentService _appointmentService;

        public ManageAppointment(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public void Start()
        {
            int choice;
            do
            {
                Console.WriteLine("\n--- Cardiologist Appointment System ---");
                Console.WriteLine("1. Add Appointment");
                Console.WriteLine("2. Search Appointments");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");
                int.TryParse(Console.ReadLine(), out choice);

                switch (choice)
                {
                    case 1:
                        AddAppointment();
                        break;
                    case 2:
                        SearchAppointments();
                        break;
                    case 3:
                        Console.WriteLine("Exiting...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

            } while (choice != 3);
        }

        private void AddAppointment()
        {
            var appointment = new Appointment();

            Console.Write("Enter Patient Name: ");
            appointment.PatientName = Console.ReadLine() ?? "";

            int age;
            Console.Write("Enter Patient Age: ");
            while (!int.TryParse(Console.ReadLine(), out age) || age < 0)
                Console.Write("Invalid. Enter valid Patient Age: ");
            appointment.PatientAge = age;

            DateTime dateTime;
            Console.Write("Enter Appointment Date & Time (yyyy-MM-dd HH:mm): ");
            while (!DateTime.TryParse(Console.ReadLine(), out dateTime))
                Console.Write("Invalid format. Try again: ");
            appointment.AppointmentDate = dateTime;

            Console.Write("Enter Reason for Visit: ");
            appointment.Reason = Console.ReadLine() ?? "";

            int id = _appointmentService.AddAppointment(appointment);
            if (id > 0)
                Console.WriteLine($"Appointment added with ID: {id}");
            else
                Console.WriteLine("Failed to add appointment.");
        }

        private void SearchAppointments()
        {
            var searchModel = new AppointmentSearchModel();

            Console.Write("Enter Patient Name to search (or press Enter to skip): ");
            var name = Console.ReadLine();
            searchModel.PatientName = string.IsNullOrWhiteSpace(name) ? null : name;

            Console.Write("Enter Appointment Date (yyyy-MM-dd) to search (or press Enter to skip): ");
            var dateStr = Console.ReadLine();
            if (DateTime.TryParse(dateStr, out DateTime date))
                searchModel.AppointmentDate = date;

            Console.Write("Enter Min Age (or press Enter to skip): ");
            var minAgeStr = Console.ReadLine();
            Console.Write("Enter Max Age (or press Enter to skip): ");
            var maxAgeStr = Console.ReadLine();

            if (int.TryParse(minAgeStr, out int minAge) && int.TryParse(maxAgeStr, out int maxAge))
            {
                searchModel.AgeRange = new Range<int> { MinVal = minAge, MaxVal = maxAge };
            }

            var results = _appointmentService.SearchAppointments(searchModel);
            if (results != null && results.Count > 0)
            {
                Console.WriteLine("\n--- Matching Appointments ---");
                foreach (var a in results)
                {
                    Console.WriteLine($"\nID: {a.Id}\nName: {a.PatientName}\nAge: {a.PatientAge}\nDate: {a.AppointmentDate}\nReason: {a.Reason}");
                }
            }
            else
            {
                Console.WriteLine("No appointments found.");
            }
        }
    }
}
