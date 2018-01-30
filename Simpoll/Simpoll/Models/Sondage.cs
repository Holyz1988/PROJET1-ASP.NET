﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simpoll.Models
{
    public class Sondage
    {
        public int IdSondage { get; private set; }
        public string QuestionSondage { get; private set; }
        public bool ChoixMultiple { get; private set; }
        public string UrlPartage { get; set; }
        public string UrlSuppression { get; set; }
        public string UrlResultat { get; set; }
        public int NbVotant { get; set; }
        public int FKIdCreateur { get; private set; }
        public string Guid { get; set; }
        public bool Actif { get; set; }

        public Sondage(int idSondage, string questionSondage, bool choixMultiple, string urlPartage, string urlSuppression, string urlResultat, int nbVotant, int fkIdCreateur, string guid, bool actif)
        {
            this.IdSondage = idSondage;
            this.QuestionSondage = questionSondage;
            this.ChoixMultiple = choixMultiple;
            this.UrlPartage = urlPartage;
            this.UrlSuppression = urlSuppression;
            this.UrlResultat = urlResultat;
            this.NbVotant = nbVotant;
            this.FKIdCreateur = fkIdCreateur;
            this.Guid = guid;
            this.Actif = actif;
        }

        public Sondage(string questionSondage, bool choixMultiple, int fkIdCreateur, string myGuid)
        {
            this.QuestionSondage = questionSondage;
            this.ChoixMultiple = choixMultiple;
            this.FKIdCreateur = fkIdCreateur;
            this.Guid = myGuid;
            this.Actif = true;
            this.NbVotant = 0;
        }
    }
}