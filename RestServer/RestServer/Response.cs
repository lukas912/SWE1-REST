using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace RestServer
{
    class Response
    {
        private byte[] data = null;
        public string status;
        private string mime;
        private Request req;
        private string[] output;

        public Response(Request r, string[] output)
        {
            this.req = r;
            this.output = output;
            PerformResponse();
        }

        private void PerformResponse()
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();

            this.mime = "text/plain";
            this.status = output[1];


            this.data = enc.GetBytes(output[0]);
        }

        public void SendResponse(NetworkStream stream)
        {
            StringBuilder header = new StringBuilder();

            header.AppendLine(HttpServer.VERSION + " " + status);
            header.AppendLine("Content-Type: " + mime);
            header.AppendLine("Content-Length: " + data.Length);
            header.AppendLine();

            List<byte> response = new List<byte>();
            response.AddRange(Encoding.ASCII.GetBytes(header.ToString()));
            response.AddRange(data);
            byte[] responseByte = response.ToArray();
            stream.Write(responseByte, 0, responseByte.Length);
        }
    }
}
