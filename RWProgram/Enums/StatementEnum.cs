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
        FluentAfterActions,
        ObservableFluentAfterActions,
        ActionCausesAlphaIfFluents,
        ActionReleasesFluent1IfFluents,
        ImpossibleActionIfFluents,
        AlwaysFluent,
        NoninertialFluent,
    }
}
