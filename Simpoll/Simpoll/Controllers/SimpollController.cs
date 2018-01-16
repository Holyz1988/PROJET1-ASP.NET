using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Simpoll.Models;
using System.Data.SqlClient;

namespace Simpoll.Controllers
{
    public class SimpollController : Controller
    {
        private const string SqlConnectionString = @"Server=.\SQLExpress;Initial Catalog=Simpoll; Trusted_Connection=Yes";
        // GET: Simpoll
        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult CreationSondage(int IdCreateur, string question, string choix1, string choix2, string choix3, string typeChoix)
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

            Sondage newSondage = new Sondage(question, choix_multiple, IdCreateur);
            int IDSondage = InsererSondageEnBdd(newSondage);

            List<Reponse> mesReponses = new List<Reponse>();
            Reponse maReponse1 = new Reponse(IDSondage);
            maReponse1.IntituleReponse = choix1;
            Reponse maReponse2 = new Reponse(IDSondage);
            maReponse2.IntituleReponse = choix2;
            Reponse maReponse3 = new Reponse(IDSondage);
            maReponse3.IntituleReponse = choix3;

            mesReponses.Add(maReponse1);
            mesReponses.Add(maReponse2);
            mesReponses.Add(maReponse3);

            foreach(var reponse in mesReponses)
            {
                InsererReponseEnBdd(reponse);
            }

            if (typeChoix == "choix_unique")
            {
                return View("page_url", newSondage);
            }
            else
            {
                return View("page_url", newSondage);
            }

        }

        public ActionResult CreationUtilisateur()
        {
            return View("page_creation_utilisateur");
        }

        public ActionResult RecupInfoUser(string nomCreateur, string prenomCreateur, string emailCreateur)
        {
            Createur monCreateur = new Createur(nomCreateur, prenomCreateur, emailCreateur);
            monCreateur.IdCreateur = InsererUtilisateurEnBdd(monCreateur);
            return View("Index", monCreateur);
        }

        public int InsererUtilisateurEnBdd(Createur unCreateur)
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand maCommande = new SqlCommand(@"INSERT INTO Createur(NomCreateur, PrenomCreateur, EmailCreateur) VALUES (@nom, @prenom, @email); SELECT SCOPE_IDENTITY()", connexion);
            maCommande.Parameters.AddWithValue("@nom", unCreateur.NomCreateur);
            maCommande.Parameters.AddWithValue("@prenom", unCreateur.PrenomCreateur);
            maCommande.Parameters.AddWithValue("@email", unCreateur.EmailCreateur);
            int IdCreateur = Convert.ToInt32(maCommande.ExecuteScalar());

            connexion.Close();

            return IdCreateur;
        }

        public int InsererSondageEnBdd(Sondage unSondage)
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand maCommande = new SqlCommand(@"INSERT INTO Sondage(QuestionSondage, ChoixMultiple, NbVotant, FKIdCreateur) VALUES (@question, @choix_multiple, @nb_votant, @FK_idCreateur); SELECT SCOPE_IDENTITY()", connexion);
            maCommande.Parameters.AddWithValue("@question", unSondage.QuestionSondage);
            maCommande.Parameters.AddWithValue("@choix_multiple", unSondage.ChoixMultiple);
            maCommande.Parameters.AddWithValue("@nb_votant", unSondage.NbVotant);
            maCommande.Parameters.AddWithValue("@FK_idCreateur", unSondage.FKIdCreateur);
            
            int IdSondage = Convert.ToInt32(maCommande.ExecuteScalar());

            connexion.Close();

            return IdSondage;
        }

        public int InsererReponseEnBdd(Reponse maReponse)
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand maCommande = new SqlCommand("INSERT INTO Reponse(IntituleReponse, NbVoteReponse, FKIdSondage) VALUES (@choix, @nb_vote_reponse, @FK_idSondage); SELECT SCOPE_IDENTITY()", connexion);
            maCommande.Parameters.AddWithValue("@choix", maReponse.IntituleReponse);
            maCommande.Parameters.AddWithValue("@nb_vote_reponse", maReponse.NbVoteReponse);
            maCommande.Parameters.AddWithValue("@FK_idSondage", maReponse.FKIdSondage);
            int IdReponse = Convert.ToInt32(maCommande.ExecuteScalar());

            connexion.Close();

            return IdReponse;
        }
    }
}