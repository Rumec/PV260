using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Jobs
{
    public interface IDataSyncJob
    {
        void Run();
        void Stop();
    }
}
