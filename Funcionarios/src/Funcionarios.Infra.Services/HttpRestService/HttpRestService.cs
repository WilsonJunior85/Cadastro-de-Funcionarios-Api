using Funcionarios.Domain.Results;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.Infra.Services.HttpRestService
{
    public class HttpRestService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string _token;
        public HttpRestService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _token = GetAccessToken();
        }

        /// <summary>
        /// For getting the resources from a web api
        /// </summary>
        /// <param name="url">API Url</param>
        /// <returns>A Task with result object of type T</returns>
        public async Task<Result> Get(string url)
        {
            var result = new ResultDomain();
            using (var client = new HttpClient())
            {
                if (_token != null)
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", _token);
                }

                var response = client.GetAsync(new Uri(url)).Result;

                response.EnsureSuccessStatusCode();
                await response.Content.ReadAsStringAsync().ContinueWith((x) =>
                {
                    if (x.IsFaulted)
                    {
                        result.AddNotification("Get", "Ocorreu um erro na solicitação");
                        result.Data = x.Exception;
                    }

                    try
                    {
                        result = JsonConvert.DeserializeObject<ResultDomain>(x.Result);
                    }
                    catch (Exception e)
                    {
                        result.Data = JsonConvert.DeserializeObject(x.Result);
                    }


                });
            }

            return result;
        }


        /// <summary>
        /// For getting the resources from a web api
        /// </summary>
        /// <param name="url">API Url</param>
        /// <returns>A Task with result object of type T</returns>
        public async Task<Result> DownloadFile(string url)
        {
            var result = new ResultDomain();
            using (var client = new HttpClient())
            {
                if (_token != null)
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", _token);
                }
                using (var get = await client.GetAsync(url))
                {
                    if (get.IsSuccessStatusCode)
                    {
                        result.Data = await get.Content.ReadAsByteArrayAsync();
                    }
                    else
                    {
                        result.AddNotification("DownloadFile", "Ocorreu um erro na solicitação");
                        result.Data = get.StatusCode;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// For creating a new item over a web api using POST
        /// </summary>
        /// <param name="apiUrl">API Url</param>
        /// <param name="postObject">The object to be created</param>
        /// <returns>A Task with created item</returns>
        public async Task<Result> PostRequest(string apiUrl, object postObject)
        {

            var json = JsonConvert.SerializeObject(postObject, Formatting.Indented);
            var stringContent = new StringContent(json);

            ResultDomain result = null;

            using (var client = new HttpClient())
            {
                if (_token != null)
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", _token);
                }
                var response = await client.PostAsync(apiUrl, stringContent).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                await response.Content.ReadAsStringAsync().ContinueWith((x) =>
                {
                    if (x.IsFaulted)
                    {
                        result.AddNotification("PostRequest", "Ocorreu um erro na solicitação");
                        result.Data = x.Exception;
                    }

                    try
                    {
                        result = JsonConvert.DeserializeObject<ResultDomain>(x.Result);
                    }
                    catch (Exception e)
                    {
                        result.Data = JsonConvert.DeserializeObject(x.Result);
                    }

                });
            }

            return result;
        }

        /// <summary>
        /// For updating an existing item over a web api using PUT
        /// </summary>
        /// <param name="apiUrl">API Url</param>
        /// <param name="putObject">The object to be edited</param>
        public async Task PutRequest(string apiUrl, object putObject)
        {
            var json = JsonConvert.SerializeObject(putObject, Formatting.Indented);
            var stringContent = new StringContent(json);

            using (var client = new HttpClient())
            {
                if (_token != null)
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", _token);
                }

                var response = await client.PutAsync(apiUrl, stringContent).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();
            }
        }

        private string GetAccessToken()
        {
            var token = _contextAccessor.HttpContext.Request.Headers["Authorization"];
            if (token.Any())
            {
                token = token.First().Replace("Authorization ", "");
                return token;
            }
            return null;
        }
    }
}
