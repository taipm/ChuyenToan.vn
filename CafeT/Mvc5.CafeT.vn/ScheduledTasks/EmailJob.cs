using Mvc5.CafeT.vn.Models;
using Mvc5.CafeT.vn.Services;
using Quartz;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Mvc5.CafeT.vn.ScheduledTasks
{    
    public class EmailJob : IJob
    {
        protected EmailService Service;
        //protected IUnitOfWorkAsync _unitOfWorkAsync;

        public void Execute(IJobExecutionContext context)
        {
            using (var message = new MailMessage("taipm.vn@gmail.com", "taipm.vn@outlook.com"))
            {
                message.Subject = "Test";
                message.Body = "Test at " + DateTime.Now;
                using (SmtpClient client = new SmtpClient
                {
                    EnableSsl = true,
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new NetworkCredential("taipm.vn@gmail.com", "P@$$w0rdPMT789")
                })
                {
                    client.Send(message);
                }
            }
        }

        public void DailySummaryEmail()
        {
            using (Service = new EmailService())
            {                
                DailySummary _summary = new DailySummary();
            }
        }
    }
}