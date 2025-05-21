using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cardiologist.Exceptions
{
    public class DuplicateEntityException : Exception
    {
        private readonly string _message;

        public DuplicateEntityException(string msg = "Duplicate entity found")
        {
            _message = msg;
        }

        public override string Message => _message;
    }
}
