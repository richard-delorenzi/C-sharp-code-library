using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileWatch
{
    class TimedEvent
    {
        public delegate void Procedure();
        public static void CallAfter(int time_ms, Procedure procedure)
        {
            var timer = new System.Timers.Timer();
            timer.AutoReset = false;
            timer.Interval = time_ms;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(
                (o, time_elapsed) => procedure() );
           
            timer.Enabled = true;

            timer.Start();
        }
    }
}
