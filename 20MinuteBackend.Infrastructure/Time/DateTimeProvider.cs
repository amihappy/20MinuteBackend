using System;
using _20MinuteBackend.Domain.Time;

namespace _20MinuteBackend.Infrastructure.Time
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
