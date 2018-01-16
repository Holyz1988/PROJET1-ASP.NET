using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simpoll.Models
{
    public class Sondage
    {
        public int IdSondage { get; set; }
        public string QuestionSondage { get; set; }
        public bool ChoixMultiple { get; set; }
        /*
        public string UrlPartage { get; set; }
        public string UrlSuppression { get; set; }
        public string UrlResultat { get; set; }
        */
        public int NbVotant { get; set; }
        public int FKIdCreateur { get; set; }

        public Sondage(string QuestionSondage, bool ChoixMultiple, int FKIdCreateur)
        {
            this.QuestionSondage = QuestionSondage;
            this.ChoixMultiple = ChoixMultiple;
            this.FKIdCreateur = FKIdCreateur;
            this.NbVotant = 0;
        }

        public Sondage ()
        {
            this.NbVotant = 0;
        }
    }
}