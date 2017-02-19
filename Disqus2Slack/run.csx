using System;
using System.Configuration;

public static void Run(TimerInfo myTimer, TraceWriter log)
{
    log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

    SendSlackNotification(log).Wait();
}

static async Task SendSlackNotification(TraceWriter log) {
    var webhookUrl = ConfigurationManager.AppSettings["SlackWebhookUrl"];

    using (var client = new HttpClient()) {
        client.DefaultRequestHeaders.Add("Accept", "application/json");

        var result = await client.PostAsJsonAsync(webhookUrl, new { text = "Hello Slack from Azure Function!"});
    }
}