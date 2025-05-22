using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeApplication.Solid
{
    //good code
    public class Report
    {
        public string Title { get; set; }

        public void Generate()
        {
            Console.WriteLine("Generating Report: " + Title);
        }
    }

    public class ReportSaver
    {
        public void Save(Report report)
        {
            Console.WriteLine("Saving Report to file...");
        }
    }

    public class ReportMailer
    {
        public void SendEmail(Report report)
        {
            Console.WriteLine("Sending Report via Email...");
        }
    }

    //bad code
    /*
    public class Report
    {
        public string Title { get; set; }

        public void GenerateReport()
        {
            Console.WriteLine("Generating Report: " + Title);
        }

        public void SaveToFile()
        {
            Console.WriteLine("Saving Report to file...");
        }

        public void EmailReport()
        {
            Console.WriteLine("Sending Report via Email...");
        }
    }
    */

}
