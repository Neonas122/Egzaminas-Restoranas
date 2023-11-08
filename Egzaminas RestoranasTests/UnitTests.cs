using Microsoft.VisualStudio.TestTools.UnitTesting;
using Egzaminas_Restoranas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egzaminas_Restoranas.Tests
{
    [TestClass()]
    public class UnitTests
    {
        [TestMethod()]
        public void Test_ParseInput_Positive()
        {
            //arrange
            string input = "153";
            int expected = 153;

            //act
            int actual = Program.ParseInput(input, 0, 1000);

            //assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void Test_ParseInput_Negative()
        {
            //arrange
            string input = "sagdfs";
            int expected = -1;

            //act
            int actual = Program.ParseInput(input, 0, 1000);

            //assert
            Assert.AreEqual(expected, actual);
        }
    }
}