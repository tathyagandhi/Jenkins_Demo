using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFrameworkTest.Data
{
    public class TestDataRowTitle : IEnumerable
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string Projects { get; set; }

        public string Projects2 { get; set; }



        public string rowdata { get; set; }
        public IEnumerator GetEnumerator()
        {   
            yield return new string[] { rowdata };
        }
    }
}
