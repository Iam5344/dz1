using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

public class User
{
    public int Id { get; set; }
    [Required][MinLength(1)] public string Username { get; set; } = null!;
    [Required][EmailAddress] public string Email { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
    public List<Movie> Movies { get; set; } = new();
}

public class Movie
{
    public int Id { get; set; }
    [Required][MaxLength(50)] public string Title { get; set; } = null!;
    [Range(1, int.MaxValue)] public int ReleaseYear { get; set; }
    public string? Description { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.Now;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}

public class UserMovieView
{
    public string Username { get; set; } = null!;
    public string MovieTitle { get; set; } = null!;
}

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Movie> Movies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer("Data Source=DESKTOP-8UTPR8Q\\IAM5344;Initial Catalog=MoviesDb;Integrated Security=True;Encrypt=False;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<Movie>().HasOne(m => m.User).WithMany(u => u.Movies).HasForeignKey(m => m.UserId);
        modelBuilder.Entity<UserMovieView>().HasNoKey().ToView("UserMoviesView");
    }
}

class Program
{
    static AppDbContext db = new AppDbContext();

    static void Main(string[] args)
    {
        db.Database.Migrate();

        db.Database.ExecuteSqlRaw(@"
            IF NOT EXISTS (SELECT * FROM sys.views WHERE name = 'UserMoviesView')
            EXEC('CREATE VIEW UserMoviesView AS
                SELECT U.Username, M.Title AS MovieTitle
                FROM Users U
                INNER JOIN Movies M ON U.Id = M.UserId')");

        db.Database.ExecuteSqlRaw(@"
            IF NOT EXISTS (SELECT * FROM sys.procedures WHERE name = 'AddUser')
            EXEC('CREATE PROCEDURE AddUser
                @Username NVARCHAR(MAX),
                @Email NVARCHAR(MAX),
                @Password NVARCHAR(MAX)
                AS BEGIN
                    INSERT INTO Users (Username, Email, Password)
                    VALUES (@Username, @Email, @Password)
                END')");

        while (true)
        {
            Console.WriteLine("\n1. Додати користувача (процедура)");
            Console.WriteLine("2. Показати подання (користувачі та фільми)");
            Console.WriteLine("0. Вихід");
            Console.Write("Вибір: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Username: "); string username = Console.ReadLine();
                Console.Write("Email: "); string email = Console.ReadLine();
                Console.Write("Пароль: "); string password = Console.ReadLine();
                db.Database.ExecuteSqlRaw("EXEC AddUser @p0, @p1, @p2", username, email, password);
                Console.WriteLine("Додано.");
            }
            else if (choice == "2")
            {
                foreach (var v in db.Set<UserMovieView>().FromSqlRaw("SELECT * FROM UserMoviesView"))
                    Console.WriteLine($"{v.Username} | {v.MovieTitle}");
            }
            else if (choice == "0") return;
        }
    }
}
