using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;

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
            //System.Console.WriteLine("Conexao aberta");  
            } catch(SqlException e){
                 Console.WriteLine($"Erro SQL: {e}"); 
                 conexao.Close();
            }
            
        }

         public void closeconexaoBD(){
            //String strconexao =Server =localhost; Database=hotel_idris; Trusted_Connection=True;";

            
            conexao.Close();
            System.Console.WriteLine("Conexao Fechada"); 
        }

        public void AdicionarPessoa(string tabela,string nome, string cpf, int idade){                   
                

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
                    comando1.Parameters.Add(new SqlParameter("@nome",nome));
                    comando1.Parameters.Add(new SqlParameter("@cpf",cpf));
                    comando1.Parameters.Add(new SqlParameter("@idade",idade));
                    comando1.ExecuteNonQuery();
                    

                    transaction.Commit();            
                    //System.Console.WriteLine("Pessoa inserida com sucesso.");
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

        public void pesquisarTodasPessoas(string tabela){

            
            try{                               
               
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
                
                        System.Console.WriteLine($"nome:"+ leitor["nome"],"cpf:"+ leitor["cpf"],"idade:"+ leitor["idade"]);
                         
                } 
                    leitor.Close();

                }else{
                    leitor.Close();
                    System.Console.WriteLine("Tabela Vazia");
                    
                }
                closeconexaoBD(); 
            }
            catch(SqlException e){
                System.Console.WriteLine("Erro na leitura dos dados!"+ e);
                closeconexaoBD();                 
            }
        }

        public void pesquisarPessoa(string tabela, string nome="",string cpf="", string idade=""){

            
            try{                               
               
                if (conexao.State != ConnectionState.Open){
                    conexao.Close();
                    conexao.Open();}               

                SqlCommand comando1 = conexao.CreateCommand();
                SqlTransaction  transaction;
                transaction = conexao.BeginTransaction("Selecionar Pessoa pelo Nome");
                comando1.Connection = conexao;
                comando1.Transaction = transaction;

                comando1.CommandText = $"select * from {tabela} where nome=@nome";
                comando1.Parameters.Add(new SqlParameter("@nome",nome));

                SqlDataReader leitor = comando1.ExecuteReader(CommandBehavior.CloseConnection);

                if(leitor.HasRows){
                    while(leitor.Read()){
                
                        System.Console.WriteLine($"nome:"+ leitor["nome"],"cpf:"+ leitor["cpf"],"idade:"+ leitor["idade"]);
                         
                } 
                    leitor.Close();

                }else{
                    leitor.Close();
                    System.Console.WriteLine("Tabela Vazia");
                    
                }
                closeconexaoBD(); 
            }
            catch(SqlException e){
                System.Console.WriteLine("Erro na leitura dos dados!"+ e);
                closeconexaoBD();                 
            }
        }

        

        //++++++++++++++++++++++++++++++++++++++++++++++++++
       
        static void Main(string[] args)
        {
            
            bancodeDados bd = new bancodeDados();

            bd.conexaoBD("localhost","Sexta");
            
            bd.pesquisarALuno("Aluno");

            bd.closeconexaoBD();                   

            System.Console.WriteLine("Fim do programa!!");
            
        }

    }
}








