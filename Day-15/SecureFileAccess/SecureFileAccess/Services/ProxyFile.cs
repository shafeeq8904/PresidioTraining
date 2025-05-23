using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecureFileAccess.Interfaces;
using SecureFileAccess.Models;

namespace SecureFileAccess.Services
{
    public class ProxyFile : IFile
    {
        private readonly File _realFile;
        private readonly User _user;

        public ProxyFile(User user)
        {
            _user = user;
            _realFile = new File();
        }

        public void Read()
        {
            switch (_user.Role.ToLower())
            {
                case "admin":
                    _realFile.Read();
                    break;

                case "user":
                    _realFile.ReadMetadata();
                    break;

                case "guest":
                    Console.WriteLine("[Access Denied] You do not have permission to read this file.");
                    break;

                default:
                    Console.WriteLine("[Access Denied] Unknown role.");
                    break;
            }
        }
    }
}
