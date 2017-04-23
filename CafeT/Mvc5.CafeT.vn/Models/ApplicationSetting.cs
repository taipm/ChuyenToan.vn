using CafeT.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5.CafeT.vn.Models
{
    public class ApplicationSetting : BaseObject
    {
        public string Title { set; get; }
        public string Url { set; get; }
        public string Name { set; get; }
        public string PathLogo { set; get; }
        public string Metas { set; get; }
        public string UploadFolder { set; get; }

        public string DbServerIp { set; get; }
        public string DbInstanceName { set; get; }
        public string DbUserName { set; get; }
        public string DbPassword { set; get; }

        public string EmailSmtp { set; get; }
        public string EmailUser { set; get; }
        public string EmailPassword { set; get; }
        public int EmailPort { set; get; }
        public bool EmailEnableSSL { set; get; }

        public virtual IEnumerable<ApplicationMessage> Messages { set; get; }

        public ApplicationSetting() : base()
        {
            //Email
            EmailSmtp = "smtp.gmail.com";
            EmailUser = "taipm.vn@gmail.com";
            EmailPassword = "P@$$w0rdPMT789";
            EmailPort = 587;
            EmailEnableSSL = true;

            //Application
            Url = "http://cafet.vn";
            Name = "CafeT.vn";
            Title = "Cộng đồng nghiên cứu và ứng dụng công nghệ";
            Metas = "CafeT.vn, ChuyenTin.vn, ChuyenToan.vn, ChuyenLy.vn, ChuyenHoa.vn"
                + "Cộng đồng, Nghiên cứu, Ứng dụng, Công nghệ thông tin, ELearning, C#, Arduino, ...";

        }
    }
}