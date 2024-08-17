// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Rules;

#endregion

namespace Fortitude.Examples.Documentation.Wiki.FortitudeBusRules.GettingStarted.Step_1;

public class HelloHelloRule : Rule
{
    public override void Start()
    {
        Console.Out.WriteLine("You say Hello");
    }

    public override void Stop()
    {
        Console.Out.WriteLine("And I say Goodbye");
    }
}
