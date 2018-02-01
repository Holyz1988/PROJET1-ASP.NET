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
    public class PartageController : Controller
    {
        //Récupère le sondage avec le guid, ce qui permet de sécuriser la page contenant les URLs
        public ActionResult page_url(string id)
        {
            return View("page_url", DAL.GetSondageByGuid(id));
        }

        public ActionResult Vote(int? id)
        {
            if (id == null)
            {
                return new HttpNotFoundResult();
            }

            Sondage monSondage;
            List<Reponse> mesReponse;

            try
            {
                monSondage = DAL.GetSondageById((int)id);
                mesReponse = DAL.GetAllReponse((int)id);
            }
            catch (Exception)
            {
                return new HttpNotFoundResult();
            }

            if (monSondage.Actif)
            {
                return View("page_vote", new SondageAvecReponse(monSondage, mesReponse));
            }
            else
            {
                return Redirect("/Partage/Resultat/" + id);
            }
        }

        public ActionResult VoteSondage(List<int> choixReponse, int? id)
        {
            //Renvoi un 404 si l'id est null
            if (id == null)
            {
                return new HttpNotFoundResult();
            }
            //On prend en compte le vote sinon

            List<Reponse> mesReponse;
            Sondage monSondage;

            try
            {
                monSondage = DAL.GetSondageById((int)id);
                mesReponse = DAL.GetAllReponse((int)id);
            }
            catch (Exception)
            {
                return new HttpNotFoundResult();
            }

            //Si un choix de réponse a été séléctionné, on prend en compte le vote
            if (monSondage.Actif)
            {
                if (choixReponse == null)
                {
                    return View("Error_choix_multiple");
                }

                HttpCookie cookie = Request.Cookies["SondageCookie" + id];
                if (cookie == null)
                {
                    // Cookie non trouvé, on en crée un nouveau
                    cookie = new HttpCookie("SondageCookie" + id);
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    // redirige vers la page de résultats
                    return View("erreur_cookie", id);
                }

                foreach (var choix in choixReponse)
                {
                    mesReponse[(int)choix].NbVoteReponse++;
                    DAL.UpdateNombreVoteReponse(mesReponse[(int)choix]);
                }
                //On récupère le sondage et on incrémente le nombre de votant max
                monSondage.NbVotant++;
                DAL.UpdateNombreVotant(monSondage);
                SondageAvecReponse monSondageVote = new SondageAvecReponse(monSondage, mesReponse);

                return Redirect(String.Format("Resultat/{0}", id));
            }

            return Redirect(String.Format("Resultat/{0}", id));
        }

        public ActionResult DesactiverSondage(string id)
        {
            if (id == null)
            {
                return new HttpNotFoundResult();
            }

            Sondage monSondage;
            try
            {
                monSondage = DAL.GetSondageByGuid(id);
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

        public ActionResult Resultat(int? id)
        {
            if (id == null)
            {
                return new HttpNotFoundResult();
            }
            else
            {
                Sondage monSondage;
                List<Reponse> mesReponse;

                try
                {
                    monSondage = DAL.GetSondageById((int)id);
                    mesReponse = DAL.GetReponseOrderedById((int)id);
                }
                catch (Exception)
                {
                    return new HttpNotFoundResult();
                }

                SondageAvecReponse SondageComplet = new SondageAvecReponse(monSondage, mesReponse);

                return View("page_resultat", SondageComplet);
            }
        }
    }
}