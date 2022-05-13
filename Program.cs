using System;
using System.Threading;
using System.IO;

class Program
{
    static Mutex mutexObj = new Mutex();
    static int x = 0;
    static void Main(string[] args)
    {
        for (int i = 0; i < 10; i++)
        {
            Thread Writer = new Thread(data);
            Thread Reader = new Thread(data);
            Writer.Name = $"Писатель " + i.ToString();
            Reader.Name = $"Читатель " + i.ToString();
            Console.WriteLine("Создание потока: " + Writer.Name);
            Console.WriteLine("Создание потока: " + Reader.Name);
            Writer.Start();
            Reader.Start();
            Thread.Sleep(100);
        }
    }

    public static void data()
    {
        mutexObj.WaitOne();
        Console.WriteLine($"{Thread.CurrentThread.Name} получил доступ. Установление блокировки доступа к файлу");
        if (Thread.CurrentThread.Name.Contains("Писатель"))
        {
            string writePath = @"C:\Users\Van\Desktop\data.txt";

            Random x = new Random();
            int n = x.Next(-100, 100);

            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(n);
            }
            Console.WriteLine($"{Thread.CurrentThread.Name} написал " + n);
            Console.WriteLine($"{Thread.CurrentThread.Name} завершил работу");
        }
        else
        {
            string path = @"C:\Users\Van\Desktop\data.txt";

            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine($"{Thread.CurrentThread.Name} прочитал " + line);
                }
            }
            Console.WriteLine($"{Thread.CurrentThread.Name} завершил работу");
        }
        mutexObj.ReleaseMutex();
        Console.WriteLine("Снятие блокировки доступа к файлу");
    }
}