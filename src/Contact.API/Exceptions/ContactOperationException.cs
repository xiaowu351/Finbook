using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Exceptions
{
    public class ContactOperationException:Exception
    {
        public ContactOperationException() : base() { }
        public ContactOperationException(string message) : base(message)
        {
        }
    }
}
