
using NetFastPack;
using Example.Base.Types;
using Microsoft.AspNetCore.Mvc;

namespace Example.Controllers;

[ApiController]
[Route("api/documents")]
public class ExampleController : ControllerBase
{
    [ByteResponse]
    [HttpGet("bytes")]
    public ActionResult GetDocumentsInBytes()
    {
        return Ok(SampleData.RandomSampleAccount(20));
    }

    [HttpGet("json")]
    public ActionResult GetDocumentsInJson()
    {
        return Ok(SampleData.RandomSampleAccount(20));
    }

    [ByteResponse]
    [HttpPost("upload-bytes")]
    public ActionResult UploadFromBytes([FromBytes] Account account)
    {
        return Ok($"Hello {account.FirstName}!");
    }

    [HttpPost("upload-json")]
    public ActionResult UploadFromJson([FromBody] Account account)
    {
        return Ok($"Hello {account.FirstName}!");
    }
}