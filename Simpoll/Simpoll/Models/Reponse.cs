using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simpoll.Models
{
    public class Reponse
    {
        public int IdReponse { get; private set; }
        public string IntituleReponse { get; private set; }
        public int NbVoteReponse { get; set; }
        public int FKIdSondage { get; private set; }

        public Reponse(int idReponse, string choixReponse, int nbVoteReponse, int fkIdSondage)
        {
            this.IntituleReponse = choixReponse;
            this.NbVoteReponse = nbVoteReponse;
            this.IdReponse = idReponse;
            this.FKIdSondage = fkIdSondage;
        }

        public Reponse(int idSondage, string choixReponse)
        {
            this.FKIdSondage = idSondage;
            this.IntituleReponse = choixReponse;
            this.NbVoteReponse = 0;
        }
    }
}