using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightNap.Core.Exceptions
{
    namespace LightNap.Core.Exceptions
    {
        public class FileLimitExceededException : UserFriendlyApiException
        {
            public FileLimitExceededException(string message)
                : base(message)
            {
            }
        }
    }

}
