//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RWProgram.Classes
//{
//    public static class Helpers
//    {
//        public static string GetAlpha(List<Fluent> alpha, List<LogicOperator> operators)
//        {
//            var str = alpha.FirstOrDefault()?.ToString();
//            for (var i = 0; i < operators.Count; i++)
//            {
//                str = str + " " + operators[i].ToString() + " " + alpha[i + 1].ToString();
//            }

//            return str;
//        }
//    }
//}
