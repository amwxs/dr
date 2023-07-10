namespace Dr.Extensions.Logging.Abstractions.Test;
public class EnhancerAccessorTest
{
    [Fact]
    public void EnhancerAccessorCreateTest()
    {
        var enhancerAccessor = new EnhancerAccessor();
        var enhancer1 = enhancerAccessor.Create();
        var enhancer2 = enhancerAccessor.Create();

        enhancer1.TryAdd("item1", 1);
        enhancer1.TryAdd("item2", 2);


        Assert.NotNull(enhancer1);
        Assert.NotNull(enhancer2);
        Assert.Equal(enhancer1, enhancer2);

        Assert.Equal(2, enhancer2.Items.Count);

        enhancer2.Dispose();

        Assert.Null(enhancerAccessor.Current);
    }
}
