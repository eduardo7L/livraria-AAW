namespace livraria.Models
{
    public class Comentario
    {

        public int Id { get; set; }
        public string Texto { get; set; }

        public Comentario(){}
        public Comentario(int id, string texto)
        {
            Id = id;
            Texto = texto;
        }
    }
}