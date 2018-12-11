using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API
{
    public class UserOperationException : Exception
    {
        public UserOperationException():base() { }
        public UserOperationException(string message) : base(message)
        {
        }
    }
}
