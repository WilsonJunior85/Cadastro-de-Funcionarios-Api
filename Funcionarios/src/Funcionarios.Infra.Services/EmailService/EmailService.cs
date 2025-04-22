using Funcionarios.Domain.Results;
using Funcionarios.Infra.Services.EmailService.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.Infra.Services.EmailService
{
    public class EmailService
    {
        private readonly EmailSettings _configuration;

        public EmailService(EmailSettings configuration)
        {
            _configuration = configuration;
        }

        public async Task<ResultDomain> EnviarEmail(List<string> emails, string titulo, string corpo) => await EnviarEmail(emails, titulo, corpo, null, null);

        public async Task<ResultDomain> EnviarEmail(List<string> emails, string titulo, string corpo, byte[] atachment, string atachmentName)
        {
            var result = new ResultDomain();
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration.Remetente, _configuration.Nome),
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };

            mailMessage.Headers.Add("X-Sender", _configuration.CabecarioEnvio);
            mailMessage.Subject = titulo;
            mailMessage.Body = corpo;

            SmtpClient client = new SmtpClient(_configuration.Smtp, _configuration.Porta);

            if (atachment != null)
            {
                var stream = new MemoryStream(atachment);
                mailMessage.Attachments.Add(new Attachment(stream, atachmentName));
            }

            foreach (var i in emails)
            {
                if (!string.IsNullOrEmpty(i))
                {
                    var emailSplit = i.Split(";");
                    if (emailSplit.Length > 1)
                    {
                        foreach (var ii in emailSplit)
                        {
                            if (!string.IsNullOrEmpty(ii))
                            {
                                try
                                {
                                    mailMessage.To.Clear();
                                    mailMessage.To.Add(ii);
                                    await client.SendMailAsync(mailMessage);
                                }
                                catch (Exception e)
                                {
                                    result.AddNotification(ii, "Email não pode ser enviado");
                                }
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            mailMessage.To.Clear();
                            mailMessage.To.Add(i);
                            await client.SendMailAsync(mailMessage);
                        }
                        catch (Exception e)
                        {
                            result.AddNotification(i, "Email não pode ser enviado");
                        }

                    }
                }
            }
            return result;
        }
    }
}
