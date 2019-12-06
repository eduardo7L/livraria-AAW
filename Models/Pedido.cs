using System;
using System.Collections.Generic;

namespace livraria.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public List<Livro> Livros { get; set; }
        public Pedido()
        {
            Livros = new List<Livro>();
        }
        
    }
}