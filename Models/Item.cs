using System;
using System.Collections.Generic;
using System.Linq;

namespace livraria.Models
{
    public class Item
    {

        public int Qtde { get; set; }
        public Livro Livro { get; set; }

        public Item()
        {
        }
        public double Total(IEnumerable<Item> items)
        {
            return items.Sum(x => x.Livro.Preco * x.Qtde);
        }

    }
}
