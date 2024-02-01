namespace Typesetterer
{
    public class Step
    {
        protected bool Destroyed
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public Step()
        {
            this.Destroyed = true;
        }

        public virtual void CreateControls()
        {
            this.Destroyed = false;
        }

        public virtual void DestroyControls()
        {
            this.Destroyed = true;
        }

        public virtual void HandleAdd()
        {
            if (this.Destroyed)
            {
                this.CreateControls();
            }
        }

        public virtual void HandleLoad()
        {
        }

        public virtual void HandleRemove()
        {
        }
    }
}