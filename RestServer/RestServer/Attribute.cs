using System;
using System.Collections.Generic;
using System.Text;

namespace RestServer
{
    class Attribute
    {
        public string Name { get; set; }
        public string Content { get; set; }

        public Attribute(string name, string content)
        {
            this.Name = name;
            this.Content = content;
        }
    }
}
