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
    private static readonly string JsonURL = "http://localhost:5000/json";
    private static readonly string BytesURL = "http://localhost:5000/bytes";

    private static HttpClient HttpClient = new HttpClient();
    private static Stopwatch timer = new Stopwatch();

    public static void Main(string[] args)
    {
        Console.WriteLine("Press any key to start...");
        Console.ReadKey();

        BytesTest();

        Console.WriteLine("Test Completed");
        Console.ReadKey();
    }

    private static void BytesTest()
    {
        Stopwatch timeTracker = new Stopwatch();

        int requestCounter = 0;

        timeTracker.Start();
        while(timeTracker.ElapsedMilliseconds <= 10_000)
        {
            Person person = Person.GenerateRandomPerson();
            byte[] arr = MessagePackSerializer.Serialize(person);

            var task = PostBytesAsync(BytesURL, arr);

            var packet = MessagePackSerializer.Deserialize<ApiResponse>(task.Result);
            string response = MessagePackSerializer.Deserialize<string>(packet.Data);

            requestCounter++;
        }
        timeTracker.Stop();

        Console.WriteLine($"{requestCounter} requests per 10 seconds");
    }

    private static void JsonTest()
    {
        Stopwatch timeTracker = new Stopwatch();

        int requestCounter = 0;

        timeTracker.Start();

        while(timeTracker.ElapsedMilliseconds <= 10_000)
        {
            Person person = Person.GenerateRandomPerson();

            timer.Restart();

            var task = PostJsonAsync(JsonURL, person);
            task.Wait();

            requestCounter++;
        }

        timeTracker.Stop();

        Console.WriteLine($"{requestCounter} requests per 10 seconds");
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

    public static Person GenerateRandomPerson()
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

        for (int i = 0; i < 300; i++)
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