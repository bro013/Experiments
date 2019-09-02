using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using static AwesomeSauceApi.Controllers.PhotoController;

namespace AwesomeSauceApi.Models
{
    public class AwesomeModelBinder:IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            const string propertyName = "photo";
            var valueProviderResult = bindingContext.ValueProvider.GetValue(propertyName);
            var base64Value = valueProviderResult.FirstValue;

            if (!string.IsNullOrEmpty(base64Value))
            {
                var bytes = Convert.FromBase64String(base64Value);
                var emotionResult = await GetEmotionResultAsync(bytes);
                var score = emotionResult.First().Scores;
                var result = new EmotionalPhoto
                {
                    Contents = bytes,
                    Scores = score
                };
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            await Task.FromResult(Task.CompletedTask);
        }

        private static async Task<EmotionResultDto[]> GetEmotionResultAsync(byte[] byteArray)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "f761a1b5bfb941aa9293e162d7d9ea7d");
            var uri = "https://northeurope.api.cognitive.microsoft.com/emotion/v1.0/recognise?";
            using (var content = new ByteArrayContent(byteArray))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                var response = await client.PostAsync(uri, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<EmotionResultDto[]>(responseContent);
                return result;
            }
        }


    }


}
