using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Simpoll.Models
{
    public class DAL
    {
        private const string SqlConnectionString = @"Server=.\SQLExpress;Initial Catalog=Simpoll; Trusted_Connection=Yes";
        //private const string SqlConnectionString = @"Server=.\192.18.240.2;Initial Catalog=Simpoll; Trusted_Connection=Yes";

        //Methode pour ajouter un utilisateur créateur de sondage en BDD
        public static int AddUtilisateur(Createur unCreateur)
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

        //Methode pour ajouter un sondage en BDD et recuperer l'ID de son createur
        public static int AddSondage(Sondage unSondage)
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand maCommande = new SqlCommand(@"INSERT INTO Sondage(QuestionSondage, ChoixMultiple, NbVotant, FKIdCreateur, Guid, Actif) " +
                                                     "VALUES (@question, @choix_multiple, @nb_votant, @FK_idCreateur, @my_guid, @actif); " +
                                                     "SELECT SCOPE_IDENTITY()", connexion);
            maCommande.Parameters.AddWithValue("@question", unSondage.QuestionSondage);
            maCommande.Parameters.AddWithValue("@choix_multiple", unSondage.ChoixMultiple);
            maCommande.Parameters.AddWithValue("@nb_votant", unSondage.NbVotant);
            maCommande.Parameters.AddWithValue("@FK_idCreateur", unSondage.FKIdCreateur);
            maCommande.Parameters.AddWithValue("@my_guid", unSondage.Guid);
            maCommande.Parameters.AddWithValue("@actif", unSondage.Actif);

            int IdSondage = Convert.ToInt32(maCommande.ExecuteScalar());

            connexion.Close();

            return IdSondage;
        }

        //methode pour ajouter les reponses d'un sondage en BDD
        public static int AddReponse(Reponse maReponse)
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand maCommande = new SqlCommand("INSERT INTO Reponse(IntituleReponse, NbVoteReponse, FKIdSondage) " +
                                                   "VALUES (@choix, @nb_vote_reponse, @FK_idSondage); " +
                                                   "SELECT SCOPE_IDENTITY()", connexion);
            maCommande.Parameters.AddWithValue("@choix", maReponse.IntituleReponse);
            maCommande.Parameters.AddWithValue("@nb_vote_reponse", maReponse.NbVoteReponse);
            maCommande.Parameters.AddWithValue("@FK_idSondage", maReponse.FKIdSondage);
            int IdReponse = Convert.ToInt32(maCommande.ExecuteScalar());

            connexion.Close();

            return IdReponse;
        }

        //Methode pour recuperer les reponses d'un sondage et en faire une liste
        public static List<Reponse> GetAllReponse(int IdSondage)
        {
            List<Reponse> mesReponse = new List<Reponse>();
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand maCommande = new SqlCommand(@"SELECT * " +
                                                    "FROM Reponse " +
                                                    "WHERE FKIdSondage=@id_sondage", connexion);
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

        //Methode pour recuperer les infos d'un sondage avec son ID
        public static Sondage GetSondageById(int idSondage)
        {
            SqlConnection connection = new SqlConnection(SqlConnectionString);
            connection.Open();

            SqlCommand maCommande = new SqlCommand(@"SELECT * " +
                                                    "FROM Sondage " +
                                                    "WHERE IdSondage=@id_sondage", connection);
            maCommande.Parameters.AddWithValue("@id_sondage", idSondage);
            SqlDataReader monReader = maCommande.ExecuteReader();

            monReader.Read();

            int id = Convert.ToInt32(monReader["IdSondage"]);
            string questionSondage = (string)monReader["QuestionSondage"];
            bool choixMultiple = (bool)monReader["ChoixMultiple"];
            string urlPartage = Convert.ToString(monReader["UrlPartage"]);
            string urlSuppression = Convert.ToString(monReader["UrlSuppression"]);
            string urlResultat = Convert.ToString(monReader["UrlResultat"]);
            int nbVotant = Convert.ToInt32(monReader["NbVotant"]);
            int fkIdCreateur = Convert.ToInt32(monReader["FKIdCreateur"]);
            string guid = Convert.ToString(monReader["Guid"]);
            bool actif = Convert.ToBoolean(monReader["Actif"]);
            Sondage monSondage = new Sondage(id, questionSondage, choixMultiple, urlPartage, urlSuppression, urlResultat, nbVotant, fkIdCreateur, guid, actif);

            connection.Close();

            return monSondage;
        }

        //methode pour mettre a jour un sondage avec ses url aprés la creation du sondage
        public static void UpdateSondage(Sondage unSondage, int idSondage)
        {
            SqlConnection connection = new SqlConnection(SqlConnectionString);
            connection.Open();

            SqlCommand maCommande = new SqlCommand(@"UPDATE Sondage 
                                                     SET UrlPartage=@url_partage, 
                                                         UrlSuppression=@url_suppression, 
                                                         UrlResultat=@url_resultat 
                                                     WHERE IdSondage=@id_sondage"
                                                   , connection);
            maCommande.Parameters.AddWithValue("@url_partage", unSondage.UrlPartage);
            maCommande.Parameters.AddWithValue("@url_suppression", unSondage.UrlSuppression);
            maCommande.Parameters.AddWithValue("@url_resultat", unSondage.UrlResultat);
            maCommande.Parameters.AddWithValue("@id_sondage", idSondage);
            maCommande.ExecuteNonQuery();

            connection.Close();
        }

        public static void DisableSondage(Sondage unSondage)
        {
            SqlConnection connection = new SqlConnection(SqlConnectionString);
            connection.Open();

            SqlCommand maCommande = new SqlCommand(@"UPDATE Sondage SET Actif=@actif WHERE IdSondage=@id_sondage", connection);
            maCommande.Parameters.AddWithValue("@actif", unSondage.Actif);
            maCommande.Parameters.AddWithValue("@id_sondage", unSondage.IdSondage);
            maCommande.ExecuteNonQuery();

            connection.Close();
        }

        //methode pour mettre a jour le nombre de vote par reponses aprés chaque vote
        public static void UpdateNombreVoteReponse(Reponse reponse)
        {
            SqlConnection connection = new SqlConnection(SqlConnectionString);
            connection.Open();

            SqlCommand maCommande = new SqlCommand(@"UPDATE Reponse SET NbVoteReponse = @nb_vote WHERE IdReponse = @id_reponse", connection);
            maCommande.Parameters.AddWithValue("@nb_vote", reponse.NbVoteReponse);
            maCommande.Parameters.AddWithValue("@id_reponse", reponse.IdReponse);
            maCommande.ExecuteNonQuery();

            connection.Close();
        }

        //methode pour mettre a jour le nombre de votant d'un sondage
        public static void UpdateNombreVotant(Sondage unSondage)
        {
            SqlConnection connection = new SqlConnection(SqlConnectionString);
            connection.Open();

            SqlCommand maCommande = new SqlCommand(@"UPDATE Sondage SET NbVotant = @nb_votant WHERE IdSondage = @id_sondage", connection);
            maCommande.Parameters.AddWithValue("@nb_votant", unSondage.NbVotant);
            maCommande.Parameters.AddWithValue("@id_sondage", unSondage.IdSondage);
            maCommande.ExecuteNonQuery();

            connection.Close();
        }

        public static Sondage GetSondageByGuid(string guid)
        {
            SqlConnection connection = new SqlConnection(SqlConnectionString);
            connection.Open();

            SqlCommand maCommande = new SqlCommand(@"SELECT * FROM Sondage WHERE Guid=@my_guid", connection);
            maCommande.Parameters.AddWithValue("@my_guid", (string)guid);
            SqlDataReader monReader = maCommande.ExecuteReader();

            int id = 0;
            string questionSondage = "";
            bool choixMultiple = true;
            string urlPartage = "";
            string urlSuppression = "";
            string urlResultat = "";
            int nbVotant = 0;
            int fkIdCreateur = 0;
            string myGuid = "";
            bool actif = true;

            monReader.Read();

            id = (int)monReader["IdSondage"];
            questionSondage = (string)monReader["QuestionSondage"];
            choixMultiple = (bool)monReader["ChoixMultiple"];
            urlPartage = Convert.ToString(monReader["UrlPartage"]);
            urlSuppression = Convert.ToString(monReader["UrlSuppression"]);
            urlResultat = Convert.ToString(monReader["UrlResultat"]);
            nbVotant = Convert.ToInt32(monReader["NbVotant"]);
            fkIdCreateur = Convert.ToInt32(monReader["FKIdCreateur"]);
            guid = Convert.ToString(monReader["Guid"]);
            actif = Convert.ToBoolean(monReader["Actif"]);
            Sondage monSondage = new Sondage(id, questionSondage, choixMultiple, urlPartage, urlSuppression, urlResultat, nbVotant, fkIdCreateur, myGuid, actif);

            connection.Close();

            return monSondage;
        }

        public static List<Reponse> GetReponseOrderedById(int idSondage)
        {
            List<Reponse> mesReponse = new List<Reponse>();

            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand maCommande = new SqlCommand("SELECT IdReponse, IntituleReponse, NbVoteReponse " +
                                                    "FROM Reponse " +
                                                    "WHERE FKIdSondage=@idSondage " +
                                                    "ORDER BY NbVoteReponse DESC", connexion);
            maCommande.Parameters.AddWithValue("@idSondage", idSondage);
            SqlDataReader monReader = maCommande.ExecuteReader();

            int idReponse = 0;
            string intituleReponse = string.Empty;
            int nbVoteReponse = 0;
            while (monReader.Read())
            {
                idReponse = (int)monReader["IdReponse"];
                intituleReponse = (string)monReader["IntituleReponse"];
                nbVoteReponse = (int)monReader["NbVoteReponse"];
                Reponse maReponse = new Reponse(idReponse, intituleReponse, nbVoteReponse);
                mesReponse.Add(maReponse);
            }


            connexion.Close();

            return mesReponse;
        }

        public static Createur GetCreateurById(int idSondage)
        {
            SqlConnection connection = new SqlConnection(SqlConnectionString);
            connection.Open();

            SqlCommand maCommande = new SqlCommand(@"SELECT IdCreateur, NomCreateur, PrenomCreateur, EmailCreateur 
                                                     FROM Createur c, Sondage s 
                                                     WHERE c.IdCreateur = s.FKIdCreateur AND IdSondage = @idSondage", connection);
            maCommande.Parameters.AddWithValue("@idSondage", idSondage);
            SqlDataReader monReader = maCommande.ExecuteReader();

            string nomCreateur = "";
            string prenomCreateur = "";
            string emailCreateur = "";
         
            monReader.Read();

            nomCreateur = (string)monReader["NomCreateur"];
            prenomCreateur = (string)monReader["PrenomCreateur"];
            emailCreateur = (string)monReader["EmailCreateur"];


            Createur monCreateur = new Createur(nomCreateur, prenomCreateur, emailCreateur);
            connection.Close();

            return monCreateur;
        }  

        public static int GetSommeVoteDesReponses(int idSondage)
        {
            SqlConnection connexion = new SqlConnection(SqlConnectionString);
            connexion.Open();

            SqlCommand maCommande = new SqlCommand(@"select sum(NbVoteReponse)" +
                                                    "FROM Reponse " +
                                                    "WHERE FKIdSondage=@id_sondage", connexion);
            maCommande.Parameters.AddWithValue("@id_sondage", idSondage);

            int sommeVoteDesReponse = (int)maCommande.ExecuteScalar();

            connexion.Close();

            return sommeVoteDesReponse;
        }


    }
}