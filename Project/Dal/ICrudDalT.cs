using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightStore.Dal
{
    /// <summary>
    /// Default CRUD actions for a class of type T
    /// </summary>
    /// <typeparam name="T">Defined type to manage with classics CRUD actions</typeparam>
    public interface ICrudDalT<T> where T : class
    {
        /// <summary>
        /// Select all <typeparamref name="T"/> from DB
        /// </summary>
        /// <returns>A list of <typeparamref name="T"/> data</returns>
        IList<T> Select();
        /// <summary>
        /// Select a specific <typeparamref name="T"/> thanks to its Id
        /// </summary>
        /// <param name="id">Id of a specific <typeparamref name="T"/></param>
        /// <returns>The wanted <typeparamref name="T"/> if exists</returns>
        T Select(int id);
        /// <summary>
        /// Create a new <typeparamref name="T"/> in DB
        /// </summary>
        /// <param name="item">Description of the new <typeparamref name="T"/> to create</param>
        /// <returns>The selected <typeparamref name="T"/> after its selection</returns>
        T Insert(T item);
        /// <summary>
        /// Update a specific <typeparamref name="T"/>
        /// </summary>
        /// <param name="item"><typeparamref name="T"/> to update</param>
        /// <returns>The selected <typeparamref name="T"/> after its selection</returns>
        T Update(T item);
        /// <summary>
        /// Delete a specific <typeparamref name="T"/> thanks to its Id
        /// </summary>
        /// <param name="id">Id of a specific <typeparamref name="T"/></param>
        /// <returns>True if delete success</returns>
        bool Delete(int id);
    }
}
