﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace Simpoll.Models
{
    public class DAL
    {
        private const string SqlConnectionString = @"Server=.\SQLExpress;Initial Catalog=Simpoll; Trusted_Connection=Yes";
        public static int AddUser(Createur unCreateur)
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

        public static int AddSondage(Sondage unSondage)
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

        public static int AddReponse(Reponse maReponse)
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

        public static List<Reponse> GetAllReponse(int IdSondage)
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

        public static Sondage GetSondageById(int idSondage)
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

        public static void UpdateSondage(Sondage unSondage, int idSondage)
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
    }
}