using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CafeT.BusinessObjects
{
    public struct Address
    {
        public string Number;
        public string Street;
        public string City;
        public string Country;
    }

    public class Company:BaseObject
    {
        public string Name { set; get; }
        public string Introduction { set; get; }
        public string Website { set; get; }
        public string Phone { set; get; }
        public string Fax { set; get; }
        
        public Address Address { set; get; }
        public Company():base()
        {
        }
    }
}