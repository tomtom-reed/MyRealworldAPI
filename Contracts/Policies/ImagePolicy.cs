using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Policies
{
    internal class ImagePolicy
    {
        private static int MIN_LENGTH = 3;
        private static int MAX_LENGTH = 256;

        public static bool ValidateAgainstImagePolicy(string image)
        {
            if (image == null)
            {
                return false;
            }
            if (image.Length < ImagePolicy.MIN_LENGTH) { return false; }
            if (image.Length > ImagePolicy.MAX_LENGTH) { return false; }
            return true;
        }
    }
}
