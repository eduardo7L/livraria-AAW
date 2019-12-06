using Microsoft.EntityFrameworkCore;

namespace livraria.Models
{
    public class livrariaContext : DbContext
    {
        public livrariaContext(DbContextOptions<livrariaContext> options)
            : base(options)
        {
        }

        public DbSet<Livro> Livros { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Carrinho> Carrinho { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }

    }
}