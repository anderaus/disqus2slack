using System;

public class DisqusResponse
{
    public Response[] Response { get; set; }
}

public class Response
{
    public string Raw_message { get; set; }
    public Author Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public Thread Thread { get; set; }
    public string Url { get; set; }
}

public class Author
{
    public string Name { get; set; }
    public string ProfileUrl { get; set; }
    public Avatar Avatar { get; set; }
}

public class Avatar
{
    public AvatarImage Small { get; set; }
}

public class AvatarImage
{
    public string Permalink { get; set; }
}

public class Thread
{
    public string Clean_title { get; set; }
    public int Posts { get; set; }
}