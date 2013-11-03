using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Richard
{
    [TestFixture]
    class LogicExtentions_nunit
    {
        [Test]
        public void Implies()
        {
            Assert.AreEqual(false.Implies(false), true);
            Assert.AreEqual(false.Implies(true),  true);
            Assert.AreEqual(true .Implies(false), false);
            Assert.AreEqual(true .Implies(true),  true);
        }

        [Test]
        public void IFF()
        {
            Assert.AreEqual(false.IFF(false), true);
            Assert.AreEqual(false.IFF(true),  false);
            Assert.AreEqual(true .IFF(false), false);
            Assert.AreEqual(true .IFF(true),  true);
        }

    }
}
