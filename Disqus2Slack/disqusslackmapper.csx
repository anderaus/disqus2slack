public static class DisqusSlackMapper
{
    public static SlackPost Map(Response disqusPost)
    {
        return new SlackPost
        {
            Text = $"New comment on <{disqusPost?.Url}|{disqusPost?.Thread?.Clean_title}>",
            Attachments = new[]
            {
                new Attachment
                {
                    Color = "#2E9FFF",
                    Author_icon = disqusPost?.Author?.Avatar?.Small?.Permalink,
                    Author_name = disqusPost?.Author?.Name,
                    Author_link = disqusPost?.Author?.ProfileUrl,
                    Text = disqusPost?.Raw_message,
                    Footer = $"{disqusPost?.Thread?.Posts} comments"
                }
            }
        };
    }
}