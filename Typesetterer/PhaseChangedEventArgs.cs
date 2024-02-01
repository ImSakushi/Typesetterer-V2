using System;

namespace Typesetterer
{
    public class PhaseChangedEventArgs : EventArgs
    {
        public Enum NewPhase
        {
            get;
            set;
        }

        public PhaseChangedEventArgs()
        {
        }
    }
}