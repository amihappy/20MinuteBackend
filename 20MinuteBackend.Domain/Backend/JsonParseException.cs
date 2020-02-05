using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20MinuteBackend.Domain.Backend
{
    public class JsonParseException : Exception
    {
        public JsonParseException(Exception ex) : base(ex.Message, ex)
        {
        }
    }
}
