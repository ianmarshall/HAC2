using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using HAC.Models.Extensions;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using System.Web.Mvc;

/// <summary>
/// Summary description for Comment
/// </summary>
public class Comment
{
    //[XmlIgnoreAttribute]
    //[ScriptIgnore]
    //public PGImage ParentImage
    //{
    //    get;
    //    set;
    //}


    public string ID { get; set; }

    [Required(ErrorMessage = "*")]
    [StringLength(100, ErrorMessage = "Value too long")]
    public string Author {get;set;}
    
    [Required]
    [ScriptIgnore] //we don't want to expose this
    public string Email { get; set; }

    [Required (ErrorMessage="*")]
    [StringLength(300, ErrorMessage = "Value too long")]
    public string Body { get; set; }
    
    public DateTime TimeStamp{ get; set; }

    [XmlIgnoreAttribute]    
    [ScriptIgnore]
    public string ImageVPath{get;set; }
    
    public string ImageThumbnailFullUrl{ get {        
        string baseUrl = string.Format("{0}://{1}{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, HttpContext.Current.Request.ApplicationPath);
        return baseUrl.TrimEnd('/') + "/ThumbnailView/120/120?ImageVPath=" + HttpUtility.UrlEncode(ImageVPath);
    } }

    //says if current user can delete this comment
    public bool CanDeleteComment { get {
        return HttpContext.Current.User.IsInRole("Admin") || (HttpContext.Current.User.Identity.Name==Email && TimeStamp.AddMinutes(20)>DateTime.Now);
    } }

    #region Display helpers
    public string GravatarHash
    {
        get
        {
            return HAC.Models.Util.GetMD5(Email);
        }
    }

    public string MainPageVirtualUrl
    {
        get
        {
            return HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/Image?ImageVPath=" + HttpUtility.UrlEncode(ImageVPath);
        }
    }

    public string FriendlyTimeStamp
    {
        get
        {
            return TimeStamp.ToPrettyDate();
        }
    }
    #endregion
}
