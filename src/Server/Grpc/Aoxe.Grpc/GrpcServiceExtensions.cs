namespace Aoxe.Grpc;

public static class GrpcServiceExtensions
{
    private static readonly ConcurrentDictionary<Type, Type> GeneratedContracts = new();

    public static void AddDynamicGrpcServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Find all public classes with public methods
        var serviceClasses = assembly
            .GetTypes()
            .Where(
                t =>
                    t is { IsClass: true, IsAbstract: false, IsPublic: true }
                    && t.GetMethods(
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly
                    ).Length != 0
            );

        foreach (var serviceClass in serviceClasses)
        {
            // Generate a service contract (interface) for each class
            var contract = GeneratedContracts.GetOrAdd(serviceClass, CreateServiceContract);

            // Register the service implementation
            services.AddSingleton(serviceClass);

            // Register the contract with the DI container
            services.AddSingleton(
                contract,
                serviceProvider => serviceProvider.GetRequiredService(serviceClass)
            );
        }

        // Add protobuf-net code-first gRPC services
        services.AddCodeFirstGrpc(_ =>
        {
            // Optional: Configure protobuf-net settings
        });
    }

    private static Type CreateServiceContract(Type serviceClass)
    {
        var assemblyName = new AssemblyName("DynamicGrpcContracts");
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
            assemblyName,
            AssemblyBuilderAccess.Run
        );
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");

        var interfaceName = $"I{serviceClass.Name}";
        var interfaceBuilder = moduleBuilder.DefineType(
            interfaceName,
            TypeAttributes.Public | TypeAttributes.Interface | TypeAttributes.Abstract
        );

        // Add [ServiceContract] attribute
        var serviceContractAttr = typeof(ServiceContractAttribute);
        var customAttrBuilder = new CustomAttributeBuilder(
            serviceContractAttr.GetConstructor(Type.EmptyTypes)!,
            []
        );
        interfaceBuilder.SetCustomAttribute(customAttrBuilder);

        // Define methods
        var methods = serviceClass.GetMethods(
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly
        );
        foreach (var method in methods)
        {
            var parameterTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();
            var methodBuilder = interfaceBuilder.DefineMethod(
                method.Name,
                MethodAttributes.Public | MethodAttributes.Abstract | MethodAttributes.Virtual,
                CallingConventions.HasThis,
                method.ReturnType,
                parameterTypes
            );

            // Add [OperationContract] attribute to each method
            var operationContractAttr = typeof(OperationContractAttribute);
            var operationAttrBuilder = new CustomAttributeBuilder(
                operationContractAttr.GetConstructor(Type.EmptyTypes)!,
                []
            );
            methodBuilder.SetCustomAttribute(operationAttrBuilder);
        }

        return interfaceBuilder.CreateTypeInfo();
    }
}
