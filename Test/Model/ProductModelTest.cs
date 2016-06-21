using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LightStore.Model;
using System.Text.RegularExpressions;

namespace LightStore.Test.Model
{
    [TestClass]
    public class ProductModelTest
    {
        [TestMethod]
        [TestCategory("Product")]
        public void Class_Are_Equal_When_Id_Are_Equal()
        {
            ProductModel product1 = new ProductModel { Id = 2, Code = "Prod1", Label = "Produit 1" };
            ProductModel product2 = new ProductModel { Id = 2, Code = "Prod2", Label = "Produit 1" };
            ProductModel product3 = new ProductModel { Id = 3, Code = "Prod1", Label = "Produit autre" };

            Assert.AreEqual(product1, product2);
            Assert.AreNotEqual(product1, product3);
            Assert.AreNotEqual(product2, product3);
        }

        [TestMethod]
        [TestCategory("Product")]
        [ExpectedException(typeof(ArgumentNullException), "A non defined code should throw an application exception")]
        public void Set_Empty_Code_Throw_ArgumentNullException()
        {
            //Test EMPTY
            try
            {
                ProductModel product = new ProductModel { Code = "" };
            }
            catch (ArgumentNullException e)
            {
                Assert.AreEqual("Code", e.ParamName);
                throw e;
            }
        }

        [TestMethod]
        [TestCategory("Product")]
        [ExpectedException(typeof(ArgumentNullException), "A non defined label should throw an application exception")]
        public void Set_Empty_Label_Throw_ArgumentNullException()
        {
            //Test EMPTY
            try
            {
                ProductModel product = new ProductModel { Label = "" };
            }
            catch (ArgumentNullException e)
            {
                Assert.AreEqual("Label", e.ParamName);
                throw e;
            }
        }

        [TestMethod]
        [TestCategory("Product")]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "A negative id should throw an application exception")]
        public void Set_Negative_Id_Throw_ArgumentOutOfRangeException()
        {
            try
            {
                ProductModel product = new ProductModel { Id = -1 };
            }
            catch (ArgumentOutOfRangeException e)
            {
                Assert.AreEqual("Id", e.ParamName);
                throw e;
            }
        }

        [TestMethod]
        [TestCategory("Product")]
        public void ModifiedDate_Is_In_Correct_Format()
        {
            ProductModel product = new ProductModel();

            Assert.IsTrue(Regex.IsMatch(product.ModifiedDate, "^(-?(?:[1-9][0-9]*)?[0-9]{4})-(1[0-2]|0[1-9])-(3[0-1]|0[1-9]|[1-2][0-9])T(2[0-3]|[0-1][0-9]):([0-5][0-9]):([0-5][0-9])(\\.[0-9]+)?(Z|[+-](?:2[0-3]|[0-1][0-9]):[0-5][0-9])?$"));
        }
    }
}
