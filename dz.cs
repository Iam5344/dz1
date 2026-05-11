using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleApp31;
class Program
{
    static void Main()
    {
        try
        {
            Console.Write("Введіть адрес сайту:");
            string host = Console.ReadLine();

            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 80);

            Socket sender = new Socket(
                ipAddr.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp
            );

            sender.Connect(ipEndPoint);

            Console.WriteLine("Socket connected to {0}",
                sender.RemoteEndPoint.ToString());

            string request =
                "GET / HTTP/1.1\r\n" +
                "Host: " + host + "\r\n" +
                "Connection: close\r\n\r\n";

            byte[] msg = Encoding.ASCII.GetBytes(request);

            int bytesSent = sender.Send(msg);

            byte[] bytes = new byte[1024];

            int bytesRec = sender.Receive(bytes);
                Encoding.ASCII.GetString(bytes, 0, bytesRec));

            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.ReadKey();
    }
}
