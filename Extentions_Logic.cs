using Richard.Contracts;

namespace Richard
{
    static class LogicExtentions
    {
        public static bool Implies(this bool a, bool b)
        {
            var Result =  b || !a;
            Contract.Ensures(Result == (b || !a));
            return Result;
        }

        public static bool IFF(this bool a, bool b)
        {
            //If and only if.
            var Result = (b == a);
            Contract.Ensures(Result == (b == a) );
            return Result;
        }
    }
}
