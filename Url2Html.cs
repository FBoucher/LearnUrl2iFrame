using System.Net;
using System.Net.Mime;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace c5m.Functions
{
    public class Url2Html
    {
        private readonly ILogger _logger;

        public Url2Html(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Url2Html>();
        }

        [Function("Url2Html")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", "get")] HttpRequest req)
        {
            try{
                string url = string.Empty;
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                if (!string.IsNullOrEmpty(requestBody)) {
                    url = requestBody.Split('=').Last<string>();
                }

                if(string.IsNullOrEmpty(url))
                {
                    url = req.Query["url"];
                }

                var urlDecoded = HttpUtility.UrlDecode(url);
                var urlParts = urlDecoded.Split('/');
                string episode = urlParts.Last<string>();
                string showName = urlParts[urlParts.Length -2];

                var iframeCode = $"<iframe width='560' height='315' src='https://learn-video.azurefd.net/vod/player?show={showName}&ep={episode}&embedUrl=contoso.net/builders/cool-ai-resources/' title='Learn video player' frameborder='0' allow='accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share' allowfullscreen></iframe>";
                
                var htmlResponse = $"<html><body>Embed Video:<br/><textarea rows='10' cols='80'>{iframeCode}</textarea><br/><br/><button onclick='window.close()'>Close</button></body><html>";
                var content = new ContentResult { Content = $"{htmlResponse}", ContentType = "text/html" };
                return content;
            }
            catch(Exception ex){
                return new ContentResult { Content = $"Oops! Error: {ex.Message}", ContentType = "text/html" };
            }
            
        }
    }
}
