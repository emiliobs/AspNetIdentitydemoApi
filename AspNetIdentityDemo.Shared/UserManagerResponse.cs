using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetIdentityDemo.Shared
{
    public class UserManagerResponse
    {
        public string Message { get; set; }
        public bool Issuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
