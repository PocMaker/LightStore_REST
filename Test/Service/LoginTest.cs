using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LightStore.Dal;
using LightStore.Test.Dal;
using Moq;
using LightStore.Service;
using LightStore.Model;
using System.Linq;
using WebOperationContext = System.ServiceModel.Web.MockedWebOperationContext;
using System.ServiceModel.Web;
using System.Security.Authentication;
using System.ServiceModel;

namespace LightStore.Test.Service
{
    [TestClass]
    public class LoginTest
    {
        private readonly Mock<IOperatorDal> _mockedRepository;
        private readonly IOperatorDal _repository;

        public LoginTest()
        {
            _mockedRepository = OperatorDalTest.GetMockedRepository();
            _repository = _mockedRepository.Object;
        }

        [TestMethod]
        public void Log_With_An_Existing_Pseudo_Returns_Info()
        {
            Login service = new Login(_repository);
            Operator serviceOp = new Operator(_repository);

            Mock<IWebOperationContext> mockCtx = new Mock<IWebOperationContext> { DefaultValue = DefaultValue.Mock };
            using (new MockedWebOperationContext(mockCtx.Object))
            {
                OperatorModel ope = serviceOp.Read().Where(o => o.Login == "bledieu").FirstOrDefault();

                CredentialModel credential = new CredentialModel { Login = "bledieu", Password = "password" };
                IdIsPasswordDefinedModel response = service.IsPasswordDefined(credential);

                Assert.IsNotNull(ope);
                Assert.IsNotNull(response);
                Assert.AreEqual(ope.Id, response.Id);
                Assert.AreEqual(true, ope.IsPasswordDefined);
                Assert.AreEqual(true, response.IsPasswordDefined);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<JsonFaultModel>))]
        public void Log_With_An_Unexisting_Pseudo_Thrown_Exception()
        {
            Login service = new Login(_repository);
            Mock<IWebOperationContext> mockCtx = new Mock<IWebOperationContext> { DefaultValue = DefaultValue.Mock };
            using (new MockedWebOperationContext(mockCtx.Object))
            {
                try
                {
                    CredentialModel credential = new CredentialModel { Login = Guid.NewGuid().ToString() };
                IdIsPasswordDefinedModel response = service.IsPasswordDefined(credential);
                }
                catch (FaultException<JsonFaultModel> e)
                {
                    Assert.AreEqual("INVALID_CREDENTIAL", e.Detail.ErrorCode);
                    throw;
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<JsonFaultModel>))]
        public void Log_With_An_Wrong_Password_Thrown_Exception()
        {
            Login service = new Login(_repository);
            Mock<IWebOperationContext> mockCtx = new Mock<IWebOperationContext> { DefaultValue = DefaultValue.Mock };
            using (new MockedWebOperationContext(mockCtx.Object))
            {
                try
                {
                    CredentialModel credential = new CredentialModel { Login = "bledieu", Password = "xxx" };
                    OperatorModel response = service.LogIn(credential);
                }
                catch (FaultException<JsonFaultModel> e)
                {
                    Assert.AreEqual("INVALID_CREDENTIAL", e.Detail.ErrorCode);
                    throw;
                }
            }
        }
    }
}
