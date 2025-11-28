using System;

class Program
{
    static void Main()
    {
    
        Console.WriteLine("Завдання 1: Переведення температури");
        Console.Write("Введіть температуру: ");
        double temp = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("Оберіть тип переведення:");
        Console.WriteLine("1 - з Фаренгейта в Цельсій");
        Console.WriteLine("2 - з Цельсія в Фаренгейт");
        int choice = Convert.ToInt32(Console.ReadLine());

        double result;

        if (choice == 1)
        {
            result = (temp - 32) * 5 / 9;
            Console.WriteLine("Результат: " + result);
        }
        else if (choice == 2)
        {
            result = temp * 9 / 5 + 32;
            Console.WriteLine("Результат: " + result);
        }
        else
        {
            Console.WriteLine("Невірний вибір!");
        }
        Console.WriteLine("\nЗавдання 2: Парні числа в діапазоні");
        Console.Write("Введіть перше число: ");
        int a = Convert.ToInt32(Console.ReadLine());

        Console.Write("Введіть друге число: ");
        int b = Convert.ToInt32(Console.ReadLine());

        if (a > b)
        {
            int tempSwap = a;
            a = b;
            b = tempSwap;
        }

        Console.WriteLine("Парні числа в діапазоні:");
        for (int i = a; i <= b; i++)
        {
            if (i % 2 == 0)
            {
                Console.Write(i + " ");
            }
        }
        Console.WriteLine("\n\nЗавдання 3: Число Армстронга");
        Console.Write("Введіть число: ");
        int number = Convert.ToInt32(Console.ReadLine());

        int original = number;
        int sum = 0;
        int digits = number.ToString().Length;

        while (number > 0)
        {
            int digit = number % 10;
            sum += (int)Math.Pow(digit, digits);
            number /= 10;
        }

        if (sum == original)
        {
            Console.WriteLine("Це число Армстронга.");
        }
        else
        {
            Console.WriteLine("Це НЕ число Армстронга.");
        }
    }
}
