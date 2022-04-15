using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librarie_online.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int Puncte { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int CarteId { get; set; }
        public Carte Carte { get; set; }
    }
}
