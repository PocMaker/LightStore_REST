using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LightStore.Model;
using System.Text.RegularExpressions;

namespace LightStore.Test.Model
{
    [TestClass]
    public class OperatorModelTest
    {
        [TestMethod]
        [TestCategory("Operator")]
        public void Class_Are_Equal_When_Id_Are_Equal()
        {
            OperatorModel operator1 = new OperatorModel { Id = 2, Login = "login", FirstName = "John", LastName = "Doe" };
            OperatorModel operator2 = new OperatorModel { Id = 2, Login = "another login", FirstName = "Jane", LastName = "Smith" };
            OperatorModel operator3 = new OperatorModel { Id = 3, Login = "login", FirstName = "Anybody", LastName = "Else" };

            Assert.AreEqual(operator1, operator2);
            Assert.AreNotEqual(operator1, operator3);
            Assert.AreNotEqual(operator2, operator3);
        }

        [TestMethod]
        [TestCategory("Operator")]
        [ExpectedException(typeof(ArgumentNullException), "A non defined firstName should throw an application exception")]
        public void Set_Empty_FirstName_Throw_ArgumentNullException()
        {
            //Test EMPTY
            try
            {
                OperatorModel operator1 = new OperatorModel { FirstName = "" };
            }
            catch (ArgumentNullException e)
            {
                Assert.AreEqual("FirstName", e.ParamName);
                throw e;
            }
        }

        [TestMethod]
        [TestCategory("Operator")]
        [ExpectedException(typeof(ArgumentNullException), "A non defined lastName should throw an application exception")]
        public void Set_Empty_LastName_Throw_ArgumentNullException()
        {
            //Test EMPTY
            try
            {
                OperatorModel operator1 = new OperatorModel { LastName = "" };
            }
            catch (ArgumentNullException e)
            {
                Assert.AreEqual("LastName", e.ParamName);
                throw e;
            }
        }

        [TestMethod]
        [TestCategory("Operator")]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "A negative id should throw an application exception")]
        public void Set_Negative_Id_Throw_ArgumentOutOfRangeException()
        {
            try
            {
                OperatorModel operator1 = new OperatorModel { Id = -1 };
            }
            catch (ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("Id", e.ParamName);
                throw e;
            }
        }

        [TestMethod]
        [TestCategory("Operator")]
        public void ModifiedDate_Is_In_Correct_Format()
        {
            OperatorModel operator1 = new OperatorModel();

            Assert.IsTrue(Regex.IsMatch(operator1.ModifiedDate, "^(-?(?:[1-9][0-9]*)?[0-9]{4})-(1[0-2]|0[1-9])-(3[0-1]|0[1-9]|[1-2][0-9])T(2[0-3]|[0-1][0-9]):([0-5][0-9]):([0-5][0-9])(\\.[0-9]+)?(Z|[+-](?:2[0-3]|[0-1][0-9]):[0-5][0-9])?$"));
        }
    }
}
