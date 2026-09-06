using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Services.ValueObjectValidators;

namespace AnuncieCompre.Domain.Services.ValueObjectFactories;

public class ValueObjectFactory
{
    public static IValueObjectValidator Handle(ValueObjectValidator valueObjectValidator)
    {
        return valueObjectValidator switch
        {
            ValueObjectValidator.CNPJ => new CnpjValidator(),
            ValueObjectValidator.CPF => new CpfValidator(),
            ValueObjectValidator.Email => new EmailValidator(),
            ValueObjectValidator.Name => new NameValidator(),
            ValueObjectValidator.Phone => new PhoneValidator(),
            ValueObjectValidator.Quantity => new QuantityValidator(),
            ValueObjectValidator.None => new NoneValidator(),
            _ => throw new NotImplementedException(),
        };
    }
}