using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWProgram.Enums
{
    public enum StatementEnum
    {
        InitiallyFluent,
        FluentAfterActionbyActor,
        FluentTypicallyAfterActionbyActor,
        ObservableFluentAfterActionByActor,
        ActionByActorCausesAlphaIfFluents,
        ActionByActorReleasesFluent1IfFluents,
        ActionByActorTypicallyCausesAlphaIfFluents,
        ActionByActorTypicallyReleasesFluent1IfFluents,
        ImpossibleActionByActorIfFluents,
        AlwaysPi,
        NoninertialFluent,
        None
    }
}
