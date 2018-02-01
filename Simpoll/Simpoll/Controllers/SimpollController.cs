using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Simpoll.Models;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;

namespace Simpoll.Controllers
{
    public class SimpollController : Controller
    {
        public ActionResult CreationUtilisateur()
        {
            return View("page_creation_utilisateur");
        }

        public ActionResult FormulaireUtilisateur(string nomCreateur, string prenomCreateur, string emailCreateur)
        {
            Createur monCreateur = new Createur(nomCreateur, prenomCreateur, emailCreateur);

            try
            {
                monCreateur.IdCreateur = DAL.AddUtilisateur(monCreateur);
            }
            catch (Exception)
            {
                return new HttpNotFoundResult();
            }

            return Redirect(String.Format("/Simpoll/page_creation_sondage/{0}", monCreateur.IdCreateur));

        }

        public ActionResult CreationSondage(int? idCreateur, string question, List<string> choix, string typeChoix)
        {
            bool choix_multiple = false;

            if ((string)typeChoix == "choix_multiple")
            {
                choix_multiple = true;
            }

            //Gestion de la saisie utilisateur
            //NullRéférence Exception
            try
            {
                choix.RemoveAll(element => (string.IsNullOrWhiteSpace(element))); //Parcour la liste et retire les chaines vide ou null ou les espace
            }
            catch (Exception)
            {
                return new HttpNotFoundResult();
            }

            if (choix.Count() < 2) // l'utilisateur doit saisir au moins deux choix de reponses
            {
                return View("erreur_nb_reponse", idCreateur);
            }

            if (string.IsNullOrWhiteSpace((string)question)) // L'utilisateur doit saisir la question
            {
                return View("erreur_question", idCreateur);
            }

            //On crée un ID unique que l'on rentre en DB
            Guid guid = Guid.NewGuid();
            string myGuid = Convert.ToString(guid);
            Sondage newSondage = new Sondage((string)question, choix_multiple, (int)idCreateur, myGuid);

            int idSondage;

            try
            {
                idSondage = DAL.AddSondage(newSondage);
            }
            catch (Exception)
            {
                return new HttpNotFoundResult();
            }

            //TODO : revoir la partie localhost
            newSondage.UrlPartage = @"localhost:8870/Partage/Vote/" + Convert.ToString(idSondage);
            newSondage.UrlSuppression = @"localhost:8870/Partage/DesactiverSondage/" + Convert.ToString(myGuid);
            newSondage.UrlResultat = @"localhost:8870/Partage/Resultat/"+ Convert.ToString(idSondage);

            //On met les URL en DB
            DAL.UpdateSondage(newSondage, idSondage);

            List<Reponse> mesReponses = new List<Reponse>();

            //On enlève les inputs vide
            foreach (var c in choix)
            {
                if (!(string.IsNullOrEmpty(c)))
                {
                    mesReponses.Add(new Reponse(idSondage, c));
                }
            }

            foreach (var reponse in mesReponses)
            {
                DAL.AddReponse(reponse);
            }

            Createur monCreateur = DAL.GetCreateurById(idSondage);

            if (!string.IsNullOrWhiteSpace(monCreateur.EmailCreateur))
            {
                EnvoiMail(newSondage, monCreateur);
            }

            return Redirect(String.Format("/Partage/page_url/{0}", newSondage.Guid));
        }

        public ActionResult page_creation_sondage(int id)
        {
            return View("page_creation_sondage", id);
        }               

        public void EnvoiMail(Sondage sondage, Createur createur)
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

            MailMessage message = new MailMessage(EmailEnvoyeur, createur.EmailCreateur, objet, htmlBody);
            message.IsBodyHtml = true;

            smtp.Send(message);
        }
    }
}