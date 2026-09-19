using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.MessageProviderReferenceAggregate;

public class MessageProviderReference : BaseEntity
{
    public Guid MessageId { get; private set; }
    public Message Message { get; private set; } = default!;
    public MessageProvider Provider { get; private set; }
    public string ProviderMessageId { get; private set; } = default!;

    private MessageProviderReference() {}

    private MessageProviderReference(Message message, MessageProvider provider, string providerMessageId)
    {
        MessageId = message.Id;
        Message = message;
        Provider = provider;
        ProviderMessageId = providerMessageId;
    }

    public static Result<MessageProviderReference> Create(Message message, MessageProvider provider, string providerMessageId)
    {
        MessageProviderReference reference = new (message, provider, providerMessageId);
        return Result<MessageProviderReference>.Success(reference, "MessageProviderReference criado com sucesso");
    }
}