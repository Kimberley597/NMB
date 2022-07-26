using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMB.Business_Layer
{
    public static class Utility
    {
        //Generates a list of ints, checking for all the instances present in a string
        //Used for SMS and Tweet classes when checking for any text speak used
        public static List<int> GetAllInstancesInString(string target, string contains)
        {
            List<int> indexes = new List<int>();
            for(int index = 0; ; index += contains.Length)
            {
                index = target.IndexOf(contains, index);

                if (index == -1)
                {
                    return indexes;
                }

                indexes.Add(index);
            }
        }

    }
}
