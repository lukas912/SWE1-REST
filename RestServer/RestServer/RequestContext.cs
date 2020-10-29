using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace RestServer
{
    class RequestContext
    {
        List<Message> messages = new List<Message>();
        int id_count = 0;
        private string output = "";

        public void checkContext(string url, string type, List<Query> queries, List<Query> body_data)
        {
            switch (type)
            {
                case "GET":
                    checkGET(url);
                    break;
                case "POST":
                    addMessage(url, body_data);
                    break;
                case "PUT":
                    editMessage(url, body_data);
                    break;
                case "DELETE":
                    deleteMessage(url);
                    break;
            }
        }

        private void checkGET(string url)
        {
            if (url == "/messages/")
            {
                listAllMessages(url);
            }

            if (url.StartsWith("/messages/") && url.Length >= 11)
            {
                listMessage(url);
            }
        }

        private void listAllMessages(string url)
        {
            output = "";
            foreach (Message msg in messages)
            {
                    output += msg.message + "\n";          
            }


        }

        private void listMessage(string url)
        {
            int id = Convert.ToInt32(getID(url));

            foreach (Message msg in messages)
            {
                if (msg.ID == id)
                {
                    output = msg.message;
                }

            }

        }

        private void addMessage(string url, List<Query> body_data)
        {
            Message msg = new Message(id_count, body_data[0].content);
            messages.Add(msg);
            incrID();
            output = "Message added";
        }

        private void editMessage(string url, List<Query> bd)
        {
            int id = Convert.ToInt32(getID(url));
            foreach (Message msg in messages)
            {
                if (msg.ID == id)
                {
                    msg.message = bd[0].content;
                    output = "Message edited";
                }

            }
        }

        private void deleteMessage(string url)
        {
            int id = Convert.ToInt32(getID(url));
            Message rm = null;

            foreach (Message msg in messages)
            {
                if (msg.ID == id)
                {
                    rm = msg;
                    output = "Deleted Message";

                }

            }

            messages.Remove(rm);
        }

        private string getID(string url)
        {
            string output = "";
            output = url.Split("/")[2];
            return output;
        }

        private void incrID()
        {
            id_count++;
        }

        public string getOutput()
        {
            return output;
        }
    }
}
