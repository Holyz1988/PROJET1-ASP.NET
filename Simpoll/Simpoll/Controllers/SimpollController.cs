using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Simpoll.Models;
using System.Web.

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

            if (typeChoix == "choix_unique")
            {
                choix_multiple = false;
            }
            else
            {
                choix_multiple = true;
            }

            ////////////////////////////////////////////////
            //////Gestion de la saisie utilisateur//////////
            //////contenu des unputs de choix////////////
            ////////////////////////////////////////////////
            choix.RemoveAll(element => (string.IsNullOrEmpty(element)));           
            if (choix.Count() < 2)
            {
                return View("erreur_zero_reponse");
            }

            ////////////////////////////////////////////////
            //////Gestion de la saisie utilisateur//////////
            //////contenu de l'input question////////////
            ////////////////////////////////////////////////

            if (string.IsNullOrEmpty(question))
            {
                return View("erreur_question");
            }

            Guid guid = Guid.NewGuid();
            string myGuid = Convert.ToString(guid);
            
            Sondage newSondage = new Sondage(question, choix_multiple, idCreateur, myGuid);
            int idSondage = DAL.AddSondage(newSondage);     

            newSondage.UrlPartage = @"localhost:8870/Simpoll/Vote?idSondage=" + Convert.ToString(idSondage);
            newSondage.UrlSuppression = @"localhost:8870/Simpoll/DesactiverSondage?myGuid=" + Convert.ToString(myGuid);
            newSondage.UrlResultat = @"localhost:8870/Simpoll/Resultat?idSondage=" + Convert.ToString(idSondage);

            DAL.UpdateSondage(newSondage, idSondage);

            List<Reponse> mesReponses = new List<Reponse>();
            foreach(var c in choix)
            {
                if(!(string.IsNullOrEmpty(c)))
                {
                    mesReponses.Add(new Reponse(idSondage, c));
                }
            }

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
        public ActionResult VoteSondageUnique(string choixReponse, int idSondage)
        {
            List<Reponse> mesReponse = DAL.GetAllReponse(idSondage);

            //On incrémente le compteur de la réponse qui à été choisi
            switch(choixReponse)
            {
                case "choix1":
                    mesReponse[0].NbVoteReponse++;
                    DAL.UpdateNombreVoteReponse(mesReponse[0]);
                    break;
                case "choix2":
                    mesReponse[1].NbVoteReponse++;
                    DAL.UpdateNombreVoteReponse(mesReponse[1]);
                    break;
                case "choix3":
                    mesReponse[2].NbVoteReponse++;
                    DAL.UpdateNombreVoteReponse(mesReponse[2]);
                    break;
                default:
                    break;
            }

            Sondage monSondage = DAL.GetSondageById(idSondage);

            //On incrémente le nombre de votant
            monSondage.NbVotant++;
            DAL.UpdateNombreVotant(monSondage);

            SondageAvecReponse monSondageVote = new SondageAvecReponse(monSondage, mesReponse);
            
            return View("page_resultat", monSondageVote);
        } 
        public ActionResult VoteSondageMultiple(bool? choixReponse1, bool? choixReponse2, bool? choixReponse3, int idSondage)
        {
            List<Reponse> mesReponse = DAL.GetAllReponse(idSondage);
            if(choixReponse1 == null && choixReponse2 == null && choixReponse3 == null)
            {
                return View("Error_choix_multiple");
            }

            if (choixReponse1 != null)
            { 
                mesReponse[0].NbVoteReponse++;
                DAL.UpdateNombreVoteReponse(mesReponse[0]);
            }

            if (choixReponse2 != null)
            {
                mesReponse[1].NbVoteReponse++;
                DAL.UpdateNombreVoteReponse(mesReponse[1]);
            }

            if (choixReponse3 != null)
            {
                mesReponse[2].NbVoteReponse++;
                DAL.UpdateNombreVoteReponse(mesReponse[2]);
            }

            Sondage monSondage = DAL.GetSondageById(idSondage);

            //On incrémente le nombre de votant
            monSondage.NbVotant++;
            DAL.UpdateNombreVotant(monSondage);

            SondageAvecReponse monSondageVote = new SondageAvecReponse(monSondage, mesReponse);


            return View("page_resultat", monSondageVote);
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