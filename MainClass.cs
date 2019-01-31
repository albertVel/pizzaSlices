using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace PizzaSlices
{
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

                int[,] slices = new int[1000, 1000];

                InitializeSlices(slices);

                if (DoPizza(slices))
                {
                }
                else
                {
                }

            }
        }

        private void InitializeSlices(int[,] slices)
        {
            for (var i = 0; i < 1000; i++)
            {
                for (var j = 0; j < 1000; j++)
                {
                    slices[i, j] = -1;
                }
            }
        }


        private bool DoPizza(int[,] slices)
        {
            var pizzaTotallySliced = PizzaTotallySliced(slices);

            if (pizzaTotallySliced)
            {
                return true;
            }

            //slices in the form of r1, c1, r2, c2
            var row = 0;
            var col = 0;

            while (row < pizzaReqs.numberRows)
            {
                while (col < pizzaReqs.numberRows)
                {
                    if (SliceFulfillRequirements(row,col, row,col))
                    {
                        int idSlice = 0;
                        slices[row,col] = idSlice;
                        if (DoPizza(slices))
                        {
                            // print(board, n); 
                            return true;
                        }
                        else
                        {
                            slices[row,col] = -1; 
                        }
                    }
                    col++;

                }

                col = 0;
                row++;
            }


                       //L (1 ≤ L ≤ 1000) is the minimum number of each ingredient cells in a slice,
            //H(1 ≤ H ≤ 1000) is the maximum total number of cells of a slice

            return true;

        }

        private bool PizzaTotallySliced(int[,] slices)
        {
            var pizzaTotallySliced = true;

            var row = 0;
            var col = 0;

            while (row < 1000 && pizzaTotallySliced)
            {
                while (col < 1000 && pizzaTotallySliced)
                {
                    if (slices[row, col] == -1)
                    {
                        pizzaTotallySliced = false;
                    }

                    col++;
                }

                col = 0;
                row++;
            }

            return pizzaTotallySliced;
        }


        private bool SliceFulfillRequirements(int row1, int col1, int row2, int col2)
        {
 
            int row = row1;
            int col = col1;
            bool fulfillRequirements = true;
            int numberIngredients = 0;
            List<char> differentIngredientsInSlice = new List<char>();

            while ((row <= row2) && (fulfillRequirements))
            {
                while ((col <= col2) && (fulfillRequirements))
                {

                    if (!differentIngredientsInSlice.Contains(pizzaReqs.ingredients[row, col]))
                    {
                        differentIngredientsInSlice.Add(pizzaReqs.ingredients[row, col]);
                    }

                    if (pizzaReqs.minNumberIngredients > differentIngredientsInSlice.Count)
                    {
                        fulfillRequirements = false;
                    }

                    numberIngredients++;

                    if (pizzaReqs.maxNumberCellsPerSlice > numberIngredients)
                    {
                        fulfillRequirements = false;
                    }

                    col++;
                }

                col = col1;
                row++;
            }

            return fulfillRequirements;

        }

      
        private void ProcessIngredients(StreamReader reader)
        {
            pizzaReqs.ingredients = new char[1000, 1000];

            for (var i = 0; i < pizzaReqs.numberRows; i++)
            {
                var line = reader.ReadLine();
                for (var j = 0; i < pizzaReqs.numberColumns; i++)
                {
                    pizzaReqs.ingredients[i, j] = line.ElementAt(j);
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
}


