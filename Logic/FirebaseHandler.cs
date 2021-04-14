using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;
using System.Threading.Tasks;

namespace Catsopinion_admin.Logic
{

    /// <summary>
    /// Class to handle firebase interaction
    /// </summary>
    class FirebaseHandler
    {
        private FirestoreDb db = null;

        public FirebaseHandler()
        {
            this.db = FirestoreDb.Create("catsopinion-backend");
        }


        /// <summary>
        /// Return all documents for queried collection
        /// Must be waited
        /// </summary>
        /// <param name="collectionName">Collection name to get</param>
        /// <returns>List of dictionarys</returns>
        public async Task<List<Dictionary<string, object>>> GetCollection(string collectionName)
        {
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            CollectionReference collection = this.db.Collection(collectionName);
            QuerySnapshot snapshot = await collection.GetSnapshotAsync();
            foreach(DocumentSnapshot document in snapshot.Documents)
            {
                Dictionary<string, object> documentDictionary = document.ToDictionary();
                documentDictionary.Add("id", document.Id);
                data.Add(documentDictionary);
            }
            return data;
        }


        /// <summary>
        /// Saves single document to firestore.
        /// Must be awaited!
        /// </summary>
        /// <param name="data">Object to save aka. single row in database</param>
        /// <param name="collection">Collection name where to save</param>
        /// <returns >
        /// On success: string["200","Succesfully added!" ] | 
        /// On error: new string["500", "Error: " + e.Message }
        /// 
        /// </returns>
        public async Task<string[]> SaveToDb(Dictionary<string, object> data, string collection)
        {
            try
            {
                await this.db.Collection(collection).AddAsync(data);
                return new string[] {"200", "Succesfully added!" };
            }
            catch (Exception e)
            {
                return new string[] { "500", "Error: " + e.Message };
            }
        }


        /// <summary>
        /// Deletes document from  firestore.
        /// Must be awaited!
        /// </summary>
        /// <param name="id">Document id number</param>
        /// <param name="collection">Collection where to remove</param>
        /// <returns>
        /// On success: string["200","Succesfully added!" ] | 
        /// On error: new string["500", "Error: " + e.Message }
        /// </returns>

        public async Task<string[]> DeleteFromDb(string id, string collection)
        {
            try
            {
                await this.db.Collection(collection).Document(id).DeleteAsync();
                return new string[] { "200", "Document removed" };
            }
            catch (Exception e)
            {
                return new string[] { "500", "Error: " + e.Message };
            }
        }
    }
}
