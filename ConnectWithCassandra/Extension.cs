using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectWithCassandra
{
    public static class Extension
    {
        public static string ToCqlString(this Guid guid)
        {
            var bytes = guid.ToByteArray();
            StringBuilder sb = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < 16; i++)
            {
                if (i == 4 || i == 6 || i == 8 || i == 10)
                {
                    sb.Append("-");
                }
                var b = bytes[i];
                sb.AppendFormat("{0:x2}", b);

            }
            return sb.ToString();
        }
    }
}
