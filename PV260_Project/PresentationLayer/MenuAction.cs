using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer
{
    public class MenuAction
    {
        public string Identifier { get; set; }
        public string Description { get; set; }
        public Func<Task> Action { get; set; }
    }
}
