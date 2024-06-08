using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultillingualFileGenerator.Targets.Model
{
    public record TargetLine
    {
        public string Name { get; init; }
        public string Value { get; init; }
        public string Comment { get; init; }
    }
}
