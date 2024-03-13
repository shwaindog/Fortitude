#region

using FortitudeBusRules.Rules;

#endregion

namespace FortitudeBusRules.Injection;

public class BasicDependencyResolver : IDependencyResolver, IBasicDependencyConfigurer
{
    private readonly Dictionary<Type, Func<object>> typeResolver = [];

    public IDependencyConfigurer Configure() => this;

    public IDependencyConfigurer BindInstance<T>(T instance)
    {
        typeResolver.Add(typeof(T), () => instance!);
        return this;
    }

    public IDependencyConfigurer BindDefaultConstructorFactory<T>() where T : new()
    {
        typeResolver.Add(typeof(T), () => new T());
        return this;
    }

    public IDependencyConfigurer BindFactory<T>(Func<T> factory)
    {
        typeResolver.Add(typeof(T), () => factory()!);
        return this;
    }

    public T Resolve<T>() => (T)typeResolver[typeof(T)].Invoke();

    public IRule ResolveRule(Type ruleType) => (IRule)typeResolver[ruleType].Invoke();
}
