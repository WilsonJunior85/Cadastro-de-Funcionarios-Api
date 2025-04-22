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
    public class UploadS3Service
    {
        private readonly AWSSettings _awsSettings;
        private AmazonS3Client _s3Client;
        public UploadS3Service(AWSSettings awsSettings)
        {
            _awsSettings = awsSettings;
            _s3Client = new AmazonS3Client(new BasicAWSCredentials(_awsSettings.AccessKey, _awsSettings.SecretKey), RegionEndpoint.USEast1);

        }

        //private string ObterExtencao(IFormFile arquivo) => Path.GetExtension(arquivo.FileName).ToLowerInvariant();
        private string ObterExtencao(string fileName) => Path.GetExtension(fileName).ToLowerInvariant();

        private string GerarNome(IFormFile arquivo, bool gerarNome = false)
        {
            var fileName = gerarNome
                ? string.Concat(Guid.NewGuid().ToString(), ObterExtencao(arquivo.FileName))
                : ContentDispositionHeaderValue.Parse(arquivo.ContentDisposition).FileName;

            return fileName;
        }


        private string GeneratePreSignedURL(string objectKey)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _awsSettings.BucketName,
                Key = objectKey,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddHours(1)
            };

            string url = _s3Client.GetPreSignedURL(request);
            return url;
        }

        public virtual async Task<Result> Upload(IFormFile arquivo, bool gerarNome = false, string pasta = "", string[] estencoes = null)
        {

            var result = new ResultDomain();

            if (arquivo == null)
                result.AddNotification("arquivo", "Nenhum arquivo recebido");

            if (estencoes != null && estencoes.Contains(ObterExtencao(arquivo.FileName)))
                result.AddNotification("arquivo", "Tipo de arquivo não suportado");

            try
            {
                if (result.IsValid)
                {
                    var filePath = Path.GetTempFileName();
                    var pathToSave = new List<string>();
                    if (pasta != "")
                        pathToSave = pasta.Split("\"").ToList();

                    var fileName = GerarNome(arquivo, gerarNome);
                    if (pathToSave.Count == 0)
                    {
                        pathToSave.Add("upload_imagens");
                        pathToSave.Add(fileName);
                    }
                    else
                        pathToSave.Add(fileName);

                    var fullPatch = AWSSDKUtils.JoinResourcePathSegmentsV2(pathToSave);

                    using (Stream fileToUpload = arquivo.OpenReadStream())
                    {
                        var putObjectRequest = new PutObjectRequest();
                        putObjectRequest.BucketName = _awsSettings.BucketName;
                        putObjectRequest.Key = fullPatch;
                        putObjectRequest.InputStream = fileToUpload;
                        putObjectRequest.ContentType = arquivo.ContentType;

                        var response = await _s3Client.PutObjectAsync(putObjectRequest);
                        if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                        {
                            result.Data = fullPatch;
                        }
                        else
                        {
                            result.AddNotification("upload S3", "Ocorreu um erro ao realizar o upload");
                            result.Data = response.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.AddNotification("upload", "Ocorreu um erro ao realizar o upload");
                result.Data = ex;
            }

            return result;
        }

        public virtual async Task<Result> Remove(string caminho)
        {
            var result = new ResultDomain();
            caminho = HttpUtility.UrlDecode(caminho);
            if (caminho == null)
                result.AddNotification("arquivo", "Nenhum arquivo encontrado");

            try
            {
                if (result.IsValid)
                {
                    var response = await _s3Client.DeleteObjectAsync(_awsSettings.BucketName, caminho);
                }
            }
            catch (Exception ex)
            {
                result.AddNotification("upload", "Ocorreu um erro ao remover o arquivo");
                result.Data = ex;
            }

            return result;
        }
    }
}
