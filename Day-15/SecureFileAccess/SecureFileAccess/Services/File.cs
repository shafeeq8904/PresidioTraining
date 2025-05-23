using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecureFileAccess.Interfaces;

namespace SecureFileAccess.Services
{
    public class File : IFile
    {
        public void Read()
        {
            Console.WriteLine("[Access Granted] Reading sensitive file content...");
        }

        public void ReadMetadata()
        {
            Console.WriteLine("[Access Limited] Reading file metadata only.");
        }
    }
}
