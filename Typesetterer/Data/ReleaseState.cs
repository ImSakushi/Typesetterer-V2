using System.Collections.Generic;

namespace Typesetterer.Data
{
    public class ReleaseState
    {
        public List<LinkedPage> LinkedPages
        {
            get;
            set;
        }

        public int PageIndex
        {
            get;
            set;
        }

        public Ps.PhotoshopVersion PhotoshopVersion
        {
            get;
            set;
        }

        public ReleaseState.States TypesettererState
        {
            get;
            set;
        }

        public ReleaseState()
        {
            this.TypesettererState = ReleaseState.States.Overlay;
        }

        public enum States
        {
            Prototyper,
            Overlay
        }
    }
}