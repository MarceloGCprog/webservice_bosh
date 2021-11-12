using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using wsteste.Models;
using System.Collections.Generic;
using System.Linq;

namespace wsteste.DAO{

    class PessoasDao{
             private SqlConnection conexao;

        public PessoasDao(){

        }
        public void conexaoBD(string server, string database ){
            
            try{
            String strconexao ="Server ="+server+";"+"Database="+database+"; Trusted_Connection=True";
            conexao = new SqlConnection(strconexao);
            conexao.Open();
              
            } catch(SqlException e){
                 Console.WriteLine($"Erro SQL: {e}"); 
                 conexao.Close();
            }
            
        }

         public void closeconexaoBD(){
            

            
            conexao.Close();
            System.Console.WriteLine("Conexao Fechada"); 
        }

        public void criarTabela(string table){
            try{
               
               
                if (conexao.State != ConnectionState.Open){
                    conexao.Close();
                    conexao.Open();}

                SqlCommand comando1 = conexao.CreateCommand();
                SqlTransaction  transaction;
                transaction = conexao.BeginTransaction("Criar tabela");
                comando1.Connection = conexao;
                comando1.Transaction = transaction;

                try{
                    comando1.CommandText = $"if object_id({table}) is null begin create Table {table}(nome varchar(50),cpf varchar(11),idade varchar(3)) end ";                    
                    comando1.ExecuteNonQuery();
                    

                    transaction.Commit();            
                    
                }catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

            
                    try
                    {
                     transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                    }}                

                

            } catch(SqlException e){
                System.Console.WriteLine("Tabela nao criada. Erro: " + e);
            }

        }

        public void cargaInicial(string tabela){

            if (pesquisarTodasPessoas(tabela).Any() == false){

                AdicionarPessoa(tabela,"Vilmar","000","29");
                AdicionarPessoa(tabela,"Mykaele","001","23");
                AdicionarPessoa(tabela,"Antonella","002","1");
                AdicionarPessoa(tabela,"Duda","003","2");      

            }
        }

