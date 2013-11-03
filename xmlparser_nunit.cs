using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics.Contracts;

namespace Richard
{
    [TestFixture]
    [Category("Fragile")] //depends on relative path to Code_Library
    [System.Diagnostics.Contracts.ContractVerification(false)]
    class xmlparser_nunit
    {
        //constructor
        public xmlparser_nunit()
        {}

        public static List<string> expectedOutput1and2and3 = new List<string> (new string[]{
            "name= fred, phone= 1235551234",
            "name= john, phone= 1235552341",
            "name= bob, phone= 1235553412"
        });

        public static object[] Cases = {
            new object[] {"../../Code_Library/XMLfile1.xml", expectedOutput1and2and3 },
            new object[] {"../../Code_Library/XMLfile2.xml", expectedOutput1and2and3 },
            new object[] {"../../Code_Library/XMLfile3.xml", expectedOutput1and2and3 },
            new object[] {"../../Code_Library/XMLfile4-broken.xml", null }
        };
            

        [Test, TestCaseSource("Cases")]
        public void xmlParse(
            string infile,
            List<string> ExpectedOut
            )
        {
            Contract.Requires(infile != null);
            var x = new XmlParser();
            var dictionary = new Dictionary<string, string>();
            x.Add(
                new String[] { "test-file", "users", "user", "name" },
                dictionary,
                (dict, str) => { dict["name"] = str; return null; },
                (dict, str) => { return null; },
                (dict, str) => { return null; }
                );
            x.Add(
                new String[] { "test-file", "users", "user", "$name" },
                dictionary,
                (dict, str) => { dict["name"] = str; return null; },
                (dict, str) => { return null; },
                (dict, str) => { return null; }
                );
            x.Add(
                new String[] { "test-file", "users", "user", "phone" },
                dictionary,
                (dict, str) => { dict["phone"] = str; return null; },
                (dict, str) => { return null; },
                (dict, str) => { return null; } 
                );

            x.Add(
               new String[] { "test-file", "users", "user" },
               dictionary,
               (dict, str) => { return null; },
               (dict, str) => { dict.Clear(); return null; },
               (dict, str) => {
                   return
                       (dict.ContainsKey("name") ? "name= " + dict["name"] : "") +
                       (dict.ContainsKey("phone") ? ", phone= " + dict["phone"] : "");
               });

            x.Process(infile);

            if (ExpectedOut != null)
            {
                Assert.That(x.Error, Is.EqualTo(XmlParser<string>.Errors.None));
                Assert.That(x.Output, Is.EqualTo(ExpectedOut));
            }
            else
            {
                Assert.That(x.Error, Is.EqualTo(XmlParser<string>.Errors.InvalidXml));
            }
        }
    }
}
