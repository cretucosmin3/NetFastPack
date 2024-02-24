using MessagePack;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Example.Base.Types;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Example.Base;

public class ApiControllerBase : ControllerBase
{
    public ActionResult Ok(object value, bool packed = true)
    {
        if (packed)
        {
            var data = MessagePackSerializer.Serialize(value);

            return new FileContentResult(data, "application/octet-stream");
        }

        return base.Ok(value);
    }
}