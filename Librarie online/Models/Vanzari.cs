using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librarie_online.Models
{
    public partial class Vanzari
    {
        public int Id { get; set; }
        public Nullable<int> utilizator { get; set; }
        public Nullable<int> carte { get; set; }
        public Nullable<int> cantitate { get; set; }
        public DateTime data { get; set; }

        public virtual Carte Carte1 { get; set; }
        public virtual User User { get; set; }
    }
}
