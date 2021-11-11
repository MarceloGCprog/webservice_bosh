using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace wsteste
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Configurando o serviço de documentação do Swagger
            //É necessário adicionar o pacote: dotnet add package Swashbuckle.AspNetCore
            //Acesse pelo browser: http://localhost:5000/swagger/index.html
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Desenvolvimento de Back-End",
                    Description = "Primeiro serviço em REST desenvolvido na disciplina!",
                    Contact = new OpenApiContact
                    {
                        Name = "Vilmar Abreu Junior",
                        Email = "vilmar.abreu@pucpr.br",
                        Url = new Uri("https://www.linkedin.com/in/vilmar-abreu-junior-242927159/"),
                    },
                    TermsOfService = new Uri("https://example.com/terms")
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Insira essas linhas para habilitar o swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Primeiro webservice em REST");
            });
            //Fim do código para habilitar o swagger

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            //Desabilitar o HTTPS para evitar problemas de certificado
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
