using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Internal;
using PizzaSlices;

namespace Tests
{
    public class TestSmallDataSet
    {
        MainClass mainclass = new MainClass();
        int[,] slices = new int[10, 10];


        [SetUp]
        public void Setup()
        {
    
            mainclass = new MainClass();

            mainclass.InitializeSlices(slices,-1);
            mainclass.ProcessInputFile("../../../../DataSets/b_small.in");
        }

        [Test]
        public void CutPizzaSuccessfullySmallDataSet()
        {
            var a = 0;
            var b = 0;
            var c = 0;
            mainclass.CutPizza(slices, ref a, ref b, ref c);
            mainclass.ShowSlices(slices);
        }

       
    }
}