using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Richard.Contracts
{
    [TestFixture]
    class Contracts_nunit
    {
        [Test]
        public void Requires_true()
        {
            Contract.Requires(true, "precondition test true");
        }
        [Test]
        [ExpectedException(typeof(PreconditionException))]
        public void Requires_false()
        { 
            Contract.Requires(false, "precondition test false"); 
        }

        [Test]
        public void Ensures_true()
        {
            Contract.Ensures(true, "postcondition test true");
        }
        [Test]
        [ExpectedException(typeof(PostconditionException))]
        public void Ensures_false()
        {
            Contract.Ensures(false, "postcontition test false");
        }

        [Test]
        public void Invariant_true()
        {
            Contract.Invariant(true, "invariant test true");
        }
        [Test]
        [ExpectedException(typeof(InvariantException))]
        public void Invariant_false()
        {
            Contract.Invariant(false, "invariant test false");
        }

        [Test]
        public void Check_true()
        {
            Contract.Check(true, "check test true");
        }
        [Test]
        [ExpectedException(typeof(CheckException))]
        public void Check_false()
        {
            Contract.Check(false, "check test false");
        }
    }
}

// only passed if contracts are confugured to throw exceptions. 
//But we prefere them not to be possable to be catched.
#if  false
namespace Richard
{
    using System.Diagnostics.Contracts;

    [TestFixture]
    [System.Diagnostics.Contracts.ContractVerification(false)]
    class Builtin_Contracts_nunit
    {
        [Test]
        public void Requires_true()
        {
            Contract.Requires(true, "precondition test true");
        }
        [Test]
        [ExpectedException]
        public void Requires_false()
        {
            Contract.Requires(false, "precondition test false");
        }

        [Test]
        public void Ensures_true()
        {
            Contract.Ensures(true, "postcondition test true");
        }
        [Test]
        [ExpectedException]
        public void Ensures_false()
        {
            Contract.Ensures(false, "postcontition test false");
        }

#if false
        [Test]
        public void Invariant_true()
        {
            Contract.Invariant(true, "invariant test true");
        }
        [Test]
        [ExpectedException()]
        public void Invariant_false()
        {
            Contract.Invariant(false, "invariant test false");
        }
#endif

        [Test]
        public void Check_true()
        {
            Contract.Assert(true, "check test true");
        }
        [Test]
        [ExpectedException]
        public void Check_false()
        {
            Contract.Assert(false, "check test false");
        }
    }
}
#endif