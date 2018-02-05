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
        /* Action qui récupère le sondage avec le guid, ce qui permet de sécuriser la page
        contenant les URLs
        */
        public ActionResult page_url(string id)
        {
            return View("page_url", DAL.GetSondageByGuid(id));
        }

        /*Action permettant de récupérer les donnés du sondage la page de vote
        et afficher la vue de la page avec l'objet SondageAvecReponse, et renvoi
        vers une page d'erreur si le sondage est désactivé
        */
        public ActionResult Vote(int? id)
        {
            if (id == null)
            {
                return new HttpNotFoundResult();
            }

            Sondage monSondage;
            List<Reponse> mesReponse;

            /*Le try catch permet d'éviter que le serveur plante si l'utilisateur
            Tappe n'importe quoi dans l'URL
            */
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
                return View("disabled_sondage", monSondage);
            }
        }

        /*Action qui permet de prendre en compte le vote que l'utilisateur à choisi.
        Cette méthode fonctionne pour les choix multiples ainsi que les choix uniques.
        */
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
                    return Redirect(string.Format("PageErreurReponse/{0}", id));
                }

                /*On test si le cookie existe. Si il existe, on renvoi vers la vue
                 * qui prévient que l'utilisateur a déja voté. Sinon on renvoi vers
                 la page de résultat et on comptabilise le vote.
                 */
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

                //On récupère le sondage et on incrémente le nombre de votant max.
                //Cette opération est également réalisable directement en base de donnée.
                monSondage.NbVotant++;
                DAL.UpdateNombreVotant(monSondage);

                return Redirect(String.Format("Resultat/{0}", id));
            }

            //Sinon on renvoi la vue du sondage désactivé
            return Redirect(String.Format("ConfirmationInactif/{0}", monSondage.Guid));
        }

        /* Action permettant de désactiver le sondage
        */
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

                return View("disable_sondage", monSondage);
            }
            else
            {
                return View("disabled_sondage", monSondage);
            }
        }

        /* Action permettant l'affichage des résulats. Il suffit de
        récupérer le SondageComplet en DB et l'envoyer en paramètre 
        avec la vue résultat.
        */
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

        /*Action qui permet d'afficher la vue qui annonce que l'utilisateur
        qui ne choisit aucune réponse lors du vote. 
        */
        public ActionResult PageErreurReponse(int? id)
        {
            if (id == null)
            {
                return new HttpNotFoundResult();
            }

            return View("erreur_vote", id);
        }
    }
}