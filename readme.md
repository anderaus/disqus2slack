# Disqus2Slack
Posts Disqus comment notifications to Slack

## Configuration

Set these keys in Azure application settings

SlackWebhookUrl (required)  
DisqusGetPostsUrlTemplate (required)  
MaxSlackPosts (deafult = 10)  
NewerThanMinutesAgo (default = 15)

## How to run locally

git clone https://github.com/anderaus/disqus2slack.git  
npm i -g azure-functions-cli  
func azure login  
func azure functionapp fetch disqus2slack  
func run disqus2slack