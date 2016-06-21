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

        [TestMethod]
        public void Send_Captcha_To_Google()
        {
            CaptchaModel captcha = new CaptchaModel { Key = "6LefWSETAAAAAPjZ6R3z5BSJf72OWR6JxNKU5zhE", Response = @"03AHJ_Vuszsr_vZ7PnOxzDeWOHzrIwdiGk - xfaW8FsfcdvUpm5t_fN8XEXs5W1bBXErFbj96cwS22UeYmuQeN57Uf7gOdfMRIpJCKXDmeoivphbeh5dEtYKZ - U - Np1Kh_dgCkDaH7M08UJvBZ5exxXq7q - uQ73lByp1bP - 1m7F9ECXUxRTA6zv2U78kKEgWZKCCy8WQhr - qsEvlzEgjFcsYN - 7CPNju95xHn1ODbanWBplq9 - oDiUqUg8U7UD6tgA94_NcrKxEX8NQ9GGcA1xVE_zmJA4L9BDepfEQ0PiI0WvqhDugB2oVKNJAXcwc0Y - sC3ylz1W7e_M3DWXRzdFM_VJe3EovY2rQofRpiy7fZ8YRInu_OTSE3Nhbqso7vKoq1td5UhWcvLMICnbc1qOiSEEmDniCQuKzpLKIfl0R8GPwmMXsXCkEtikXp4hqjhhfTLA - a_Ddl2UZMXy874AJf28rEnJSpe8h9J7UFJ_gf7mzYOtwPSbHHrqXmzjY9PfDT7WsBV9ZRyMTY0mf0gB5jAHexQrP4G1lm6j8CGAklICEZM - 3bZqlwq7UU9tcitnsrv8n83JYO65yfoQLdPu9CS53_jC3s - NnqWrtsns9uYP - pjtQHWfZX34_N6_UpHQMzbia48AjIBi8u73_3KHg3tFsi18L3vrZzOsS_CVZ0R1n3SRoa3s_e2_dvyTNW2BPLaMuML0Wa11y4ikQZlJP_pdybHR4HFiGI8c_cF_nPVEb7Os4MXiuSbqVXkzaSkoeC2DbzLyYSGKkB5ntaIkZsf0AR6phN0CE75tTb - OS4s0fTjGVik - M4uqJNlBBCcYKXF7LAPx4M7nLhkbuSfIVogVw6ARgO6T86nPUSZ5aWway_Pqe9KQM4Ys_seATlGd3gAZlUR04itphGA62eAmCHrYpADJYsi_HEpHHITUV5_stPclA3z9jXVuVxT3vZtRTpuJJzAGozLkj" };
            Login service = new Login(_repository);
            service.Captcha(captcha);
        }
    }
}
