using System.Collections.Generic;
using Projects.Domain.SeedWork;

namespace Projects.Domain.AggregatesModel
{
    public class ProjectProperty:ValueObject
    {
        public int ProjectId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public string Text { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Key;
            yield return Value;
            yield return Text;
        }
    }
}