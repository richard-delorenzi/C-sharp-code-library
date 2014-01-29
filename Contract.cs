using System;
using System.Collections.Generic;
using System.Text;

namespace Richard.Contracts
{
    public class Contract
    {
        /*!
         * \short 
         * A simple implementation of "Design By Contract / Code Contracts"
         * 
         * \details
         * \see 
         *   http://www.eiffel.com/developers/design_by_contract.html and
         *   http://en.wikipedia.org/wiki/Design_by_contract
         *   
         * 
         */


        public static void Requires(bool pred) { Requires(pred,"");}
        public static void Requires(bool pred, string message)
        {
            var stack = 
                "\n" + new System.Diagnostics.StackTrace(new System.Diagnostics.StackFrame(1)).ToString() +
                "\n" + new System.Diagnostics.StackTrace(new System.Diagnostics.StackFrame(2)).ToString();
            if (!pred) { throw new PreconditionException(message, stack); }
        }

        public static void Ensures(bool pred) { Ensures(pred,"");}
        public static void Ensures(bool pred, string message)
        {
            var stack =
                "\n" + new System.Diagnostics.StackTrace(new System.Diagnostics.StackFrame(1)).ToString();
            if (!pred) { throw new PostconditionException(message, stack);}
        }

        public static void Invariant(bool pred) { Invariant(pred,"");}
        public static void Invariant(bool pred, string message )
        {
            var stack =
                "\n" + new System.Diagnostics.StackTrace(new System.Diagnostics.StackFrame(1)).ToString();
            if (!pred) { throw new InvariantException(message, stack); }
        }

        public static void Check(bool pred) { Check(pred,"");}
        public static void Check(bool pred, string message)
        {
            var stack =
                "\n" + new System.Diagnostics.StackTrace(new System.Diagnostics.StackFrame(1)).ToString();
            if (!pred) { throw new CheckException(message, stack); }
        }
    }

    public class ContractException : Exception
    {
        public ContractException(string msg, string stack) :
            base(((msg == null) ? "" : msg) + " : " + ((stack==null) ? "" : stack))
        {}
    }

    public class PreconditionException : ContractException 
    {
        public PreconditionException(string msg, string stack) : base(msg,stack){}
    }
    public class PostconditionException : ContractException 
    {
        public PostconditionException(string msg, string stack) : base(msg, stack) { }
    }
    public class InvariantException : ContractException 
    {
        public InvariantException(string msg, string stack) : base(msg, stack) { }
    }
    public class CheckException : ContractException 
    {
        public CheckException(string msg, string stack) : base(msg, stack) { }
    }
}
