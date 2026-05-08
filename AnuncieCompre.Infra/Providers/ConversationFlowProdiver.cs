using AnuncieCompre.Domain.Aggregates.ConversationAggregate.Flows;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Infra.Providers;

public class ConversationFlowProvider
{
    private readonly IReadOnlyDictionary<string, IConversationNode> InitialRegistration;
    private readonly IReadOnlyDictionary<string, IConversationNode> Customer;
    private readonly IReadOnlyDictionary<string, IConversationNode> Vendor;

    public ConversationFlowProvider()
    {
        InitialRegistration = InitialRegistrationFlow.Build();
        Customer = CustomerFlow.Build(InitialRegistration);
        Vendor = VendorFlow.Build(InitialRegistration);




        InitialRegistration["initial_ask_confirmation"].Transitions["1"] = Customer["customer_ask_cpf"];
        InitialRegistration["initial_ask_user_type"].Transitions["2"] = Customer["vendor_ask_company_category"];
    }
    
    public IConversationNode GetById(string? id)
    {
        if (id is null) return InitialRegistration["initial_start"];
        IConversationNode? conversationNode;

        if (InitialRegistration.TryGetValue(id, out conversationNode)) return conversationNode;
        if (Customer.TryGetValue(id, out conversationNode)) return conversationNode;
        if (Vendor.TryGetValue(id, out conversationNode)) return conversationNode;

        throw new KeyNotFoundException();
    }
}