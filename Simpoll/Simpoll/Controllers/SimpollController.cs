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
        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult CreationUtilisateur()
        {
            return View("page_creation_utilisateur");
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
                //On renvoit une exception ?
                return View("erreur_zero_reponse");
            }

            if (string.IsNullOrWhiteSpace((string)question)) // L'utilisateur doit saisir la question
            {
                return View("erreur_question");
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
            newSondage.UrlPartage = @"localhost:8870/Simpoll/Vote?idSondage=" + Convert.ToString(idSondage);
            newSondage.UrlSuppression = @"localhost:8870/Simpoll/DesactiverSondage?myGuid=" + Convert.ToString(myGuid);
            newSondage.UrlResultat = @"localhost:8870/Simpoll/Resultat?idSondage=" + Convert.ToString(idSondage);

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

            //On met les réponses en DB
            foreach (var reponse in mesReponses)
            {
                DAL.AddReponse(reponse);
            }

            Createur monCreateur = DAL.GetCreateurById(idSondage);

            //Envoie un mail au createur du sondage

            if (!string.IsNullOrWhiteSpace(monCreateur.EmailCreateur))
            {
                EnvoiMail(newSondage, monCreateur);
            }

            return View("page_url", DAL.GetSondageById(idSondage));
        }

        public ActionResult RecupInfoUtilisateur(string nomCreateur, string prenomCreateur, string emailCreateur)
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

            return View("Index", monCreateur);
        }

        public ActionResult Vote(int? idSondage)
        {
            if (idSondage == null)
            {
                return new HttpNotFoundResult();
            }
            else
            {
                Sondage monSondage;
                List<Reponse> mesReponse;

                try
                {
                    monSondage = DAL.GetSondageById((int)idSondage);
                    mesReponse = DAL.GetAllReponse((int)idSondage);
                }
                catch (Exception)
                {
                    return new HttpNotFoundResult();
                }

                SondageAvecReponse SondageComplet = new SondageAvecReponse(monSondage, mesReponse);

                if (monSondage.Actif)
                {
                    if (!monSondage.ChoixMultiple)
                    {
                        return View("vote_choix_unique", SondageComplet);
                    }
                    else
                    {
                        return View("vote_choix_multiple", SondageComplet);
                    }
                }
                else
                {
                    return Redirect("/Simpoll/Resultat?idSondage=" + idSondage);
                }
            }
        }

        public ActionResult VoteSondageUnique(int? choixReponse, int? idSondage)
        {
            //Renvoi un 404 si l'id est null
            if (idSondage == null)
            {
                return new HttpNotFoundResult();
            }
            //On prend en compte le vote sinon

            List<Reponse> mesReponse;

            try
            {
                mesReponse = DAL.GetAllReponse((int)idSondage);
            }
            catch (Exception)
            {
                return new HttpNotFoundResult();
            }

            //Si un choix de réponse a été séléctionné, on prend en compte le vote
            if (choixReponse.HasValue)
            {
                HttpCookie cookie = Request.Cookies["SondageCookie" + idSondage];
                if (cookie == null)
                {
                    // Cookie non trouvé, on en crée un nouveau
                    cookie = new HttpCookie("SondageCookie" + idSondage);
                    cookie.Values["SondageId"] = idSondage.ToString();
                }
                else
                {
                    // redirige vers la page de résultats
                    return View("erreur_cookie");
                }

                Response.Cookies.Add(cookie);

                mesReponse[(int)choixReponse].NbVoteReponse++;
                DAL.UpdateNombreVoteReponse(mesReponse[(int)choixReponse]);

                //On récupère le sondage et on incrémente le nombre de votant max
                Sondage monSondage = DAL.GetSondageById((int)idSondage);
                monSondage.NbVotant++;
                DAL.UpdateNombreVotant(monSondage);

                SondageAvecReponse monSondageVote = new SondageAvecReponse(monSondage, mesReponse);

                return View("page_resultat", monSondageVote);
            }

            return View("Error_choix_multiple");
        }

        public ActionResult VoteSondageMultiple(List<int> choixReponse, int? idSondage)
        {
            if (idSondage == null)
            {
                return new HttpNotFoundResult();
            }

            List<Reponse> mesReponse = DAL.GetAllReponse((int)idSondage);

            if (choixReponse == null)
            {
                return View("Error_choix_multiple");
            }

            HttpCookie cookie = Request.Cookies["SondageCookie" + idSondage];
            if (cookie == null)
            {
                // Cookie non trouvé, on en crée un nouveau
                cookie = new HttpCookie("SondageCookie" + idSondage);
                cookie.Values["SondageId"] = idSondage.ToString();
            }
            else
            {
                // redirige vers la page de résultats
                return View("erreur_cookie");
            }

            Response.Cookies.Add(cookie);
            //Si la checkbox est coché, on valide en DB
            foreach (var choix in choixReponse)
            {
                mesReponse[choix].NbVoteReponse++;
                DAL.UpdateNombreVoteReponse(mesReponse[choix]);
            }
            Sondage monSondage = DAL.GetSondageById((int)idSondage);

            //On incrémente le nombre de votant
            monSondage.NbVotant++;
            DAL.UpdateNombreVotant(monSondage);
            SondageAvecReponse monSondageVote = new SondageAvecReponse(monSondage, mesReponse);

            return Redirect("Resultat?idSondage=" + idSondage);

        }


        public ActionResult DesactiverSondage(string myGuid)
        {
            Sondage monSondage;

            if (myGuid == null)
            {
                return new HttpNotFoundResult();
            }

            try
            {
                monSondage = DAL.GetSondageByGuid(myGuid);
            }

            catch (Exception)
            {
                return new HttpNotFoundResult();
            }

            //Test si le sondage est actif ou non
            if (monSondage.Actif)
            {
                monSondage.Actif = false;
                DAL.DisableSondage(monSondage);
                return View("disable_sondage");
            }
            else
            {
                return View("disabled_sondage");
            }

        }

        public ActionResult Resultat(int? idSondage)
        {
            if (idSondage == null)
            {
                return new HttpNotFoundResult();
            }
            else
            {
                Sondage monSondage;
                List<Reponse> mesReponse;

                try
                {
                    monSondage = DAL.GetSondageById((int)idSondage);
                    mesReponse = DAL.GetReponseOrderedById((int)idSondage);
                }
                catch (Exception)
                {
                    return new HttpNotFoundResult();
                }

                SondageAvecReponse SondageComplet = new SondageAvecReponse(monSondage, mesReponse);

                return View("page_resultat", SondageComplet);
            }
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