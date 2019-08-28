using System;
using Xml;
using System.Threading.Tasks;

namespace MTUComm
{
    public sealed class Delegates
    {
        public delegate void Empty ();

        public delegate void ProgresshHandler ( object sender, ProgressArgs args );
        public class ProgressArgs : EventArgs
        {
            public int    Step { get; private set; }
            public int    TotalSteps { get; private set; }
            public string Message { get; private set; }

            public ProgressArgs ( string message )
            {
                Message = message;
            }

            public ProgressArgs ( int step, int totalsteps, string message )
            {
                Step       = step;
                TotalSteps = totalsteps;
                Message    = message;
            }
        }

        public delegate Task ActionHandler ( ActionArgs args = null );
        public class ActionArgs : EventArgs
        {
            public Mtu Mtu { get; private set; }
            public dynamic Map { get; private set; }
            public dynamic[] Extra { get; private set; }

            public ActionArgs (
                Mtu mtuType,
                dynamic map = null,
                params dynamic[] extraArgs )
            {
                this.Mtu   = mtuType;
                this.Map   = map;
                this.Extra = extraArgs;
            }
        }

        public delegate Task ActionFinishHandler ( object sender, ActionFinishArgs args = null );
        public class ActionFinishArgs : EventArgs
        {
            public ActionResult Result { get; private set; }
            public Mtu Mtu  { get; private set; }
            //public AddMtuLog FormLog;
            public dynamic[] Extra { get; private set; }

            public ActionFinishArgs ()
            {
                this.Result = new ActionResult ();
            }

            public ActionFinishArgs (
                ActionResult result,
                Mtu mtu = null,
                params dynamic[] extraArgs )
            {
                this.Result = result;
                this.Mtu    = mtu;
                this.Extra  = extraArgs;
            }
        }
    }
}
