using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GrpcServer
{
    public class MainServerException : Exception
    {
        public MainServerException(string message) : base(message) { }
        public MainServerException(string message, Exception innerException) : base(message, innerException) { }
        public MainServerException() { }

        public string Content { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public ErrorHttpMessage? ErrorHttpMessage { get; set; } 
    }
}
