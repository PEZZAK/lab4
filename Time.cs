using System;

namespace Lab4Time
{
    public class Time
    {
        private byte _hours;
        private byte _minutes;

        public byte Hours
        {
            get { return _hours; }
            set
            {
                if (value > 23)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Часы должны быть от 0 до 23.");
                }
                _hours = value;
            }
        }

        public byte Minutes
        {
            get { return _minutes; }
            set
            {
                if (value > 59)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Минуты должны быть от 0 до 59.");
                }
                _minutes = value;
            }
        }

        public Time()
        {
            _hours = 0;
            _minutes = 0;
        }

        public Time(byte hours, byte minutes)
        {
            Hours = hours;
            Minutes = minutes;
        }

        public override string ToString()
        {
            return $"{_hours:D2}:{_minutes:D2}";
        }

        // ================================================================
        // ЗАДАНИЕ 6: вычитание времени (Time) из Time (с переходом в предыдущие сутки).
        // ================================================================
        public Time Subtract(Time other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            int totalMinutes = _hours * 60 + _minutes;
            int otherMinutes = other._hours * 60 + other._minutes;

            int diff = totalMinutes - otherMinutes;

            if (diff < 0)
            {
                diff += 24 * 60;
            }

            return new Time((byte)(diff / 60), (byte)(diff % 60));
        }

        // ================================================================
        // ЗАДАНИЕ 7: перегрузка операторов.
        // ================================================================

        public static Time operator ++(Time time)
        {
            return AddMinutes(time, 1);
        }

        public static Time operator --(Time time)
        {
            return AddMinutes(time, -1);
        }

        public static explicit operator byte(Time time)
        {
            if (time == null)
            {
                throw new ArgumentNullException(nameof(time));
            }
            return time._hours;
        }

        public static implicit operator bool(Time time)
        {
            if (time == null)
            {
                throw new ArgumentNullException(nameof(time));
            }
            return time._hours != 0 || time._minutes != 0;
        }

        public static Time operator +(Time time, uint minutes)
        {
            return AddMinutes(time, (int)minutes);
        }

        public static Time operator +(uint minutes, Time time)
        {
            return AddMinutes(time, (int)minutes);
        }

        public static Time operator +(Time left, Time right)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }
            if (right == null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            int total = (left._hours * 60 + left._minutes)
                      + (right._hours * 60 + right._minutes);
            total %= 24 * 60;

            return new Time((byte)(total / 60), (byte)(total % 60));
        }

        private static Time AddMinutes(Time time, int minutes)
        {
            if (time == null)
            {
                throw new ArgumentNullException(nameof(time));
            }

            int total = time._hours * 60 + time._minutes + minutes;
            total = ((total % (24 * 60)) + (24 * 60)) % (24 * 60);

            return new Time((byte)(total / 60), (byte)(total % 60));
        }
    }
}