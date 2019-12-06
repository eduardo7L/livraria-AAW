using System;
using System.Collections.Generic;

namespace livraria.Models
{
    public class Carrinho
    {

        public int Id { get; set; }
        public List<Livro> Livros { get; set; }
        public Carrinho()
        {
            Livros = new List<Livro>();
        }
        
    }
}