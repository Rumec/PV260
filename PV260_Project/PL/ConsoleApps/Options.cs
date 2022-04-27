using CommandLine;

namespace PL.ConsoleApps
{
    /// <summary>
    /// Class used by CommandLineParser for parsing command line arguments
    /// </summary>
    public class Options
    {
        [Option('d', "delim", Required = true, HelpText = "Set the delimiter of CSV files.")]
        public string Delim { get; set; }
        
        // currently we provided input file paths as "file1_path,file2_path,...,fileN_path" - separated by comma
        // it would also be possible to create flags e.g. '--first' and '--second' for only two input files
        [Option('i', "input", Min = 2, Max = 2, Separator = ',', Required = true,
            HelpText = "Set the paths of two input CSV files, which you want to compare.")]
        public IEnumerable<String> InputFiles { get; set; }
        
        public List<String> InputFilesList
        {
            get => InputFiles.ToList();
        }
        
        [Option('o', "output", Required = false,
            HelpText =
                "Set the path of output file, where the comparison result will be written\n - if not set, STDOUT is used.")]
        public string? OutputFile { get; set;  }
    }
}