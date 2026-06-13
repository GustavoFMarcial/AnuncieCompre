namespace AnuncieCompre.Domain.Enums;

public enum CompanyCategory
{
    Autopeça = 1,
    MaterialdeConstrução,
    Automóvel,
    AparelhosEletrônicos,
    Eletrodomésticos,
    SemCategoria,
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

    public static CompanyCategory StringToCompanyCategory(string companyCategory)
    {
        return companyCategory switch
        {
            "Autopeça" => CompanyCategory.Autopeça,
            "MaterialdeConstrução" => CompanyCategory.MaterialdeConstrução,
            "Automóvel" => CompanyCategory.Automóvel,
            "AparelhosEletrônicos" => CompanyCategory.AparelhosEletrônicos,
            "Eletrodomésticos" => CompanyCategory.Eletrodomésticos,
            _ => CompanyCategory.SemCategoria,
        };
    }
}