using System;

namespace MTUComm
{
    public sealed class Delegates
    {
        public delegate void Empty ();

        public delegate void ProgresshHandler ( object sender, ProgressArgs e );
        public class ProgressArgs : EventArgs
        {
            public int    Step { get; private set; }
            public int    TotalSteps { get; private set; }
            public string Message { get; private set; }

            public ProgressArgs ( int step, int totalsteps )
            {
                Step       = step;
                TotalSteps = totalsteps;
                Message    = "";
            }

            public ProgressArgs ( int step, int totalsteps, string message )
            {
                Step       = step;
                TotalSteps = totalsteps;
                Message    = message;
            }
        }
    }
}
