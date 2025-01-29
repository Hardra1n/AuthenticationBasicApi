using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InnerLogicException : DomainException
    {
        public InnerLogicException() : base() { }

        public InnerLogicException(string message) : base(message) { }

        public InnerLogicException(string message,  Exception innerException) : base(message, innerException) { }
    }
}
