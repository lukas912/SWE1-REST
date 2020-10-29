using System;
using System.Collections.Generic;
using System.Text;

namespace RestServer
{
    class Message
    {
        public int ID { get; set; }
        public string message { get; set; }

        public Message(int id, string message)
        {
            this.ID = id;
            this.message = message;
        }
    }
}
