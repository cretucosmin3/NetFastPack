# NetFastPack

This project aims to simplify the use of [MessagePack](https://msgpack.org/index.html) in C# web APIs.

MessagePack is very performant and we can use it to improve and help scale our APIs, some considerations:
- Improve overall data transfer (with small models it also help with < 4g connections).
- Improve data parsing so your API can process more requests and cope better when highly loaded.
- Improve sending & parsing of large responses such as big listed & nested objects.

<br>

There are some solutions out there to implement MessagePack but this should give you a way with minimal hassle.

## Quick Start

I. You only need to call one method to configure the service into your API.

```csharp
public virtual void ConfigureServices(IServiceCollection services)
{
    services.AddNetFastPack();
}
```
<br>

II. To deserialize & serialize you need to use 2 attributes, `[ByteResponse]` and `[FromBytes]`.<br>
These two can be used independently e.g., if you only want to receive bytes but still want to return json.

```csharp
[ByteResponse]
[HttpPost("say-hi")]
public ActionResult UploadFromBytes([FromBytes] User user)
{
    return Ok($"Hi {user.FirstName}!");
}
```

- [ByteResponse] - response is packed to bytes.
- [FromBytes] - request data is unpacked from bytes to your type.

<br>

III. In order for all the above magic to work, you need to add these two attributes to your models, `[BytePacked]` and `[Ix]` these are `[MessagePackObject]` and `[Key]` from the MessagePack library, they're renamed so you don't have to import 2 libraries, also BytePacked is subjectively a better naming for our use case.

```csharp
[BytePacked]
public class User
{
    [Ix(0)]
    public string FirstName { get; set; }

    [Ix(1)]
    public string LastName { get; set; }

    [Ix(2)]
    public int Age { get; set; }
}
```

The order provided with [Ix] is important for packing!

<br>
That's all! I promised you minimal hassle.

---

### How does this black magic work behind the scene?
- `[ByteResponse]` - A filter is configured in your services to run last on your endpoint response, this uses MessagePack to pack the response body to an array of bytes. This ensures any kind of object will be serialized as long as you have your attributes in the right place.

- `[FromBytes]` - A model binder is configured to parse the request data to your required object, this custom model binder replaces the default binder.

<br>

## Future improvements
- JSON parsing on development so that you can develop in peace and read the content.
    - One main argument against this approach is that it's not human readable, in reality this is not a good argument because regardless of its shape, if the data doesn't match the schema, your endpoint should return a 415 (Unsupported Media Type).
- More tests to highlight the positives and negatives.

<br>

## Ok but what about performance?

To benchmark this the following models were used. <br>
Where the charts will mention data size, what that refers to is the number of documents added to the Account instance.

![Testing Models](https://github.com/cretucosmin3/NetFastPack/blob/main/Github/testing-data.png?raw=true)

<br>

#### Some things to keep in mind for all benchmarks below.
- No networking delays involved, all tested locally.
- Requests were sent for 6 seconds
- Test data was randomized once and used for both to make sure it's a fair comparison.
<br>

---

### Requests made Binary vs Json
As you can see, at every scale Binary is always better, but especially at very large objects.<br>
At 5120 documents, through binary 701 requests were made and only 127 with json, that is 5.5x

![Requests JSON vs Binary](https://github.com/cretucosmin3/NetFastPack/blob/main/Github/requests-chart.png?raw=true)

<br>

### What is the % difference?

![Difference](https://github.com/cretucosmin3/NetFastPack/blob/main/Github/requests-difference-chart.png?raw=true)

<br>

### How much data in bytes is being sent?
At 5120 documents in the main Account model:
- Binary = 234.180 bytes or **0.23 MB**  
- JSON = 998.450 bytes or **1mb**

Binary is close to a linear grouth while JSON is exponentially growing.

![Difference](https://github.com/cretucosmin3/NetFastPack/blob/main/Github/size-chart.png?raw=true)


<br>

## Future tests
More tests to record in the near future:
- Single threaded (on the internet)
- Multi-threaded requests (locally)
- Multi-threaded requests (on the internet)

