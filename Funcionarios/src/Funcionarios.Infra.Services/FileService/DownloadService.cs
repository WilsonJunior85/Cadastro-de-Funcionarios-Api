using Funcionarios.Domain.Results;
using Funcionarios.Infra.Services.FileService.Settings;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funcionarios.Infra.Services.FileService.Settings;

namespace Funcionarios.Infra.Services.FileService
{
    public class DownloadService
    {
        private readonly FileConf _fileConf;
        private readonly IConfiguration _configuration;
        public DownloadService(FileConf fileConf)
        {
            _fileConf = fileConf;
        }

        public async Task<FileDownload> Download(string arquivo, string pasta = "")
        {
            var result = new Result();

            if (arquivo != null)
                result.AddNotification("arquivo", "Nenhum arquivo informado");

            var path = Path.Combine(_configuration.GetSection("ApplicationSettings:DiretorioArquivos").Value, pasta, arquivo);
            //var path = GetPath(arquivo, pasta);
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return new FileDownload(Path.GetFileName(path), GetContentType(path), memory);
        }

        public async Task<Byte[]> View(string arquivo, string pasta = "")
        {
            var result = new ResultDomain();

            if (arquivo != null)
                result.AddNotification("arquivo", "Nenhum arquivo informado");

            var path = GetPath(arquivo, pasta);

            var b = System.IO.File.ReadAllBytes(path);
            return b;
        }


        public string GetContentType(string arquivo, string pasta = "")
        {
            var path = GetPath(arquivo, pasta);
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        public string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        protected string GetPath(string arquivo, string pasta = "")
        {
            return Path.Combine(_fileConf.DiretorioArquivos, pasta, arquivo);
        }

        protected virtual Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
