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
            MessageBox(IntPtr.Zero, "Загадайте число від 0 до 100 і натисніть OK", "Нова гра", 0);

            int min = 0;
            int max = 100;

            while (true)
            {
                int guess = (min + max) / 2;
                int result = MessageBox(
                    IntPtr.Zero,
                    $"Комп'ютер думає: {guess}\n\nТак — число більше\nНі — число менше\nСкасувати — вгадав",
                    "Вгадай число",
                    3
                );
                if (result == 2)
                {
                    MessageBox(IntPtr.Zero, $"Я вгадав! Це число {guess}!", "Перемога!", 0);
                    break;
                }
                else if (result == 6)
                {
                    min = guess + 1;
                }
                else if (result == 7)
                {
                    max = guess - 1;
                }
            }

            int play = MessageBox(IntPtr.Zero, "Зіграти ще раз?", "Нова гра", 4);
            playAgain = play == 6;
        }
    }
}
