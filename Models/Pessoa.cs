using System;

namespace wsteste.Models
{
    public class Pessoa
    {
        public string nome {get;set;}
        public string cpf {get;set;}
        public int idade {get;set;}

        public Pessoa()
        {
            
        }
        public Pessoa(string nome, string cpf, int idade)
        {
            this.nome = nome;
            this.cpf = cpf;
            this.idade = idade;
        }
    }
}