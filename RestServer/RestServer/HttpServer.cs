using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace RestServer
{
    class HttpServer
    {
        public const string VERSION = "HTTP/1.1";
        public const string NAME = "MyRestServer v0.1";
        public const string dir_err = "/data/err";
        public const string dir_html = "/data/html";
        public const string dir_json = "/data/json";
        public const string dir_plaintext = "/data/text";
        public const string dir_img = "/data/img";
        bool is_running = false;
        TcpListener listener;


        public HttpServer(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
        }

        public void startServer()
        {
            Thread st = new Thread(new ThreadStart(runServer));
            st.Start();
            Console.WriteLine("Server started!");
        }

        private void runServer()
        {
            is_running = true;
            listener.Start();

            while (is_running)
            {
                Console.WriteLine("Waiting for connection ...");
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected!");
                HandleClient(client);
                client.Close();
            }


            listener.Stop();
            is_running = false;
        }

        private void HandleClient(TcpClient client)
        {
            StreamReader reader = new StreamReader(client.GetStream());
            string data = "";

            while (reader.Peek() != -1)
            {
                data += reader.ReadLine();
            }

            Debug.Write(data);
            Request req = Request.GetRequest(data);
            //Console.WriteLine(req.body_data.Count + " " + req.queries.Count);

            RequestContext.checkContext(req.url, req.httpverb, req.queries, req.body_data);
            Response res = new Response(req, RequestContext.getOutput());
            res.sendResponse(client.GetStream());

        }
    }
}
