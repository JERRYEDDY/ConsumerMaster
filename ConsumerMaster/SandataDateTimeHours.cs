using System;

namespace ConsumerMaster
{
    public class SandataDateTimeDuration
    {
        public DateTime Start;
        public DateTime End;
        public TimeSpan Duration;

        public SandataDateTimeDuration()
        {

        }

        public SandataDateTimeDuration(DateTime start, DateTime end, TimeSpan duration)
        {
            Start = start;
            End  = end;
            Duration = duration;
        }
    }
}