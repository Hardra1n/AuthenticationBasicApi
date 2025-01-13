using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InnerLogicExeption : DomainException
    {
        public InnerLogicExeption() { }

        public InnerLogicExeption(string message) : base(message) { }

        public InnerLogicExeption(string message,  Exception innerException) : base(message, innerException) { }
    }
}
