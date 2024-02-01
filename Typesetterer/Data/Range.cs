namespace Typesetterer.Data
{
    public struct Range
    {
        public int Start;

        public int End;

        public int Count
        {
            get
            {
                if (this.Start <= -1)
                {
                    return 0;
                }
                return this.End - this.Start;
            }
        }

        public Range(int Start, int End)
        {
            this.Start = Start;
            this.End = End;
        }

        public bool Contains(int n)
        {
            if (n < this.Start)
            {
                return false;
            }
            return n < this.End;
        }

        public int[] ToArray()
        {
            int[] start = new int[this.Count];
            for (int i = 0; i < (int)start.Length; i++)
            {
                start[i] = this.Start + i;
            }
            return start;
        }
    }
}