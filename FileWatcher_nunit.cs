using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Richard;
using System.Diagnostics.Contracts;

namespace FileWatch
{
    [System.Diagnostics.Contracts.ContractVerification(false)]
    [TestFixture]
    class FileWatcher_nunit
    {
        String tempPath = System.IO.Path.GetTempPath();
        String newName()
        {
            return "radFileWatchTest" + Guid.NewGuid().ToString() + ".txt";
        }
        int event_Count;
        string fileName;
        string fullName;
        string fileName2;
        string fullName2;
        FileWatcher watcher;
        bool is_setup { get; set; }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(is_setup.Implies(fileName != null));
            Contract.Invariant(is_setup.Implies(fullName != null));
            Contract.Invariant(is_setup.Implies(fileName2 != null));
            Contract.Invariant(is_setup.Implies(fullName2 != null));
            Contract.Invariant(is_setup.Implies(fileName != null));
            Contract.Invariant(is_setup.Implies(watcher != null));
        }

        [SetUp]
        public void Setup()
        {
            Contract.Ensures(is_setup);
            event_Count = 0;
            fileName = newName();
            fullName = tempPath + fileName;
            fileName2 = newName();
            fullName2 = tempPath + fileName2;
            watcher = new FileWatcher(tempPath, fileName);
            Contract.Assert(watcher != null);
            is_setup = true;
            Assert.That(event_Count, Is.EqualTo(0));
        }

        public void CreateFile()
        {
            Contract.Requires(is_setup);
            Assert.That(event_Count, Is.EqualTo(0));
            Assert.That(!System.IO.File.Exists(fullName));
            Assert.That(!System.IO.File.Exists(fullName2));
            System.IO.File.Create(fullName).Close();
            System.IO.File.Create(fullName2).Close();
            Assert.That(System.IO.File.Exists(fullName));
            Assert.That(System.IO.File.Exists(fullName2));
        }

        public void DeleteFiles()
        {
            Contract.Requires(is_setup);
            System.IO.File.Delete(fullName);
            System.IO.File.Delete(fullName2);
            Assert.That(!System.IO.File.Exists(fullName));
            Assert.That(!System.IO.File.Exists(fullName2));
        }

        [TearDown]
        public void TearDown()
        {
            Contract.Requires(is_setup);
            DeleteFiles();
        }

        [Test, Description("All events handled")] 
        [Category("ALittleSlow")]
        public void TestAddHandler()
        {
            Contract.Requires(is_setup);
            watcher.addHandler( () => ++event_Count );
            watcher.enableHandlers();
            try
            {
                CreateFile();

                System.IO.File.SetLastWriteTimeUtc(fullName,  DateTime.Now);
                System.IO.File.SetLastWriteTimeUtc(fullName,  DateTime.Now);
                System.IO.File.SetLastWriteTimeUtc(fullName2, DateTime.Now);
            }
            finally
            {
                DeleteFiles();
            }
            Assert.That(!System.IO.File.Exists(fullName));
            Assert.That(!System.IO.File.Exists(fullName2));
            System.Threading.Thread.Sleep(1000); //:hack:
            Assert.That(event_Count, Is.EqualTo(3));
        }

        [Test, Description("Ignores earlyer events, even when registered. And events between waits.")]
        [Timeout(5000)]
        [Category("ALittleSlow")]
        public void TestWait()
        {
            Contract.Requires(is_setup);
            CreateFile();

            System.IO.File.SetLastWriteTimeUtc(fullName, DateTime.Now);
            System.IO.File.SetLastWriteTimeUtc(fullName, DateTime.Now);
            System.IO.File.SetLastWriteTimeUtc(fullName, DateTime.Now);

            var before = DateTime.Now;
            TimedEvent.CallAfter(1000, () => System.IO.File.SetLastWriteTimeUtc(fullName, DateTime.Now));
            watcher.wait();
            var after = DateTime.Now;

            Assert.That(after, Is.AtLeast(before.AddMilliseconds(1000)));
        }   
    }
}
