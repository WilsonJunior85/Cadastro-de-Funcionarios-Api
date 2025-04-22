using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.Infra.Services.FileService
{
    public class FileDownload
    {
        public string FileName { get; private set; }
        public string ContentType { get; private set; }
        public MemoryStream Data { get; private set; }

        public FileDownload(string fileName, string contentType, MemoryStream data)
        {
            FileName = fileName;
            ContentType = contentType;
            Data = data;
        }
    }
}
