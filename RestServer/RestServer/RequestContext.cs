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
        private string[] output = { "", "" };

        //0 = Content
        //1 = Statuscode

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
            output[0] = "";

            if(messages.Count == 0)
            {
                output[0] = "Error: No Messages found";
                output[1] = "404 Not Found";
            }

            else
            {
                foreach (Message msg in messages)
                {
                    output[0] += msg.message + "\n";
                }

                output[1] = "200 OK";
            }

        }

        private void listMessage(string url)
        {
            int id = Convert.ToInt32(getID(url));

            if(messages.Exists(msg => msg.ID == id) == false)
            {
                output[0] = "Error: No Message found with this ID";
                output[1] = "404 Not found";
            }

            else
            {
                foreach (Message msg in messages)
                {
                    if (msg.ID == id)
                    {
                        output[0] = msg.message;
                    }

                }

                output[1] = "200 OK";
            }

        }

        private void addMessage(string url, List<Query> body_data)
        {
            if(body_data[0].content == "" || body_data[0].content == null)
            {
                output[0] = "Error: Message can't be empty";
                output[1] = "400 Bad Request";
            }

            else
            {
                Message msg = new Message(id_count, body_data[0].content);
                messages.Add(msg);
                incrID();
                output[0] = "Message added";
                output[1] = "200 OK";
            }

        }

        private void editMessage(string url, List<Query> bd)
        {
            int id = Convert.ToInt32(getID(url));

            if (messages.Exists(msg => msg.ID == id) == false)
            {
                output[0] = "Error: No Message found with this ID";
                output[1] = "404 Not found";
            }

            else if(bd[0].content == "" || (bd[0].content == null)) {
                output[0] = "Error: Message can't be empty";
                output[1] = "400 Bad Request";
            }

            else
            {
                foreach (Message msg in messages)
                {
                    if (msg.ID == id)
                    {
                        msg.message = bd[0].content;
                        output[0] = "Message edited";
                    }

                }

                output[1] = "200 OK";
            }

        }

        private void deleteMessage(string url)
        {
            int id = Convert.ToInt32(getID(url));
            Message rm = null;

            if (messages.Exists(msg => msg.ID == id) == false)
            {
                output[0] = "Error: No Message found with this ID";
                output[1] = "404 Not found";
            }

            else
            {
                foreach (Message msg in messages)
                {
                    if (msg.ID == id)
                    {
                        rm = msg;
                        output[0] = "Deleted Message";
                    }
                }

                output[1] = "200 OK";

                messages.Remove(rm);
            }
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

        public string[] getOutput()
        {
            return output;
        }
    }
}
