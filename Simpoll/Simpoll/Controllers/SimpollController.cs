using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Simpoll.Models;
using System.Web.UI.WebControls;

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
            if (choix.Count() < 2) // l'utilisateur dois saisir au moins deux choix de reponses
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
            Sondage monSondage = DAL.GetSondageById(idSondage);
            List<Reponse> mesReponse = DAL.GetAllReponse(idSondage);

            SondageAvecReponse SondageComplet = new SondageAvecReponse(monSondage, mesReponse);

            if(!monSondage.ChoixMultiple)
            {
                return View("vote_choix_unique", SondageComplet);
            }
            else
            {
                return View("vote_choix_multiple", SondageComplet);
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
            
                return View("page_resultat", monSondageVote);
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

                return View("page_resultat", monSondageVote);
            }
        }

        public ActionResult DesactiverSondage(string myGuid)
        {
            Sondage monSondage = DAL.GetSondageByGuid(myGuid);

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
    }
}