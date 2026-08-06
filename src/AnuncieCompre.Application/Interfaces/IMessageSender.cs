namespace AnuncieCompre.Application.Interfaces;

public interface IMessageSender
{
    public Task SendMessageAsync(string to, string message);
}