//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RWProgram.Classes
//{
//    public class LogicOperator
//    {
//        public override string ToString() { return string.Empty; }

//        public virtual RWLogic.Formula.symbol ToLogic() { return RWLogic.Formula.symbol.NULL; }
//    }

//    public class And : LogicOperator
//    {
//        public override string ToString() { return "and"; }

//        public override RWLogic.Formula.symbol ToLogic()
//        {
//            return RWLogic.Formula.symbol.AND;
//        }
//    }

//    public class Or : LogicOperator
//    {
//        public override string ToString() { return "or"; }

//        public override RWLogic.Formula.symbol ToLogic()
//        {
//            return RWLogic.Formula.symbol.OR;
//        }
//    }

//    public class Implies : LogicOperator
//    {
//        public override string ToString() { return "→"; }

//        public override RWLogic.Formula.symbol ToLogic()
//        {
//            return RWLogic.Formula.symbol.THEN;
//        }
//    }
//}
