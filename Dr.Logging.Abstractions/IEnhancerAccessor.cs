namespace Dr.Logging.Abstractions;

public interface IEnhancerAccessor
{
    Enhancer? Current { get; }

    Enhancer Create();
}