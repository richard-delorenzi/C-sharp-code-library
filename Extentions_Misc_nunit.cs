using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Richard
{
    [TestFixture]
    class EnumUtils_nunit
    {
        enum zzzz { h, el, lo, _, w, or, ld };
        [Test]
        public void GetValues()
        {
            var TestString="";
            foreach (var v in EnumUtil.GetValues<zzzz>() )
            {
                TestString += v.ToString();
            }
            Assert.AreEqual(TestString,"hello_world");
        }
    }
}
