using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librarie_online.Models
{
    public partial class Categorie
    {
        public Categorie()
        {
            this.Carte = new HashSet<Carte>();
        }

        public int Id { get; set; }
        public string denumire { get; set; }

        public virtual ICollection<Carte> Carte { get; set; }
    }
}
