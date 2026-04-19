using System;
using System.Threading;

class BankAccount
{
    public static int balance = 1000;
    public static object locker = new object();
}

Thread[] threads1 = new Thread[5];

for (int i = 0; i < threads1.Length; i++)
{
    threads1[i] = new Thread(() =>
    {
        Random rnd = new Random();
        for (int j = 0; j < 5; j++)
        {
            lock (BankAccount.locker)
            {
                int amount = rnd.Next(1, 200);
                bool deposit = rnd.Next(0, 2) == 0;

                if (deposit)
                {
                    BankAccount.balance += amount;
                    Console.WriteLine($"Потік {Thread.CurrentThread.ManagedThreadId}: +{amount} | Баланс: {BankAccount.balance}");
                }
                else
                {
                    if (amount <= BankAccount.balance)
                    {
                        BankAccount.balance -= amount;
                        Console.WriteLine($"Потік {Thread.CurrentThread.ManagedThreadId}: -{amount} | Баланс: {BankAccount.balance}");
                    }
                    else
                    {
                        Console.WriteLine($"Потік {Thread.CurrentThread.ManagedThreadId}: недостатньо коштів | Баланс: {BankAccount.balance}");
                    }
                }
            }
        }
    });
    threads1[i].Start();
}

for (int i = 0; i < threads1.Length; i++)
    threads1[i].Join();

Semaphore semaphore = new Semaphore(3, 3);

Thread[] threads2 = new Thread[10];

for (int i = 0; i < threads2.Length; i++)
{
    threads2[i] = new Thread((id) =>
    {
        Console.WriteLine($"Потік {id} очікує...");
        semaphore.WaitOne();

        Console.WriteLine($"Потік {id} виконується...");
        Random rnd = new Random();
        for (int j = 0; j < 5; j++)
            Console.WriteLine($"  Потік {id}: {rnd.Next(0, 1000)}");

        Console.WriteLine($"Потік {id} завершився");
        semaphore.Release();
    });
    threads2[i].Start(i);
}

for (int i = 0; i < threads2.Length; i++)
    threads2[i].Join();
