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
        public List<Query> body_data;

        public Request(string httpverb, string url, string host, List<Query> q, List<Query> bd)
        {
            this.httpverb = httpverb;
            this.url = url;
            this.host = host;
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
            List<Query> queries = getQueries(url);
            List<Query> body_data = getBodyData(tokens);


            //Console.WriteLine("Request from {0}: {1} {2}", host, httpverb, url);

            return new Request(httpverb, url, host, queries, body_data);
        }

        private static List<Query> getBodyData(string[] tokens)
        {
            List<Query> output = new List<Query>();
            int counter = 0;
            int bd_id = 0;

            foreach(string item in tokens)
            {
                if(item == "form-data;")
                {
                    bd_id = counter + 1;
                }

                if(counter == bd_id)
                {
                    try
                    {
                        Query q = new Query(item.Split("\"")[1], item.Split("\"")[2].Remove(item.Split("\"")[2].IndexOf("-")));

                        output.Add(q);
                    }

                    catch
                    {

                    }

                }

                counter++;
            }



            return output;
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
