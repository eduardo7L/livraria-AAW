using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace livraria.Models
{        
    public class Livro
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O nome do livro é obrigatório")]
        public string Nome { get; set; }

        [Display(Name = "Preço")]
        [DisplayFormat(DataFormatString = "R$ {0:F2}")]
        [Range(0.05, double.MaxValue,ErrorMessage = "Digite um valor mínimo")]
        [Required(ErrorMessage = "O Preço do livro é obrigatório")]
        [RegularExpression("^[\\d,.?!]+$", ErrorMessage = "Digite apenas números")]
        public double Preco { get; set; }

        [Display(Name = "Estoque")]
        [Range(0,int.MaxValue,ErrorMessage ="Estoque não pode ser negativo")]
        [Required(ErrorMessage = "A quantidade em estoque do livro é obrigatório")]
        public int Estoque { get; set; }

        public List<Comentario> Comentarios { get; set; }

        public Livro()
        {
            Comentarios = new List<Comentario>();
        }

        // public Livro(int id, string nome, double preco, int estoque)
        // {
        //     Id = id;
        //     Nome = nome;
        //     Preco = preco;
        //     Estoque = estoque;
        // }
    }
}