        public void AdicionarPessoa(string tabela,string nome, string cpf, string idade){                   
                
                Pessoa pessoa = new Pessoa(nome,cpf,idade);

            try{
               
               
                if (conexao.State != ConnectionState.Open){
                    conexao.Close();
                    conexao.Open();}

                SqlCommand comando1 = conexao.CreateCommand();
                SqlTransaction  transaction;
                transaction = conexao.BeginTransaction("Adicionar Pessoa");
                comando1.Connection = conexao;
                comando1.Transaction = transaction;

                try{
                    comando1.CommandText = $"Insert Into {tabela} (nome,cpf,idade) VALUES (@nome,@cpf,@idade)";
                    comando1.Parameters.Add(new SqlParameter("@nome",pessoa.nome));
                    comando1.Parameters.Add(new SqlParameter("@cpf",pessoa.cpf));
                    comando1.Parameters.Add(new SqlParameter("@idade",pessoa.idade));
                    comando1.ExecuteNonQuery();
                    

                    transaction.Commit();            
                    
                }catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

            
                    try
                    {
                     transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                    }}                

                

            } catch(SqlException e){
                System.Console.WriteLine("Valores nao adicionados. Problema na consistencia dos dados ou na conexao com o banco!" + e);
            }

        }

        public List<Pessoa> pesquisarTodasPessoas(string tabela){

            List<Pessoa> resultado = new List<Pessoa>();
                                          
               
                if (conexao.State != ConnectionState.Open){
                    conexao.Close();
                    conexao.Open();}               

                SqlCommand comando1 = conexao.CreateCommand();
                SqlTransaction  transaction;
                transaction = conexao.BeginTransaction("Selecionar Pessoa");
                comando1.Connection = conexao;
                comando1.Transaction = transaction;

                comando1.CommandText = $"select * from {tabela}";                      
                SqlDataReader leitor = comando1.ExecuteReader(CommandBehavior.CloseConnection);

                if(leitor.HasRows){
                    

                    while(leitor.Read()){

                        Pessoa p = new Pessoa((string)leitor["nome"],(string)leitor["cpf"],(string)leitor["idade"]);
                
                        resultado.Add(p);                         
                    } 

                leitor.Close();
                closeconexaoBD();
                return resultado;    
                

                }
                else{
                    leitor.Close();
                    closeconexaoBD();
                    return resultado;
                    
                }
                
                
            }
            
        

        public List<Pessoa> pesquisarPessoa(string tabela, string nome="%",string cpf="%", string idade="%"){

            List<Pessoa> resultado = new List<Pessoa>();
            
            
                if (conexao.State != ConnectionState.Open){
                    conexao.Close();
                    conexao.Open();}               

                SqlCommand comando1 = conexao.CreateCommand();
                SqlTransaction  transaction;
                transaction = conexao.BeginTransaction("Selecionar Pessoa ");
                comando1.Connection = conexao;
                comando1.Transaction = transaction;
                comando1.CommandText = $"select * from {tabela} where cpf LIKE @cpf and  nome LIKE @nome and idade LIKE @idade";                
                comando1.Parameters.Add(new SqlParameter("@nome",nome));
                comando1.Parameters.Add(new SqlParameter("@cpf",cpf));
                comando1.Parameters.Add(new SqlParameter("@idade",idade));
                

                SqlDataReader leitor = comando1.ExecuteReader(CommandBehavior.CloseConnection);

                if(leitor.HasRows){
                    while(leitor.Read()){
                
                        Pessoa p = new Pessoa((string)leitor["nome"],(string)leitor["cpf"],(string)leitor["idade"]);
                
                        resultado.Add(p);
                         
                }                 

                    

                } 
                leitor.Close();
                closeconexaoBD();
                return resultado;
                    
                    
                }           
                
                                
         public void DeletarPessoa(string tabela, string nome,string cpf, string idade){                   
                

            try{
               
               
                if (conexao.State != ConnectionState.Open){
                    conexao.Close();
                    conexao.Open();}

                SqlCommand comando1 = conexao.CreateCommand();
                SqlTransaction  transaction;
                transaction = conexao.BeginTransaction("Deletar Pessoa");
                comando1.Connection = conexao;
                comando1.Transaction = transaction;

                try{
                    comando1.CommandText = $"Delete from {tabela} Where nome= @nome and cpf= @cpf and idade= @idade";        
                    
                    comando1.Parameters.Add(new SqlParameter("@nome",nome));
                    comando1.Parameters.Add(new SqlParameter("@cpf",cpf));
                    comando1.Parameters.Add(new SqlParameter("@idade",idade));                                     
                    comando1.ExecuteNonQuery();
                    

                    transaction.Commit();            
                    
                }catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

            
                    try
                    {
                     transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                    }}                

                

            } catch(SqlException e){
                System.Console.WriteLine("Instrucao não realizada" + e);
            }

        }

        public void AlterarPessoa(string tabela,string nome,string cpf, string idade){                   
                

            try{
               
               
                if (conexao.State != ConnectionState.Open){
                    conexao.Close();
                    conexao.Open();}

                SqlCommand comando1 = conexao.CreateCommand();
                SqlTransaction  transaction;
                transaction = conexao.BeginTransaction("Alterar Pessoa");
                comando1.Connection = conexao;
                comando1.Transaction = transaction;

                try{
                    comando1.CommandText = $"Update {tabela} Set nome= @novoNome, idade= @novaIdade  Where  cpf= @cpf";                  
                    comando1.Parameters.Add(new SqlParameter("@cpf",cpf));
                    comando1.Parameters.Add(new SqlParameter("@novoNome",nome));                    
                    comando1.Parameters.Add(new SqlParameter("@novaIdade",idade));                    
                    comando1.ExecuteNonQuery();
                    

                    transaction.Commit();            
                    
                }catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

            
                    try
                    {
                     transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                    }}                

                

            } catch(SqlException e){
                System.Console.WriteLine("Instrucao não realizada" + e);
            }

        }        

    }
}









