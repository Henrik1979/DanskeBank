namespace DanskeBank;
public class Configuration
{
    public string[] InputFiles { get; init; }
    public string OutputPath { get; init; }
    public string ExcludeFilePath { get; init; }
    public string OutPutExcludeFileName => "exclude_word_count.txt";
}