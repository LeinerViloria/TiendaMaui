
using Backend.DTOS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace Backend.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var Headers = context.HttpContext.Request.Headers;

        try
        {
            var Token = Headers.Authorization[0]!.Split(' ')[^1];

            if(string.IsNullOrEmpty(Token))
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var Request = client.PostAsync("https://pruebax-091bcc393168.herokuapp.com/validatetoken", null)
                .GetAwaiter()
                .GetResult();

            var Result = Request.Content.ReadAsStringAsync()
                .GetAwaiter()
                .GetResult();

            var Json = JObject.Parse(Result);
            var Data = Json["data"]!.ToObject<UserDTO>();

            context.HttpContext.Items["TokenValidationResult"] = Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            context.Result = new JsonResult(new { message = "Invalid token" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}