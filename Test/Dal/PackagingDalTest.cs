using LightStore.Dal;
using LightStore.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LightStore.Test.Dal
{
    public class PackagingDalTest
    {
        public static Mock<IPackagingDal> GetMockedRepository()
        {
            #region List of mocked person

            IList<PackagingModel> packagings = new List<PackagingModel>
            {
                new PackagingModel { Id = 1, ProductId = 1, Quantity = 10, Unity = enmUnity.CASE.ToString(), GTIN = "0111111111" },
                new PackagingModel { Id = 2, ProductId = 1, FromId = 1, Quantity = 15, Unity = enmUnity.LAYER.ToString(), GTIN = "0222222222" },
                new PackagingModel { Id = 3, ProductId = 1, FromId = 2, Quantity = 3, Unity = enmUnity.PALETT.ToString(), GTIN = "0333333333" },
                new PackagingModel { Id = 4, ProductId = 2, Quantity = 5, Unity = enmUnity.CASE.ToString(), GTIN = "0444444444" },
                new PackagingModel { Id = 5, ProductId = 2, FromId = 4, Quantity = 10, Unity = enmUnity.LAYER.ToString(), GTIN = "0555555555" },
                new PackagingModel { Id = 6, ProductId = 2, FromId = 5, Quantity = 8, Unity = enmUnity.PALETT.ToString(), GTIN = "0666666666" },
            };

            #endregion

            Mock<IPackagingDal> repository = new Mock<IPackagingDal>();
            repository.Setup(r => r.Select(It.IsAny<int>())).Returns((int productId) => packagings.Where(x => x.ProductId == productId).ToList());
            repository.Setup(r => r.Select(It.IsAny<int>(), It.IsAny<int>())).Returns((int productId, int id) => packagings.Where(x => x.ProductId == productId && x.Id == id).SingleOrDefault());
            repository.Setup(r => r.Insert(It.IsAny<PackagingModel>())).Returns(
                (PackagingModel target) =>
                {
                    PackagingModel result = new PackagingModel { Id = packagings.Max(o => o.Id) + 1, ProductId = target.ProductId, FromId = target.FromId, GTIN = target.GTIN, Unity = target.Unity, Quantity = target.Quantity };
                    PrivateObject po = new PrivateObject(result);
                    po.SetFieldOrProperty("internalModifiedDate", DateTime.Now);

                    packagings.Add(result);

                    return result;
                }
                );
            repository.Setup(r => r.Delete(It.IsAny<int>(), It.IsAny<int?>())).Returns(
                (int productId, int? id) =>
                {
                    var packaging = packagings.Where(q => q.ProductId == productId && (id == null || q.Id == id)).SingleOrDefault();
                    if (packaging == null) return false;

                    return packagings.Remove(packaging);
                }
                );

            return repository;
        }
    }
}
