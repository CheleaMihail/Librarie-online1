using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librarie_online.Models
{
    public partial class Autor
    {
        public Autor()
        {
            this.Carte = new HashSet<Carte>();
        }

        public int Id { get; set; }
        public string nume { get; set; }
        public string prenume { get; set; }

        public virtual ICollection<Carte> Carte { get; set; }
    }
}
