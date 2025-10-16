using Microsoft.AspNetCore.Mvc;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.AspNetCoreServer;

namespace Caprim.API.Serverless.Controllers;

[Route("api/[controller]")]
public class ValuesController : ControllerBase
{
    // GET api/values
    [HttpGet]
    public IActionResult Get() // <--- Mueve la l�gica a este m�todo
    {
        var apiGatewayRequest = (APIGatewayProxyRequest)HttpContext?.Items?["LambdaRequestObject"];
        var claims = apiGatewayRequest?.RequestContext?.Authorizer?.Claims;

        if (claims == null)
        {
            return BadRequest("No se encontraron los claims del autorizador.");
        }

        if (claims.TryGetValue("email", out var email))
        {
            return Ok(new { Email = email });
        }
        else
        {
            return NotFound("El claim 'email' no fue encontrado en el token.");
        }
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        return Ok($"value{id}");
    }

    // POST api/values
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}