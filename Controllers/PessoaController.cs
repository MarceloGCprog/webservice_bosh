using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using wsteste.Models;

namespace wsteste.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PessoaController : ControllerBase
    {
        //postman
        //localhost/WeatherForecast
        //localhost/api/Values

        static private List<Pessoa> pessoas;
        public PessoaController()
        {
            if( pessoas == null )
            {
                pessoas = new List<Pessoa>();
                CargaInicial();
            }
        }

        void CargaInicial()
        {
            pessoas.Add(new Pessoa("Vilmar","000",29));
            pessoas.Add(new Pessoa("Mykaele","001",23));
            pessoas.Add(new Pessoa("Antonella","002",1));
            pessoas.Add(new Pessoa("Duda","003",2));
        }

        [HttpGet]
        public List<Pessoa> getTodasPessoas()
        {
            return pessoas;
        }
        
        [HttpGet("byCPF/{cpf}")]
        public IActionResult getPessoaByCPF(string cpf)
        {
            foreach( Pessoa p in pessoas )
            {
                if( p.cpf == cpf )
                    return Ok(p);
            }
            //Como retornar null?
            return NotFound("Pessoa não encontrada");
        }


        [HttpGet("byNome")]
        public IActionResult getPessoaByNome([FromQuery(Name="name")] string nome)
        {
            if( nome == null || nome == "" )
                return BadRequest();
            foreach( Pessoa p in pessoas )
            {
                if( p.nome == nome )
                    return Ok(p);
            }
            return NotFound();
        }

        
        [HttpPost("{nome}/{cpf}/{idade}")]
        public List<Pessoa> inserirPessoa(string nome, string cpf, int idade)
        {
            Pessoa p = new Pessoa(nome,cpf,idade);
            pessoas.Add(p);
            return pessoas;
        }

        [HttpPost]
        public IActionResult inserirPessoaCompleta(Pessoa novaPessoa)
        {
            pessoas.Add(novaPessoa);
            return Ok();
        }
    }
}
