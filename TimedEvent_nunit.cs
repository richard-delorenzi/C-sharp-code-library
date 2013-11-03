using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace FileWatch
{
    [System.Diagnostics.Contracts.ContractVerification(false)]
    [TestFixture]
    class TimedEvent_nunit
    {
        [Test]
        public void NopCall()
        {
            TimedEvent.CallAfter(1, () => new object());
        }

        private int _Call_Count = 0;
        [Test]
        [Category("ALittleSlow")]
        public void Call()
        {
            Assert.That(_Call_Count, Is.EqualTo(0));
            TimedEvent.CallAfter(1000, () => ++_Call_Count);
            Assert.That(_Call_Count, Is.EqualTo(0));
            Assert.That( () => _Call_Count, Is.EqualTo(0).After(900));
            Assert.That( () => _Call_Count, Is.EqualTo(1).After(1000));
        }
    }
}
