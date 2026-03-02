namespace Core.Messages;

public class TestMessage : IMessage
{
    public string Subject => "Test Email";
    public string Body => "This is a test email from pokerbunch.com";
}