using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Nexus.Services
{
    public class HuggingFaceService
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;
        private readonly string _modelEndpoint;

        public HuggingFaceService(IConfiguration configuration)
        {
            _client = new HttpClient();
            _apiKey = configuration["HuggingFaceSettings:ApiKey"];
            _modelEndpoint = configuration["HuggingFaceSettings:ModelEndpoint"];

            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<string> GetAiResponse(string userMessage)
        {
            var requestBody = new
            {
                inputs = $"<s>[INST] {userMessage} [/INST]",
                parameters = new
                {
                    max_new_tokens = 512,
                    temperature = 0.7,
                    top_p = 0.95,
                    do_sample = true
                }
            };

            var requestJson = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(_modelEndpoint, content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.PaymentRequired)
                {
                    return "Sorry, this model requires a paid plan. Please upgrade your Hugging Face account or switch to a free model.";
                }
                return $"Sorry, I couldn't process your request. Status code: {response.StatusCode}";
            }

            var resultArray = JsonConvert.DeserializeObject<JArray>(responseString);
            if (resultArray != null && resultArray.Count > 0 && resultArray[0]["generated_text"] != null)
            {
                var generatedText = resultArray[0]["generated_text"].ToString();
                if (!string.IsNullOrEmpty(generatedText))
                {
                    var responsePart = generatedText.Contains("[/INST]")
                        ? generatedText.Split(["[/INST]"], StringSplitOptions.None)[1].Trim()
                        : generatedText;
                    return responsePart;
                }
            }



            return $"AI response: {responseString.Replace("\"", "").Replace("\\n", "\n")}";
        }
    }
}
