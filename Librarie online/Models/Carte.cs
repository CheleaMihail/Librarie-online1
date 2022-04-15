using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librarie_online.Models
{
    public partial class Carte
    {
        public Carte()
        {
            this.Vanzari = new HashSet<Vanzari>();
        }

        public int Id { get; set; }
        public string denumire { get; set; }
        public Nullable<int> autor { get; set; }
        public Nullable<int> editura { get; set; }
        public string limba { get; set; }
        public Nullable<float> pret { get; set; }
        public Nullable<int> reducere { get; set; }
        public Nullable<int> stoc { get; set; }
        public string descriere { get; set; }
        public string imagine { get; set; }
        public virtual Autor Autor1 { get; set; }
        public virtual Editura Editura1 { get; set; }
        public virtual ICollection<Vanzari> Vanzari { get; set; }
        public List<Rating> Rating { get; set; }
        public List<Cos> Cos { get; set; }
    }
}
