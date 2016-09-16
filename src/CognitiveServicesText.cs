using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// Implementation of https://www.microsoft.com/cognitive-services/en-us/text-analytics/documentation
/// </summary>
public class CognitiveServicesTextAnalysis : ICognitiveServicesTextAnalysis
{
    #region Requests
    private class TextRequest
    {
        public TextRequest()
        {
            Documents = new List<TextDocument>();
        }
        [JsonProperty("documents")]
        public List<TextDocument> Documents { get; set; }
    }
    private class TextDocument
    {
        public TextDocument(string text, string language)
        {
            Id = Guid.NewGuid().ToString();
            Language = language;
            Text = text;
        }
        [JsonProperty("language")]
        public string Language { get; private set; }
        [JsonProperty("id")]
        public string Id { get; private set; }
        [JsonProperty("text")]
        public string Text { get; private set; }
    } 
    #endregion

    private readonly HttpClient _httpClient;
    /// <summary>
    /// Cognitive Text service endpoint
    /// </summary>
    private const string serviceEndpoint = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/";
    public CognitiveServicesTextAnalysis(string apiKey)
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);
    }

    /// <summary>
    /// Key phrase analysis
    /// </summary>
    /// <param name="language"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public async Task<List<string>> KeyPhrases(string language, string text)
    {
        if(string.IsNullOrEmpty(language) || string.IsNullOrEmpty(text))
        {
            throw new ArgumentNullException();
        }
        var request = new TextRequest();
        request.Documents.Add(new TextDocument(text, language));
        var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
        var result = await _httpClient.PostAsync($"{serviceEndpoint}keyPhrases", content);
        var response = JObject.Parse(await result.Content.ReadAsStringAsync());
        CatchAndThrow(response);
        return response["documents"].Children().First().Value<JArray>("keyPhrases").ToObject<List<string>>();
    }

    /// <summary>
    /// Sentiment analysis
    /// </summary>
    /// <param name="language"></param>
    /// <param name="text"></param>
    /// <returns>From 0 to 1 (1 being totally positive sentiment)</returns>
    public async Task<double> Sentiment(string language, string text)
    {
        if(string.IsNullOrEmpty(language) || string.IsNullOrEmpty(text))
        {
            throw new ArgumentNullException();
        }
        var request = new TextRequest();
        request.Documents.Add(new TextDocument(text, language));
        var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
        var result = await _httpClient.PostAsync($"{serviceEndpoint}sentiment", content);
        var response = JObject.Parse(await result.Content.ReadAsStringAsync());
        CatchAndThrow(response);
        return response["documents"].Children().First().Value<double>("score");
    }

    /// <summary>
    /// Generic catch and throw that detects errors on the response body
    /// </summary>
    /// <param name="response"></param>
    private void CatchAndThrow(JObject response)
    {
        if (response["errors"] != null && response["errors"].Children().Any())
        {
            throw new Exception(response["errors"].Children().First().Value<string>("message"));
        }
    }
}
