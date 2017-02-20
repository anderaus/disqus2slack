public class SlackPost
{
    public string Text { get; set; }
    public Attachment[] Attachments { get; set; }
}

public class Attachment
{
    public string Color { get; set; }
    public string Text { get; set; }
    public string Author_name { get; set; }
    public string Author_icon { get; set; }
    public string Author_link { get; set; }
    public string Footer { get; set; }
}