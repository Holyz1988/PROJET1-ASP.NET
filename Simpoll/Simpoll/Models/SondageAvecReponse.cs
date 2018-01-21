using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simpoll.Models
{
    public class SondageAvecReponse
    {
        public Sondage Sondage { get; set; }
        public List<Reponse> MesReponses { get; set; }

        public SondageAvecReponse(Sondage sondage, List<Reponse> mesReponses)
        {
            this.Sondage = sondage;
            this.MesReponses = mesReponses;
        }
    }
}