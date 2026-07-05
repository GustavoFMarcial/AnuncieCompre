namespace AnuncieCompre.UseCase.Interfaces;

public interface IMessageSender
{
    public Task SendMessageAsync(string to, string message);
}