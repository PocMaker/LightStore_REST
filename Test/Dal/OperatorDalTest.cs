using LightStore.Dal;
using LightStore.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightStore.Test.Dal
{
    public class OperatorDalTest
    {
        public static Mock<IOperatorDal> GetMockedRepository()
        {
            #region List of mocked person

            IList<OperatorModel> operators = new List<OperatorModel>
            {
                new OperatorModel { Id = 1, Login = "SuperAdmin", FirstName = "Super", LastName="Admin" },
                new OperatorModel { Id = 2, Login = "bledieu", FirstName = "Baptiste", LastName = "Ledieu", IsPasswordDefined = true, Email = "bledieu@tesfri.fr" },
                new OperatorModel { Id = 3, Login = "gjonik", FirstName = "Grégory", LastName = "Jonik", Email = "gjonik@tesfri.fr" },
                new OperatorModel { Id = 4, Login = "aestienne", FirstName = "Alexandre", LastName = "Estienne", Email = "aestienne@tesfri.fr" },
                new OperatorModel { Id = 5, Login = "pdemarcellus", FirstName = "Philippe", LastName = "De Marcellus", Email = "pdemarcellus@tesfri.fr" },
                new OperatorModel { Id = 6, Login = "fpillot", FirstName = "Frédéric", LastName = "Pillot", Email = "fpillot@tesfri.fr" },
                new OperatorModel { Id = 7, Login = "cemallet", FirstName = "Christine", LastName = "Mallet", Email = "cemallet@tesfri.fr" },
                new OperatorModel { Id = 8, Login = "jlrautureau", FirstName = "Jean-Louis", LastName = "Rautureau", Email = "jlrautureau@tesfri.fr" },
                new OperatorModel { Id = 9, Login = "pgilard", FirstName = "Pierre", LastName = "Gilard", Email = "pgilard@tesfri.fr" },
                new OperatorModel { Id = 10, Login = "cmallet", FirstName = "Christian", LastName = "Mallet", Email = "cmallet@tesfri.fr" },
                new OperatorModel { Id = 11, Login = "caudureau", FirstName = "Céline", LastName = "Audureau", Email = "caudureau@tesfri.fr" },
                new OperatorModel { Id = 12, Login = "rbruneau", FirstName = "Rodolphe", LastName = "Bruneau", Email = "rbruneau@tesfri.fr" },
                new OperatorModel { Id = 13, Login = "gfaivre", FirstName = "Grégor", LastName = "Faivre", Email = "gfaivre@tesfri.fr" }
            };

            #endregion

            Mock<IOperatorDal> repository = new Mock<IOperatorDal>();
            repository.Setup(r => r.Select()).Returns(operators);
            repository.Setup(r => r.Select(It.IsAny<int>())).Returns((int id) => operators.Where(x => x.Id == id).SingleOrDefault());
            repository.Setup(r => r.Select(It.IsAny<string>())).Returns((string login) => operators.Where(x => x.Login == login).FirstOrDefault());
            repository.Setup(r => r.Insert(It.IsAny<OperatorModel>())).Returns(
                (OperatorModel target) =>
                {
                    OperatorModel result = new OperatorModel { Id = operators.Max(o => o.Id) + 1, Login = target.Login, FirstName = target.FirstName, LastName = target.LastName };
                    PrivateObject po = new PrivateObject(result);
                    po.SetFieldOrProperty("internalModifiedDate", DateTime.Now);

                    operators.Add(result);

                    return result;
                }
                );
            repository.Setup(r => r.Update(It.IsAny<OperatorModel>())).Returns(
                (OperatorModel target) =>
                {
                    var original = operators.Where(q => q.Id == target.Id).SingleOrDefault();
                    var result = new OperatorModel { Id = original.Id, Login = original.Login, FirstName = target.FirstName, LastName = target.LastName };
                    result.Email = target.Email;

                    PrivateObject po = new PrivateObject(result);
                    po.SetFieldOrProperty("internalModifiedDate", DateTime.Now);

                    operators.Remove(original);
                    operators.Add(result);

                    return result;
                }
                );
            repository.Setup(r => r.Delete(It.IsAny<int>())).Returns(
                (int target) =>
                {
                    var ope = operators.Where(q => q.Id == target).SingleOrDefault();
                    if (ope == null) return false;

                    return operators.Remove(ope);
                }
                );

            return repository;
        }
    }
}
