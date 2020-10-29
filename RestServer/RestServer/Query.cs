using System;
using System.Collections.Generic;
using System.Text;

namespace RestServer
{
    class Query
    {
        public string name { get; set; }
        public string content { get; set; }

        public Query(string name, string content)
        {
            this.name = name;
            this.content = content;
        }
    }
}
