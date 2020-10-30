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
        bool is_running = false;
        TcpListener listener;
        RequestContext rc;


        public HttpServer(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            rc = new RequestContext();
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
                //Console.WriteLine("Client connected!");
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


            rc.checkContext(req.url, req.httpverb, req.queries, req.body_data);
            Response res = new Response(req, rc.getOutput());
            res.sendResponse(client.GetStream());
            Console.WriteLine("Request from {0}: {1} {2} {3}", req.host, req.httpverb, req.url, res.status);

        }
    }
}
