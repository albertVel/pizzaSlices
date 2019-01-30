using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace hashCodePractice
{
    class Program
    {
        static void Main(string[] args)
        {

            var mainClass = new MainClass();
            mainClass.Run();
        }

       
    }

    internal class MainClass
    {
        private PizzaReqs pizzaReqs;

        public void Run()
        {
            FileStream fileStream = new FileStream("../../../a_example.in", FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line = reader.ReadLine();

                ProcessHeader(line);

                ProcessIngredients(reader);

                int[,] slices= new int[1000,1000];

                if (DoPizza(pizzaReqs,slices))
                {
                }
                else
                {
                }

            }
        }

        private bool DoPizza(PizzaReqs pizzaReqs, int [,] slices)
        {
            //slices in the form of r1, c1, r2, c2

            //L (1 ≤ L ≤ 1000) is the minimum number of each ingredient cells in a slice,
            //H(1 ≤ H ≤ 1000) is the maximum total number of cells of a slice

            return true;

        }

        private void ProcessIngredients(StreamReader reader)
        {
            pizzaReqs.ingredients = new char[1000, 1000];

            for (var i = 0; i < pizzaReqs.numberRows; i++)
            {
                var line=reader.ReadLine();
                for (var j = 0; i < pizzaReqs.numberColumns; i++)
                {
                    pizzaReqs.ingredients[i,j]= line.ElementAt(j);
                }
            }
        }

        private void ProcessHeader(string line)
        {
            var header = line.Split(' ');

            this.pizzaReqs = new PizzaReqs();

            pizzaReqs.numberRows = int.Parse(header[0]);
            pizzaReqs.numberColumns = int.Parse(header[1]);
            pizzaReqs.minNumberIngredients = int.Parse(header[2]);
            pizzaReqs.maxNumberCellsPerSlice = int.Parse(header[2]);

        }
    }

    internal class PizzaReqs
    {
        public int numberRows;
        public int numberColumns;
        public int minNumberIngredients;
        public int maxNumberCellsPerSlice;
        public char[,] ingredients;
    }
}
