using Alamut.Abstractions.Structure;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BehinRahkar.Catalog.Application.Product.Commands
{

    public class SaveFileCommand : IRequest<Result>
    {
        public SaveFileCommand(IFormFile file, string filename)
        {
            File = file;
            FileName = filename;
        }

        public IFormFile File { get; }
        public string FileName { get; }
    }

    public class SaveFileCommandHandler : IRequestHandler<SaveFileCommand, Result>
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<SaveFileCommandHandler> _logger;

        public SaveFileCommandHandler(IHostingEnvironment hostingEnvironment, ILogger<SaveFileCommandHandler> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public async Task<Result> Handle(SaveFileCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (command.File == null) return Result.Okay();

                var directory = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads");
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (FileStream filestream = File.Create(Path.Combine(directory, command.FileName)))
                {
                    await command.File.CopyToAsync(filestream, cancellationToken);
                    filestream.Flush();
                }

                return Result.Okay();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Exception was occured in save file with name:{command.FileName}=>{exception.Message}");
                throw;
            }
        }
    }
}
