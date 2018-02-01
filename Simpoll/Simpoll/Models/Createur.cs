using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simpoll.Models
{
    public class Createur
    {
        public int IdCreateur { get; set; }
        public string NomCreateur { get; set; }
        public string PrenomCreateur { get; set; }
        public string EmailCreateur { get; set; }

        public Createur(int id , string nom, string prenom, string email)
        {
            this.IdCreateur = id;
            this.NomCreateur = nom;
            this.PrenomCreateur = prenom;
            this.EmailCreateur = email;
        }
        public Createur(string nom, string prenom, string email)
        {
            this.NomCreateur = nom;
            this.PrenomCreateur = prenom;
            this.EmailCreateur = email;
        }

    }
}