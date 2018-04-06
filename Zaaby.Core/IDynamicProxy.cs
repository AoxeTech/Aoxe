namespace Zaaby.Core
{
    public interface IDynamicProxy
    {
        T GetService<T>();
    }
}