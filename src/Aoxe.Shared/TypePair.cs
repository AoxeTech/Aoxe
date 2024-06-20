namespace Aoxe.Shared;

public class TypePair(Type interfaceType, Type implementationType)
{
    public Type InterfaceType { get; } = interfaceType;
    public Type ImplementationType { get; } = implementationType;
}
