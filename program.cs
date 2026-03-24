using Microsoft.Data.SqlClient;
using Dapper;

class Program
{
    static string cs = "Data Source=DESKTOP-8UTPR8Q\\IAM5344;Initial Catalog=ShelterDb;Integrated Security=True;Encrypt=False;";

    static void Main()
    {
        using var con = new SqlConnection(cs);
        con.Open();

        con.Execute(@"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Dogs' AND xtype='U')
            CREATE TABLE Dogs (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Name NVARCHAR(100) NOT NULL,
                Age INT NOT NULL,
                Breed NVARCHAR(100) NOT NULL,
                IsAdopted BIT NOT NULL DEFAULT 0
            )");

        while (true)
        {
            Console.WriteLine("\n1. Додати собаку");
            Console.WriteLine("2. Всі собаки");
            Console.WriteLine("3. Собаки в притулку");
            Console.WriteLine("4. Забрані собаки");
            Console.WriteLine("5. Пошук за кличкою");
            Console.WriteLine("6. Пошук за Id");
            Console.WriteLine("7. Пошук за породою");
            Console.WriteLine("0. Вихід");
            Console.Write("Вибір: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Кличка: "); string name = Console.ReadLine();
                Console.Write("Вік: "); int age = int.Parse(Console.ReadLine());
                Console.Write("Порода: "); string breed = Console.ReadLine();

                con.Execute("INSERT INTO Dogs (Name, Age, Breed, IsAdopted) VALUES (@Name, @Age, @Breed, 0)",
                    new { Name = name, Age = age, Breed = breed });
                Console.WriteLine("Додано.");
            }
            else if (choice == "2")
            {
                var dogs = con.Query("SELECT * FROM Dogs");
                PrintDogs(dogs);
            }
            else if (choice == "3")
            {
                var dogs = con.Query("SELECT * FROM Dogs WHERE IsAdopted = 0");
                PrintDogs(dogs);
            }
            else if (choice == "4")
            {
                var dogs = con.Query("SELECT * FROM Dogs WHERE IsAdopted = 1");
                PrintDogs(dogs);
            }
            else if (choice == "5")
            {
                Console.Write("Кличка: "); string name = Console.ReadLine();
                var dogs = con.Query("SELECT * FROM Dogs WHERE Name = @Name", new { Name = name });
                PrintDogs(dogs);
            }
            else if (choice == "6")
            {
                Console.Write("Id: "); int id = int.Parse(Console.ReadLine());
                var dogs = con.Query("SELECT * FROM Dogs WHERE Id = @Id", new { Id = id });
                PrintDogs(dogs);
            }
            else if (choice == "7")
            {
                Console.Write("Порода: "); string breed = Console.ReadLine();
                var dogs = con.Query("SELECT * FROM Dogs WHERE Breed = @Breed", new { Breed = breed });
                PrintDogs(dogs);
            }
            else if (choice == "0") return;
        }
    }

    static void PrintDogs(IEnumerable<dynamic> dogs)
    {
        foreach (var d in dogs)
            Console.WriteLine($"{d.Id} | {d.Name} | {d.Age} р. | {d.Breed} | {(d.IsAdopted ? "Забрали" : "В притулку")}");
    }
}
