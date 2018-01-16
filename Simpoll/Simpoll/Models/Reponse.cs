﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simpoll.Models
{
    public class Reponse
    {
        public int IdReponse { get; set; }
        public string IntituleReponse { get; set; }
        public int NbVoteReponse { get; set; }
        public int FKIdSondage { get; set; }

        public Reponse(string choixReponse)
        {
            this.IntituleReponse = choixReponse;
            this.NbVoteReponse = 0;
        }

        public Reponse(string choixReponse, int nbVoteReponse)
        {
            this.IntituleReponse = choixReponse;
            this.NbVoteReponse = NbVoteReponse;
        }

        public Reponse(int idSondage)
        {
            this.FKIdSondage = idSondage;
        }
    }
}