using System.IO;
using NUnit.Framework;
using NUnit.Framework.Internal;
using PizzaSlices;

namespace Tests
{
    public class TestSmallDataSet
    {
        MainClass mainclass = new MainClass();
        int[,] slices = new int[5, 5];


        [SetUp]
        public void Setup()
        {
    
            mainclass = new MainClass();

            mainclass.InitializeSlices(slices,-1);
            mainclass.ProcessInputFile("../../../../DataSets/a_example.in");
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
            Assert.DoesNotThrow(()=>mainclass.DoSlice(0, 0, slices));
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
    }
}