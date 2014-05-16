using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capture
{
    public abstract class Recorder
    {
        protected bool pause { get; set; }
        protected bool rec { get; set; }
        public bool IsPaused { get { return pause; } }
        public abstract void Record(object o);
        public void Stop()
        {
            rec = false;
        }
        public void TogglePause()
        {
            if (pause)
                pause = false;
            else
                pause = true;
        }

    }
}
