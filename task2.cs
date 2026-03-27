using Microsoft.Data.SqlClient;
using Dapper;

class Program
{
    static string cs = "Data Source=DESKTOP-8UTPR8Q\\IAM5344;Initial Catalog=BoardGamesDb;Integrated Security=True;Encrypt=False;";

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n1. Всі сесії");
            Console.WriteLine("2. Топ-3 ігри за годинами");
            Console.WriteLine("3. Рейтинг учасників");
            Console.WriteLine("4. Загальна статистика");
            Console.WriteLine("5. Статистика за період");
            Console.WriteLine("0. Вихід");
            Console.Write("Вибір: ");
            string choice = Console.ReadLine();

            using var con = new SqlConnection(cs);
            con.Open();

            if (choice == "1")
            {
                var sessions = con.Query(@"
                    SELECT S.Id, G.Title AS Game, S.Date, S.DurationMinutes,
                           STRING_AGG(M.FullName, ', ') AS Members
                    FROM Sessions S
                    INNER JOIN Games G ON S.GameId = G.Id
                    INNER JOIN MembersSessions MS ON S.Id = MS.SessionId
                    INNER JOIN Members M ON MS.MemberId = M.Id
                    GROUP BY S.Id, G.Title, S.Date, S.DurationMinutes");

                foreach (var s in sessions)
                    Console.WriteLine($"{s.Game} | {s.Date:dd.MM.yyyy} | {s.DurationMinutes} хв | {s.Members}");
            }
            else if (choice == "2")
            {
                var top = con.Query(@"
                    SELECT TOP 3 G.Title, SUM(S.DurationMinutes) / 60.0 AS Hours
                    FROM Sessions S
                    INNER JOIN Games G ON S.GameId = G.Id
                    GROUP BY G.Title
                    ORDER BY Hours DESC");

                foreach (var t in top)
                    Console.WriteLine($"{t.Title} – {t.Hours:F1} год");
            }
            else if (choice == "3")
            {
                var rating = con.Query(@"
                    SELECT M.FullName, SUM(S.DurationMinutes) AS TotalMinutes
                    FROM Members M
                    INNER JOIN MembersSessions MS ON M.Id = MS.MemberId
                    INNER JOIN Sessions S ON MS.SessionId = S.Id
                    GROUP BY M.FullName
                    ORDER BY TotalMinutes DESC");

                foreach (var r in rating)
                    Console.WriteLine($"{r.FullName} – {r.TotalMinutes} хв");
            }
            else if (choice == "4")
            {
                var stat = con.QueryFirst(@"
                    SELECT COUNT(*) AS TotalSessions, SUM(DurationMinutes) AS TotalMinutes
                    FROM Sessions");

                Console.WriteLine($"Сесій: {stat.TotalSessions} | Тривалість: {stat.TotalMinutes} хв");
            }
            else if (choice == "5")
            {
                Console.Write("З (рррр-мм-дд): "); string from = Console.ReadLine();
                Console.Write("По (рррр-мм-дд): "); string to = Console.ReadLine();

                var stat = con.QueryFirst(@"
                    SELECT COUNT(*) AS TotalSessions, SUM(DurationMinutes) AS TotalMinutes
                    FROM Sessions
                    WHERE Date >= @from AND Date <= @to",
                    new { from = DateTime.Parse(from), to = DateTime.Parse(to) });

                Console.WriteLine($"Сесій: {stat.TotalSessions} | Тривалість: {stat.TotalMinutes} хв");
            }
            else if (choice == "0") return;
        }
    }
}
