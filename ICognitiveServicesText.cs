using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICognitiveServicesTextAnalysis
{
    Task<List<string>> KeyPhrases(string language, string text);
    Task<double> Sentiment(string language, string text);
}   