using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedApp.Models
{
    public class SecondaryIdentity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool InActiveUse { get; set; }
        public string PrimarySSN { get; set; }
        public Employee PrimaryIdentity { get; set; }
    }
}
