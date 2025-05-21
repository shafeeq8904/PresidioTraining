using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cardiologist.Exceptions
{
    public class CollectionEmptyException : Exception
    {
        private readonly string _message;

        public CollectionEmptyException(string msg = "Collection is empty")
        {
            _message = msg;
        }

        public override string Message => _message;
    }
}
