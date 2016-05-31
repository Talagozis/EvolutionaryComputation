using EvolutionaryComputation.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EvolutionaryComputation.Models
{
    public class Person
    {
        public Person()
        {
            
        }
        public Person(int bits)
        {
            for (int j = 0; j < bits; j++)
            {
                if (Core.rnd < 0.5)
                    dna += "0";
                else
                    dna += "1";
            }
        }
        public string dna { get; set; }
        public int str2Int()
        {
            int i, ak = 0, pow2 = 1 << (dna.Length - 1);
            for (i = 1; i < dna.Length; i++)
            {
                if (dna[i] == '1') ak += pow2;
                pow2 /= 2;
            }
            return ak;
        }

        public double geno2Pheno()
        {
            int ak = this.str2Int();
            return ak/(Math.Pow(2, dna.Length)-1)* Math.PI * 1d;
        }

        public double fitness()
        {
            double x = geno2Pheno();
            return Core.function(x);
        }

        

    }

}
