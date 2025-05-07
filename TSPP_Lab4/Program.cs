using System;
using System.Collections.Generic;

// Singleton – Logger
public class Logger
{
    private static readonly Lazy<Logger> _instance = new(() => new Logger());
    private Logger() { }

    public static Logger Instance => _instance.Value;

    public void Log(string message)
    {
        Console.WriteLine($"[LOG {DateTime.Now:HH:mm:ss}] {message}");
    }
}

// Adapter – Text format converter

public interface ITextFormat
{
    void Load(string content);
    string GetFormatted();
}

public class TxtFormat : ITextFormat
{
    private string _content;
    public void Load(string content) => _content = content;
    public string GetFormatted() => $"TXT: {_content}";
}

public class JsonFormat : ITextFormat
{
    private string _content;
    public void Load(string content) => _content = content;
    public string GetFormatted() => $"{{ \"data\": \"{_content}\" }}";
}

public class XmlFormat : ITextFormat
{
    private string _content;
    public void Load(string content) => _content = content;
    public string GetFormatted() => $"<data>{_content}</data>";
}

public class TextFormatAdapter
{
    private ITextFormat _format;

    public TextFormatAdapter(ITextFormat format)
    {
        _format = format;
    }

    public void SetContent(string content)
    {
        _format.Load(content);
    }

    public void DisplayFormatted()
    {
        Console.WriteLine(_format.GetFormatted());
    }
}

// Observer – Chat notification system

public class Chat
{
    public string Name { get; }
    public event Action<string, string> MessageReceived;

    public Chat(string name)
    {
        Name = name;
    }

    public void SendMessage(string message)
    {
        Logger.Instance.Log($"[{Name}] Message sent: {message}");
        MessageReceived?.Invoke(Name, message);
    }
}

public class ChatUser
{
    private string _username;

    public ChatUser(string username)
    {
        _username = username;
    }

    public void Subscribe(Chat chat)
    {
        chat.MessageReceived += ReceiveMessage;
        Logger.Instance.Log($"{_username} subscribed to {chat.Name}");
    }

    private void ReceiveMessage(string chatName, string message)
    {
        Console.WriteLine($"{_username} received in {chatName}: {message}");
    }
}

// Тестування
class Program
{
    static void Main()
    {
        // Singleton test
        Logger.Instance.Log("Starting application...");

        // Adapter test
        var txtAdapter = new TextFormatAdapter(new TxtFormat());
        txtAdapter.SetContent("Hello TXT");
        txtAdapter.DisplayFormatted();

        var jsonAdapter = new TextFormatAdapter(new JsonFormat());
        jsonAdapter.SetContent("Hello JSON");
        jsonAdapter.DisplayFormatted();

        var xmlAdapter = new TextFormatAdapter(new XmlFormat());
        xmlAdapter.SetContent("Hello XML");
        xmlAdapter.DisplayFormatted();

        // Observer test
        var generalChat = new Chat("General Chat");
        var userA = new ChatUser("Oleg");
        var userB = new ChatUser("Dima");

        userA.Subscribe(generalChat);
        userB.Subscribe(generalChat);

        generalChat.SendMessage("Welcome to the chat!");
    }
}