using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrapey.Objects
{
    public class PageNotScrapedException : Exception
    {
        public PageNotScrapedException()
        {
        }

        public PageNotScrapedException(string message) : base(message)
        {
        }
    }
}
