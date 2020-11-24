using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using MimeKit;
using MailKit;
using MailKit.Net.Smtp;

namespace PAC.Models
{
    public class Courriel
    {
        private MimeMessage message;

        public Courriel(string nomDestinataire,string courielDestinataire, string sujet, string contenue)
        {
            message = new MimeMessage();
            message.From.Add(new MailboxAddress("Project Mail", "projetDICJ@cegepjonquiere.ca"));

            message.To.Add(new MailboxAddress(nomDestinataire, courielDestinataire));

            message.Subject = sujet;

            message.Body= new TextPart("Plain") {
                Text = contenue};
        }

        public void Envoyer()
        {
            try 
            { 
            using(var client = new MailKit.Net.Smtp.SmtpClient()){


                try
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.ConnectAsync("smtp.gmail.com", 587, false);
                    client.Authenticate("projetDICJ@cegepjonquiere.ca", "DICJ2020+");
                    client.Send(message);
                    client.Disconnect(true); 
                }
                catch (Exception)
                {

                }
            }
            }
            catch (Exception)
            {

            }
        }
    }
}
