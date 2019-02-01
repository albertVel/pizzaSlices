using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;

namespace PizzaSlices
{
    public class MainClass
    {
        private PizzaReqs pizzaReqs;

        public void Run()
        {
            int[,] slices = new int[5, 5];
            InitializeSlices(slices,-1);

            ProcessInputFile("../../../../DataSets/a_example.in");

            if (DoPizza(slices, 0, 0, 0))
            {
            }
            else
            {
            }

        }

        public void ProcessInputFile(string inputFile)
        {
            FileStream fileStream = new FileStream(inputFile, FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line = reader.ReadLine();

                ProcessHeader(line);

                ProcessIngredients(reader);
            }
        }

        public void InitializeSlices(int[,] slices, int value)
        {
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    slices[i, j] = value;
                }
            }
        }


        private bool DoPizza(int[,] slicesCutted, int idSlice, int startRow, int startCol)
        {
            var slice = new Slice();
            var pizzaTotallySliced = PizzaTotallySliced(slicesCutted);

            if (pizzaTotallySliced)
            {
                return true;
            }

            while (startRow < pizzaReqs.numberRows)
            {
                while (startCol < pizzaReqs.numberRows)
                {
                    slice = DoSlice(startRow, startCol, slicesCutted);

                    if (slice.IsInitialized)
                    {
                        TagSlice(slice, slicesCutted, idSlice);
                        idSlice++;


                        if (slice.EndRow == pizzaReqs.numberRows || slice.EndCol == pizzaReqs.numberColumns)
                        {
                            return false;
                        }
                        else
                        {
                            startRow = slice.EndRow;
                            startCol = slice.EndCol;
                        }
                        

                        if (DoPizza(slicesCutted, idSlice, startRow, startCol))
                        {
                            // print(board, n); 
                            return true;
                        }
                        else
                        {
                            TagSlice(slice, slicesCutted, -1);
                        }
                    }

                }

            }

            return false; 
        }

        private void TagSlice(Slice slice, int[,] slices, int idSlide)
        {
            for (int row = slice.StartRow; row < slice.EndRow; row++)
            {
                for (int col = slice.StartCol; col < slice.EndCol; col++)
                {
                    slices[row, col] = idSlide;
                }
            }
            
        }

        public Slice DoSlice(int startRow, int startCol, int[,] slicesCutted)
        {
            bool stop = false;
            int endRow = startRow;
            int endCol = startCol;


            while (!SliceFulfillRequirements(startRow, startCol, endRow, endCol, slicesCutted) && !stop)
            {
                endRow++;

                if (endRow == pizzaReqs.numberRows && endCol == pizzaReqs.numberColumns)
                {
                    stop = true;
                }

                if (endRow > startRow + 2)
                {
                    endRow = startRow;
                    endCol = endCol + 1;
                }
            }

            if (!stop)
            {
                return new Slice(startRow, startCol, endRow, endCol);
            }
            else
            {
                return new Slice();
            }
        }

        private bool PizzaTotallySliced(int[,] slices)
        {
            var pizzaTotallySliced = true;

            var row = 0;
            var col = 0;

            while (row < 5 && pizzaTotallySliced)
            {
                while (col < 5 && pizzaTotallySliced)
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


        public bool SliceFulfillRequirements(int startRow, int startCol, int endRow, int endCol, int [,] slicesCutted)
        {
            int row = startRow;
            int col = startCol;
            bool fulfillRequirements = true;
            int numberIngredients = 0;
            List<char> differentIngredientsInSlice = new List<char>();

            while ((row <= endRow) && (fulfillRequirements))
            {
                while ((col <= endCol) && (fulfillRequirements))
                {
                    if (!differentIngredientsInSlice.Contains(pizzaReqs.ingredients[row, col]))
                    {
                        differentIngredientsInSlice.Add(pizzaReqs.ingredients[row, col]);
                    }

                    if (slicesCutted[row, col] != -1)
                    {
                        fulfillRequirements = false;
                    }

                    numberIngredients++;

                    col++;
                }

                col = startCol;
                row++;
            }

            if (pizzaReqs.minEachIngredient +1 != differentIngredientsInSlice.Count)
            {
                fulfillRequirements = false;
            }

            if (pizzaReqs.maxNumberCellsPerSlice < numberIngredients)
            {
                fulfillRequirements = false;
            }

            return fulfillRequirements;

        }


        private void ProcessIngredients(StreamReader reader)
        {
            pizzaReqs.ingredients = new char[5, 5];

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
            pizzaReqs.minEachIngredient = int.Parse(header[2]);
            pizzaReqs.maxNumberCellsPerSlice = int.Parse(header[3]);

        }
    }
}


