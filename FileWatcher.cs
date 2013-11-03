using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileWatch
{
    class FileWatcher : IDisposable
    {
        private System.IO.FileSystemWatcher myWatcher;
        public delegate void handler_type();
        public FileWatcher(string directory, string file_glob)
        {
            myWatcher = new System.IO.FileSystemWatcher(directory, file_glob);  
        }
        ~FileWatcher()
        {
            Dispose();
        }
        public void Dispose()
        {
            if (myWatcher != null)
            {
                myWatcher.Dispose();
                GC.SuppressFinalize(this);
                myWatcher = null;
            }  
        }

        public void addHandler(handler_type handler)
        {
            var _handler = new System.IO.FileSystemEventHandler((o, a) => handler());
            myWatcher.Changed += _handler;
            myWatcher.Created += _handler;
        }

        public void disableHandlers()
        {
            myWatcher.EnableRaisingEvents = false;
        }

        public void enableHandlers()
        {
            myWatcher.EnableRaisingEvents = true;
        }

        public void wait()
        {
            myWatcher.EnableRaisingEvents = true;
            var dummy = myWatcher.WaitForChanged(
                System.IO.WatcherChangeTypes.Created |
                System.IO.WatcherChangeTypes.Changed);
        } 
    }
}
