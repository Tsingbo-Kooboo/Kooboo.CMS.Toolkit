using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Mail;

using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Toolkit
{
    public class EmailUtility
    {
        public static void Send(string from, string to, string subject, string body)
        {
            Send(from, from, to, to, subject, body);
        }

        public static void Send(string from, string[] to, string subject, string body)
        {
            Send(from, from, to, subject, body);
        }

        public static void Send(string from, string fromName, string to, string toName, string subject, string body)
        {
            Send(new MailAddress(from, fromName), new MailAddress(to, toName), subject, body);
        }

        public static void Send(string from, string fromName, string replyTo, string replyToName, string to, string toName, string subject, string body)
        {
            Send(new MailAddress(from, fromName), new MailAddress(replyTo, replyToName), new MailAddress(to, toName), subject, body);
        }

        public static void Send(string from, string fromName, string[] to, string subject, string body)
        {
            MailAddressCollection toList = new MailAddressCollection();
            foreach (string item in to)
            {
                toList.Add(new MailAddress(item));
            }

            Send(new MailAddress(from, fromName), null, toList, subject, body);
        }

        public static void Send(string from, string fromName, string replyTo, string replyToName, string[] to, string subject, string body)
        {
            MailAddressCollection toList = new MailAddressCollection();
            foreach (string item in to)
            {
                toList.Add(new MailAddress(item));
            }

            Send(new MailAddress(from, fromName), new MailAddress(replyTo, replyToName), toList, subject, body);
        }

        public static void Send(MailAddress from, MailAddress to, string subject, string body)
        {
            Send(from, null, to, subject, body);
        }

        public static void Send(MailAddress from, MailAddress replyTo, MailAddress to, string subject, string body)
        {
            MailAddressCollection toList = new MailAddressCollection();
            toList.Add(to);

            Send(from, replyTo, toList, subject, body);
        }

        public static void Send(MailAddress from, MailAddress replyTo, MailAddressCollection to, string subject, string body)
        {
            MailMessage message = new MailMessage();
            message.From = from;
            if (replyTo != null)
            {
                message.ReplyToList.Add(replyTo);
            }

            foreach (var item in to)
            {
                message.To.Add(item);
            }

            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            Send(message);
        }

        public static void Send(MailMessage message)
        {
            SmtpClient smtpClient = new SmtpClient();
            Smtp smtpSetting = Site.Current.Smtp;
            if (smtpSetting != null)
            {
                smtpClient.Host = smtpSetting.Host;
                smtpClient.Port = smtpSetting.Port;
                smtpClient.EnableSsl = smtpSetting.EnableSsl;
                if (!String.IsNullOrEmpty(smtpSetting.UserName) && !String.IsNullOrEmpty(smtpSetting.Password))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(smtpSetting.UserName, smtpSetting.Password);
                }
            }

            smtpClient.Send(message);
        }
    }
}