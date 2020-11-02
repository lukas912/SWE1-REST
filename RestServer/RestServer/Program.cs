using System;

namespace RestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Server ...");
            HttpServer server = new HttpServer(7000);
            server.StartServer();
        }
    }
}
