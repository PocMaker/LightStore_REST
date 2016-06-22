using LightStore.Dal;
using LightStore.Model;
using LightStore.ServiceConfig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Hosting;

#if DEBUG
using WebOperationContext = System.ServiceModel.Web.MockedWebOperationContext;
#endif

namespace LightStore.Service
{
    /// <summary>
    /// Product description
    /// </summary>
    [ServiceContract]
    public interface IProduct : IServiceCrudT<ProductModel>
    {
        /// <summary>
        /// Retrieve all products
        /// </summary>
        /// <returns>Products data</returns>
        [BasicAuthenticationInvoker]
        [OperationContract]
        [WebInvoke(UriTemplate = "products", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        new IList<ProductModel> Read();

        /// <summary>
        /// Retrieve a specific product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Product data</returns>
        [BasicAuthenticationInvoker]
        [OperationContract]
        [WebInvoke(UriTemplate = "products/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        new ProductModel ReadOne(string id);

        /// <summary>
        /// Retrieve a specific product
        /// </summary>
        /// <param name="cab">Product or Packaging gtin describe by "01" ai in can</param>
        /// <returns>Product data</returns>
        [BasicAuthenticationInvoker]
        [OperationContract]
        [WebInvoke(UriTemplate = "products?cab={cab}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ProductModel ReadOneGtin(string cab);


        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="data">Product to create</param>
        /// <returns>Product data after creation</returns>
        [BasicAuthenticationInvoker]
        [OperationContract]
        [WebInvoke(UriTemplate = "products", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        new ProductModel CreateOne(ProductModel data);

        /// <summary>
        /// Delete product with the specified id
        /// </summary>
        /// <param name="id">Product ID to delete</param>
        [BasicAuthenticationInvoker]
        [OperationContract]
        [WebInvoke(UriTemplate = "products/{id}", Method = "DELETE", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        new void DeleteOne(string id);

        /// <summary>
        /// Retrieve all packagings for a specified product
        /// </summary>
        /// <returns>Packagings data</returns>
        [BasicAuthenticationInvoker]
        [OperationContract]
        [WebInvoke(UriTemplate = "products/{productId}/packaging", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IList<PackagingModel> ReadPac(string productId);

        /// <summary>
        /// Retrieve a specific packaging for a specific product
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <param name="id">Packaging Id</param>
        /// <returns>Pacakging data</returns>
        [BasicAuthenticationInvoker]
        [OperationContract]
        [WebInvoke(UriTemplate = "products/{productId}/packaging/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        PackagingModel ReadOnePac(string productId, string id);

        /// <summary>
        /// Create a new packaging
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <param name="data">Packaging to create</param>
        /// <returns>Packaging data after creation</returns>
        [BasicAuthenticationInvoker]
        [OperationContract]
        [WebInvoke(UriTemplate = "products/{productId}/packaging", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        PackagingModel CreateOnePac(string productId, PackagingModel data);

        /// <summary>
        /// Delete product with the specified id
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <param name="id">Product ID to delete</param>
        [BasicAuthenticationInvoker]
        [OperationContract]
        [WebInvoke(UriTemplate = "products/{productId}/packaging/{id}", Method = "DELETE", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void DeleteOnePac(string productId, string id);

        /// <summary>
        /// Upload a photo to the server for a ligitation
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <param name="filename">Local name of the file</param>
        /// <param name="photo">Photo to add to a specific ligitation</param>
        /// <returns>Success result</returns>
        [BasicAuthenticationInvoker]
        [OperationContract]
        [WebInvoke(UriTemplate = "products/{productId}/photo?filename={filename}", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        bool UploadPhoto(string productId, string filename, Stream photo);
    }

    /// <summary>
    /// Product description
    /// </summary>
    public sealed class Product : AServiceBase, IProduct
    {
        private readonly IProductDal _productDal;
        private readonly IPackagingDal _packagingDal;

        /// <summary>
        /// Interact with DB
        /// </summary>
        public Product() : this(new ProductDal(), new PackagingDal()) { }
        /// <summary>
        /// Interact with a mock (unit tests only)
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if mocked Dal is null</exception>
        /// <param name="productDal">Db mock for unit test (required)</param>
        /// <param name="packagingDal">Db mock for unit test (required)</param>
        public Product(IProductDal productDal, IPackagingDal packagingDal)
        {
            if (productDal == null) throw new ArgumentNullException("productDal", "Mocked Dal cannot be null");
            if (packagingDal == null) throw new ArgumentNullException("packagingDal", "Mocked Dal cannot be null");
            _productDal = productDal;
            _packagingDal = packagingDal;
        }



        /// <summary>
        /// Retrieve all products
        /// </summary>
        /// <returns>Products data</returns>
        public IList<ProductModel> Read()
        {
            return _productDal.Select();
        }

        /// <summary>
        /// Retrieve a specific product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <exception cref="FaultException">Thrown when <paramref name="id"/> is not a strictly positive numeric data</exception>
        /// <returns>Product data</returns>
        public ProductModel ReadOne(string id)
        {
            int localId;

            if (!int.TryParse(id, out localId)) this.ThrowFaultException("ID_IS_NOT_NUMERIC", "id is not a numeric value");
            if (localId <= 0) this.ThrowFaultException("ID_IS_NOT_POSITIVE", "id have to be a positive value");

            return _productDal.Select(localId);
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="data">Product to create</param>
        /// <exception cref="FaultException">Thrown if <paramref name="data"/> is null</exception>
        /// <returns>Product data after creation</returns>
        public ProductModel CreateOne(ProductModel data)
        {
            if (data == null) this.ThrowFaultException("PRODUCT_NULL", "Product to create cannot be null");

            ProductModel product = null;
            try
            {
                product = _productDal.Insert(data);
            }
            catch (Exception e)
            {
                this.ThrowFaultException(e);
            }

            WebOperationContext.Current.OutgoingResponse.Location = String.Format("/products/{0}", product.Id);
            return product;
        }

        /// <summary>
        /// Delete product with the specified id
        /// </summary>
        /// <param name="id">Product ID to delete</param>
        /// <exception cref="FaultException">Thrown when <paramref name="id"/> is not a strictly positive numeric data</exception>
        public void DeleteOne(string id)
        {
            int localId;

            if (!int.TryParse(id, out localId)) this.ThrowFaultException("ID_IS_NOT_NUMERIC", "id is not a numeric value");
            if (localId <= 0) this.ThrowFaultException("ID_IS_NOT_POSITIVE", "id have to be a positive value");

            _productDal.Delete(localId);
        }

        /// <exclude/>
        public ProductModel UpdateOne(ProductModel data, string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieve all packagings for a specified product
        /// </summary>
        /// <exception cref="FaultException">Thrown when <paramref name="productId"/> is not a strictly positive numeric data</exception>
        /// <returns>Packagings data</returns>
        public IList<PackagingModel> ReadPac(string productId)
        {
            int localId;

            if (!int.TryParse(productId, out localId)) this.ThrowFaultException("ID_IS_NOT_NUMERIC", "id is not a numeric value");
            if (localId <= 0) this.ThrowFaultException("ID_IS_NOT_POSITIVE", "id have to be a positive value");

            return _packagingDal.Select(localId);
        }

        /// <summary>
        /// Retrieve a specific packaging for a specific product
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <param name="id">Packaging Id</param>
        /// <exception cref="FaultException">Thrown when <paramref name="productId"/> is not a strictly positive numeric data</exception>
        /// <exception cref="FaultException">Thrown when <paramref name="id"/> is not a strictly positive numeric data</exception>
        /// <returns>Pacakging data</returns>
        public PackagingModel ReadOnePac(string productId, string id)
        {
            int localId, localId2;

            if (!int.TryParse(productId, out localId)) this.ThrowFaultException("ID_IS_NOT_NUMERIC", "productId is not a numeric value");
            if (localId <= 0) this.ThrowFaultException("ID_IS_NOT_POSITIVE", "productId have to be a positive value");

            if (!int.TryParse(id, out localId2)) this.ThrowFaultException("ID_IS_NOT_NUMERIC", "id is not a numeric value");
            if (localId2 <= 0) this.ThrowFaultException("ID_IS_NOT_POSITIVE", "id have to be a positive value");


            return _packagingDal.Select(localId, localId2);
        }

        /// <summary>
        /// Create a new packaging
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <param name="data">Packaging to create</param>
        /// <exception cref="FaultException">Thrown when <paramref name="data"/> is NULL</exception>
        /// <returns>Packaging data after creation</returns>
        public PackagingModel CreateOnePac(string productId, PackagingModel data)
        {
            if (data == null) this.ThrowFaultException("PACKAGING_NULL", "Packaging to create cannot be null");

            PackagingModel packaging = null;
            try
            {
                packaging = _packagingDal.Insert(data);
            }
            catch (Exception e)
            {
                this.ThrowFaultException(e);
            }

            WebOperationContext.Current.OutgoingResponse.Location = String.Format("/products/{0}/packaging/{1}", packaging.ProductId, packaging.Id);
            return packaging;
        }

        /// <summary>
        /// Delete product with the specified id
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <param name="id">Product ID to delete</param>
        /// <exception cref="FaultException">Thrown when <paramref name="productId"/> is not a strictly positive numeric data</exception>
        /// <exception cref="FaultException">Thrown when <paramref name="id"/> is not a strictly positive numeric data</exception>
        public void DeleteOnePac(string productId, string id)
        {
            int localId, localId2;

            if (!int.TryParse(productId, out localId)) this.ThrowFaultException("ID_IS_NOT_NUMERIC", "productId is not a numeric value");
            if (localId <= 0) this.ThrowFaultException("ID_IS_NOT_POSITIVE", "productId have to be a positive value");

            if (!int.TryParse(id, out localId2)) this.ThrowFaultException("ID_IS_NOT_NUMERIC", "id is not a numeric value");
            if (localId2 <= 0) this.ThrowFaultException("ID_IS_NOT_POSITIVE", "id have to be a positive value");

            _packagingDal.Delete(localId, localId2);
        }

        /// <summary>
        /// Retrieve a specific product
        /// </summary>
        /// <param name="cab">Product or Packaging gtin describe by "01" ai in can</param>
        /// <exception cref="FaultException">Thrown when <paramref name="cab"/> is NULL</exception>
        /// <returns>Product data</returns>
        public ProductModel ReadOneGtin(string cab)
        {
            if (String.IsNullOrWhiteSpace(cab)) this.ThrowFaultException("NULL_CAB", "Cab cannot be null");

            return _productDal.Select(cab);
        }

        /// <summary>
        /// Upload a photo to the server for a ligitation
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <param name="filename">Local name of the file</param>
        /// <param name="photo">Photo to add to a specific ligitation</param>
        /// <returns>Success result</returns>
        public bool UploadPhoto(string productId, string filename, Stream photo)
        {
            filename = Path.Combine(HostingEnvironment.MapPath("~/App_Data"), filename);
            FileStream fileToupload = new FileStream(filename, FileMode.Create);

            byte[] bytearray = new byte[10000];
            int bytesRead, totalBytesRead = 0;
            do
            {
                bytesRead = photo.Read(bytearray, 0, bytearray.Length);
                totalBytesRead += bytesRead;
            } while (bytesRead > 0);

            fileToupload.Write(bytearray, 0, bytearray.Length);
            fileToupload.Close();
            fileToupload.Dispose();   

            return true;
        }
    }
}
