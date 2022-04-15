using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librarie_online.Models
{
    public partial class Carte_Categorie
    {
        public Carte_Categorie()
        {
            this.Vanzari = new HashSet<Vanzari>();
        }

        public int Id { get; set; }
        public Nullable<int> carte { get; set; }
        public Nullable<int> categorie { get; set; }

        public virtual Carte Carte1 { get; set; }
        public virtual Categorie Categorie1 { get; set; }
        public virtual ICollection<Vanzari> Vanzari { get; set; }
    }
}
