using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HAC.Models
{
    public class Util
    {

        public static string GetApplicationFullUrlWithoutLastSlash() {
            string baseUrl = string.Format("{0}://{1}{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, HttpContext.Current.Request.ApplicationPath);
            return baseUrl.TrimEnd('/');
        }

        public static string GetMD5(string str)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }


        public static string StripLastSlashFromVirtualPath(string vpath)
        {
            if (string.IsNullOrWhiteSpace(vpath))
                throw new ArgumentException("null or empty 'vpath' argument");

            if (vpath.IndexOf("\\") > -1)
            {
                throw new ArgumentException(vpath + " is not a valida Virtual Path");
            }
            else
            {
                if (vpath[vpath.Length-1] == '/')
                    vpath = vpath.Substring(0, vpath.Length - 1);

                return vpath;
            }

        }
        public static string StripFirstSlashFromVirtualPath(string vpath){
            if (string.IsNullOrWhiteSpace(vpath))
                throw new ArgumentException("null or empty 'vpath' argument");

            if (vpath.IndexOf("\\")>-1){
                throw new ArgumentException( vpath + " is not a valida Virtual Path");
            }
            else{
                if (vpath[0] == '/')
                vpath = vpath.Substring(1);

                return vpath;
            }
        }

        public static bool IsSameVpath(string vpath1, string vpath2) {
            vpath1 = vpath1.Replace("//", "/");
            vpath1 = StripLastSlashFromVirtualPath(vpath1);
            vpath1 = StripFirstSlashFromVirtualPath(vpath1);

            vpath2 = vpath2.Replace("//", "/");                        
            vpath2 = StripLastSlashFromVirtualPath(vpath2);
            vpath2 = StripFirstSlashFromVirtualPath(vpath2);
            

            return (vpath1.ToLower() == vpath2.ToLower());
        }


        public static bool IsValidImage(string imagePath)
        {
            if (Configuration.GetConfiguration().ValidImageExtensions.Length == 0)
                return false;

            return (Configuration.GetConfiguration().ValidImageExtensions.Any(s => Path.GetExtension(imagePath).ToLower().IndexOf(s) > -1));
        }


        public static string CleanVPath(string vPath){
            if (string.IsNullOrWhiteSpace(vPath))
                throw new ArgumentException("null or empty 'vpath' argument");

            vPath = StripFirstSlashFromVirtualPath(vPath).Replace("/", "\\");

            return vPath;
        }


        public static string GetVirtualFolderPathFromImagePath(string ImageVPath)
        {
            if (string.IsNullOrWhiteSpace(ImageVPath))
                throw new ArgumentException("null or empty 'vpath' argument");

            string rootFolder = Path.GetDirectoryName(ImageVPath);
            rootFolder=rootFolder.Replace("\\","/");
            return StripFirstSlashFromVirtualPath(rootFolder);
        }
    }
}