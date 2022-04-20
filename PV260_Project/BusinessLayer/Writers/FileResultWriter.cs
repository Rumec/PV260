using System.Globalization;
using DAL.Models;

namespace BL.Writers
{
    public class FileResultWriter : IResultWriter
    {
        private readonly string _filePath;
        private readonly CultureInfo _info;
        private readonly DateTime? _date;
        private readonly string _separator;

        public FileResultWriter(string filePath, CultureInfo info, DateTime? date = null, string separator = "\t")
        {
            _filePath = filePath;
            _info = info;
            _date = date;
            _separator = separator;
        }


        public void Print(List<HoldingChanges> holdingChanges)
        {
            using (StreamWriter outputFile = new StreamWriter(_filePath))
            {
                outputFile.Write(DiffStringGenerator.GenerateSeparatedString(holdingChanges, _info, _date, _separator));
            }
        }
    }
}