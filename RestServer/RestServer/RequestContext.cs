using System;
using System.Collections.Generic;
using System.Text;

namespace RestServer
{
    static class RequestContext
    {
        static List<Message> messages = new List<Message>();
        static int id_count = 0;

        public static void checkContext(string url, string type, List<Query> queries, List<Query> body_data)
        {
            switch (type)
            {
                case "GET":
                    
                    break;
                case "POST":
                    addMessage(url, body_data);
                    break;
                case "PUT":
                    
                    break;
                case "DELETE":
                    
                    break;
            }
        }

        private static string addMessage(string url, List<Query> body_data)
        {
            Message msg = new Message(id_count, body_data[0].content);
            messages.Add(msg);
            incrID();
            return "Message added";
        }

        private static string getID(string url)
        {
            string output = "";
            output = url.Split("/")[2];
            return output;
        }

        private static void incrID()
        {
            id_count++;
        }
    }
}
