using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packages
{
    public class Todo
    {
        public bool Complete
        {
            get;
            private set;
        }
        public String Id
        {
            get;
            private set;
        }
        public String Note
        {
            get;
            private set;
        }
        public String Task
        {
            get;
            private set;
        }
        
        public Todo(string task,bool complete = false,string note = "",string id = null)
        {
            this.Id = id ?? Guid.NewGuid().ToString();
            this.Note = note ?? "";
            this.Task = task;
            this.Complete = complete;
        }
    }

}
