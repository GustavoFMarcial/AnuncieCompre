namespace AnuncieCompre.Domain.Enums;

public enum UserType
{
    Unknown = 0,
    Customer,
    Vendor,
}

public class UserTypeExtensions
{
    public static string[] ToStringArray()
    {
        return 
            Enum.GetValues<UserType>()
            .Select(u => ((int)u).ToString())
            .ToArray();
    }
}