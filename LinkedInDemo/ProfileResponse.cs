using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LinkedInDemo
{
    //public class ProfileResponse
    //{
    //    [XmlRoot(ElementName = "site-standard-profile-request")]
    //    public class Sitestandardprofilerequest
    //    {
    //        [XmlElement(ElementName = "url")]
    //        public string Url { get; set; }
    //    }

    //    [XmlRoot(ElementName = "person")]
    //    public class Person
    //    {
    //        [XmlElement(ElementName = "id")]
    //        public string Id { get; set; }
    //        [XmlElement(ElementName = "first-name")]
    //        public string Firstname { get; set; }
    //        [XmlElement(ElementName = "email-address")]
    //        public string Email { get; set; }
    //        [XmlElement(ElementName = "last-name")]
    //        public string Lastname { get; set; }
    //        [XmlElement(ElementName = "headline")]
    //        public string Headline { get; set; }
    //        [XmlElement(ElementName = "public-profile-url")]
    //        public string ProfileLink { get; set; }
    //        [XmlElement(ElementName = "picture-url")]
    //        public string ProfileImage { get; set; }
    //        [XmlElement(ElementName = "site-standard-profile-request")]
    //        public Sitestandardprofilerequest Sitestandardprofilerequest { get; set; }
    //    }


    //    public class Localized
    //    {
    //        public string en_US { get; set; }
    //    }

    //    public class PreferredLocale
    //    {
    //        public string country { get; set; }
    //        public string language { get; set; }
    //    }

    //    public class LastName
    //    {
    //        public LastName()
    //        {
    //            localized = new Localized();
    //            preferredLocale = new PreferredLocale();
    //        }
    //        public Localized localized { get; set; }
    //        public PreferredLocale preferredLocale { get; set; }
    //    }

    //    public class Localized2
    //    {
    //        public string en_US { get; set; }
    //    }

    //    public class PreferredLocale2
    //    {
    //        public string country { get; set; }
    //        public string language { get; set; }
    //    }

    //    public class FirstName
    //    {
    //        public FirstName()
    //        {
    //            localized = new Localized2();
    //            preferredLocale = new PreferredLocale2();
    //        }
    //        public Localized2 localized { get; set; }
    //        public PreferredLocale2 preferredLocale { get; set; }
    //    }

    //    public class ProfilePicture
    //    {
    //        public string displayImage { get; set; }
    //    }


    //    public class Handle
    //    {
    //        public string emailAddress { get; set; }
    //    }

    //    public class Element
    //    {
    //        public Handle handle { get; set; }
    //        public string handlez { get; set; }
    //    }

    //    public class RootObject
    //    {
    //        public RootObject()
    //        {
    //            elements = new List<Element>();
    //            lastName = new LastName();
    //            firstName = new FirstName();
    //            profilePicture = new ProfilePicture();
    //        }
    //        public List<Element> elements { get; set; }
    //        public string localizedLastName { get; set; }
    //        public LastName lastName { get; set; }
    //        public FirstName firstName { get; set; }
    //        public ProfilePicture profilePicture { get; set; }
    //        public string id { get; set; }
    //        public string localizedFirstName { get; set; }
    //    }
    //}

    public class ProfileNew
    {
        public class Localized
        {
            public string en_US { get; set; }
        }

        public class PreferredLocale
        {
            public string country { get; set; }
            public string language { get; set; }
        }

        public class FirstName
        {
            public FirstName()
            {
                localized = new Localized();
                preferredLocale = new PreferredLocale();
            }
            public Localized localized { get; set; }
            public PreferredLocale preferredLocale { get; set; }
        }

        public class Localized2
        {
            public string en_US { get; set; }
        }

        public class PreferredLocale2
        {
            public string country { get; set; }
            public string language { get; set; }
        }

        public class LastName
        {
            public LastName()
            {
                localized = new Localized2();
                preferredLocale = new PreferredLocale2();
            }
            public Localized2 localized { get; set; }
            public PreferredLocale2 preferredLocale { get; set; }
        }

        public class Paging
        {
            public Paging()
            {
                links = new List<object>();
            }
            public int count { get; set; }
            public int start { get; set; }
            public List<object> links { get; set; }
        }

        public class StorageSize
        {
            public int width { get; set; }
            public int height { get; set; }
        }

        public class StorageAspectRatio
        {
            public double widthAspect { get; set; }
            public double heightAspect { get; set; }
            public string formatted { get; set; }
        }

        public class RawCodecSpec
        {
            public string name { get; set; }
            public string type { get; set; }
        }

        public class DisplaySize
        {
            public string uom { get; set; }
            public double width { get; set; }
            public double height { get; set; }
        }

        public class DisplayAspectRatio
        {
            public double widthAspect { get; set; }
            public double heightAspect { get; set; }
            public string formatted { get; set; }
        }

        public class ComLinkedinDigitalmediaMediaartifactStillImage
        {
            public StorageSize storageSize { get; set; }
            public StorageAspectRatio storageAspectRatio { get; set; }
            public string mediaType { get; set; }
            public RawCodecSpec rawCodecSpec { get; set; }
            public DisplaySize displaySize { get; set; }
            public DisplayAspectRatio displayAspectRatio { get; set; }
        }

        public class Data
        {
            [JsonProperty("com.linkedin.digitalmedia.mediaartifact.StillImage")]
            public ComLinkedinDigitalmediaMediaartifactStillImage comlinkedindigitalmediamediaartifactStillImage { get; set; }
        }

        public class Identifier
        {
            public string identifier { get; set; }
            public string file { get; set; }
            public int index { get; set; }
            public string mediaType { get; set; }
            public string identifierType { get; set; }
            public int identifierExpiresInSeconds { get; set; }
        }

        public class Element
        {
            public Element()
            {
                data = new Data();
                identifiers = new List<Identifier>();
            }
            public string artifact { get; set; }
            public string authorizationMethod { get; set; }
            public Data data { get; set; }
            public List<Identifier> identifiers { get; set; }
        }

        public class DisplayImage
        {
            public DisplayImage()
            {
                elements = new List<Element>();
            }
            public Paging paging { get; set; }
            public List<Element> elements { get; set; }
        }

        public class ProfilePicture
        {
            public ProfilePicture()
            {
                displayImage = new DisplayImage();
            }
            [JsonProperty("displayImage~")]
            public DisplayImage displayImage { get; set; }
        }

        public class RootObject
        {
            public RootObject()
            {
                firstName = new FirstName();
                lastName = new LastName();
                profilePicture = new ProfilePicture();
            }
            public List<Element> elements { get; set; }
            public FirstName firstName { get; set; }
            public LastName lastName { get; set; }
            public ProfilePicture profilePicture { get; set; }
            public string id { get; set; }
        }

        
        
    }
    public class FacebookModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public Picture picture { get; set; }
    }
    public class Data
    {
        public int height { get; set; }
        public bool is_silhouette { get; set; }
        public string url { get; set; }
        public int width { get; set; }
    }
    public class Picture
    {
        public Data data { get; set; }
    }

    public class EmailResponse
    {
        public class Handle
        {
            public string emailAddress { get; set; }
        }

        public class Element
        {
            public Element()
            {
                handle = new Handle();
            }
            [JsonProperty("handle~")]
            public Handle handle { get; set; }
        }

        public class RootObject
        {
            public RootObject()
            {
                elements = new List<Element>();
            }
            public List<Element> elements { get; set; }
        }
    }
}