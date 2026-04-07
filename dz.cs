using System;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    static void Main()
    {
        Console.Write("Введіть повідомлення: ");
        string text = Console.ReadLine();
        MessageBox(IntPtr.Zero, text, "Ваше повідомлення", 0);

        bool playAgain = true;

        while (playAgain)
        {
            Console.WriteLine("Загадайте число від 0 до 100");

            int min = 0;
            int max = 100;

            while (true)
            {
                int guess = (min + max) / 2;

                Console.WriteLine($"Це {guess}? (більше / менше / так)");
                string answer = Console.ReadLine().ToLower();

                if (answer == "так")
                {
                    MessageBox(IntPtr.Zero, $"Я вгадав число: {guess}", "Перемога!", 0);
                    break;
                }
                else if (answer == "більше")
                {
                    min = guess + 1;
                }
                else if (answer == "менше")
                {
                    max = guess - 1;
                }
            }

            Console.WriteLine("Грати ще?");
            playAgain = Console.ReadLine().ToLower() == "так";
        }
    }
}
