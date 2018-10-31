namespace BGayet.GIA.Models
{
    public class Table
    {
        public int Numero { get; set; }
        public StatutTable Statut { get; set; }
    }

    public enum StatutTable
    {
        Libre = 0,
        Occupe = 1
    }
}