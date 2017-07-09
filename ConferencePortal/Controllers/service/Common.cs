using Postal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferencePortal.Controllers.service
{
    public class Common
    {
        conferencedbEntities en = new conferencedbEntities();
        public EmailConfiguration GetEmailConfiguration(int ConventionID)
        {
            return (EmailConfiguration)en.EmailConfigurations.Where(w => w.ConventionID == ConventionID);
        }

        public string GenerateRandomPassword()
        {
            return System.Web.Security.Membership.GeneratePassword(25, 2);
        }

        public void SendEmail(EmailModel email)
        {
            // Pay Later
            if (email.EmailTemplete == "")
            {
                dynamic _email = new Email(email.EmailTemplete);
                _email.To = email.EmailTo; 
                _email.From = email.EmailFrom;
                _email.ClientName = email.ClientName;
                _email.BookingRef = email.BookingRef;
                _email.Cc = email.EmailCC;
                _email.Bcc = email.EmailBCC;
                _email.UserName = email.UserName;
                _email.Password = email.Password;
                _email.LoginURL = email.LoginURL;
            }
            // Invoice
            else if (email.EmailTemplete == "")
            {
                dynamic _email = new Email(email.EmailTemplete);
                _email.To = email.EmailTo;
                _email.From = email.EmailFrom;
                _email.ClientName = email.ClientName;
                _email.BookingRef = email.BookingRef;
                _email.Cc = email.EmailCC;
                _email.Bcc = email.EmailBCC;
                _email.Amount = email.Amount;
            }
        }
    }

    public class EmailModel
    {
        public string EmailTo { get; set; }
        public string EmailFrom { get; set; }
        public string ClientName { get; set; }
        public string EmailCC { get; set; }
        public string EmailBCC { get; set; }
        public string BookingRef { get; set; }
        public string EmailTemplete { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string LoginURL { get; set; }
        public string Amount { get; set; }
    }
}