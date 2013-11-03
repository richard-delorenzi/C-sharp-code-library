using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Richard
{
    static class toString
    {
        public static string AsString<T>(this System.Collections.Generic.IEnumerable<T> e)
        {
            Contract.Requires(e != null);
            string Result="";
            foreach (var v in e)
            {
                Result = ((Result == "") ? "" : Result + ", ") + v;
            }
            return Result;
        }
    }
}
