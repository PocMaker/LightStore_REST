using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LightStore.Service;
using LightStore.Model;
using Moq;
using System.Collections.Generic;
using LightStore.Dal;
using LightStore.Test.Dal;
using System.ServiceModel.Web;

namespace LightStore.Test.Service
{
    [TestClass]
    public sealed class OperatorTest
    {
        private readonly Mock<IOperatorDal> _mockedRepository;
        private readonly IOperatorDal _repository;

        public OperatorTest()
        {
            _mockedRepository = OperatorDalTest.GetMockedRepository();
            _repository = _mockedRepository.Object;
        }

        [TestMethod]
        public void Read_Without_Parameter_Returns_All_Operator()
        {
            //arrange
            Operator service = new Operator(_repository);

            //act
            var result = service.Read();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(13, result.Count);
            Assert.IsInstanceOfType(result, typeof(IList<OperatorModel>));
            _mockedRepository.Verify(x => x.Select(), Times.Once);
        }

        [TestMethod]
        public void Read_With_Parameter_Returns_One_Operator()
        {
            //arrange
            Operator service = new Operator(_repository);

            //act
            var result = service.ReadOne("1");

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OperatorModel));
            Assert.AreNotEqual(0, result.Id);
            Assert.AreEqual("SuperAdmin", result.Login);
            _mockedRepository.Verify(x => x.Select(1), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Read_With_Non_Numeric_Parameter_Throws_Exception()
        {
            //arrange
            Operator service = new Operator(_repository);

            //act
            try
            {
                var result = service.ReadOne("");
            }
            catch (FormatException e)
            {
                //assert
                Assert.AreEqual("id is not a numeric value", e.Message);
                _mockedRepository.Verify(x => x.Select(), Times.Never);
                _mockedRepository.Verify(x => x.Select(It.IsAny<int>()), Times.Never);
                throw e;
            }
        }

        [TestMethod]
        public void Create_Operator_Returns_New_Operator()
        {
            OperatorModel ope = new OperatorModel { Login = "new_pseudo", FirstName = "new", LastName = "pseudo" };
            Operator service = new Operator(_repository);
            var nbOpe = service.Read().Count;

            Mock<IWebOperationContext> mockCtx = new Mock<IWebOperationContext> { DefaultValue = DefaultValue.Mock };
            using (new MockedWebOperationContext(mockCtx.Object))
            {
                var result = service.CreateOne(ope);

                Assert.IsNotNull(result);
                Assert.AreNotSame(ope, result);
                Assert.AreNotEqual(0, result.Id);
                Assert.AreEqual("new_pseudo", result.Login);
                Assert.AreEqual("new", result.FirstName);
                Assert.AreEqual("pseudo", result.LastName);
                Assert.AreEqual(nbOpe + 1, service.Read().Count);
                _mockedRepository.Verify(x => x.Insert(It.IsAny<OperatorModel>()), Times.Once);
                mockCtx.VerifySet(c => c.OutgoingResponse.Location = String.Format("/operators/{0}", result.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_Null_Operator_Throws_Exception()
        {
            Operator service = new Operator(_repository);

            try
            {
                var result = service.CreateOne(null);
            }
            catch (ArgumentNullException e)
            {
                Assert.AreEqual("data", e.ParamName);
                _mockedRepository.Verify(x => x.Insert(It.IsAny<OperatorModel>()), Times.Never);
                throw e;
            }
        }

        [TestMethod]
        public void Update_Operator_Set_Properties()
        {
            string newFirstName = Guid.NewGuid().ToString();
            string newLastName = Guid.NewGuid().ToString();

            Operator service = new Operator(_repository);
            OperatorModel oldOperator = service.ReadOne("2");

            string oldFirstName = oldOperator.FirstName;
            string oldLastName = oldOperator.LastName;

            oldOperator.FirstName = newFirstName;
            oldOperator.LastName = newLastName;

            OperatorModel newOperator = service.UpdateOne(oldOperator, "2");

            OperatorModel newOperator2 = service.ReadOne("2");

            Assert.IsNotNull(newOperator);
            Assert.AreNotSame(oldOperator, newOperator);
            Assert.AreNotEqual(oldFirstName, newOperator.FirstName);
            Assert.AreNotEqual(oldLastName, newOperator.LastName);
            Assert.AreEqual(newFirstName, newOperator2.FirstName);
            Assert.AreEqual(newLastName, newOperator2.LastName);
            _mockedRepository.Verify(x => x.Update(It.IsAny<OperatorModel>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_Null_Operator_Throws_Exception()
        {
            Operator service = new Operator(_repository);

            try
            {
                var result = service.UpdateOne(null, "2");
            }
            catch (ArgumentNullException e)
            {
                Assert.AreEqual("data", e.ParamName);
                _mockedRepository.Verify(x => x.Update(It.IsAny<OperatorModel>()), Times.Never);
                throw e;
            }
        }

        [TestMethod]
        public void Delete_Operator()
        {
            Operator service = new Operator(_repository);

            int count = service.Read().Count;
            OperatorModel ope = service.ReadOne("2");

            service.DeleteOne("2");

            Assert.AreEqual(count - 1, service.Read().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Delete_Operator_With_Non_Numeric_Id_Throws_Exception()
        {
            Operator service = new Operator(_repository);

            try
            {
                service.DeleteOne("");
            }
            catch (FormatException e)
            {
                Assert.AreEqual("id is not a numeric value", e.Message);
                throw e;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Delete_Operator_With_Negative_Id_Throws_Exception()
        {
            Operator service = new Operator(_repository);

            try
            {
                service.DeleteOne("-12");
            }
            catch (FormatException e)
            {
                Assert.AreEqual("id have to be a positive value", e.Message);
                throw e;
            }
        }
    }
}

