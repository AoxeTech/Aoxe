namespace Zaaby.Core
{
    public interface IEntity<out TId>
    {
        TId Id { get;}
    }
}