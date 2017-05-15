using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telegram.Net.Core;

namespace HelloEveAPI.Models
{
    public class Telegram
    {
    }

    public class SendCode_Request
    {
        public string PhoneNumber { get; set; }
    }
    public class SendCode_Response
    {
        public string PhoneHash { get; set; }
        public string Token { get; set; }
    }
    public class SignIn_Request
    {
        public string Token { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneCodeHash { get; set; }
        public string Code { get; set; }
    }
    public class SignIn_Response
    {
        public bool Successfull { get; set; }

    }
    public class SendMessage_Request
    {
        public string Token { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public int? UserId { get; set; }
    }
    public class SendMessage_Response
    {
        public int? UserId { get; set; }
        public bool Successfull { get; set; }
    }

}