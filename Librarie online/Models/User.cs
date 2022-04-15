using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librarie_online.Models
{
    public partial class User
    {
        public User()
        {
            this.Vanzari = new HashSet<Vanzari>();
        }

        public int Id { get; set; }
        public string nume { get; set; }
        public string prenume { get; set; }
        public Nullable<System.DateTime> dataNasterii { get; set; }
        public string telefon { get; set; }
        public string email { get; set; }
        public string adresa { get; set; }
        public string parola { get; set; }
        public string image { get; set; }
        public bool isAdmin { get; set; }
        public virtual ICollection<Vanzari> Vanzari { get; set; }
        public List<Rating> Rating { get; set; }
        public List<Cos> Cos { get; set; }
    }
}
