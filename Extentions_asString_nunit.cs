using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Richard
{
    [TestFixture]
    class Extentions_asString_nunit
    {

        [Test]
        public void listInitAsString()
        {
            var str = new List<string>(new string[] { "say", "hello", "world" }).AsString();

            Assert.That(str, Is.EqualTo("say, hello, world"));
        }

        [Test]
        public void listAsString()
        {
            var stack = new List<string>();
          
            stack.Add("say");
            stack.Add("hello");
            stack.Add("world");

            var str = stack.AsString();
            Assert.That(str, Is.EqualTo("say, hello, world"));
        }

        [Test]
        public void stackInitAsString()
        {
            var str = new Stack<string>(new string[] { "world", "hello", "say" }).AsString();

            Assert.That(str, Is.EqualTo("say, hello, world"));
        }

        [Test]
        public void stackAsString()
        {
            var stack = new Stack<string>();
            stack.Push("world");
            stack.Push("hello");
            stack.Push("say");

            var str = stack.AsString();
            Assert.That(str, Is.EqualTo("say, hello, world"));
        }
    }
}
