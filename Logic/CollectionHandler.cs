using System.Configuration;


namespace Catsopinion_admin.Logic
{
    /// <summary>
    /// Gets corresponding collection name from app.config file
    /// </summary>
    class CollectionHandler
    {

        /// <summary>
        /// Public method to get message collection. DEPRECATED, WILL BE REMOVED
        /// </summary>
        /// <returns>string of collection name</returns>
        public string GetMessageCollection()
        {
            return ConfigurationManager.AppSettings.Get("messageCollection");
        }

        /// <summary>
        /// Public method to get series collection.
        /// </summary>
        /// <returns>string collection name</returns>
        public string GetSeries()
        {
            return ConfigurationManager.AppSettings.Get("series");
        }


        /// <summary>
        /// Public method to get language codes used
        /// </summary>
        /// <returns>"en" or "fi"</returns>
        public string GetLocales()
        {
            return ConfigurationManager.AppSettings.Get("locales");
        }


       /// <summary>
       /// Public method to get collection where hot news are stored
       /// </summary>
       /// <param name="localeIdentifier">"en" or "fi"</param>
       /// <returns>Collection name</returns>
        public string GetNewsCollection(string localeIdentifier)
        {
            switch (localeIdentifier)
            {
                case "fi":
                    return ConfigurationManager.AppSettings.Get("newsCollection-fi");
                case "en":
                    return ConfigurationManager.AppSettings.Get("newsCollection-en");
                default:
                    return ConfigurationManager.AppSettings.Get("newsCollection-en"); ;
            }
        }


        /// <summary>
        /// Public method to get blog collection
        /// </summary>
        /// <param name="localeIdentifier"></param>
        /// <returns>string collection name</returns>
        public string GetBlogCollection(string localeIdentifier)
        {
            switch (localeIdentifier)
            {
                case "fi":
                    return ConfigurationManager.AppSettings.Get("BlogCollection-fi");
                case "en":
                    return ConfigurationManager.AppSettings.Get("BlogCollection-en");
                default:
                    return ConfigurationManager.AppSettings.Get("BlogCollection-en"); ;
            }
        }
    }
}
