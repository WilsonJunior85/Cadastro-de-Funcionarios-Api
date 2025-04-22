using Amazon.Runtime;
using Amazon.S3.Model;
using Amazon.S3;
using Amazon.Util;
using Funcionarios.Domain.Results;
using Funcionarios.Infra.Services.FileService.Settings;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Amazon;

namespace Funcionarios.Infra.Services.FileService
{
    public class DownloadS3Service
    {
        private readonly AWSSettings _awsSettings;
        private AmazonS3Client _s3Client;
        public DownloadS3Service(AWSSettings awsSettings)
        {
            _awsSettings = awsSettings;
            _s3Client = new AmazonS3Client(new BasicAWSCredentials(_awsSettings.AccessKey, _awsSettings.SecretKey), RegionEndpoint.USEast1);
        }


        public async Task<FileDownload> Download(string arquivo, string pasta = "")
        {
            var result = new ResultDomain();

            arquivo = HttpUtility.UrlDecode(arquivo);

            if (arquivo == null)
                result.AddNotification("arquivo", "Nenhum arquivo informado");

            // Create a GetObject request
            var request = new GetObjectRequest
            {
                BucketName = _awsSettings.BucketName,
                Key = arquivo,
            };

            try
            {
                // Issue request and remember to dispose of the response
                using GetObjectResponse response = await _s3Client.GetObjectAsync(request);


                var memory = new MemoryStream();

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    await response.ResponseStream.CopyToAsync(memory);
                    memory.Position = 0;
                    return new FileDownload(Path.GetFileName(arquivo), GetContentType(arquivo), memory);
                }
                return null;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error saving {arquivo}: {ex.Message}");
                return null;
            }
        }

        public async Task<byte[]> View(string arquivo, string pasta = "")
        {
            var result = new ResultDomain();
            arquivo = HttpUtility.UrlDecode(arquivo);

            var key = new List<string>();
            key = arquivo.Split("\"").ToList();

            if (key.Count == 1)
            {
                key.Insert(0, "upload_imagens");
            }
            else
            {
                if (key[0] != "upload_imagens")
                {
                    key.Insert(0, "upload_imagens");
                }
            }

            var fullkey = AWSSDKUtils.JoinResourcePathSegmentsV2(key);

            fullkey = HttpUtility.UrlDecode(fullkey);

            var request = new GetObjectRequest
            {
                BucketName = _awsSettings.BucketName,
                Key = fullkey,
            };

            try
            {
                // Issue request and remember to dispose of the response
                using GetObjectResponse response = await _s3Client.GetObjectAsync(request);

                var memory = new MemoryStream();

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await response.ResponseStream.CopyToAsync(ms);
                        return ms.ToArray();
                    }
                }
                return null;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error {fullkey}: {ex.Message}");
                return null;
            }
        }

        public string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
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
                {".csv", "text/csv"},
                {".zip","application/zip"},

                {".mp4","video/mp4"},
                {".m4v","video/m4v"},
                {".ogg","video/ogg"},
                {".ogv","video/ogg"},
                {".webm","video/webm"}

            };
        }
    }
}
