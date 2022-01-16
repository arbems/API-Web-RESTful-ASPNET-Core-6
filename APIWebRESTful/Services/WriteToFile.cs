using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace APIWebRESTful.Services
{
    public class WriteToFile : IHostedService
    {
        private readonly IWebHostEnvironment env;

        public WriteToFile(IWebHostEnvironment env)
        {
            this.env = env;
        }

        /*
          Contiene la lógica para iniciar la tarea en segundo plano.
             StartAsync antes de que:
                  La canalización de procesamiento de solicitudes de la aplicación está configurada.
                  El servidor se haya iniciado.
         */
        Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            Write("Proceso iniciado.");

            return Task.CompletedTask;
        }

        /*
          Se activa cuando el host está realizando un cierre estable.
         */
        Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            Write("Proceso finalizado.");

            return Task.CompletedTask;
        }

        void Write(string msg)
        {
            var root = $@"{env.ContentRootPath}/wwwroot/test.txt";
            using (StreamWriter writer = new StreamWriter(root, append: true))
            {
                writer.WriteLine(msg);
            }
        }
    }
}
