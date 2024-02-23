using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MessagePack;
using ProtoBuf;
using Google.Protobuf;
using System.Diagnostics;
using Newtonsoft.Json;

public static class Program
{
    private static readonly string Host = "localhost";
    private static readonly string JsonURL = $"http://{Host}:5000/json";
    private static readonly string BytesURL = $"http://{Host}:5000/bytes";

    private static HttpClient HttpClient = new HttpClient();

    public static void Main(string[] args)
    {
        Console.WriteLine("Press any key to start...");
        Console.ReadKey();

        // 5 threads
        new Thread(BytesTest).Start();
        new Thread(BytesTest).Start();

        Thread.Sleep(15_000);

        Console.WriteLine("Test Completed");
        Console.ReadKey();
    }

    private static Person[] TestData()
    {
        List<Person> personList = new();

        for (int i = 0; i < 4000; i++)
        {
            personList.Add(Person.GenerateRandomPerson(5));
        }

        return personList.ToArray();
    }

    private static void BytesTest()
    {
        Console.WriteLine("Starting Binary test...");
        Console.WriteLine("--------------------------");

        var testData = TestData();

        int byteCount = MessagePackSerializer.Serialize(testData).Length;
        Console.WriteLine($"Testing with requests of {byteCount} bytes");
        // Console.WriteLine();

        var result = TestFor(10_000, () =>
        {
            byte[] arr = MessagePackSerializer.Serialize(testData);

            var task = PostBytesAsync(BytesURL, arr);
            task.Wait();

            var packet = MessagePackSerializer.Deserialize<ApiResponse>(task.Result);
            string response = MessagePackSerializer.Deserialize<string>(packet.Data);
        });

        // Console.WriteLine("Results:");
        // Console.WriteLine("-----------------------");
        Console.WriteLine($"{result.Counter} requests made");
        Console.WriteLine($"{result.TimePerAction:0.00} average request time");
    }

    private static void JsonTest()
    {
        Console.WriteLine("Starting JSON test...");
        Console.WriteLine("--------------------------");

        var testData = TestData();

        string jsonString = JsonConvert.SerializeObject(testData).Replace(" ", "");

        int byteCount = Encoding.ASCII.GetByteCount(jsonString);
        Console.WriteLine($"Testing with requests of {byteCount} bytes");
        // Console.WriteLine();

        var result = TestFor(10_000, () =>
        {
            var task = PostJsonAsync(JsonURL, testData);
            task.Wait();
        });

        // Console.WriteLine("Results:");
        // Console.WriteLine("-----------------------");
        Console.WriteLine($"{result.Counter} requests made");
        Console.WriteLine($"{result.TimePerAction:0.00} average request time");
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
            // Optionally, add any headers to the content. For example, content-type if it's known
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

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

[MessagePackObject]
public class ApiResponse
{
    [Key(0)]
    public byte[] Data { get; set; } = [];

    [Key(1)]
    public List<string> Messages { get; set; } = null!;

    [Key(2)]
    public int Success { get; set; }
}

[MessagePackObject]
public class Person
{
    [Key(0)]
    public int Age { get; set; } = 25;

    [Key(1)]
    public string FirstName { get; set; } = "Bob";

    [Key(2)]
    public string LastName { get; set; } = "Bob";

    [Key(3)]
    public string Address { get; set; } = "St. Jon 14";

    [Key(4)]
    public bool CanLogin { get; set; } = true;

    [Key(5)]
    public bool IsDeleted { get; set; } = true;

    [Key(6)]
    public int LoginAttemptsLeft { get; set; } = 10;

    [Key(7)]
    public int TopScore { get; set; } = 10;

    [Key(8)]
    public List<Game> Games { get; set; } = new List<Game>();

    public static Person GenerateRandomPerson(int games)
    {
        Random random = new Random();

        string[] firstNames = { "Alice", "Bob", "Charlie", "Diana", "Edward", "Michael", "John", "Richard" };
        string[] lastNames = { "Smith", "Johnson", "Williams", "Jones", "Brown", "Robb", "Kelly" };
        string[] addresses = { "St. Jon 14", "Elm Street 5", "Maple Avenue 23", "Oak Road 11", "Pine Lane 6" };

        var person = new Person
        {
            Age = random.Next(18, 100),
            FirstName = firstNames[random.Next(firstNames.Length)],
            LastName = lastNames[random.Next(lastNames.Length)],
            Address = addresses[random.Next(addresses.Length)],
            CanLogin = random.Next(2) == 1,
            IsDeleted = random.Next(2) == 1,
            LoginAttemptsLeft = random.Next(5, 20),
            TopScore = random.Next(5, 100)
        };

        for (int i = 0; i < games; i++)
        {
            person.Games.Add(Game.RandomGame());
        }

        return person;
    }
}

[MessagePackObject]
public class Game
{
    [Key(0)]
    public int Id { get; set; } = 1;

    [Key(1)]
    public int Score { get; set; } = 1;

    [Key(2)]
    public int Time { get; set; } = 1;

    public static Game RandomGame()
    {
        Random random = new Random();

        return new Game
        {
            Id = random.Next(18, 10000),
            Score = random.Next(18, 999),
            Time = random.Next(18, 10000),
        };
    }
}