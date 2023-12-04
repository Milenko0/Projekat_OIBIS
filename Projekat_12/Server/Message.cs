using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Message
    {
        private int id;
        private string text;


        public Message(int id, string text)
        {
            Id = id;
            Text = text;
        }
        public int Id { get => id; set => id = value; }
        public string Text { get => text; set => text = value; }
    }
}
