using EvolutionaryComputation.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EvolutionaryComputation.App_Code
{
    public class Core
    {
        public ObservableCollection<KeyValuePair<double, double>> chartPopulation { get; private set; } 
        public ObservableCollection<KeyValuePair<double, double>> fitnessFunction { get; private set; }
        public ObservableCollection<KeyValuePair<double, string>> consolePopulation { get; private set; }
        public int generations { get; set; }
        private static Random random = new Random();
        public static double rnd
        {
            get
            {
                return random.NextDouble();
            }
        }
        public static double function(double x)
        {
            return Math.Pow(Math.Abs(Math.Sin(x) * Math.Sin(parameterA * x)), parameterB);
        }
        public int numberOfPopulation { get; set; }
        public int bits { get; set; }
        public double mutation { get; set; }
        public double noCrossover { get; set; }
        public static double parameterA { get; set; }
        public static double parameterB { get; set; }
        private List<Person> parents = new List<Person>();
        private List<Person> children = new List<Person>();

        public Core()
        {
            this.chartPopulation = new ObservableCollection<KeyValuePair<double, double>>();
            this.fitnessFunction = new ObservableCollection<KeyValuePair<double, double>>();
            this.consolePopulation = new ObservableCollection<KeyValuePair<double, string>>();
        }

        public List<Person> Initialize(int numberOfPopulation, int bits, double mutation, double noCrossover, double parameterA, double parameterB)
        {
            this.numberOfPopulation = numberOfPopulation;
            this.bits = bits;
            this.mutation = mutation;
            this.noCrossover = noCrossover;
            Core.parameterA = parameterA;
            Core.parameterB = parameterB;

            this.generations = 0;

            parents.Clear();
            for (int i = 0; i < numberOfPopulation; i++)
            {
                parents.Add(new Person(bits));
            }

            initializeGraph();
            showPopulation();

            return parents;
        }

        public List<Person> Next(int steps)
        {
            for (int g = 0; g < steps; g++)
            {            
                children = new List<Person>();

                //Elitism
                Person best = FindBest();
                children.Add(best);
            
                //Population
                for (int i=1 ; i< numberOfPopulation; i++)
                {
                    Person parent1 = Roulette();
                    Person parent2 = Roulette();
                    Person child;

                    //Crossover
                    if (rnd > this.noCrossover)
                        child = Crossover(parent1, parent2);
                    else
                        child = rnd < 0.5 ? parent1 : parent2;

                
                    //Mutation
                    child = Mutation(child);

                    children.Add(child);
                }

                //if (parents.Max(a => a.fitness) > children.Max(a => a.fitness))
                //{
                //    var pMax = parents.Max(a => a.fitness);
                //    var cMax = parents.Max(a => a.fitness);


                //    var x = 0;
                //}

                parents = children;
            }

            this.generations += steps;

            showPopulation();

            return parents;
        }

        private Person FindBest()
        {
            double best = parents.Max(a => a.fitness);
            return parents.FirstOrDefault(a => a.fitness == best);
        }

        private Person Roulette()
        {
            double sum = parents.Sum(a => a.fitness);
            double sumrnd = rnd * sum;

            int i;
            for (i = 0; i < numberOfPopulation; i++)
            {
                sumrnd -= parents[i].fitness;
                if (sumrnd < 0)
                    break;
            }
            return parents[i];
        }

        private Person Crossover(Person parent1, Person parent2)
        {
            int pos = (int)Math.Ceiling(rnd * (bits - 1));
            Person child = new Person();

            string part1 = parent1.dna.Substring(0, pos);
            string part2 = parent2.dna.Substring(pos);

            child.dna = part1 + part2;

            return child;
        }

        private Person Mutation(Person child)
        {
            double points, rest;
            int bits, i, pos;
            points = this.bits * this.mutation;
            bits = (int)points;//akeraio meros
            rest = points - bits;//dekadiko
            if (rnd < rest)
                bits++;

            StringBuilder sb = new StringBuilder(child.dna);
            for (i = 0; i < bits; i++)
            {
                pos = (int)Math.Ceiling(rnd * this.bits) - 1;
                if (child.dna[pos] == '0')
                    sb[pos] = '1';
                else
                    sb[pos] = '0';

                child.dna = sb.ToString();
            }
            return child;
        }

        private void showPopulation()
        {
            chartPopulation.Clear();
            consolePopulation.Clear();
            foreach (Person person in parents.OrderByDescending(a => a.fitness))
            {
                chartPopulation.Add(new KeyValuePair<double, double>(person.geno2Pheno, person.fitness));

                string description = person.dna + " - " + person.geno2Pheno.ToString("N20") + " - " + person.fitness.ToString("N20");
                consolePopulation.Add(new KeyValuePair<double, string>(person.fitness, description));
                Debug.WriteLine(description);
            }
        }

        private void initializeGraph()
        {
            fitnessFunction.Clear();
            for (int i = 0; i < 500; i++)
            {
                double x = (i * 1d * Math.PI) / 500;
                double y = function(x);
                fitnessFunction.Add(new KeyValuePair<double, double>(x, y));
            }
        }

        
    }

}
