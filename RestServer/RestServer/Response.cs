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
        private string status;
        private string mime;
        private Request req;
        private string output;

        public Response(Request r, string output)
        {
            this.req = r;
            this.output = output;
            performResponse();
        }

        private void performResponse()
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();

            this.mime = "text/plain";
            this.status = "200 OK";
            this.data = enc.GetBytes(output);
        }

        public void sendResponse(NetworkStream stream)
        {
            StringBuilder sbHeader = new StringBuilder();

            sbHeader.AppendLine(HttpServer.VERSION + " " + status);
            sbHeader.AppendLine("Content-Type: " + mime);
            // CONTENT-LENGTH
            sbHeader.AppendLine("Content-Length: " + data.Length);

            // Append one more line breaks to seperate header and content.
            sbHeader.AppendLine();

            List<byte> response = new List<byte>();
            // response.AddRange(bHeadersString);
            response.AddRange(Encoding.ASCII.GetBytes(sbHeader.ToString()));
            response.AddRange(data);
            byte[] responseByte = response.ToArray();
            stream.Write(responseByte, 0, responseByte.Length);
        }
    }
}
