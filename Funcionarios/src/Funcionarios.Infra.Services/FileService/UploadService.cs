using Funcionarios.Domain.Results;
using Funcionarios.Infra.Services.FileService.Settings;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.Infra.Services.FileService
{
    public class UploadService
    {
        private readonly FileConf _fileConf;
        public UploadService(FileConf fileConf)
        {
            _fileConf = fileConf;
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

        private bool VerificarDiretorio(string caminho)
        {
            if (!Directory.Exists(caminho))
            {
                var a = Directory.CreateDirectory(caminho);
                return a.Exists ? true : false;
            }

            return true;
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
                    var pathToSave = GetPath(pasta);
                    var fileName = GerarNome(arquivo, gerarNome);
                    var fullPatch = Path.Combine(pathToSave, fileName.Replace("\"", "").Trim());


                    if (!VerificarDiretorio(pathToSave))
                        result.AddNotification("diretorio", "Diretório não existe e não pode ser criado");

                    if (result.IsValid && arquivo?.Length > 0)
                    {
                        using (var stream = new FileStream(fullPatch, FileMode.OpenOrCreate))
                        {
                            await arquivo.CopyToAsync(stream);
                        }

                        result.Data = fileName;
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

            if (caminho == null)
                result.AddNotification("arquivo", "Nenhum arquivo encontrado");

            try
            {
                if (result.IsValid)
                {

                    var pathToSave = GetPath("");
                    var fileName = caminho;
                    var fullPatch = Path.Combine(pathToSave, fileName.Replace("\"", "").Trim());


                    if (!VerificarDiretorio(pathToSave))
                        result.AddNotification("diretorio", "Diretório não existe e não pode ser criado");

                    if (result.IsValid)
                    {
                        File.Delete(fullPatch);
                    }
                }
            }
            catch (Exception ex)
            {
                result.AddNotification("upload", "Ocorreu um erro ao remover o arquivo");
                result.Data = ex;
            }

            return result;
        }

        protected string GetPath(string pasta = "") => Path.Combine(_fileConf.DiretorioArquivos, pasta);
    }
}
