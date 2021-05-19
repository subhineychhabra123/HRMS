using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace EMPMGMT.Utility
{
    public class ReadConfiguration
    {
        public static string SiteUrl;
        static ReadConfiguration()
        {
            SiteUrl = ConfigurationManager.AppSettings["SiteUrl"];
        }


        public static string EMailID
        {
            get
            {
                return ConfigurationManager.AppSettings["EMailID"];

            }
        }
        public static string EMailName
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailFromName"];

            }
        }
        public static string SMTPHostName
        {
            get
            {
                return ConfigurationManager.AppSettings["SMTPHostName"];

            }
        }
        public static bool SMTPEnableSSL
        {
            get
            {
                bool enableSSL = false;
                bool.TryParse(ConfigurationManager.AppSettings["EnableSSL"], out enableSSL);
                return enableSSL;
            }
        }
        public static string SMTPUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["SMTPUserName"];

            }
        }
        public static string SMTPPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["SMTPPassword"];

            }
        }
        public static int ProfileImageWidth
        {
            get
            {
                int width = 0;
                int.TryParse(ConfigurationManager.AppSettings["ProfileThumbnailResizeWidth"], out width);
                return width;
            }
        }
        public static int ProfileImageHieght
        {
            get
            {
                int height = 0;
                int.TryParse(ConfigurationManager.AppSettings["ProfileThumbnailResizeHeight"], out height);
                return height;
            }
        }
        public static int PageSize
        {
            get
            {
                int pageSize;
                int.TryParse(ConfigurationManager.AppSettings["PageSize"], out pageSize);
                return pageSize;

            }
        }





        public static int SubAdminProfileID
        {
            get
            {
                int profileId = 0;
                int.TryParse(ConfigurationManager.AppSettings["Sub-AdminProfileId"], out profileId);
                return profileId;
            }
        }

        public static string WebsiteUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["WebsiteUrl"];
            }
        }

        public static string WebsiteLogoPath
        {
            get
            {
                return ConfigurationManager.AppSettings["WebsiteLogoPath"];
            }
        }
        public static string MailTemplateFolder { get { return ConfigurationManager.AppSettings["MailTemplateFolder"];}}
        public static string WebsiteLoginURL { get { return ConfigurationManager.AppSettings["WebsiteLoginURL"]; } }
        
    }
}
