using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using wsteste.Models;
using wsteste.DAO;

namespace wsteste.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PessoaController : ControllerBase
    {   
        
        public string server { get; set; }
        public string bancoDados{ get; set; }
        public string Table { get; set; }        
        private PessoasDao PD;
        
        
        public PessoaController()
        {
            server = "localhost";
            bancoDados = "bancoDados";
            Table = "dbo.Pessoas";
            PD= new PessoasDao();
            PD.conexaoBD(server,bancoDados);            
                      
            
        }

        

        [HttpGet]
        public string DadosConexao()
        {
            
            return $"Dados Predefinidos - Server {server} / Database: {bancoDados} / Table: {Table} ";
        }

        [HttpGet("pessoas")]

        public IActionResult getTodasPessoas(){
            
            try{
                return Ok(PD.pesquisarTodasPessoas(Table));

            }catch{
                return BadRequest();

            }           
           
        }
        
        
        [HttpGet("byCPF/{cpf}")]
        public IActionResult getPessoaByCPF(string cpf)
        {
            try
            {
                
                    return Ok(PD.pesquisarPessoa(Table,cpf: cpf));
            }catch{
                return NotFound("Pessoa não encontrada");
            }
            
            
        }


        [HttpGet("bynome/{nome}")]
        public IActionResult getPessoaByNome(string nome)
        {
             try
            {
                
                return Ok(PD.pesquisarPessoa(Table, nome: nome));
            }catch{
                return NotFound("Pessoa não encontrada");
            }
            
        }
        
        [HttpGet("byidade/{idade}")]
        public IActionResult getPessoaByIdade(string idade)
        {
             try
            {
                
                return Ok(PD.pesquisarPessoa(Table, idade: idade));
            }catch{
                return NotFound("Pessoa não encontrada");
            }
            
        }

         [HttpPost("cargainicial/{tabela}")]
        public List<Pessoa> cargaInicial(string tabela)
        {
            
            PD.cargaInicial(tabela);
            
            return PD.pesquisarTodasPessoas(Table);
        }
        
        [HttpPost("inserirPessoa/{nome}/{cpf}/{idade}")]
        public List<Pessoa> inserirPessoa(string nome, string cpf, string idade)
        {
            
            PD.AdicionarPessoa("Pessoas",nome,cpf,idade);
            
            return PD.pesquisarTodasPessoas(Table);
        }

        [HttpPost("deletarPessoa/{nome}/{cpf}/{idade}")]
        public List<Pessoa> deletarPessoa(string nome, string cpf, string idade)
        {
            
            PD.DeletarPessoa("Pessoas",nome,cpf,idade);
            
            return PD.pesquisarTodasPessoas(Table);
        }

        [HttpPost("AltPessoaCpf/{nome}/{cpf}/{idade}")]
        public List<Pessoa> UpdatePessoaCpf(string nome, string cpf, string idade)
        {
            
            PD.AlterarPessoa("Pessoas",nome,cpf,idade);
            
            return PD.pesquisarTodasPessoas(Table);
        }

        
    }
}
