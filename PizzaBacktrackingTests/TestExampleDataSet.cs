using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Internal;
using PizzaSlices;

namespace Tests
{
    public class TestExampleDataSet
    {
        MainClass mainclass = new MainClass();
        int[,] slices = new int[10, 10];


        [SetUp]
        public void Setup()
        {
    
            mainclass = new MainClass();

            mainclass.InitializeSlices(slices,-1);
            mainclass.ProcessInputFile("../../../../DataSets/a_example.in");
        }

        [Test]
        public void CutPizzaSuccessfully()
        {
            var a = 0;
            var b = 0;
            var c = 0;
            mainclass.CutPizza(slices, ref a, ref b, ref c);
            mainclass.ShowSlices(slices);
        }

        [Test]
        public void SliceNotValidDueToNot2DifferentIngredients()
        {
            Assert.IsFalse(mainclass.SliceFulfillRequirements(0, 0, 2, 0, slices));
        }

        [Test]
        public void SliceNotValidDueToIsTooBig()
        {
            Assert.IsFalse(mainclass.SliceFulfillRequirements(0, 0, 2, 2, slices));
        }

        [Test]
        public void SliceIsValid()
        {
            Assert.IsTrue(mainclass.SliceFulfillRequirements(0, 0, 2, 1, slices));
        }

        [Test]
        public void DoSlice()
        {
            var calculatedSlices=mainclass.DoSlices(0, 0, slices);

            Assert.AreEqual(0,calculatedSlices[0].CompareTo(new Slice(0, 0, 2, 1,6)));
        }

        [Test]
        public void DoSlices()
        {
            var calculatedSlices = mainclass.DoSlices(0, 2, slices);

            Assert.AreEqual(2,calculatedSlices.Count);
            Assert.AreEqual(3,calculatedSlices[0].Points);
            Assert.AreEqual(6,calculatedSlices[1].Points);

        }

        [Test]
        public void TagSliceProperly()
        {
            mainclass.TagSlice(new Slice(0, 0, 2, 0, 6),slices,1);

            Assert.AreEqual(1,slices[0,0]);
            Assert.AreEqual(1, slices[1, 0]);
            Assert.AreEqual(1, slices[2, 0]);
        }

        [Test]
        public void TagLargeSliceProperly()
        {
            mainclass.TagSlice(new Slice(0, 0, 2, 1, 6), slices, 1);

            Assert.AreEqual(1, slices[0, 0]);
            Assert.AreEqual(1, slices[1, 0]);
            Assert.AreEqual(1, slices[2, 0]);
            Assert.AreEqual(1, slices[0, 1]);
            Assert.AreEqual(1, slices[1, 1]);
            Assert.AreEqual(1, slices[2, 1]);
        }

        [Test]
        public void SliceNotValidDueToIsCutted()
        {
            mainclass.InitializeSlices(slices, 0);
            Assert.IsFalse(mainclass.SliceFulfillRequirements(0, 0, 2, 1, slices));
        }

        [Test]
        public void SliceNotValidDueToIsPartiallyCutted()
        {
            slices[1, 0] = 0;
            Assert.IsFalse(mainclass.SliceFulfillRequirements(0, 0, 2, 1, slices));
        }

        [Test]
        public void SliceNotValidDueToIsNotRectangular()
        {
            Assert.IsFalse(mainclass.SliceFulfillRequirements(0, 0, 0, 1, slices));
        }

        [Test]
        public void SliceOutOfBounds()
        {
            var calculatedSlices = mainclass.DoSlices(0, 4, slices);

            Assert.AreEqual(0, calculatedSlices.Count);
        }

        [Test]
        public void RowOutOfBounds()
        {
            var calculatedSlices = mainclass.DoSlices(4, 0, slices);

            Assert.AreEqual(0, calculatedSlices.Count);
        }

        [Test]
        public void ColumnOutOfBounds()
        {
            var calculatedSlices = mainclass.DoSlices(0, 6, slices);

            Assert.AreEqual(0,calculatedSlices.Count);
        }

        [Test]
        public void NumberOfIngredientsIsCorrect()
        {
            Dictionary<char,int> dictio= new Dictionary<char, int>();
            for (int col = 0; col < mainclass.pizzaReqs.numberColumns; col++)
            {
                for (int row = 0; row < mainclass.pizzaReqs.numberRows; row++)
                {
                    var key = mainclass.pizzaReqs.ingredients[row, col];

                    if (!dictio.ContainsKey(key))
                    {
                        dictio[key] = 1;
                    }
                    else
                    {
                        dictio[key] = dictio[key] + 1;
                    }
                }

            }
            Assert.AreEqual(12, dictio['T']);
            Assert.AreEqual(3, dictio['M']);

        }

        [Test]
        public void PizzaNotTotallySliced()
        {
            Assert.IsFalse(mainclass.PizzaTotallySliced(slices));
        }

        [Test]
        public void PizzaTotallySliced()
        {
            mainclass.InitializeSlices(slices, 0);

            Assert.IsTrue(mainclass.PizzaTotallySliced(slices));

            mainclass.ShowSlices(slices);

        }

        [Test]
        public void PizzaNotReallyTotallySliced()
        {
            mainclass.InitializeSlices(slices, 2);

            slices[0, 2]= 1;
            slices[1, 2] = 1;
            slices[2, 2] = 1;

            slices[0, 3] = 1;
            slices[1, 3] = 1;
            slices[2, 3] = 1;

            slices[0, 4] = -1;
            slices[1, 4] = -1;
            slices[2, 4] = -1;

                
                mainclass.ShowSlices(slices);


            Assert.False(mainclass.PizzaTotallySliced(slices));


        }

    }
}