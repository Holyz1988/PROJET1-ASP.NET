using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simpoll.Models
{
    public class SondageAvecReponse
    {
        public string Question { get; set; }
        public List<Reponse> mesReponses { get; set; }

        public SondageAvecReponse(string question, List<Reponse> mesReponses)
        {
            this.Question = question;
            this.mesReponses = mesReponses;
        }
    }
}