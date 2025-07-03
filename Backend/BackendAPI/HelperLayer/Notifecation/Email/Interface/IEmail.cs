using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLayer.Notifecation.Email.Interface
{
    public interface IEmail
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string body);
    }
}
