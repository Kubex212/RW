using System;
using System.Collections.Generic;
using System.Text;

namespace RWLogic
{
    public class Test
    {
        List<Noninertial> noninertial = new List<Noninertial>();
        List<Always> always = new List<Always>();
        List<Causes> causes = new List<Causes>();
        List<TypicallyCauses> typicallyCauses = new List<TypicallyCauses>();
        List<Releases> releases = new List<Releases>();
        List<TypicallyReleases> typicallyReleases = new List<TypicallyReleases>();
        List<Initially> initially = new List<Initially>();
        List<After> after = new List<After>();
        List<TypicallyAfter> typicallyAfter = new List<TypicallyAfter>();
        List<ObservableAfter> observableAfter = new List<ObservableAfter>();

        public void Run()
        {
            // Przyklad 1

            // List<string> fluents = new() { "loaded", "alive" };
            // List<string> agents = new() { "Bill", "Jim" };
            // List<string> actions = new() { "LOAD", "SHOOT" };

            // initially.Add( new Initially( new Formula((1, true), (0, false), Formula.symbol.AND) ) );
            // causes.Add(new Causes( 0, 0, new Formula((0, true))) ); //load by bill causes loaded
            // causes.Add(new Causes( 0, 1, new Formula((0, false))) ); //shoot by bill causes not loaded
            // causes.Add(new Causes( 1, 1, new Formula((0, false)))); //shoot by jim causes not loaded
            // typicallyCauses.Add(new TypicallyCauses( 0, 1, new Formula((1, false)), new Formula((0, true)) ) ); //shoot by bill typically causes not alive if loaded
            // causes.Add(new Causes( 1, 1, new Formula(false)) ); //shoot by jim causes false

            // Przyklad 2

            //List<string> fluents = new() { "f", "g" };
            //List<string> agents = new() { "Tom" };
            //List<string> actions = new() { "A" };

            //always.Add(new Always(new Formula((0, true), (1, true), Formula.symbol.OR)));
            //initially.Add(new Initially(new Formula((0, true), (1, true), Formula.symbol.AND)));
            //causes.Add(new Causes(0, 0, new Formula((0, false))));
            //typicallyCauses.Add(new TypicallyCauses(0, 0, new Formula((1, false)), new Formula((0, true))));

            // Przyklad 3

            //List<string> fluents = new List<string>() { "loaded", "alive" };
            //List<string> agents = new List<string>() { "Bill" };
            //List<string> actions = new List<string>() { "LOAD", "SHOOT", "SPIN" };

            //initially.Add(new Initially(new Formula((0, false))));
            //causes.Add(new Causes(0, 0, new Formula((0, true))));
            //causes.Add(new Causes(0, 1, new Formula((0, false))));
            //causes.Add(new Causes(0, 1, new Formula((1, false)), new Formula((0, true))));
            //releases.Add(new Releases(0, 2, 0, new Formula((0, true))));
            //after.Add(new After(new List<(int agent, int action)>() { (0, 1) }, new Formula((1, false))));

            //Fasada X = new Fasada(fluents, agents, actions, noninertial, always, causes, typicallyCauses, releases, typicallyReleases, initially, after, typicallyAfter, observableAfter);
            //Console.WriteLine(X.test());
        }
    }
}
