using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Conversation.NodeValidators;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Services.ValueObjectFactories;
using AnuncieCompre.Domain.Services.ValueObjectValidators;

namespace AnuncieCompre.Domain.Services.NodeValidatorFactories;

public class NodeValidatorFactory()
{
    public static INodeValidator Handle(ConversationNode node)
    {
        IValueObjectValidator valueObjectValidator = ValueObjectFactory.Handle(node.ValueObjectValidator);

        return node.ValidationKind switch
        {
            ValidationKind.Final => new FinalNodeValidator(),
            ValidationKind.None => new NoneNodeValidator(),
            ValidationKind.Option => new OptionNodeValidator(node.Options),
            ValidationKind.Validation => new ValidationNodeValidator(valueObjectValidator),
            _ => throw new NotImplementedException(),
        };
    }
}