using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librarie_online.Views
{
   public class BookList
    {
        public int id { get; set; }
        public string denumire { get; set; }
        public string  autor { get; set; }
        public string editura { get; set; }
        public string categorii { get; set; }
        public List<int> categoriiId { get; set; }
        public string limba { get; set; }
        public int stoc { get; set; }
        public string descriere { get; set; }
        public string imagine { get; set; }
        public int count { get; set; }
        public string pretAnterior { get; set; }
        public string pretCurent { get; set; }
        public bool star1 { get; set; }
        public bool star2 { get; set; }
        public bool star3 { get; set; }
        public bool star4 { get; set; }
        public bool star5 { get; set; }
        public string data { get; set; }
    }
}
