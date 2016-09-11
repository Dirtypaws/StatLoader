using System;
using System.Linq;
using CommandLine;
using CommandLine.Text;
using Nest;
using Newtonsoft.Json;

namespace StatLoader
{
    class Program
    {
        public class Options
        {
            [Option('p', "path", Required = true, HelpText = "Supply the path to the Excel file you'd like to load into Elasticsearch")]
            public string Path { get; set; }

            [Option('s', "server", Required = true, HelpText = "Supply the path to the Elasticsearch server (ex: http://localhost:9200/")]
            public string Server { get; set; }

            [Option('i', "index", DefaultValue = "players", HelpText = "The 'index' on Elasticsearch where the data wil be loaded")]
            public string Index { get; set; }

            [Option('c', "chunkSize", DefaultValue = 2000, HelpText = "Specify the chunk size to use when loading data to the 'index'")]
            public int ChunkSize { get; set; }

            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
            }
        }

        static void Main(string[] args)
        {
            var opts = new Options();
            if (Parser.Default.ParseArguments(args, opts))
            {
                RebuildIndex(opts.Path, opts.Server, opts.Index.ToLower(), opts.ChunkSize);
            }
            else
                Console.WriteLine(opts.GetUsage());
        }

        public static void RebuildIndex(string path, string server, string index = "players", int chunkSize = 2000)
        {
            var players = Reader.GetPlayers(path);

            var conn = new ConnectionSettings(new Uri(server));
            var client = new ElasticClient(conn);

            var ex = client.IndexExists(index);
            if (ex.Exists) client.DeleteIndex(index);

            var skaters = players.Item1.Select((s, i) => new { Value = s, Index = i })
                     .GroupBy(item => item.Index / chunkSize, item => item.Value);

            foreach (var chunk in skaters)
                client.IndexMany(chunk, index);

            var goalies = players.Item2.Select((s, i) => new { Value = s, Index = i })
                .GroupBy(item => item.Index / chunkSize, item => item.Value);

            foreach (var chunk in goalies)
                client.IndexMany(chunk, index);
        }
    }
}
