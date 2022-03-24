using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpulTechTest.Response
{
    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public string ResponseType { get; set; }

        public string Message { get; set; }
        public T Result { get; set; }

        public Response()
        {

        }
        public Response(bool _issuccess, string _responsetype, string _message)
        {
            this.IsSuccess = _issuccess;
            this.ResponseType = _responsetype;
            this.Message = _message;
        }

        public override string ToString()
        {
            return $"Response result is {ResponseType} : {Message}";
        }
    }
}
