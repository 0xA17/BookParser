using System;

namespace BookParser.MVVM.Model
{
    public class ProgressInfo
    {
        public Int16 ParseProgressValue { get; set; }

        public ProgressInfo(Int16 value)
        {
            ParseProgressValue = value;
        }
    }
}
