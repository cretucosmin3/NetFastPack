using System;
using System.Text;
using MessagePack;
using System.Diagnostics;
using Newtonsoft.Json;

public static class Program
{
    static MessagePackSerializerOptions Options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4Block);
    static readonly string Host = "localhost";
    static readonly string JsonURL = $"http://{Host}:5000/api/documents/upload-json";
    static readonly string BytesURL = $"http://{Host}:5000/api/documents/upload-bytes";
    static readonly int TestingTime = 6_000;
    static readonly int[] DocumentSizesTests = new int[] {0, 5, 10, 20, 40, 80, 160, 320, 640, 1280, 2560, 5120};

    private class TestResults
    {
        // Json
        public int JsonRequests = 0;
        public int JsonByteSize = 0;

        // Binary
        public int BinaryRequests = 0;
        public int BinaryByteSize = 0;

        public float RequestsFactor { get => (float)BinaryRequests / (float)JsonRequests; }
        public float SizeFactor { get => (float)BinaryByteSize / (float)JsonByteSize; }
    }

    private static HttpClient HttpClient = new HttpClient();

    public static void Main(string[] args)
    {
        Console.WriteLine("Press any key to start...");
        Console.ReadKey();

        Dictionary<int, TestResults> Results = new Dictionary<int, TestResults>();
        
        Warmup();

        foreach (var documents in DocumentSizesTests)
        {
            Console.WriteLine($"----- Testing account with {documents} documents");

            var testData = SampleData.RandomSampleAccount(documents);
            var testResults = TestWithData(testData);

            Console.WriteLine("");

            Results.Add(documents, testResults);
        }

        foreach (var results in Results)
        {
            var v = results.Value;
            Console.WriteLine($"docs: {results.Key} | binaryReq: {v.BinaryRequests} | binarySize: {v.BinaryByteSize} | jsonReq: {v.JsonRequests} | jsonSize: {v.JsonByteSize} | reqFactor: {v.RequestsFactor} | sizeFactor{v.SizeFactor}");
        }

        Console.WriteLine("Test Completed");
        Console.ReadKey();
    }

    private static Account TestData()
    {
        return SampleData.RandomSampleAccount(50);
    }

    private static void Warmup()
    {
        Console.WriteLine("Warmup started");

        var testData = TestData();

        var result = TestFor(5000, () =>
        {
            var task = PostJsonAsync(JsonURL, testData);
            task.Wait();
        });

        Console.WriteLine("Warmup Completed \n\n");
    }

    private static TestResults TestWithData(Account account)
    {
        var binaryResults = BinaryTest(account);
        var jsonResults = JsonTest(account);

        return new TestResults()
        {
            BinaryRequests = binaryResults.Requests,
            BinaryByteSize = binaryResults.ByteSize,
            JsonRequests = jsonResults.Requests,
            JsonByteSize = jsonResults.ByteSize
        };
    }

    private static (int Requests, int ByteSize) BinaryTest(Account data)
    {
        int byteCount = MessagePackSerializer.Serialize(data, options: Options).Length;
        
        Console.WriteLine($"--- Binary Test : {byteCount} bytes");

        var result = TestFor(TestingTime, () =>
        {
            byte[] arr = MessagePackSerializer.Serialize(data, options: Options);

            var task = PostBytesAsync(BytesURL, arr);
            task.Wait();

            string response = MessagePackSerializer.Deserialize<string>(task.Result, options: Options);
        });

        return (result.Counter, byteCount);
    }

    private static (int Requests, int ByteSize) JsonTest(Account data)
    {
        string jsonString = JsonConvert.SerializeObject(data).Replace(" ", "");
        int byteCount = Encoding.ASCII.GetByteCount(jsonString);

        Console.WriteLine($"--- JSON Test : {byteCount} bytes");

        var result = TestFor(TestingTime, () =>
        {
            var task = PostJsonAsync(JsonURL, data);
            task.Wait();
        });

        return (result.Counter, byteCount);
    }

    private static (int Counter, float TimePerAction) TestFor(float time, Action act)
    {
        Stopwatch timeTracker = new();
        Stopwatch actionTimer = new();

        int counter = 0;
        float actionTotal = 0;

        timeTracker.Start();

        while (timeTracker.ElapsedMilliseconds <= time)
        {
            actionTimer.Restart();

            act.Invoke();

            actionTimer.Stop();

            actionTotal += actionTimer.ElapsedMilliseconds;

            counter++;
        }

        return new(counter, actionTotal / (float)counter);
    }

    private static async Task<byte[]> PostBytesAsync(string uri, byte[] byteArray)
    {
        using (var content = new ByteArrayContent(byteArray))
        {
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-msgpack");

            try
            {
                HttpResponseMessage response = await HttpClient.PostAsync(uri, content);

                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsByteArrayAsync();

                return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

        return [];
    }

    private static async Task<string> PostJsonAsync<T>(string uri, T dataObject)
    {
        string jsonString = JsonConvert.SerializeObject(dataObject).Replace(" ", "");

        using (var content = new StringContent(jsonString, Encoding.UTF8, "application/json"))
        {
            try
            {
                HttpResponseMessage response = await HttpClient.PostAsync(uri, content);

                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

        return string.Empty;
    }
}