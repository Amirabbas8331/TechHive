
namespace TechHive.Domain.Enums;

public sealed class Currency : Enumeration<Currency>
{
    public string Symbol { get; }

    public Currency(int id, string code, string name, string symbol)
        : base(id, code, name)
    {
        Symbol = symbol;
    }

    public static void LoadFromList(
        IEnumerable<(int Id, string Code, string Name, string Symbol)> items
    )
    {

    }
}

