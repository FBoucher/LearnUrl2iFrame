using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace c5m.Functions
{
    public class UrlForm
    {
        private readonly ILogger _logger;

        public UrlForm(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<UrlForm>();
        }


        [Function("UrlForm")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            var htmlResponse = $"<html><body><form action='/api/Url2Html' method='post'>URL: <input id='txtUrl' name='url' type='text' size='50' /><input type='Submit' value='Submit' /></form></html>";
            var content = new ContentResult { Content = $"{htmlResponse}", ContentType = "text/html" };

            return content;
        }
    }
}
