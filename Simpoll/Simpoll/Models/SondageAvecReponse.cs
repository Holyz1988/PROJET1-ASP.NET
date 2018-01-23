using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simpoll.Models
{
    public class SondageAvecReponse
    {
        public Sondage Sondage { get; private set; }
        public List<Reponse> MesReponses { get; private set; }

        public SondageAvecReponse(Sondage sondage, List<Reponse> mesReponses)
        {
            this.Sondage = sondage;
            this.MesReponses = mesReponses;
        }
    }
}