using System.Globalization;
using DataLayer.Models;

namespace BusinessLayer.Writers
{
    public class ConsoleResultWriter : IResultWriter
    {
        private readonly CultureInfo _info;
        private readonly DateTime? _date;
        private readonly string _separator;

        public ConsoleResultWriter(CultureInfo info, DateTime? date=null, string separator = ",")
        {
            _info = info;
            _date = date;
            _separator = separator;
        }


        public void Print(List<HoldingChanges> holdingChanges)
        {
           Console.WriteLine(DiffStringGenerator.GenerateSeparatedString(holdingChanges, _info, _date, _separator));
        }
    }
}
