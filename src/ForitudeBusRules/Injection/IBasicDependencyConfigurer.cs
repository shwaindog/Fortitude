namespace FortitudeBusRules.Injection;

public interface IBasicDependencyConfigurer : IDependencyConfigurer
{
    IDependencyConfigurer BindInstance<T>(T instance);
    IDependencyConfigurer BindDefaultConstructorFactory<T>() where T : new();
    IDependencyConfigurer BindFactory<T>(Func<T> factory);
}
