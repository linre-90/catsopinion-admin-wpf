using System.Configuration;


namespace Catsopinion_admin.Logic
{
    class CollectionHandler
    {

        public string GetMessageCollection()
        {
            return ConfigurationManager.AppSettings.Get("messageCollection");
        }

        public string GetSeries()
        {
            return ConfigurationManager.AppSettings.Get("series");
        }

        public string GetLocales()
        {
            return ConfigurationManager.AppSettings.Get("locales");
        }

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
