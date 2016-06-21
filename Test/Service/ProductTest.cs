using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LightStore.Service;
using LightStore.Model;
using Moq;
using System.Collections.Generic;
using LightStore.Dal;
using LightStore.Test.Dal;
using System.ServiceModel.Web;
using System.ServiceModel;

namespace LightStore.Test.Service
{
    [TestClass]
    public sealed class ProductTest
    {
        private readonly Mock<IProductDal> _mockedRepositoryProduct;
        private readonly IProductDal _repositoryProduct;
        private readonly Mock<IPackagingDal> _mockedRepositoryPackaging;
        private readonly IPackagingDal _repositoryPackaging;

        public ProductTest()
        {
            _mockedRepositoryProduct = ProductDalTest.GetMockedRepository();
            _repositoryProduct = _mockedRepositoryProduct.Object;
            _mockedRepositoryPackaging = PackagingDalTest.GetMockedRepository();
            _repositoryPackaging = _mockedRepositoryPackaging.Object;
        }

        [TestMethod]
        public void Read_Without_Parameter_Returns_All_Products()
        {
            //arrange
            Product service = new Product(_repositoryProduct,_repositoryPackaging);

            //act
            var result = service.Read();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.Count);
            Assert.IsInstanceOfType(result, typeof(IList<ProductModel>));
            _mockedRepositoryProduct.Verify(x => x.Select(), Times.Once);
        }

        [TestMethod]
        public void Read_With_Parameter_Returns_One_Product()
        {
            //arrange
            Product service = new Product(_repositoryProduct, _repositoryPackaging);

            //act
            var result = service.ReadOne("1");

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ProductModel));
            Assert.AreNotEqual(0, result.Id);
            Assert.AreEqual("MockedProduct1", result.Code);
            _mockedRepositoryProduct.Verify(x => x.Select(1), Times.Once);
        }

        [TestMethod]
        public void Read_With_Gtin_Returns_One_Product()
        {
            //arrange
            Product service = new Product(_repositoryProduct, _repositoryPackaging);

            //act
            var result = service.ReadOneGtin(new DefaultJsonString { Value = "]C10111111111111111" });

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ProductModel));
            Assert.AreNotEqual(0, result.Id);
            Assert.AreEqual("MockedProduct1", result.Code);
            _mockedRepositoryProduct.Verify(x => x.Select("]C10111111111111111"), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<JsonFaultModel>))]
        public void Read_With_Non_Numeric_Parameter_Throws_Exception()
        {
            //arrange
            Product service = new Product(_repositoryProduct, _repositoryPackaging);

            //act
            try
            {
                var result = service.ReadOne("");
            }
            catch (FaultException<JsonFaultModel> e)
            {
                //assert
                Assert.AreEqual("ID_IS_NOT_NUMERIC", e.Detail.ErrorCode);
                Assert.AreEqual("ReadOne", e.Detail.Method);
                _mockedRepositoryProduct.Verify(x => x.Select(), Times.Never);
                _mockedRepositoryProduct.Verify(x => x.Select(It.IsAny<int>()), Times.Never);
                throw e;
            }
        }

        [TestMethod]
        public void Create_Product_Returns_New_Product()
        {
            ProductModel product = new ProductModel { Code = "NewProduct", Label = "New product" };
            Product service = new Product(_repositoryProduct, _repositoryPackaging);
            var nbOpe = service.Read().Count;

            Mock<IWebOperationContext> mockCtx = new Mock<IWebOperationContext> { DefaultValue = DefaultValue.Mock };
            using (new MockedWebOperationContext(mockCtx.Object))
            {
                var result = service.CreateOne(product);

                Assert.IsNotNull(result);
                Assert.AreNotSame(product, result);
                Assert.AreNotEqual(0, result.Id);
                Assert.AreEqual("NewProduct", result.Code);
                Assert.AreEqual("New product", result.Label);
                Assert.AreEqual(nbOpe + 1, service.Read().Count);
                _mockedRepositoryProduct.Verify(x => x.Insert(It.IsAny<ProductModel>()), Times.Once);
                mockCtx.VerifySet(c => c.OutgoingResponse.Location = String.Format("/products/{0}", result.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<JsonFaultModel>))]
        public void Create_Null_Product_Throws_Exception()
        {
            Product service = new Product(_repositoryProduct, _repositoryPackaging);

            try
            {
                var result = service.CreateOne(null);
            }
            catch (FaultException<JsonFaultModel> e)
            {
                Assert.AreEqual("PRODUCT_NULL", e.Detail.ErrorCode);
                _mockedRepositoryProduct.Verify(x => x.Insert(It.IsAny<ProductModel>()), Times.Never);
                throw e;
            }
        }

        [TestMethod]
        public void Delete_Product()
        {
            Product service = new Product(_repositoryProduct, _repositoryPackaging);

            int count = service.Read().Count;
            ProductModel product = service.ReadOne("2");

            service.DeleteOne("2");

            Assert.AreEqual(count - 1, service.Read().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<JsonFaultModel>))]
        public void Delete_Product_With_Non_Numeric_Id_Throws_Exception()
        {
            Product service = new Product(_repositoryProduct, _repositoryPackaging);

            try
            {
                service.DeleteOne("");
            }
            catch (FaultException<JsonFaultModel> e)
            {
                Assert.AreEqual("ID_IS_NOT_NUMERIC", e.Detail.ErrorCode);
                throw e;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<JsonFaultModel>))]
        public void Delete_Product_With_Negative_Id_Throws_Exception()
        {
            Product service = new Product(_repositoryProduct, _repositoryPackaging);

            try
            {
                service.DeleteOne("-12");
            }
            catch (FaultException<JsonFaultModel> e)
            {
                Assert.AreEqual("ID_IS_NOT_POSITIVE", e.Detail.ErrorCode);
                throw e;
            }
        }
    }
}

