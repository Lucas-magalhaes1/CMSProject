namespace CMS.Domain.Services;

public interface IPrototype<T>
{
    T Clone();
}