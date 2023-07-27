namespace Dr.Extensions.Logging.Abstractions;

public class EnhancerAccessor : IEnhancerAccessor
{
    private static readonly AsyncLocal<Enhancer?> _asyncLocal = new();

    public Enhancer? Current => _asyncLocal.Value;

    public Enhancer Create()
    {
        _asyncLocal.Value ??= new Enhancer(() => _asyncLocal.Value = null);
        return _asyncLocal.Value;
    }
}
