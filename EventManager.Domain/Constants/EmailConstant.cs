using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Domain.Constants
{
    public static class EmailConstant
    {
        private static readonly List<string> _DisposableDomain = new List<string>
        {
            "10minutemail.co.za", "10minutemail.com", "20minutemail.com", "yopmail.com", "yopmail.fr"," yopmail.net"
        };

        public static IEnumerable<string> DisposableDomain
        {
            get { return _DisposableDomain.AsReadOnly(); }
        }
    }
}
