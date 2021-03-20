using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;
using System.Threading.Tasks;

namespace Catsopinion_admin.Logic
{
    class FirebaseHandler
    {
        private FirestoreDb db = null;

        public FirebaseHandler()
        {
            this.db = FirestoreDb.Create("catsopinion-backend");
        }

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
        
        public async Task<string[]> SaveToDb(Dictionary<string, object> data, string collection)
        {
            try
            {
                // note this only catches network errors and does not verify that documnet actually exists
                await this.db.Collection(collection).AddAsync(data);
                return new string[] {"200", "Succesfully added!" };
            }
            catch (Exception e)
            {
                return new string[] { "500", "Error: " + e.Message };
            }
        }

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
