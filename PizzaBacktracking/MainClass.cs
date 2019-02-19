using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;

namespace PizzaSlices
{
    public class MainClass
    {
        public PizzaReqs pizzaReqs;

        public void Run()
        {
            int[,] slices = new int[10, 10];
            InitializeSlices(slices, -1);

            ProcessInputFile("../../../../DataSets/a_example.in");

            var idSlice = 0;
            var startRow = 0;
            var endRow = 0;

            var retVal = CutPizza(slices, ref idSlice, ref startRow, ref endRow);
            if (retVal != -1)
            {
                ShowSlices(slices);

            }
            else
            {
                Console.WriteLine("Sth wrong happened");
            }

        }

        public void ShowSlices(int[,] slices)
        {
            Console.WriteLine("----------");
            for (int rowIndex = 0; rowIndex < pizzaReqs.numberRows; rowIndex++) { 

                for (int columnIndex = 0; columnIndex < pizzaReqs.numberColumns; columnIndex++)
                {
                    Console.Write("|"+slices[rowIndex, columnIndex]+"|");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("----------");

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
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    slices[i, j] = value;
                }
            }
        }


        public int CutPizza(int[,] slicesCutted, ref int idSlice, ref int startRow, ref int startCol)
        {
            Debug.WriteLine("Cutting slice: " + idSlice);

            var slices = new List<Slice>();

            while (!PizzaTotallySliced(slicesCutted))
            {
                slices = DoSlices(startRow, startCol, slicesCutted);

                if (slices.Count > 0)
                {
                    Slice betterSlice=new Slice();
                    foreach (var slice in slices)
                    {
                        if (slice.IsInitialized)
                        {
                            Debug.WriteLine("Slice: {0}-{1} : {2}-{3}. Points: {4}", slice.StartRow, slice.StartCol, slice.EndRow, slice.EndCol, slice.Points);

                            int returnedPoints = 0;

                            if (!IsOutOfBounds(0, slice.EndCol + 1))
                            {
                                var newstartRow = 0;
                                var newstartCol = slice.EndCol + 1;
                            

                                returnedPoints = CutPizza(slicesCutted, ref idSlice, ref newstartRow, ref newstartCol);
                            }

                            if (slice.Points >= returnedPoints)
                            {
                                Debug.WriteLine("Tagging slice: {0}-{1} : {2}-{3}. Points: {4}", slice.StartRow, slice.StartCol, slice.EndRow, slice.EndCol, slice.Points);

                                TagSlice(slice, slicesCutted, idSlice);
                                idSlice++;

                                ShowSlices(slicesCutted);

                                if (slice.Points > betterSlice.Points)
                                {
                                    betterSlice = slice;
                                }

                            }
                            else
                            {
                                if (IsSliceTagged(slice, slicesCutted))
                                {
                                    TagSlice(slice, slicesCutted, -1);
                                    idSlice--;
                                }

                                startRow = slice.StartRow;
                                startCol = slice.StartCol;
                            }
                        }
                    }

                    return betterSlice.Points;

                }
                else
                {
                    return -1;
                }
            }

            return 0;
        }

        private bool IsSliceTagged(Slice slice, int[,] slicesToTag)
        {
            var isSliceTagged = false;
            for (int row = slice.StartRow; row <= slice.EndRow; row++)
            {
                for (int col = slice.StartCol; col <= slice.EndCol; col++)

                {
                    if (slicesToTag[row, col] != -1)
                    {
                        isSliceTagged = true;
                    }
                }
            }

            return isSliceTagged;
        }

        public void TagSlice(Slice slice, int[,] slicesToTag, int idSlide)
        {
            for (int row = slice.StartRow; row <= slice.EndRow; row++){ 

                for (int col = slice.StartCol; col <= slice.EndCol; col++)
            
                {
                    slicesToTag[row, col] = idSlide;
                }
            }

        }

        public bool IsOutOfBounds(int row, int col)
        {
            return row >= pizzaReqs.numberRows || col >= pizzaReqs.numberColumns;
        }

        public List<Slice> DoSlices(int startRow, int startCol, int[,] slicesCutted)
        {
            int numCells = 0;
            bool stop = false;
            int endRow = startRow;
            int endCol = startCol;
            var slices = new List<Slice>();
            var numSlices = 0;


            if (IsOutOfBounds(startRow, startCol))
            {
                stop = true;
            }

            while (!stop)
            {
                numCells = numCells + 3;

                endRow = endRow + 2;

                if (IsOutOfBounds(endRow, endCol))
                {
                    stop = true;
                }
                else
                {
                    if (SliceFulfillRequirements(startRow, startCol, endRow, endCol, slicesCutted))
                    {
                        slices.Add(new Slice(startRow, startCol, endRow, endCol, numCells));
                    }
                }

                endRow = startRow;
                endCol = endCol + 1;

            }

            return slices;
        }

        public bool PizzaTotallySliced(int[,] slices)
        {
            var pizzaTotallySliced = true;

            for (int row = 0; row < pizzaReqs.numberRows; row++)
            {
                for (int col = 0; col < pizzaReqs.numberColumns; col++)
                {
                    if (slices[row, col] == -1)
                    {
                        pizzaTotallySliced = false;
                    }

                }

            }

            return pizzaTotallySliced;
        }


        public bool SliceFulfillRequirements(int startRow, int startCol, int endRow, int endCol, int[,] slicesCutted)
        {
            int row = startRow;
            int col = startCol;
            bool fulfillRequirements = true;
            int numberIngredients = 0;
            List<char> differentIngredientsInSlice = new List<char>();

            while ((col <= endCol) && (fulfillRequirements))
            {
                while ((row <= endRow) && (fulfillRequirements))
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

                    row++;
                }

                row = startRow;
                col++;
            }

            if (pizzaReqs.minEachIngredient + 1 != differentIngredientsInSlice.Count)
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
            pizzaReqs.ingredients = new char[10, 10];

            for (var i = 0; i < pizzaReqs.numberRows; i++)
            {
                var line = reader.ReadLine();
                for (var j = 0; j < pizzaReqs.numberColumns; j++)
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


