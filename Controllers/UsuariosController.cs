using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GastosAPIMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GastosAPIMvc.Controllers
{
    public class UsuariosController : Controller
    {
         public string uriBase= "http://localhost:5246/usuarios/";

        [HttpGet]
        public ActionResult Index()
        {
            return View("CadastrarUsuarios");
        }

        
        [HttpPost]
        public async Task<ActionResult> RegistrarAsync(UsuarioViewModel u)
        {
            try
            {
             HttpClient httpClient = new HttpClient();
             string uriComplementar = "Registrar";

             var content = new StringContent(JsonConvert.SerializeObject(u));
             content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("applications/json");
             HttpResponseMessage response = await httpClient.PostAsync(uriBase + uriComplementar, content);

             string serialized = await response.Content.ReadAsStringAsync();

             if (response.StatusCode == System.Net.HttpStatusCode.OK)
             {
                TempData["Mensagem"] = 
                string.Format("Usuario {0} Registrado com sucesso! faça o login para acessar.", u.Username);
                return View("AutenticarUsuario");
             }
             else {
                throw new System.Exception(serialized);
             }
            }



            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

         [HttpPost]
        public async Task<ActionResult> AutenticarAsync(UsuarioViewModel u)
        {
            try
            {
                HttpClient httpClient = new  HttpClient();
                string uriComplementar = "Autenticar";

                var content = new StringContent(JsonConvert.SerializeObject(u));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await httpClient.PostAsync(uriBase + uriComplementar, content);

                string serialized = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    UsuarioViewModel uLogado = JsonConvert.DeserializeObject<UsuarioViewModel>(serialized);
                    HttpContext.Session.SetString("SessionTokenUsuario", uLogado.Token);
                    TempData["Mensagem"] = string.Format("Bem vindo {0}!!!", uLogado.Username);
                    return RedirectToAction("Index");
                }
                else{
                    throw new System.Exception(serialized);
                }
            }
            catch (System.Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                //return IndexLogin();              
                return RedirectToAction("IndexLogin");
            }
        }


        [HttpGet]
        public ActionResult IndexLogin()
        {
            return View("AutenticarUsuario");
        }
    }
}