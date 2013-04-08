using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class NotificationConfig
{
    public string EmailFrom { get; set; }
    public string PostmarkKey { get; set; }
    public bool Enabled { get; set; }
    public NotificationConfig()
    {
        Enabled = true;
        EmailFrom = "youremail@here.net";
        PostmarkKey = "your-api-key-from-postmark-here";
    }
}

/// <summary>
/// Summary description for Configuration
/// </summary>
public class Configuration : PGEntityBase
{
    public string RepositoryPhysicalPath { get; set; }
    public bool UseHardCacheInDisk { get; set; }
    public string CacheFolderPhysicalPath { get; set; }
    public string[] ValidImageExtensions { get; set; }
    public string[] IgnoreFolders { get; set; }
    public string[] AdminEmails { get; set; }
    public SortAction ImagesSortCriteria { get; set; }
    public SortAction FoldersSortCriteria { get; set; }
    public NotificationConfig Notifications { get; set; }
    public string AnalyticsCode { get; set; }
    public bool HidePrivateFolders { get; set; } //if true, there will not show. false: padlock icon and restricted access
    public string DefaultTheme { get; set; }
    public bool ReadBackWardsCompatibilityTxtFiles { get; set; } //reads txt files per image for backwards compatibility with previous version

    public static string ConfigurationFilePhysicalPath
    {
        get
        {
            return HttpContext.Current.Server.MapPath("~/ajaxphotogallery.config");
        }
    }

    public static string GetPicturesPhysicalPath
    {
        get
        {
            return HttpContext.Current.Server.MapPath(HttpRuntime.AppDomainAppVirtualPath) + "\\" + "Pictures";
        }
    }

      public static string  GetCacheFolderPhysicalPath
    {
        get
        {
            return HttpContext.Current.Server.MapPath(HttpRuntime.AppDomainAppVirtualPath) + "\\" + "Pictures" + "\\" + "DevCache";
        }
    }

    public static string GetNoImageFilePath
    {
        get
        {
            return HttpContext.Current.Server.MapPath(HttpRuntime.AppDomainAppVirtualPath) + "\\" +
                   "Content\\images" + "\\" + "noimage.jpg";
        }
    }


    private static Configuration _config;
    public static Configuration GetConfiguration()
    {
        if (_config == null)
        {
            _config = new Configuration();
            if (File.Exists(ConfigurationFilePhysicalPath))
            {
                _config = _config.ReadFromXmlCache<Configuration>(ConfigurationFilePhysicalPath);
            }
        }

        if (!File.Exists(ConfigurationFilePhysicalPath))
        {
            _config.WriteToXmlCache<Configuration>(ConfigurationFilePhysicalPath);
        }

        return _config;
    }


    //This is the default configuration. It only make sense then you delete the config file.
    //It will regenerate with the following values.
    public Configuration()
    {
        DefaultTheme = "bw";

        AdminEmails = new string[] { "yourdefaultadminemailhere@domain.net", "other@domain.net" };
        ValidImageExtensions = new string[] { ".gif", ".jpg", ".bmp", ".pnb", ".tiff" };
        IgnoreFolders = new string[] { ".svn", ".git" };

        RepositoryPhysicalPath = @"D:\Photos\Dev";

        UseHardCacheInDisk = true;
        CacheFolderPhysicalPath = @"D:\Photos\DevCache";

        ImagesSortCriteria = SortAction.NameASC;
        FoldersSortCriteria = SortAction.NameASC;
        AnalyticsCode = "UA-300651-XXX"; //your google analytics code here.
        Notifications = new NotificationConfig();

        ReadBackWardsCompatibilityTxtFiles = false;
    }
}
