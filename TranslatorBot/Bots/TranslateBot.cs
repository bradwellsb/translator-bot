// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.10.2

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Options;
using TranslatorBot.Models;
using TranslatorBot.Models.Config;
using System.Text.Json;
using System.Net.Http;
using System;
using System.Text;
using TranslatorBot.Models;
using System.Linq;

namespace TranslatorBot.Bots
{
    public class TranslateBot : ActivityHandler
    {
        private readonly IOptions<TranslatorConfig> _options;
        
        public TranslateBot(IOptions<TranslatorConfig> options)
        {
            _options = options;
        }

        private async Task<DetectLanguageResult> DetectLanguageAsync(string subscriptionKey, string endpoint, string route, string inputText)
        {
            object[] body = new object[] { new { Text = inputText } };
            var requestBody = JsonSerializer.Serialize(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Build the request.
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                // Send the request and get response.
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);

                // Read response as a stream.
                var result = await response.Content.ReadAsStreamAsync();
                return (await JsonSerializer.DeserializeAsync<DetectLanguageResult[]>(result)).FirstOrDefault();
            }
        }

        private async Task<TranslationResult> TranslateTextAsync(string subscriptionKey, string endpoint, string route, string inputText)
        {
            object[] body = new object[] { new { Text = inputText } };
            var requestBody = JsonSerializer.Serialize(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Build the request.
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                // Send the request and get response.
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);

                // Read response as a stream.
                var result = await response.Content.ReadAsStreamAsync();
                return (await JsonSerializer.DeserializeAsync<TranslationResult[]>(result)).FirstOrDefault();
            }
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            string detectRoute = "/detect?api-version=3.0";
            string translateRoute = null;
            string replyText = null;
            var detectResult = await DetectLanguageAsync(_options.Value.Key, _options.Value.Endpoint, detectRoute, turnContext.Activity.Text);

            switch (detectResult.language)
            {
                case "en":
                    translateRoute = "/translate?api-version=3.0&to=fr";
                    break;
                case "fr":
                    translateRoute = "/translate?api-version=3.0&to=en";
                    break;
                default:
                    translateRoute = null;
                    break;
            }

            if (!string.IsNullOrEmpty(translateRoute))
            {
                var translateResult = await TranslateTextAsync(_options.Value.Key, _options.Value.Endpoint, translateRoute, turnContext.Activity.Text);
                replyText = translateResult.translations.FirstOrDefault().text;
            }
            else
            {
                replyText = "Uh-oh... I only support French and English translations.";
            }
            
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Enter the text to translate.";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
