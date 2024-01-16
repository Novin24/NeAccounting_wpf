namespace DomainShared.Errore;

public static class NeErrorCodes
{

    public static string IsDuplicateException(string name)
    {
        return $"{name} تکراری است";
    }

    public static string NotFound(string name)
    {
        return $"{name} یافت نشد";
    }

    public static string InvalidId(string name)
    {
        return $"شناسه {name} نامعتبر است";
    }

    public static string Invalid(string name)
    {
        return $"{name} نامعتبر است";
    }

    public static string IsLess(string name1, string name2)
    {
        return $"{name1} باید از {name2} کمتر باشد";
    }

    public static string IsMore(string name1, string name2)
    {
        return $"{name1} باید از {name2} بیشتر باشد";
    }

    public static string MaxLength(string name, int length)
    {
        return $"طول {name} از {length} بیشتر است";
    }

    public static string EqualLength(string name, int length)
    {
        return $"طول {name} باید {length} باشد";
    }

    public static string IsMandatory(string name)
    {
        return $"وارد کردن {name} الزامی است !!!";
    }

    public static string Useing(string name)
    {
        return $"{name} مورد استفاده قرار گرفته شده است";
    }

}
