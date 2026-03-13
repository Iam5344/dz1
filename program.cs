using System;
using Microsoft.Data.SqlClient;

class Program
{
    static string connectionString = "Data Source=DESKTOP-8UTPR8Q\\IAM5344;Initial Catalog=SportShop1212;Integrated Security=True;Encrypt=False;";

    static void Main()
    {
        CreateTable();

        while (true)
        {
            Console.WriteLine("\n1. Додати користувача");
            Console.WriteLine("2. Показати всіх");
            Console.WriteLine("3. Пошук за username");
            Console.WriteLine("4. Пошук за email");
            Console.WriteLine("5. Видалити за ID");
            Console.WriteLine("0. Вихід");
            Console.Write("Вибір: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Username: ");
                string username = Console.ReadLine();
                Console.Write("Email: ");
                string email = Console.ReadLine();
                Console.Write("Дата народження (рррр-мм-дд): ");
                string birthDate = Console.ReadLine();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Users (Username, Email, BirthDate) VALUES (@u, @e, @b)", con);
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@e", email);
                    cmd.Parameters.AddWithValue("@b", birthDate);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Додано.");
                }
            }
            else if (choice == "2")
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlDataReader reader = new SqlCommand("SELECT * FROM Users", con).ExecuteReader();
                    while (reader.Read())
                        Console.WriteLine($"{reader["UserID"]} | {reader["Username"]} | {reader["Email"]} | {reader["BirthDate"]}");
                }
            }
            else if (choice == "3")
            {
                Console.Write("Username: ");
                string username = Console.ReadLine();
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE Username = @u", con);
                    cmd.Parameters.AddWithValue("@u", username);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                        Console.WriteLine($"{reader["UserID"]} | {reader["Username"]} | {reader["Email"]} | {reader["BirthDate"]}");
                }
            }
            else if (choice == "4")
            {
                Console.Write("Email: ");
                string email = Console.ReadLine();
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE Email = @e", con);
                    cmd.Parameters.AddWithValue("@e", email);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                        Console.WriteLine($"{reader["UserID"]} | {reader["Username"]} | {reader["Email"]} | {reader["BirthDate"]}");
                }
            }
            else if (choice == "5")
            {
                Console.Write("ID: ");
                string id = Console.ReadLine();
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Users WHERE UserID = @id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Видалено.");
                }
            }
            else if (choice == "0")
            {
                return;
            }
        }
    }

    static void CreateTable()
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string sql = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
                           CREATE TABLE Users (
                               UserID INT PRIMARY KEY IDENTITY(1,1),
                               Username NVARCHAR(50),
                               Email NVARCHAR(100),
                               BirthDate DATE
                           )";
            new SqlCommand(sql, con).ExecuteNonQuery();
        }
    }
}
