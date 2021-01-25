using System;
using System.Collections.Generic;
using System.Text;

namespace BeeHiveManagement
{
   interface IDefend
    {
        public void Defend();
    }

    interface IWorker
    {
        public string Job { get;  }
        public void WorkTheNextShift();
    }



}
