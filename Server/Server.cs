using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SocketListener
{
    public static int Main(String[] args)
    {
        StartServer();
        return 0;
    }
    public static void StartServer()
    {
        IPHostEntry host = Dns.GetHostEntry("localhost");
        IPAddress ipAddress = host.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        try
        {
            Socket listner = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listner.Bind(localEndPoint);
            listner.Listen(10);

            Console.WriteLine("Waiting for a connection...");
            Socket handler = listner.Accept();
            Console.WriteLine("Connection Accepted");

            string data = null;
            byte[] bytes = null;

            while (true)
            {
                bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                if(data.IndexOf("<EOF>") > -1)
                {
                    break;
                }
            }

            Console.WriteLine($"Text recieved {data}");

            string returnMsg;
            if(data == "Marco<EOF>")
            {
                returnMsg = "Polo<EOF>";
            }
            else
            {
                returnMsg = "Zolo<EOF>";
            }

            byte[] msg = Encoding.ASCII.GetBytes(returnMsg);
            handler.Send(msg);
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\nPress any key to continue");
        Console.ReadKey();
    }

}