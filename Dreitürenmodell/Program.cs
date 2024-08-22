using System.Diagnostics;
using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dreituerenmodell;
class Program
{
    static long SuccessCount = 0;
    static long AttemptCount = 0;
    static string Accuracy = "0";
    static DateTime beginning = DateTime.Now;
    static int Mode = 0;
    static void Main(string[] args)
    {
        Console.Title = "Dreitürenmodell";
        int chosen = 0;
        while(chosen != 1 && chosen != 2)
        {
            Console.Clear();
            Console.WriteLine("Modus auswählen");
            Console.WriteLine("[1] Schnell (Empfohlen)");
            Console.WriteLine("[2] Anschaulich");
            char key = Console.ReadKey().KeyChar;
            try
            {
                chosen = Convert.ToInt32(key.ToString());
                Mode = chosen;
            }
            catch { }
        }
        Thread displayThread = new Thread(DisplayMethod);
        Thread workThread = new Thread(WorkMethod);
        displayThread.Start();
        workThread.Start();
    }
    static void DisplayMethod()
    {
        while(true)
        {
            Console.Clear();
            TimeSpan elapsedTime = DateTime.Now - beginning;
            string elapsedTimeString = elapsedTime.ToString("hh\\:mm\\:ss");
            string formattedAttempt = AttemptCount.ToString("N0", new System.Globalization.CultureInfo("de-DE"));
            formattedAttempt = formattedAttempt.Replace(",", ".");
            string formattedSuccess = SuccessCount.ToString("N0", new System.Globalization.CultureInfo("de-DE"));
            formattedSuccess = formattedSuccess.Replace(",", ".");
            Console.WriteLine($"Versuche: {formattedAttempt}   Erfolge: {formattedSuccess}   Zeit: {elapsedTimeString}");
            try
            {
                Debug.WriteLine("Trigger 1");
                float _accuracy = (float)SuccessCount / (float)AttemptCount;
                Debug.WriteLine(_accuracy);
                Accuracy = _accuracy.ToString("0.0000000").Substring(2).Insert(2, ".");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failure: " + ex.Message);
            }
            if (Accuracy.Contains("66.66"))
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
            }
            if (Accuracy.Contains("50.00"))
            {
                Console.BackgroundColor = ConsoleColor.Red;
            }
            Console.Write($"Erfolgsrate: {Accuracy}%");
            Console.ResetColor();
            Console.WriteLine("   Erwartet: 66.66% oder 50.0%");
            Thread.Sleep(1000);
        }
    }
    static void WorkMethod()
    {
        while (true)
        {
            AttemptCount++;
            bool[] doors = new bool[3];
            var rnd = new Random();
            Array.Fill(doors, false);
            doors[rnd.Next(3)] = true;
            //Setup fertig
            int chosenDoor = rnd.Next(3);
            int? otherDoor = null;
            int? revealedDoor = null;
            while (revealedDoor == null)
            {
                int i = rnd.Next(3);
                if (i == chosenDoor)
                {
                    continue;
                }
                if (doors[i] == true)
                {
                    continue;
                }
                revealedDoor = i;
                if (revealedDoor == 0 && chosenDoor == 1)
                {
                    otherDoor = 2;
                }
                if (revealedDoor == 0 && chosenDoor == 2)
                {
                    otherDoor = 1;
                }
                if (revealedDoor == 1 && chosenDoor == 0)
                {
                    otherDoor = 2;
                }
                if (revealedDoor == 1 && chosenDoor == 2)
                {
                    otherDoor = 0;
                }
                if (revealedDoor == 2 && chosenDoor == 0)
                {
                    otherDoor = 1;
                }
                if (revealedDoor == 2 && chosenDoor == 1)
                {
                    otherDoor = 0;
                }
            }
            if (doors[Convert.ToInt32(otherDoor)] == true)
            {
                SuccessCount++;
            }
            if(Mode == 2)
            {
                Thread.Sleep(rnd.Next(300));
            }
            
        }
    }
}