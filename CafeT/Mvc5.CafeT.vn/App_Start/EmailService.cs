using CafeT.Text;
using Microsoft.AspNet.Identity;
using Mvc5.CafeT.vn.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace Mvc5.CafeT.vn
{
    public class EmailService : IIdentityMessageService, IDisposable
    {
        public Task SendAsync(IdentityMessage message)
        {
            MailMessage _msg = new MailMessage("taipm.vn@gmail.com", "taipm.vn@outlook.com");
            _msg.Subject = message.Subject;
            _msg.To.Add(new MailAddress(message.Destination));
            _msg.Body = message.Body;
            _msg.IsBodyHtml = true;
            
            using (SmtpClient client = new SmtpClient
            {
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential("taipm.vn@gmail.com", "P@$$w0rdPMT789")
            })
            {
                client.Send(_msg);
            }
            return Task.FromResult(0);
        }

        public Task SendAsync(IssueModel model)
        {
            MailMessage _msg = new MailMessage("taipm.vn@gmail.com", "taipm.vn@outlook.com");
            _msg.Subject = "[Issue] " + model.Title;
            if (model.CreatedBy.IsEmail())
            {
                _msg.To.Add(new MailAddress(model.CreatedBy));
                _msg.Body = model.Description;
                _msg.IsBodyHtml = true;

                using (SmtpClient client = new SmtpClient
                {
                    EnableSsl = true,
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new NetworkCredential("taipm.vn@gmail.com", "P@$$w0rdPMT789")
                })
                {
                    client.Send(_msg);
                }
            }
            return Task.FromResult(0);
        }

        public Task SendAsync(QuestionModel model)
        {
            MailMessage _msg = new MailMessage("taipm.vn@gmail.com", "taipm.vn@outlook.com");
            _msg.Subject = "[Question] - No answer " + model.Title;
            if (model.CreatedBy.IsEmail())
            {
                _msg.To.Add(new MailAddress(model.CreatedBy));
                _msg.Body = model.Content;
                _msg.IsBodyHtml = true;

                using (SmtpClient client = new SmtpClient
                {
                    EnableSsl = true,
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new NetworkCredential("taipm.vn@gmail.com", "P@$$w0rdPMT789")
                })
                {
                    client.Send(_msg);
                }
            }
            return Task.FromResult(0);
        }

        public Task SendAsync(FileModel model)
        {
            MailMessage _msg = new MailMessage("taipm.vn@gmail.com", "taipm.vn@outlook.com");
            _msg.Subject = "[File] " + model.Title;
            if (model.CreatedBy.IsEmail())
            {
                _msg.To.Add(new MailAddress(model.CreatedBy));
                _msg.Body = model.Description;
                _msg.IsBodyHtml = true;

                using (SmtpClient client = new SmtpClient
                {
                    EnableSsl = true,
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new NetworkCredential("taipm.vn@gmail.com", "P@$$w0rdPMT789")
                })
                {
                    client.Send(_msg);
                }
            }
            return Task.FromResult(0);
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}