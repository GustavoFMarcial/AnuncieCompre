namespace AnuncieCompre.Domain.Enums;

public enum CompanyCategory
{
    Autopeça = 1,
    MaterialdeConstrução,
    Automóvel,
    AparelhosEletrônicos,
    Eletrodomésticos,
}

public static class CompanyCategoryExtensions
{
    public static string PrintNames()
    {
        string message = "";
        string[] names = Enum.GetNames<CompanyCategory>();

        for (int i = 0; i < names.Length; i++)
        {
            message += $"{i + 1} - {names[i]}\n";
        }

        message += "0 - Nenhuma das opções";

        return message;
    }

    public static int Lenght()
    {
        Enum.GetValues<CompanyCategory>();
        return Enum.GetNames<CompanyCategory>().Length;
    }

    public static string[] ToStringArray()
    {
        return Enum.GetValues<CompanyCategory>()
            .Select(c => ((int)c).ToString())
            .ToArray();
    }
}