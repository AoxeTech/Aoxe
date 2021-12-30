namespace Zaaby.Shared;

public class TypePair
{
    public Type InterfaceType { get; }
    public Type ImplementationType { get; }

    public TypePair(Type interfaceType, Type implementationType) =>
        (InterfaceType, ImplementationType) = (interfaceType, implementationType);
}