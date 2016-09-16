# Microsoft Cognitive Services Text Analysis API
This is a **.Net Core** implementation of [Microsoft Cognitive Services Text Analysis](https://www.microsoft.com/cognitive-services/en-us/text-analytics-api) API as described by the [documentation](https://westus.dev.cognitive.microsoft.com/docs/services/TextAnalytics.V2.0/operations/56f30ceeeda5650db055a3c7).

## Analysis of Key phrases 

    ICognitiveServicesTextAnalysis textAnalysis = new CognitiveServicesTextAnalysis(ApiKey);
    var keyPhrases = await textAnalysis.KeyPhrases(language:"en", text:"I had a wonderful experience! The rooms were wonderful and the staff were helpful.");

Returns:

    [
    	"staff",
    	"wonderful experience",
    	"rooms"
    ]

## Analysis of Sentiment

The Sentiment is a number from 0 to 1 that expresses positiveness, 1 being totally positive and 0 totally negative.

    ICognitiveServicesTextAnalysis textAnalysis = new CognitiveServicesTextAnalysis(ApiKey);
    var keyPhrases = await textAnalysis.Sentiment(language:"en", text:"I had a wonderful experience! The rooms were wonderful and the staff were helpful.");

Returns:

`0.9750894`


