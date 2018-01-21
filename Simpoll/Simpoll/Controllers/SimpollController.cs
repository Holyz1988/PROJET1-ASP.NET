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

        public ActionResult CreationUtilisateur()
        {
            return View("page_creation_utilisateur");
        }

        public ActionResult CreationSondage(int idCreateur, string question, string choix1, string choix2, string choix3, string typeChoix)
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

            Sondage newSondage = new Sondage(question, choix_multiple, idCreateur);
            int idSondage = InsertSondage(newSondage);

            newSondage.UrlPartage = @"localhost:8870/Simpoll/Vote?idSondage=" + Convert.ToString(idSondage);
            newSondage.UrlSuppression = @"localhost:8870/Simpoll/Suppression?idSondage=" + Convert.ToString(idSondage);
            newSondage.UrlResultat = @"localhost:8870/Simpoll/Resultat?idSondage=" + Convert.ToString(idSondage);


            UpdateSondage(newSondage, idSondage);

            List<Reponse> mesReponses = new List<Reponse>();
            Reponse maReponse1 = new Reponse(idSondage, choix1);
            Reponse maReponse2 = new Reponse(idSondage, choix2);
            Reponse maReponse3 = new Reponse(idSondage, choix3);

            mesReponses.Add(maReponse1);
            mesReponses.Add(maReponse2);
            mesReponses.Add(maReponse3);

            foreach(var reponse in mesReponses)
            {
                InsertReponse(reponse);
            }

            return View("page_url", newSondage);

        }

        public ActionResult RecupInfoUser(string nomCreateur, string prenomCreateur, string emailCreateur)
        {
            Createur monCreateur = new Createur(nomCreateur, prenomCreateur, emailCreateur);
            monCreateur.IdCreateur = InsertUser(monCreateur);
            return View("Index", monCreateur);
        }

        public ActionResult Vote(int idSondage)
        {
            Sondage monSondage = GetSondageById(idSondage);
            List<Reponse> mesReponse = GetAllReponseByID(idSondage);

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

        public ActionResult VoteSondage(string choixReponse, int idSondage)
        {
            List<Reponse> mesReponse = GetAllReponseByID(idSondage);
            switch(choixReponse)
            {
                case "choix1":
                    mesReponse[0].NbVoteReponse++;
                    UpdateNombreVoteReponse(mesReponse[0]);
                    break;
                case "choix2":
                    mesReponse[1].NbVoteReponse++;
                    UpdateNombreVoteReponse(mesReponse[1]);
                    break;
                case "choix3":
                    mesReponse[2].NbVoteReponse++;
                    UpdateNombreVoteReponse(mesReponse[2]);
                    break;
                default:
                    break;
            }

            Sondage monSondage = GetSondageById(idSondage);

            monSondage.NbVotant++;
            UpdateNombreVotant(monSondage);

            SondageAvecReponse monSondageVote = new SondageAvecReponse(monSondage, mesReponse);
            
            return View("page_creation_utilisateur");
        }

        public int InsertUser(Createur unCreateur)
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

        public int InsertSondage(Sondage unSondage)
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

        public int InsertReponse(Reponse maReponse)
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
        
        public List<Reponse> GetAllReponseByID(int IdSondage)
        {
            List<Reponse> mesReponse = new List<Reponse>();
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand maCommande = new SqlCommand(@"SELECT * FROM Reponse WHERE FKIdSondage=@id_sondage", connexion);
            maCommande.Parameters.AddWithValue("@id_sondage", IdSondage);
            SqlDataReader monReader = maCommande.ExecuteReader();

            int idReponse = 0;
            string choixReponse = "";
            int nbVotant = 0;
            int fkId = 0;

            while (monReader.Read())
            {
                idReponse = Convert.ToInt32(monReader["IdReponse"]);
                choixReponse = (string)monReader["IntituleReponse"];
                nbVotant = (int)monReader["NbVoteReponse"];
                fkId = Convert.ToInt32(monReader["FKIdSondage"]);
                Reponse maReponse = new Reponse(idReponse, choixReponse, nbVotant, fkId);
                mesReponse.Add(maReponse);
            }

            connexion.Close();

            return mesReponse;
        }

        public Sondage GetSondageById(int idSondage)
        {
            SqlConnection connection = new SqlConnection(SqlConnectionString);
            connection.Open();

            SqlCommand maCommande = new SqlCommand(@"SELECT * FROM Sondage WHERE IdSondage=@id_sondage", connection);
            maCommande.Parameters.AddWithValue("@id_sondage", idSondage);
            SqlDataReader monReader = maCommande.ExecuteReader();

            int id = 0;
            string questionSondage = "";
            bool choixMultiple = true;
            string urlPartage = "";
            string urlSuppression = "";
            string urlResultat = "";
            int nbVotant = 0;
            int fkIdCreateur = 0;

            monReader.Read();

            id = (int)monReader["IdSondage"];
            questionSondage = (string)monReader["QuestionSondage"];
            choixMultiple = (bool)monReader["ChoixMultiple"];
            urlPartage = Convert.ToString(monReader["UrlPartage"]);
            urlSuppression = Convert.ToString(monReader["UrlSuppression"]);
            urlResultat = Convert.ToString(monReader["UrlResultat"]);
            nbVotant = Convert.ToInt32(monReader["NbVotant"]);
            fkIdCreateur = Convert.ToInt32(monReader["FKIdCreateur"]);
            Sondage monSondage = new Sondage(id, questionSondage, choixMultiple, urlPartage, urlSuppression, urlResultat, nbVotant, fkIdCreateur);

            connection.Close();

            return monSondage;
        }

        public void UpdateSondage(Sondage unSondage, int idSondage)
        {
            SqlConnection connection = new SqlConnection(SqlConnectionString);
            connection.Open();

            SqlCommand maCommande = new SqlCommand(@"UPDATE Sondage SET UrlPartage=@url_partage, UrlSuppression=@url_suppression, UrlResultat=@url_resultat WHERE IdSondage=@id_sondage", connection);
            maCommande.Parameters.AddWithValue("@url_partage", unSondage.UrlPartage);
            maCommande.Parameters.AddWithValue("@url_suppression", unSondage.UrlSuppression);
            maCommande.Parameters.AddWithValue("@url_resultat", unSondage.UrlResultat);
            maCommande.Parameters.AddWithValue("@id_sondage", idSondage);
            maCommande.ExecuteNonQuery();

            connection.Close();
        }

        public void UpdateNombreVoteReponse(Reponse reponse)
        {
            SqlConnection connection = new SqlConnection(SqlConnectionString);
            connection.Open();

            SqlCommand maCommande = new SqlCommand(@"UPDATE Reponse SET NbVoteReponse = @nb_vote WHERE IdReponse = @id_reponse", connection);
            maCommande.Parameters.AddWithValue("@nb_vote", reponse.NbVoteReponse);
            maCommande.Parameters.AddWithValue("@id_reponse", reponse.IdReponse);
            maCommande.ExecuteNonQuery();

            connection.Close();
        }

        public void UpdateNombreVotant(Sondage unSondage)
        {
            SqlConnection connection = new SqlConnection(SqlConnectionString);
            connection.Open();

            SqlCommand maCommande = new SqlCommand(@"UPDATE Sondage SET NbVotant = @nb_votant WHERE IdSondage = @id_sondage", connection);
            maCommande.Parameters.AddWithValue("@nb_votant", unSondage.NbVotant);
            maCommande.Parameters.AddWithValue("@id_sondage", unSondage.IdSondage);
            maCommande.ExecuteNonQuery();

            connection.Close();
        }
    }
}