#region

using FortitudeBusRules.Rules;

#endregion

namespace FortitudeBusRules.Injection;

public interface IDependencyResolver
{
    T Resolve<T>();
    IRule ResolveRule(Type ruleType);
}
