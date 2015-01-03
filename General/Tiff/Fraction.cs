using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace con.azi
{
    public class Fraction
    {
        int numenator;
        int denominator;

        public Fraction(int n, int d)
        {
            numenator = n;
            denominator = d;
        }

        public override string ToString()
        {
            return numenator + "/" + denominator;
        }
    }
}
