using ASPWebAPIDemo.Models;
using ASPWebAPIDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Configuration;

using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;

namespace ASPWebAPIDemo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CatController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public CatController(IConfiguration config)
        {
            this.configuration = config;
        }

        [HttpGet]
        public ActionResult<List<Cat>> GetAll() => CatService.GetAll();

        [HttpGet("{id}")]
        public ActionResult<Cat> Get(int id) => CatService.GetAll().FirstOrDefault(x => x.Id == id);
        [HttpGet]
        [Route("mycfg")]
        public ActionResult<String> GetConfig()
        {
            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                 }
            };

            var client = new SecretClient(new Uri("https://ryandeploykeyvault.vault.azure.net/"), 
                new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = "0bceb401-b0de-4f63-8006-01cf6943eadf" }), options);
           
            KeyVaultSecret secret;
            try
            {
                secret = client.GetSecret("mykeyvaultkey");

            }
            catch (AuthenticationFailedException ex)
            {
                return $"Error: {ex.Message}";
            }

            string secretValue = secret.Value;
            string appSettingValue = configuration.GetSection("key1").Value;
            string connetionString = configuration.GetConnectionString("MySqlServerDB");
            return $"Appsettings: {appSettingValue}; Secret:{secretValue}; ConnectionString:{connetionString}";

        }

        [HttpPost]
        public IActionResult Create(Cat cat)
        {
            if(cat == null)
            {
                return BadRequest();
            }

            CatService.Add(cat);
            return CreatedAtAction(nameof(Create), new {id = cat.Id}, cat);
        }

        /*[HttpPost(Name = "Create2")]
        public IActionResult Create2(Cat cat)
        {
            CatService.Add(cat);
            return CreatedAtAction(nameof(Create), new { id = cat.Id }, cat);
        }*/
    }
}
