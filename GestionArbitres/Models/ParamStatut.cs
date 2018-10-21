using SQLite;

namespace GestionArbitres.Models
{
    public class ParamStatut
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Code { get; set; }
        public int LibelleLong { get; set; }
        public int LibelleCourt { get; set; }
        public int Description { get; set; }
    }
}