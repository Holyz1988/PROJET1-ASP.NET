using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;

namespace Simpoll.Models
{
    public class Methodes
    {
        public static void EnvoiMail(Sondage sondage, Createur createur)
        {
            string EmailEnvoyeur = "simpoll.sondage@gmail.com";
            string password = "Simpoll68@";
            string objet = "Votre sondage a été crée ";

            string htmlBody = @"<!doctype html>
                                            <html>
                                                <body>
                                                <headers>
                                                    <img src=""https://image.noelshack.com/fichiers/2018/05/2/1517339614-simpoll.png"" alt=""Logo Simpoll""/>
                                                </headers>
                                                <div>
                                                <h2>Merci " + createur.PrenomCreateur + @" d'avoir choisi Simpoll pour créer votre sondage</h2>
                                                </div>
                                                <p>Voici vos 3 liens :</p>
                                                    <ul>
                                                        <li>" + sondage.UrlPartage + @"</li>
                                                        <li>" + sondage.UrlResultat + @"</li>
                                                        <li>" + sondage.UrlSuppression + @"</li>
                                                    </ul>
                                                <p>La team Simpoll vous dit à bientôt pour de nouveaux sondage !!</p>
                                                </body>
                                                </html>";

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 25,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(EmailEnvoyeur, password),
            };

            MailMessage message = new MailMessage(EmailEnvoyeur, createur.EmailCreateur, objet, htmlBody)
            {
                IsBodyHtml = true
            };

            smtp.Send(message);
        }

    }
}