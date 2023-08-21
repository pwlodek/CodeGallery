using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Data.Model
{
    public class TodoItem
    {
        public string Name { get; set; }

        public bool Completed { get; set; }

        public DateTime DueDate { get; set; }

        public string UserName { get; set; }
    }
}
