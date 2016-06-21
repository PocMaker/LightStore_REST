using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LightStore.Model
{
    /// <summary>
    /// Type of unity
    /// </summary>
    public enum enmUnity
    {
        /// <summary> 
        /// Consumer sales unit
        /// </summary>
        CSU = 0,
        /// <summary>
        /// Case (generally set of CSU)
        /// </summary>
        CASE = 1,
        /// <summary>
        /// Layer (generally set of cases)
        /// </summary>
        LAYER = 2,
        /// <summary>
        /// Palett (generally set of layers)
        /// </summary>
        PALETT = 3,
    }

    /// <summary>
    /// Describe a product
    /// </summary>
    [DataContract]
    public sealed class ProductModel : AIdEquatableT<ProductModel>
    {
        private string _code;
        /// <summary>
        /// Unique product code
        /// </summary>
        [DataMember]
        public string Code
        {
            get { return _code; }
            set
            {
                if (String.IsNullOrWhiteSpace(value)) throw new ArgumentNullException("Code", "Code cannot be empty");
                _code = value;
            }
        }

        private string _label;
        /// <summary>
        /// Unique product code
        /// </summary>
        [DataMember]
        public string Label
        {
            get { return _label; }
            set
            {
                if (String.IsNullOrWhiteSpace(value)) throw new ArgumentNullException("Label", "Label cannot be empty");
                _label = value;
            }
        }

        internal enmUnity? internalUnity = null;
        /// <summary>
        /// Base unity type
        /// </summary>
        [DataMember]
        public string Unity
        {
            get { return (internalUnity == null ? null : internalUnity.ToString()); }
            set
            {
                if (String.IsNullOrWhiteSpace(value)) internalUnity = null;
                else internalUnity = (enmUnity)Enum.Parse(typeof(enmUnity), value);
            }
        }

        /// <summary>
        /// Weight for base unity describe above
        /// </summary>
        [DataMember]
        public decimal Weight { get; set; }

        /// <summary>
        /// Tare for base unity describe above
        /// </summary>
        [DataMember]
        public decimal Tare { get; set; }

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