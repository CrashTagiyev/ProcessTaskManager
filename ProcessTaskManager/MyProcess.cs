using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessTaskManager
{
   public class MyProcess
    {
        public MyProcess(int id, string name, int threadsCount, int handlesCount)
        {
            Id = id;
            Name = name;
            ThreadsCount = threadsCount;
            HandlesCount = handlesCount;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int ThreadsCount { get; set; }
        public int HandlesCount { get; set; }
    }
}
