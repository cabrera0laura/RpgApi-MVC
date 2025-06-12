using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
//using AutenticationHeaderValue; // tlvs sj erro
using System.Net.Http.Headers;
using System.Net.Http;
using System.Collections.Generic;
using RpgMvc.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace RpgMvc.Controllers
{
    [Route("[controller]")]
    public class PersonagensController : Controller
    {
        public string uriBase = "http://salghoseok.somee.com/rpgapi/personagens/";


        private readonly ILogger<PersonagensController> _logger;

        public PersonagensController(ILogger<PersonagensController> logger)
        {
            _logger = logger;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        [HttpGet]
        public async Task<ActionResult> IndexAsync()
        {
            try
            {
                string uriComplementar = "GetAll"; //1
                HttpClient httpClient = new HttpClient(); //2
                string token = HttpContext.Session.GetString("SessionTokenUsuario"); //3
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); //4

                HttpResponseMessage response = await httpClient.GetAsync(uriBase + uriComplementar);
                string serialized = await response.Content.ReadAsStringAsync();

                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<PersonagemViewModel> listapersonagens  = await Task.Run(() => JsonConvert.DeserializeObject<List<PersonagemViewModel>>(serialized));
                    return View(listapersonagens);
                }
                else
                    throw new System.Exception(serialized);

            }
            catch (System.Exception ex)
            {
                TempData["MensgemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

    }
}