using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Ultility
{
    public class LoginModel
    {
        public Value value { get; set; }
        public List<object> formatters { get; set; }
        public List<object> contentTypes { get; set; }
        public object declaredType { get; set; }
        public int statusCode { get; set; }
    }

    public class Value
    {
        public string token { get; set; }
        public DateTime expiration { get; set; }
    }
}
