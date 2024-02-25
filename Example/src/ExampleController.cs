
using NetFastPack;
using Example.Base.Types;
using Microsoft.AspNetCore.Mvc;

namespace Example.Controllers;

[ApiController]
[Route("api/example")]
public class ExampleController : ControllerBase
{
    [BytePacked]
    [HttpPost("bytes")]
    public ActionResult GetPersonFromBytes([FromPackedBytes] Person person)
    {
        return Ok($"Hello {person.FirstName}!");
    }

    [HttpPost("json")]
    public ActionResult GetPersonFromJson([FromBody] Person person)
    {
        return Ok($"Hello {person.FirstName}!");
    }
}