using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Domain.Constants
{
    public static class PseudoConstant
    {
        private static readonly List<string> _Forbidden = new List<string>
        {
            "Admin", "Administrator", "Root"
        };

        public static IEnumerable<string> Forbidden
        {
            get { return _Forbidden.AsReadOnly(); }
        }
    }
}
