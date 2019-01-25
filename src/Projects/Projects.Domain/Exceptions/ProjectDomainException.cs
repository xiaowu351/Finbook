using System;
using System.Collections.Generic;
using System.Text;

namespace Projects.Domain.Exceptions
{
    public class ProjectDomainException : Exception
    {
        public ProjectDomainException() : base() { }
        public ProjectDomainException(string message) : base(message)
        {
        }

        public ProjectDomainException(string message,Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
