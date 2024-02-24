
using Example.Base;
using Example.Base.Types;
using Microsoft.AspNetCore.Mvc;
using NetFastPack;

namespace Example.Controllers;

[ApiController]
[Route("api/example")]
public class ExampleController : ApiControllerBase
{
    [HttpPost("bytes")]
    public ActionResult GetPersonFromBytes([FromPackedBytes] Person[] data)
    {
        if (data != null)
            return Ok($"Hello {data[0].FirstName}!");

        return Ok($"Who are you?");
    }

    [HttpPost("json")]
    public ActionResult GetPersonFromJson([FromBody] Person[] data)
    {
        if (data != null)
            return Ok($"Hello {data[0].FirstName}!", packed: false);

        return Ok($"Who are you?", packed: false);
    }

    [HttpGet("page")]
    public ActionResult Page()
    {
        return Ok($"Bob", packed: false);
    }
}