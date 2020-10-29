using System;
using System.Collections.Generic;
using System.Text;

namespace RestServer
{
    class Request
    {
        public string httpverb { get; set; }
        public string url { get; set; }
        public string host { get; set; }
        public List<Query> queries;

        public Request(string httpverb, string url, string host, List<Query> q)
        {
            this.httpverb = httpverb;
            this.url = url;
            this.host = host;
            this.queries = q;
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
            List<Query> queries = getQueries(url);


            Console.WriteLine("Request from {0}: {1} {2}", host, httpverb, url);

            return new Request(httpverb, url, host, queries);
        }

        public static List<Query> getQueries(string url)
        {
            List<Query> output = new List<Query>();

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
                    Query q = new Query(c[0], c[1]);
                    output.Add(q);
                }

            }

            return output;

        }
    }
}
