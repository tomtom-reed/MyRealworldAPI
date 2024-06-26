using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Policies
{
    internal class ArticlePolicy
    {
        public const int MIN_TITLE_LENGTH = 3;
        public const int MAX_TITLE_LENGTH = 140;
        public const int MIN_DESCRIPTION_LENGTH = 4;
        public const int MAX_DESCRIPTION_LENGTH = 400;
        public const int MIN_BODY_LENGTH = 10;
        public const int MAX_BODY_LENGTH = 20000;
        public const int MIN_TAG_LENGTH = 1;
        public const int MAX_TAG_LENGTH = 30;
        public const int SLUG_LENGTH = 44; // Base64 encoded SHA256 characters are only 44 bytes in length without the padding '='
    }
}
