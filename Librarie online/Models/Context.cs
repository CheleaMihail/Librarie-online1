using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librarie_online.Models
{
    public partial class ModelConnection : DbContext
    {
        public ModelConnection() : base("DefaultConnection") { }
        public virtual DbSet<Autor> Autors { get; set; }
        public virtual DbSet<Carte> Cartes { get; set; }
        public virtual DbSet<Editura> Edituras { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Vanzari> Vanzaris { get; set; }
        public virtual DbSet<Categorie> Categories { get; set; }
        public virtual DbSet<Carte_Categorie> Carte_Categories { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Cos> Cos { get; set; }
    }
}
