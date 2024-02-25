
public static class SampleData
{
    static List<string> FirstNames = ["Alice", "Phill", "Bob", "Charlie", "Diana", "Stephen", "Edward", "Michael", "John", "Richard", "Alex"];
    static List<string> LastNames = ["Smith", "Johnson", "Williams", "Jones", "Brown", "Thomas", "Kelly", "Wilson"];
    static List<string> DocPaths = [
        "/docs/financial/",
        "/docs/misc/",
        "/docs/people/",
        "/docs/accounts/",
        "/docs/outsource/"
    ];

    public static Account RandomSampleAccount(int noOfDocuments)
    {
        Random random = new Random();

        var person = new Account
        {
            Age = random.Next(18, 100),
            FirstName = RandomFrom(FirstNames),
            LastName = RandomFrom(LastNames),
            LoginAttemptsLeft = random.Next(5, 20),
            CanLogin = true,
            IsDeleted = false,
        };

        for (int i = 0; i < noOfDocuments; i++)
        {
            person.Documents.Add(RandomDocument());
        }

        return person;
    }

    private static Document RandomDocument()
    {
        Random random = new Random();

        return new Document
        {
            Id = random.Next(18, 10000),
            Path = $"{RandomFrom(DocPaths)}/{RandomString(10)}.txt",
            CreatedOn = RandomDate(15, 60),
            LastUpdated = RandomDate(0, 10),
            Views = Random.Shared.Next(500),
            SharedWith = $"{RandomFrom(FirstNames)},{RandomFrom(FirstNames)},{RandomFrom(FirstNames)}"
        };
    }

    public static DateTime RandomDate(int limit, int maxDaysInPast)
    {   
        return DateTime.Now.AddDays((limit + Random.Shared.Next(maxDaysInPast)) * -1);
    }

    public static string RandomString(int size)
    {
        char[] letters = new char[size];

        for (int i = 0; i < size; i++)
        {
            bool isUpper = RandomBool();

            // ASCII: A-Z (65-90), a-z (97-122)
            letters[i] = (char)(isUpper ? Random.Shared.Next(65, 91) : Random.Shared.Next(97, 123));
        }

        return new string(letters);
    }

    private static bool RandomBool()
    {
        return Random.Shared.Next(2) == 1;
    }

    private static T RandomFrom<T>(List<T> list)
    {
        return list[Random.Shared.Next(list.Count())];
    }
}