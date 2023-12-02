#region

using Fortitude.EventProcessing.BusRules.Rules;

#endregion

namespace Fortitude.EventProcessing.BusRules.Injection;

public interface IDependencyResolver
{
    T Resolve<T>();
    IRule ResolveRule(Type ruleType);
}
