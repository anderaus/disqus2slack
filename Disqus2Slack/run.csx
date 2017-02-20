#load ".\slackpost.csx"
#load ".\disqusresponse.csx"
#load ".\disqusslackmapper.csx"

#r "Newtonsoft.Json"

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

static readonly string SlackWebhookUrl = ConfigurationManager.AppSettings["SlackWebhookUrl"];
static readonly string DisqusApiUrlTemplate = ConfigurationManager.AppSettings["DisqusGetPostsUrlTemplate"];
static readonly int MaxSlackPosts = int.Parse(ConfigurationManager.AppSettings["MaxSlackPosts"] ?? "10");
static readonly int NewerThanMinutesAgo = int.Parse(ConfigurationManager.AppSettings["NewerThanMinutesAgo"] ?? "15");

static readonly HttpClient HttpClient = new HttpClient();

static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
{
    ContractResolver = new CamelCasePropertyNamesContractResolver()
};

public static void Run(TimerInfo myTimer, TraceWriter log)
{
    var cutoffTimeString = DateTime.UtcNow.AddMinutes(-NewerThanMinutesAgo).ToString("o");
    var disqusApiUrl = DisqusApiUrlTemplate.Replace("{starttime}", cutoffTimeString);
    
    log.Info($"Cutoff time is: {cutoffTimeString}");
    log.Info($"MaxSlackPosts time is: {MaxSlackPosts}");
    log.Info($"NewerThanMinutesAgo time is: {NewerThanMinutesAgo}");

    var latestPosts = GetLatestCommentsFromDisqus(log, disqusApiUrl).GetAwaiter().GetResult();
    if (latestPosts.Any())
    {
        var slackPosts = latestPosts.Take(MaxSlackPosts).Select(DisqusSlackMapper.Map).ToArray();
        SendSlackNotifications(log, slackPosts).GetAwaiter().GetResult();
    }
}

static async Task<Response[]> GetLatestCommentsFromDisqus(TraceWriter log, string disqusApiUrl)
{
    var result = await HttpClient.GetStringAsync(disqusApiUrl);
    var disqusComments = JsonConvert.DeserializeObject<DisqusResponse>(result, JsonSettings);
    log.Info($"Fetched {disqusComments?.Response?.Length} new comments from Disqus");
    return disqusComments?.Response;
}

static async Task SendSlackNotifications(TraceWriter log, IReadOnlyCollection<SlackPost> slackPosts)
{
    foreach (var post in slackPosts)
    {
        var slackPostJson = JsonConvert.SerializeObject(post, JsonSettings);
        await HttpClient.PostAsync(SlackWebhookUrl, new StringContent(slackPostJson));
    }
    log.Info($"Forwarded {slackPosts.Count} comments to Slack");
}