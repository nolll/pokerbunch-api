namespace Tests.Integration;

public class OrderAttribute(int suite, int order) : Attribute
{
    public int SortOrder { get; } = suite * 100 + order;
}