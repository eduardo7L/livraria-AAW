using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using livraria.Models;


namespace livraria.Controllers
{
    [Route("api/livraria")]
    [ApiController]
    public class livrariaController : ControllerBase
    {
        private readonly livrariaContext _context;

        public livrariaController(livrariaContext context)
        {
            _context = context;

            if (_context.Livros.Count() == 0)
            {
                
                Livro DomCasmurro =  new Livro() { Nome = "Dom Casmurro", Preco = 120, Estoque = 14 };
                DomCasmurro.Comentarios.AddRange(new List<Comentario>() {new Comentario(1, "Muito bom."),
                                                                        new Comentario(2, "Um dos melhores que já lí."),
                                                                        new Comentario(3, "Recomendo demais da conta sô.")});
                
                Livro OAlienista =  new Livro() { Nome = "O Alienista", Preco = 152, Estoque = 20 };
                OAlienista.Comentarios.AddRange(new List<Comentario>() {new Comentario(4, "O Alienista é uma das obras mais conhecidas de Machado de Assis."),
                                                                        new Comentario(5, "Considerada ora conto ora novela, seu destaque fica por conta do humor mordaz apresentado através de uma bem azeitada crítica sócio-política além da caprichada análise de personagens."),
                                                                        new Comentario(6, "Sua história remonta ao Brasil Colonial."),
                                                                        new Comentario(7, "Recomendo a leitura de Machado de Assis, pela importância do autor, por ter sido um homem à frente de seu tempo e, acima de tudo, pela qualidade de seus textos.")});
                
                

                _context.Livros.AddRange(new List<Livro>(){ DomCasmurro, 
                                                            new Livro() { Nome = "Vidas secas", Preco = 116, Estoque = 12 },
                                                            new Livro() { Nome = "Macunaíma", Preco = 90, Estoque = 7 }, 
                                                            new Livro() { Nome = "O triste Fim de Policarpo Quaresma", Preco = 85, Estoque = 0 }, 
                                                            OAlienista,
                                                            new Livro() { Nome = "O guarani", Preco = 133, Estoque = 8 } } );

                
                Carrinho carrinho =  new Carrinho();
                carrinho.Livros.AddRange(new List<Livro>(){ OAlienista, DomCasmurro});
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Carrinho> entityEntry = _context.Carrinhos.Add(carrinho);
                _context.SaveChanges();
            }
        }

        // GET: api/livraria
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Livro>>> GetLivros()
        {
            return await _context.Livros.Where(e => e.Estoque > 0).ToListAsync();
        }

        // GET: api/livraria/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Livro>> GetLivro(int id)
        {
            var livro = await _context.Livros.FindAsync(id);

            if (livro == null)
            {
                return NotFound();
            }

            return livro;
        }

         // GET: api/livraria/5
        [HttpGet("GetCarrinhos")]
        public async Task<ActionResult<IEnumerable<Carrinho>>> GetCarrinhos()
        {
            return await _context.Carrinhos.ToListAsync();
        }
        
        [HttpGet("GetLivroPeloNome/{nome}")]
        public async Task<ActionResult<IEnumerable<Livro>>> GetLivroPeloNome(string nome)
        {
            var livros = await _context.Livros.Where(e => e.Nome == nome).ToListAsync();

            if (livros == null)
            {
                return NotFound();
            }

            return livros;
        }

        [HttpGet("GetLivrosPelaFaixaDeValor/{min}/{max}")]
        public async Task<ActionResult<IEnumerable<Livro>>> GetLivrosPelaFaixaDeValor(int min, int max)
        {
            var livros = await _context.Livros.Where(e => e.Preco >= min && e.Preco <= max).ToListAsync();

            if (livros == null)
            {
                return NotFound();
            }

            return livros;
        }

        [HttpGet("FecharPedido/{idCarrinho}")]
        public async Task<ActionResult<IEnumerable<Livro>>> FecharPedido(int idCarrinho)
        {
            Carrinho carrinho = _context.Carrinhos.Where(e => e.Id == idCarrinho).FirstOrDefault();

            Pedido pedido = new Pedido();
            pedido.Livros = carrinho.Livros;

            _context.Entry(pedido).State = EntityState.Modified; 

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }

            return pedido.Livros;
        }

        [HttpGet("GetLivrosIndisponiveis")]
        public async Task<ActionResult<IEnumerable<Livro>>> GetLivrosIndisponiveis()
        {
            var livros = await _context.Livros.Where(e => e.Estoque == 0).ToListAsync();

            if (livros == null)
            {
                return NotFound();
            }

            return livros;
        }

        // PUT: api/livraria/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLivro(int id, Livro livro)
        {
            if (id != livro.Id)
            {
                return BadRequest();
            }

            _context.Entry(livro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LivroExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPut("PutLivroNoCarrinho/{id}/{idCarrinho}")]
        public async Task<IActionResult> PutLivroNoCarrinho(int id, int idCarrinho)
        {
            var livro = await _context.Livros.FindAsync(id);

            if (livro == null)
            {
                return NotFound();
            }

            Carrinho carrinho = _context.Carrinhos.Where(e => e.Id == idCarrinho).FirstOrDefault();
            carrinho.Livros.Add(livro);

            _context.Entry(carrinho).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        [HttpPost("PutComentarioLivro/{id}/{comentario}")]
        public async Task<IActionResult> PutComentarioLivro(int id, string comentario)
        {

            var livro = await _context.Livros.FindAsync(id);

            if (livro == null)
            {
                return NotFound();
            }
            
            livro.Comentarios.Add(new Comentario(1, comentario));

            _context.Entry(livro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LivroExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/livraria
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Livro>> PostLivro(Livro livro)
        {
            _context.Livros.Add(livro);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLivro), new { id = livro.Id }, livro);
        }

        // DELETE: api/livraria/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Livro>> DeleteLivro(int id)
        {
            var livro = await _context.Livros.FindAsync(id);
            if (livro == null)
            {
                return NotFound();
            }

            _context.Livros.Remove(livro);
            await _context.SaveChangesAsync();

            return livro;
        }

        private bool LivroExists(int id)
        {
            return _context.Livros.Any(e => e.Id == id);
        }
    }
}
