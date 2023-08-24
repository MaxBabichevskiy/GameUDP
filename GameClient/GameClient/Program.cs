using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main()
    {
        UdpClient client = new UdpClient();
        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);

        Console.WriteLine("Введите свое имя:");
        string playerName = Console.ReadLine();

        while (true)
        {
            Console.Write("Введите число: ");
            string guess = Console.ReadLine();

            byte[] data = Encoding.ASCII.GetBytes(guess);
            client.Send(data, data.Length, serverEndPoint);

            byte[] response = client.Receive(ref serverEndPoint);
            string responseMessage = Encoding.ASCII.GetString(response);
            Console.WriteLine($"Ответ от сервера: {responseMessage}");
        }
    }
}
