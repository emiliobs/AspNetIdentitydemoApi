using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetIdentitydemoApi.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
    }   
}
