namespace Shared;

public class User
{
    public User(ConsoleColor color,string username)
    {
        Color = color;
        Username = username;
    }
    
    public ConsoleColor Color;
    public string Username;
}

public class Message
{
    public Message(User user, string body)
    {
        User = user;
        Body = body;
    }

    public User User;
    public string Body;
    
    public void Print()
    {
        Console.ForegroundColor = User.Color;
        Console.Write($"{User.Username}:");
        Console.ResetColor();
        Console.WriteLine($" {Body}");
    }
}

