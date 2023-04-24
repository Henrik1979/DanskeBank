using DanskeBank.FileReader;
using DanskeBank.WordCounter;
using SimpleInjector;
using System.CommandLine;
using DanskeBank.FileWriter;
using DanskeBank.Guard;
using DanskeBank.LineSplitter;
using DanskeBank.WordExcluder;
using DanskeBank;
using System.Threading.Channels;
using DanskeBank.FileReaderManager;
using DanskeBank.FileWriterManager;
using DanskeBank.WordCountManager;
using DanskeBank.WordCountWorkFlow;

class Program
{
    private static readonly Container container;

    static Program()
    {
        char[] separators = { ',', '.', ' ', ';' };
        container = new Container();

        // Register dependencies
        container.RegisterSingleton<IWordCountWorkFlow, WordCountWorkFlow>();
        container.RegisterSingleton<IFileWriterManager, FileWriterManager>();
        container.RegisterSingleton<IFileReader, StreamFileReader>();
        container.RegisterSingleton(() => Guard.Init);    
        container.RegisterSingleton<IFileWriter, StreamFileWriter>();    
        container.RegisterSingleton<ILineSplitter>(() => new LineSplitter(container.GetInstance<IGuard>()));
        container.RegisterSingleton<IWordCounter>(() => new ConcurrentDictionaryWordCounter(container.GetInstance<IGuard>()));
        container.RegisterSingleton<IWordExcluder>(() => new WordExcluder(container.GetInstance<IGuard>()));
        container.RegisterSingleton<IWordCountManager, WordCountManager>();
        container.RegisterSingleton<IFileReaderManager>(() => new FileReaderManager(
                container.GetInstance<IGuard>(),
                container.GetInstance<IFileReader>(),
                container.GetInstance<ILineSplitter>(),
                separators));

        // Verify the container's configuration
        container.Verify();
    }

    public static Configuration BuildConfiguration(string absoluteInputPath, string absoluteOutputPath, string excludeFileName)
    {
        var excludeFile = Directory.EnumerateFiles(absoluteInputPath, "*.txt", SearchOption.AllDirectories).FirstOrDefault(s => s.EndsWith(excludeFileName)) ?? throw new ArgumentException("No exclude file found");
        var files = Directory.EnumerateFiles(absoluteInputPath, "*.txt", SearchOption.AllDirectories).Where(s => !s.EndsWith(excludeFileName)).ToArray();

        if (files.Length == 0)
        {
            throw new ArgumentException("No input files found");
        }

        return new Configuration
        {
            InputFiles = files.ToArray(),
            OutputPath = absoluteOutputPath,
            ExcludeFilePath = excludeFile
        };
    }

    public static async Task<int> Main(string[] args)
    {
        const string DefaultInputPath = "./Data";
        const string DefaultOutputPath = "./OutputData";
        const string DefaulExcludeFileName = "exclude.txt";

        var inputPathOption = new Option<string>("--input", $"Path to the directory, where the input files are located)");
        inputPathOption.AddAlias("-i");
        inputPathOption.SetDefaultValue(DefaultInputPath);

        var outputPathOption = new Option<string>("--output", $"Path to the directory, where processed data will be saved");
        outputPathOption.AddAlias("-o");
        outputPathOption.SetDefaultValue(DefaultOutputPath);

        var excludeFileName = new Option<string>("--excludefile", $"Name of the exclude file");
        excludeFileName.AddAlias("-x");
        excludeFileName.SetDefaultValue(DefaulExcludeFileName);

        var rootCommand = new RootCommand("Word counting utility");
        rootCommand.AddOption(inputPathOption);
        rootCommand.AddOption(outputPathOption);
        rootCommand.AddOption(excludeFileName);

        rootCommand.SetHandler(async (context) =>
        {
            CancellationTokenSource source = new();

            try
            {            
                // Configure
                string @in = context.ParseResult.GetValueForOption(inputPathOption) ?? DefaultInputPath;
                string @out = context.ParseResult.GetValueForOption(outputPathOption) ?? DefaultOutputPath;
                string @name = context.ParseResult.GetValueForOption(excludeFileName) ?? DefaulExcludeFileName;

                var channel = Channel.CreateUnbounded<string>(new UnboundedChannelOptions
                {
                    SingleWriter = false,
                    SingleReader = true,
                    AllowSynchronousContinuations = true
                });

                var absoluteInputPath = Path.GetFullPath(@in);
                var absoluteOutputPath = Directory.CreateDirectory(@out).FullName;
                var configuration = BuildConfiguration(absoluteInputPath, absoluteOutputPath, name);


                //Preload exclude words
                var fileReader = container.GetInstance<IFileReader>();
                var wordexcluder = container.GetInstance<IWordExcluder>();
                await foreach (var word in fileReader.ReadAllLinesAsync(configuration.ExcludeFilePath, source.Token))
                {
                    wordexcluder.AddExcludeWord(word);
                }

                // Run workflow
                await container.GetInstance<IWordCountWorkFlow>().Execute(
                    channel,
                    configuration,
                    source.Token,
                    wordCorrector: (word) => word.ToLower(),
                    channelReaderFilter: string.IsNullOrWhiteSpace);


                // Summary
                Console.WriteLine($"Unique words: '{container.GetInstance<IWordCounter>().GetWordCount().Count()} Total count: {container.GetInstance<IWordCounter>().GetWordCount().Sum(x => x.Value)}");
                Console.WriteLine($"Exclude words: '{container.GetInstance<IWordExcluder>().GetExcludeWordCount().Count()}' Total words excluded: '{container.GetInstance<IWordExcluder>().GetExcludeWordCount().Sum(x => x.Value)}");                           
                Console.WriteLine($"Words read in total: '{container.GetInstance<IWordExcluder>().GetExcludeWordCount().Sum(x => x.Value) + container.GetInstance<IWordCounter>().GetWordCount().Sum(x => x.Value)}'");

                context.ExitCode = 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                source.Cancel();
                context.ExitCode = -1;
            }
        });

        return await rootCommand.InvokeAsync(args);
    }
}