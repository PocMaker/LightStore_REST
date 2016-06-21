using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LightStore.Model
{
    /// <summary>
    /// Describe a product
    /// </summary>
    [DataContract]
    public sealed class PackagingModel : AIdEquatableT<PackagingModel>
    {
        /// <summary>
        /// Product id
        /// </summary>
        [DataMember]
        public int ProductId { get; set; }

        /// <summary>
        /// Id of packaging which this one depends on
        /// </summary>
        [DataMember]
        public int? FromId { get; set; }

        internal enmUnity internalUnity = enmUnity.CSU;
        /// <summary>
        /// Base unity type
        /// </summary>
        [DataMember]
        public string Unity
        {
            get { return internalUnity.ToString(); }
            set
            {
                internalUnity = (enmUnity)Enum.Parse(typeof(enmUnity), value);
            }
        }

        /// <summary>
        /// Quantity for current unity
        /// </summary>
        [DataMember]
        public int Quantity { get; set; }

        /// <summary>
        /// Unique global identifier for this product
        /// </summary>
        [DataMember]
        public string GTIN { get; set; }

        internal DateTime internalModifiedDate { get; set; }
        /// <summary>
        /// Last modified date in DB (format "yyyy-mm-ddThh:MM:ss")
        /// </summary>
        [DataMember]
        public string ModifiedDate
        {
            get { return internalModifiedDate.ToString("O", CultureInfo.InvariantCulture); }
            set { }
        }
    }
}