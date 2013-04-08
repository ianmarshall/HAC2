using System;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HAC.Models.POCO;
using System.Xml.Serialization;
using HAC.Models;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for Image
/// </summary>
public class PGImage : PGEntityBase
{

    #region Constructor
    public PGImage(string physicalPath)
    {
        PhysicalPath = physicalPath;
        Comments = new List<Comment>();
        ImageDescr = "";
    }
   
    #endregion

    [ScriptIgnore]
    public PGFolder ParentFolder
    {
        get;
        set;
    }

    public string FileName { get; set; }
    public string FriendlyName { get; set; }
    public string ImageDescr { get; set; }
    
    [ScriptIgnore]
    public string PhysicalPath { get; set; }

    public string FolderVPath {
        get {
            return ParentFolder.VPath;
        }
    }

    public string VPath { 
        get {
            string basePath = Configuration.GetPicturesPhysicalPath;
            return PhysicalPath.Replace(basePath, "").Replace("\\", "/");
        } 
    }

    [ScriptIgnore]
    public List<Comment> Comments { get; set; }
    public DateTime TimeStamp { get; set; }

    [ScriptIgnore]
    public EXIF EXIF = new EXIF();

    #region Helper properties
    public string ImageFullUrl
    {
        get
        {
            return Util.GetApplicationFullUrlWithoutLastSlash() + "/ImageView?ImageVPath=" + HttpUtility.UrlEncode(VPath);
        }
    }

    public string MainPageFullUrl
    {
        get
        {
            return Util.GetApplicationFullUrlWithoutLastSlash() + "/Image?ImageVPath=" + HttpUtility.UrlEncode(VPath);
        }
    }

    public string MainPageVirtualUrl
    {
        get
        {
            return HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/Image?ImageVPath=" + HttpUtility.UrlEncode(VPath);
        }
    }


    public string ImageThumbnailFullUrl
    {
        get
        {
            return Util.GetApplicationFullUrlWithoutLastSlash() + "/ThumbnailView/120/120?ImageVPath=" + HttpUtility.UrlEncode(VPath);
        }
    }
    #endregion

}
