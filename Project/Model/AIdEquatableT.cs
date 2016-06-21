using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LightStore.Model
{
    /// <summary>
    /// Class whith Id property as unique key mark
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    public abstract class AIdEquatableT<T> : IEquatable<T> where T : class, new()
    {
        private int _id;
        /// <summary>
        /// Unique id in DB (always strictly positive)
        /// </summary>
        [DataMember]
        public int Id
        {
            get { return _id; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Id", "Id cannot be negative");
                _id = value;
            }
        }

        #region Equatable

        /// <summary>
        /// Equality with an other object
        /// </summary>
        /// <param name="obj">Object to compare with</param>
        /// <returns>True if <paramref name="obj"/> is an <typeparamref name="T"/> with the same hashcode</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as T);
        }
        /// <summary>
        /// Equality between 2 operators
        /// </summary>
        /// <param name="other">Other <typeparamref name="T"/> to compare with</param>
        /// <returns>True if other <typeparamref name="T"/> have the same hashcode</returns>
        public bool Equals(T other)
        {
            if (other == null) return false;
            return GetHashCode() == other.GetHashCode();
        }
        /// <summary>
        /// Product hashcode is equal to its Id
        /// </summary>
        /// <returns>Value of Id property</returns>
        public override int GetHashCode()
        {
            return Id;
        }

        #endregion
    }
}