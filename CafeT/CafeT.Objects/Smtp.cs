using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeT.Objects
{
    public class Smtp
    {
        //SMTP parameters
        public static string SmtpAdress = "smtp.mail.yahoo.com";
        public static int PortNumber = 587;
        public static bool EnableSSL = true;
        //need it for the secured connection
        public static string From = "sender email";
        public static string Password = "password of the above email";
    }

}
