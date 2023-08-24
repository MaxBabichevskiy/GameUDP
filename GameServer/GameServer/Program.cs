using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    static void Main()
    {
        UdpClient server = new UdpClient(12345);
        IPEndPoint client1EndPoint = null;
        IPEndPoint client2EndPoint = null;
        string numberToGuess = GenerateRandomNumber();

        Console.WriteLine("Сервер запущен...");
        Console.WriteLine($"Число для угадывания: {numberToGuess}");

        while (true)
        {
            byte[] data = server.Receive(ref client1EndPoint);
            string guess = Encoding.ASCII.GetString(data);

            if (client2EndPoint == null)
            {
                client2EndPoint = client1EndPoint;
                Console.WriteLine("Ожидание второго игрока...");
                continue;
            }

            Console.WriteLine($"Игрок 1 ({client1EndPoint}): {guess}");

            int bulls, cows;
            CalculateBullsAndCows(guess, numberToGuess, out bulls, out cows);

            string response = $"Быки: {bulls}, Коровы: {cows}";
            byte[] responseData = Encoding.ASCII.GetBytes(response);

            server.Send(responseData, responseData.Length, client1EndPoint);
            server.Send(responseData, responseData.Length, client2EndPoint);

            Swap(ref client1EndPoint, ref client2EndPoint); // Смена порядка клиентов
        }
    }

    static string GenerateRandomNumber()
    {
        Random rand = new Random();
        int[] digits = new int[4];

        for (int i = 0; i < 4; i++)
        {
            int digit;
            do
            {
                digit = rand.Next(1, 10);
            } while (Array.Exists(digits, element => element == digit));

            digits[i] = digit;
        }

        return string.Join("", digits);
    }

    static void CalculateBullsAndCows(string guess, string numberToGuess, out int bulls, out int cows)
    {
        bulls = cows = 0;

        for (int i = 0; i < 4; i++)
        {
            if (guess[i] == numberToGuess[i])
            {
                bulls++;
            }
            else if (numberToGuess.Contains(guess[i]))
            {
                cows++;
            }
        }
    }

    static void Swap(ref IPEndPoint ep1, ref IPEndPoint ep2)
    {
        IPEndPoint temp = ep1;
        ep1 = ep2;
        ep2 = temp;
    }
}
