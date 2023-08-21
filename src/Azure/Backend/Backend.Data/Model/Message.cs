using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Data.Model
{
    public class Message
    {
        public Operation Operation { get; set; }

        public TodoItem Item { get; set; }
    }

    public enum Operation { Add, Update }
}
