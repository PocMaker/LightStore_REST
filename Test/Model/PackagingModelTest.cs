using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LightStore.Model;
using System.Text.RegularExpressions;

namespace LightStore.Test.Model
{
    [TestClass]
    public class PackagingModelTest
    {
        [TestMethod]
        [TestCategory("Packaging")]
        public void Class_Are_Equal_When_Id_Are_Equal()
        {
            PackagingModel item1 = new PackagingModel { Id = 2, Unity = enmUnity.CSU.ToString(), GTIN = "123" };
            PackagingModel item2 = new PackagingModel { Id = 2, Unity = enmUnity.CASE.ToString(), GTIN = "123" };
            PackagingModel item3 = new PackagingModel { Id = 3, Unity = enmUnity.CSU.ToString(), GTIN = "123" };

            Assert.AreEqual(item1, item2);
            Assert.AreNotEqual(item1, item3);
            Assert.AreNotEqual(item2, item3);
        }

        [TestMethod]
        [TestCategory("Packaging")]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "A negative id should throw an application exception")]
        public void Set_Negative_Id_Throw_ArgumentOutOfRangeException()
        {
            try
            {
                PackagingModel product = new PackagingModel { Id = -1 };
            }
            catch (ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("Id", e.ParamName);
                throw e;
            }
        }

        [TestMethod]
        [TestCategory("Packaging")]
        public void ModifiedDate_Is_In_Correct_Format()
        {
            PackagingModel product = new PackagingModel();

            Assert.IsTrue(Regex.IsMatch(product.ModifiedDate, "^(-?(?:[1-9][0-9]*)?[0-9]{4})-(1[0-2]|0[1-9])-(3[0-1]|0[1-9]|[1-2][0-9])T(2[0-3]|[0-1][0-9]):([0-5][0-9]):([0-5][0-9])(\\.[0-9]+)?(Z|[+-](?:2[0-3]|[0-1][0-9]):[0-5][0-9])?$"));
        }
    }
}
