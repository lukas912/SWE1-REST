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

        public void CheckContext(string url, string type, List<Attribute> queries, List<Attribute> body_data)
        {
            switch (type)
            {
                case "GET":
                    CheckGET(url);
                    break;
                case "POST":
                    AddMessage(url, body_data);
                    break;
                case "PUT":
                    EditMessage(url, body_data);
                    break;
                case "DELETE":
                    DeleteMessage(url);
                    break;
            }
        }

        private void CheckGET(string url)
        {
            if (url == "/messages/")
            {
                ListAllMessages(url);
            }

            if (url.StartsWith("/messages/") && url.Length >= 11)
            {
                ListMessage(url);
            }
        }

        private void ListAllMessages(string url)
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
                    output[0] += msg.Msg + "\n";
                }

                output[1] = "200 OK";
            }

        }

        private void ListMessage(string url)
        {
            int id = Convert.ToInt32(GetID(url));

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
                        output[0] = msg.Msg;
                    }

                }

                output[1] = "200 OK";
            }

        }

        private void AddMessage(string url, List<Attribute> body_data)
        {
            if(body_data[0].Content == "" || body_data[0].Content == null)
            {
                output[0] = "Error: Message can't be empty";
                output[1] = "400 Bad Request";
            }

            else
            {
                Message msg = new Message(id_count, body_data[0].Content);
                messages.Add(msg);
                IncrID();
                output[0] = "Message added";
                output[1] = "200 OK";
            }

        }

        private void EditMessage(string url, List<Attribute> bd)
        {
            int id = Convert.ToInt32(GetID(url));

            if (messages.Exists(msg => msg.ID == id) == false)
            {
                output[0] = "Error: No Message found with this ID";
                output[1] = "404 Not found";
            }

            else if(bd[0].Content == "" || (bd[0].Content == null)) {
                output[0] = "Error: Message can't be empty";
                output[1] = "400 Bad Request";
            }

            else
            {
                foreach (Message msg in messages)
                {
                    if (msg.ID == id)
                    {
                        msg.Msg = bd[0].Content;
                        output[0] = "Message edited";
                    }

                }

                output[1] = "200 OK";
            }

        }

        private void DeleteMessage(string url)
        {
            int id = Convert.ToInt32(GetID(url));
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

        private string GetID(string url)
        {
            string output = url.Split("/")[2];
            return output;
        }

        private void IncrID()
        {
            id_count++;
        }

        public string[] GetOutput()
        {
            return output;
        }
    }
}
