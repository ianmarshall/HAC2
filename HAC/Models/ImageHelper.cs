using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace MVC.Image.Resize.Helpers
{
    //public static class ImageHelper
    //{
    //    //0 - Image Source
    //    //1 - Alt Tag
    //    //2 - Styles
    //    //3 - Class
    //    const string ImageTag = "<img src=\"{0}\" alt=\"{1}\" {2} {3} />";

    //    public static string Thumbnail(this HtmlHelper Helper,
    //                     string Controllername,
    //                     string Action,
    //                     int Width,
    //                     int Height,
    //                     string File)
    //    {
    //        string Imagelocation = ThumbnailLocation(Helper,Controllername, Action, Width, Height, File);
            
    //        //WC3 - ALT tag always needed so when not set then set to the filename.
    //        return string.Format(ImageTag, Imagelocation, File, null, null);
    //    }

    //    public static string Thumbnail(this HtmlHelper Helper, 
    //                                    string Controllername, 
    //                                    string Action, 
    //                                    int Width, 
    //                                    int Height, 
    //                                    string File,
    //                                    string Alt)
    //    {
    //        string Imagelocation = ThumbnailLocation(Helper, Controllername, Action, Width, Height, File);

    //        return string.Format(ImageTag, Imagelocation, Alt, null, null);
    //    }

    //    public static string Thumbnail(this HtmlHelper Helper,
    //                            string Controllername,
    //                            string Action,
    //                            int Width,
    //                            int Height,
    //                            string File,
    //                            string Alt,
    //                            IDictionary<string, object> styleAttributes)
    //    {
    //        string Imagelocation = string.Format("/{0}/{1}/{2}/{3}/{4}", Controllername, Action, Width, Height, File);

    //        StringBuilder builder = new StringBuilder();
    //        string Class = string.Empty;

    //        if (styleAttributes != null && styleAttributes.Count > 0)
    //        {
    //            builder = new StringBuilder("style=\"");
    //            foreach (KeyValuePair<string, object> styleAttribute in styleAttributes)
    //            {
    //                if (styleAttribute.Key == "class")
    //                {
    //                    Class = string.Format("class=\"{0}\"", styleAttribute.Value);
    //                }
    //                else
    //                {
    //                    builder.Append(styleAttribute.Key + ":" + styleAttribute.Value + ";");                        
    //                }
    //            }
    //            builder.Append("\"");
    //        }
            
    //        return string.Format(ImageTag, Imagelocation, Alt, builder, Class);
    //    }

    //    public static string ThumbnailLocation(this HtmlHelper Helper,                                 
    //                            string Controllername,
    //                            string Action,
    //                            int Width,
    //                            int Height,
    //                            string File)
    //    {
    //        return string.Format("/{0}/{1}/{2}/{3}/{4}", Controllername, Action, Width, Height, File);
            
    //    }
    //}
}