using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoFromImage
{
    public class RegClass
    {
        public class Result
        {
            public bool MemberRegisterVerifySwitch { get; set; }
            public int AvailableMinutes { get; set; }
            public Token Token { get; set; }
        }

        public class Root
        {
            public List<object> Error { get; set; }
            public int Code { get; set; }
            public Result Result { get; set; }
            public DateTime ReplyTime { get; set; }
        }

        public class Token
        {
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
        }
    }
}
