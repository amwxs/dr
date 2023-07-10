namespace Dr.Extensions.Logging.Abstractions;

public class EnhancerAccessor : IEnhancerAccessor
{
    private static readonly AsyncLocal<Enhancer?> _asyncLocal = new();

    private static readonly Lazy<Enhancer> _lazyEnhancer = new
        (() => new Enhancer(() => _asyncLocal.Value = null));

    public Enhancer? Current => _asyncLocal.Value;

    public Enhancer Create()
    {
        _asyncLocal.Value ??= _lazyEnhancer.Value;
        return _asyncLocal.Value;
    }
}
