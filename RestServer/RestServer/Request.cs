using System;
using System.Collections.Generic;
using System.Text;

namespace RestServer
{
    class Request
    {
        public string Httpverb { get; set; }
        public string Url { get; set; }
        public string Host { get; set; }
        public List<Attribute> queries;
        public List<Attribute> body_data;

        public Request(string httpverb, string url, string host, List<Attribute> q, List<Attribute> bd)
        {
            this.Httpverb = httpverb;
            this.Url = url;
            this.Host = host;
            this.queries = q;
            this.body_data = bd;
        }

        public static Request GetRequest(string req)
        {
            if (String.IsNullOrEmpty(req))
            {
                return null;
            }

            string[] tokens = req.Split(" ");
            string httpverb = tokens[0];
            string url = tokens[1];
            string host = tokens[4];
            List<Attribute> queries = GetQueries(url);
            List<Attribute> body_data = GetBodyData(req.Split("form-data;"));


            //Console.WriteLine("Request from {0}: {1} {2}", host, httpverb, url);

            return new Request(httpverb, url, host, queries, body_data);
        }

        public static List<Attribute> GetBodyData(string[] tokens)
        {
            List<Attribute> output = new List<Attribute>();
            bool bd_token = true;
            int counter = 0;
            int bd_id = 0;
            int n = 0;

            foreach (string item in tokens)
            {
                if (item.StartsWith(" name"))
                {
                    Console.WriteLine(item);
                    string a = item.Split("\"")[1];
                    string b = item.Split("\"")[2].Remove(item.IndexOf("-")).Replace("-", "");
                    Console.WriteLine("a: " + a);
                    Console.WriteLine("b: " + b);

                    Attribute ab = new Attribute(a, b);
                    output.Add(ab);
                }


            }


            Console.WriteLine(output.Count);
            return output;
        }

        public static List<Attribute> GetQueries(string url)
        {
            List<Attribute> output = new List<Attribute>();

            if (url.Contains("?") == false)
            {
                return null;
            }

            else
            {
                string[] a = url.Split("?");

                string[] b = a[1].Split("&&");

                foreach (string item in b)
                {
                    string[] c = item.Split("=");
                    Attribute q = new Attribute(c[0], c[1]);
                    output.Add(q);
                }

            }

            return output;

        }
    }
}
