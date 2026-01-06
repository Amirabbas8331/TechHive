
namespace TechHive.Domain.Enums;

public sealed class Currency : Enumeration<Currency>
{
    public static readonly Currency USD =
        new(1, "USD", "US Dollar", "$");

    public static readonly Currency EUR =
        new(2, "EUR", "Euro", "€");

    public static readonly Currency IRR =
        new(3, "IRR", "Iranian Rial", "﷼");

    public string Symbol { get; }

    private Currency(int id, string code, string name, string symbol)
        : base(id, code, name)
    {
        Symbol = symbol;
    }
}


