using System;

namespace RestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Server ...");
            HttpServer server = new HttpServer(10001);
            server.StartServer();
        }
    }
}
