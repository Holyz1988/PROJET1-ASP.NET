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

        public ActionResult CreationSondage(int idCreateur, string question, List<string> choix, string typeChoix)
        {
            bool choix_multiple = false;

            if (typeChoix == "choix_multiple")
            {
                choix_multiple = true;
            }

            //Gestion de la saisie utilisateur
            choix.RemoveAll(element => (string.IsNullOrWhiteSpace(element))); //Parcour la liste et retire les chaines vide ou null ou les espace
            if (choix.Count() < 2) // l'utilisateur doit saisir au moins deux choix de reponses
            {
                //On renvoit une exception ?
                return View("erreur_zero_reponse");
            }
            
            if (string.IsNullOrWhiteSpace(question)) // L'utilisateur doit saisir la question
            {
                return View("erreur_question");
            }

            //On crée un ID unique que l'on rentre en DB
            Guid guid = Guid.NewGuid();
            string myGuid = Convert.ToString(guid);
            Sondage newSondage = new Sondage(question, choix_multiple, idCreateur, myGuid);
            int idSondage = DAL.AddSondage(newSondage);

            //TODO : revoir la partie localhost
            newSondage.UrlPartage = @"localhost:8870/Simpoll/Vote?idSondage=" + Convert.ToString(idSondage);
            newSondage.UrlSuppression = @"localhost:8870/Simpoll/DesactiverSondage?myGuid=" + Convert.ToString(myGuid);
            newSondage.UrlResultat = @"localhost:8870/Simpoll/Resultat?idSondage=" + Convert.ToString(idSondage);

            //On met les URL en DB
            DAL.UpdateSondage(newSondage, idSondage);

            List<Reponse> mesReponses = new List<Reponse>();

            //On enlève les inputs vide
            foreach(var c in choix)
            {
                if(!(string.IsNullOrEmpty(c)))
                {
                    mesReponses.Add(new Reponse(idSondage, c));
                }
            }

            //On met les réponses en DB
            foreach(var reponse in mesReponses)
            {
                DAL.AddReponse(reponse);
            }

            Createur monCreateur = DAL.GetCreateurById(idSondage);

            //Envoie un mail au createur du sondage
            //TODO tester si l'envoie de mail fonctionne au campus
            

            string EmailEnvoyeur = "simpoll.sondage@gmail.com";
            string EmailReception = monCreateur.EmailCreateur ;
            string password = "Simpoll68@";
            string objet = "Votre sondage a été crée ";
       
            string htmlBody = @"<!doctype html>
                                    <html>
                                        <headers>
                                            <img src=""https://image.noelshack.com/fichiers/2018/04/6/1517067469-simpoll.png"" alt=""Logo Simpoll""/>
                                        </headers>
                                        <h1>Merci " + monCreateur.PrenomCreateur + @" d'avoir choisi Simpoll pour créer votre sondage</h1>
                                        <h2>Voici vos 3 liens :</h2>
                                            <ul>
                                                <li>" + newSondage.UrlPartage + @"</li>
                                                <li>" + newSondage.UrlResultat + @"</li>
                                                <li>" + newSondage.UrlSuppression + @"</li>
                                            </ul>
                                        <p>La team Simpoll vous dit à bientôt pour de nouveaux sondage !!</p>";
                                        
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 25,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(EmailEnvoyeur, password),

            };

            MailMessage message = new MailMessage(EmailEnvoyeur, EmailReception, objet, htmlBody);
            message.IsBodyHtml = true;

            smtp.Send(message);     

            return View("page_url", DAL.GetSondageById(idSondage));
        }

        public ActionResult RecupInfoUtilisateur(string nomCreateur, string prenomCreateur, string emailCreateur)
        {
            Createur monCreateur = new Createur(nomCreateur, prenomCreateur, emailCreateur);
            monCreateur.IdCreateur = DAL.AddUtilisateur(monCreateur);
            return View("Index", monCreateur);
        }

        public ActionResult Vote(int idSondage)
        {
            HttpCookie cookie = Request.Cookies["SondageCookie" + idSondage];
            if (cookie == null )
            {
                // Cookie non trouvé, on en crée un nouveau
                cookie = new HttpCookie("SondageCookie" + idSondage);
                cookie.Values["SondageId"] = idSondage.ToString();
            }
            else
            {
                // redirige vers la page de résultats
                return Redirect("/Simpoll/Resultat?idSondage=" + idSondage);
            }

            Response.Cookies.Add(cookie);

            Sondage monSondage = DAL.GetSondageById(idSondage);
            List<Reponse> mesReponse = DAL.GetAllReponse(idSondage);

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

        public ActionResult VoteSondageUnique(int? choixReponse, int idSondage)
        { 
            List<Reponse> mesReponse = DAL.GetAllReponse(idSondage);

            if(choixReponse.HasValue)
            {
                mesReponse[(int)choixReponse].NbVoteReponse++;
                DAL.UpdateNombreVoteReponse(mesReponse[(int)choixReponse]);

                //On récupère le sondage et on incrémente le nombre de votant max
                Sondage monSondage = DAL.GetSondageById(idSondage);
                monSondage.NbVotant++;
                DAL.UpdateNombreVotant(monSondage);

                SondageAvecReponse monSondageVote = new SondageAvecReponse(monSondage, mesReponse);
            
                return Redirect("Resultat?idSondage=" + idSondage);
            }
            else
            {
                return Redirect("Vote?idSondage=" + idSondage);
            }
        } 

        public ActionResult VoteSondageMultiple(List<int> choixReponse, int idSondage)
        {
            //https://www.productiveedge.com/blog/asp-net-mvc-checkboxfor-explained/
            List<Reponse> mesReponse = DAL.GetAllReponse(idSondage);

            if (choixReponse == null)
            {
                return View("Error_choix_multiple");
            }
            else
            {
                //Si la checkbox est coché, on valide en DB
                foreach (var choix in choixReponse)
                {
                    mesReponse[choix].NbVoteReponse++;
                    DAL.UpdateNombreVoteReponse(mesReponse[choix]);
                }
                Sondage monSondage = DAL.GetSondageById(idSondage);

                //On incrémente le nombre de votant
                monSondage.NbVotant++;
                DAL.UpdateNombreVotant(monSondage);
                SondageAvecReponse monSondageVote = new SondageAvecReponse(monSondage, mesReponse);

                return Redirect("Resultat?idSondage=" + idSondage);
            }
        }

        public ActionResult DesactiverSondage(string myGuid)
        {
            Sondage monSondage = DAL.GetSondageByGuid(myGuid);

            //Test si le sondage est actif ou non
            if(monSondage.Actif)
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

        public ActionResult Resultat(int idSondage)
        {
            Sondage monSondage = DAL.GetSondageById(idSondage);
            List<Reponse> mesReponse = DAL.GetReponseOrderedById(idSondage);

            SondageAvecReponse SondageComplet = new SondageAvecReponse(monSondage, mesReponse);

            return View("page_resultat", SondageComplet);
        }

       
        
    }
}