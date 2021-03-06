﻿using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Divan
{
    /// <summary>
    /// This is a view result from a CouchQuery that can return CouchDocuments for
    /// resulting documents (include_docs) and/or ICanJson documents for the
    /// result values. A value returned from a CouchDB view does not need to be
    /// a CouchDocument.
    /// </summary>
    public class CouchGenericViewResult : CouchViewResult
    {
        /// <summary>
        /// Return all found values as documents of given type
        /// </summary>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <returns>All found values.</returns>
        public IList<T> ValueDocuments<T>() where T : ICanJson, new()
        {
            return RetrieveDocuments<T>("value");
        }

        /// <summary>
        /// Return first value found as document of given type.
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <returns>First value found or null if not found.</returns>
        public T ValueDocument<T>() where T : ICanJson, new()
        {
            return RetrieveDocument<T>("value");
        }

        /// <summary>
        /// Return all found docs as documents of given type
        /// </summary>
        /// <typeparam name="T">Type of documents.</typeparam>
        /// <returns>List of documents found.</returns>
        public IList<T> Documents<T>() where T : ICouchDocument, new()
        {
            return RetrieveDocuments<T>("doc");
        }

        /// <summary>
        /// Return all found docs as CouchJsonDocuments.
        /// </summary>
        /// <returns>List of documents found.</returns>
        public IList<CouchJsonDocument> Documents()
        {
            return RetrieveDocuments<CouchJsonDocument>("doc");
        }

        /// <summary>
        /// Return first document found as document of given type
        /// </summary>
        /// <typeparam name="T">Type of document</typeparam>
        /// <returns>First document found or null if not found.</returns>
        public T Document<T>() where T : ICouchDocument, new()
        {
            return RetrieveDocument<T>("doc");
        }

        protected virtual IList<T> RetrieveDocuments<T>(string docOrValue) where T : ICanJson, new()
        {
            var list = new List<T>();
            foreach (JToken row in Rows())
            {
                var doc = new T();
                doc.ReadJson(row[docOrValue].Value<JObject>());
                list.Add(doc);
            }
            return list;
        }

        protected virtual T RetrieveDocument<T>(string docOrValue) where T : ICanJson, new()
        {
            foreach (JToken row in Rows())
            {
                var doc = new T();
                doc.ReadJson(row[docOrValue].Value<JObject>());
                return doc;
            }
            return default(T);
        }

        public IList<CouchQueryDocument> RowDocuments()
        {
            return RowDocuments<CouchQueryDocument>();
        }

        public IList<T> RowDocuments<T>() where T : ICanJson, new()
        {
            var list = new List<T>();
            foreach (JObject row in Rows())
            {
                var doc = new T();
                doc.ReadJson(row);
                list.Add(doc);
            }
            return list;
        }
    }
}