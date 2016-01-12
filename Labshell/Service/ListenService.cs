using Labshell.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labshell.Service
{
    class ListenService
    {
        private Task t;

        private List<ListenPath> paths = new List<ListenPath>();

        public void Start()
        {
            t = Task.Factory.StartNew(Listen);
        }

        private void Listen()
        {
            while (true)
            {
                try
                {

                }
                catch (Exception)
                {
 
                }
            }
        }

        public void SetPaths(List<ListenPath> lp)
        {
            this.paths = lp;
        }
    }
}
