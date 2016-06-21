using LightStore.Dal;
using LightStore.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LightStore.Test.Dal
{
    public class ProductDalTest
    {
        public static Mock<IProductDal> GetMockedRepository()
        {
            #region List of mocked person

            IList<ProductModel> products = new List<ProductModel>
            {
                new ProductModel { Id = 1, Code = "MockedProduct1", Label = "Mocked product n° 1", Unity = "CSU", GTIN = "11111111111111" },
                new ProductModel { Id = 2, Code = "MockedProduct2", Label = "Mocked product n° 2", Unity = "CSU", GTIN = "22222222222222" },
                new ProductModel { Id = 3, Code = "MockedProduct3", Label = "Mocked product n° 3", Unity = "CSU", GTIN = "33333333333333" },
                new ProductModel { Id = 4, Code = "MockedProduct4", Label = "Mocked product n° 4", Unity = "CSU", GTIN = "44444444444444" },
                new ProductModel { Id = 5, Code = "MockedProduct5", Label = "Mocked product n° 5", Unity = "CSU", GTIN = "55555555555555" },
                new ProductModel { Id = 6, Code = "MockedProduct6", Label = "Mocked product n° 6", Unity = "CSU", GTIN = "66666666666666" },
            };

            #endregion

            Mock<IProductDal> repository = new Mock<IProductDal>();
            repository.Setup(r => r.Select()).Returns(products);
            repository.Setup(r => r.Select(It.IsAny<int>())).Returns((int id) => products.Where(x => x.Id == id).SingleOrDefault());
            repository.Setup(r => r.Select(It.IsAny<string>())).Returns((string cab) => products.Where(x => x.GTIN == cab.Substring(cab.IndexOf("01") + 2, 14)).SingleOrDefault());
            repository.Setup(r => r.Insert(It.IsAny<ProductModel>())).Returns(
                (ProductModel target) =>
                {
                    ProductModel result = new ProductModel { Id = products.Max(o => o.Id) + 1, Code = target.Code, Label = target.Label, GTIN = target.GTIN, Unity = target.Unity, Tare = target.Tare, Weight = target.Weight };
                    PrivateObject po = new PrivateObject(result);
                    po.SetFieldOrProperty("internalModifiedDate", DateTime.Now);

                    products.Add(result);

                    return result;
                }
                );
            repository.Setup(r => r.Delete(It.IsAny<int>())).Returns(
                (int target) =>
                {
                    var product = products.Where(q => q.Id == target).SingleOrDefault();
                    if (product == null) return false;

                    return products.Remove(product);
                }
                );

            return repository;
        }
    }
}
