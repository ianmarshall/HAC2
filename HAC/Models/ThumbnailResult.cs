using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web.Caching;
using ImageResizer;

namespace MVC.Image.Resize.Helpers
{
    public class ThumbnailResult : ActionResult
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string ImageVPath { get; set; }
        public string CacheFilePath { get; set; }


        private void SetCache(HttpResponseBase response)
        {
            response.Cache.SetCacheability(HttpCacheability.Public);
            response.Cache.SetExpires(Cache.NoAbsoluteExpiration);
            response.Cache.SetLastModifiedFromFileDependencies();
            //Response.AppendHeader("Content-Length", imageBytes.Length.ToString());
            response.ContentType = "image/jpeg";

            DateTime dt = DateTime.Now.AddDays(10);
            response.Cache.SetMaxAge(new TimeSpan(dt.ToFileTime()));
            response.Cache.SetExpires(dt);
        }

        public ThumbnailResult(int Width, int Height, string physicalCacheFilePath, string vpath)
        {
            this.Width = Width;
            this.Height = Height;
            this.ImageVPath = vpath;
            this.CacheFilePath = physicalCacheFilePath;
        }


        public override void ExecuteResult(ControllerContext context)
        {
            if (string.IsNullOrEmpty(ImageVPath))
                throw new NullReferenceException("parameter Image cannot be null or empty");

            if (ImageVPath[0] == '/')
                ImageVPath = ImageVPath.Substring(1);

            string basePath = Configuration.GetPicturesPhysicalPath;
            string FilePath = basePath + "\\" + ImageVPath;

            if (!File.Exists(FilePath))
            {
                //  throw new FileNotFoundException("Image does not exist at " + FilePath);
                string noimageFilePath = Configuration.GetNoImageFilePath;
               
                Bitmap bitmap = new Bitmap(noimageFilePath);
                context.HttpContext.Response.ContentType = "image/gif";
                bitmap.Save(context.HttpContext.Response.OutputStream, ImageFormat.Jpeg);
                bitmap.Dispose();
                return;

            }
           
            //do we have cache for this file?
            if (!string.IsNullOrWhiteSpace(CacheFilePath) && File.Exists(CacheFilePath))
            {
                //devolvemos cache
                SetCache(context.HttpContext.Response);

                Bitmap bitmap = new Bitmap(CacheFilePath);
                context.HttpContext.Response.ContentType = "image/gif";
                bitmap.Save(context.HttpContext.Response.OutputStream, ImageFormat.Jpeg);
                bitmap.Dispose();
                return;
            }
            else
            {
                Bitmap bitmap = new Bitmap(FilePath);
                try
                {
                   
                    if (bitmap.Width < Width && bitmap.Height < Height)
                    {
                        context.HttpContext.Response.ContentType = "image/gif";
                        bitmap.Save(context.HttpContext.Response.OutputStream, ImageFormat.Jpeg);
                        bitmap.Dispose();
                        return;
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    bitmap.Dispose();
                }

                Bitmap FinalBitmap = null;

                try
                {
                    #region Conversion
                    bitmap = new Bitmap(FilePath);

                    int BitmapNewWidth;
                    decimal Ratio;
                    int BitmapNewHeight;

                    if (bitmap.Width > bitmap.Height)
                    {
                        Ratio = (decimal)Width / bitmap.Width;
                        BitmapNewWidth = Width;

                        decimal temp = bitmap.Height * Ratio;
                        BitmapNewHeight = (int)temp;
                    }
                    else
                    {
                        Ratio = (decimal)Height / bitmap.Height;
                        BitmapNewHeight = Height;
                        decimal temp = bitmap.Width * Ratio;
                        BitmapNewWidth = (int)temp;
                    }

                    FinalBitmap = new Bitmap(BitmapNewWidth, BitmapNewHeight);
                    Graphics graphics = Graphics.FromImage(FinalBitmap);
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.FillRectangle(Brushes.White, 0, 0, BitmapNewWidth, BitmapNewHeight);
                    graphics.DrawImage(bitmap, 0, 0, BitmapNewWidth, BitmapNewHeight);

                    context.HttpContext.Response.ContentType = "image/gif";

                    #endregion

                    FinalBitmap.Save(context.HttpContext.Response.OutputStream, ImageFormat.Jpeg);

                    //salvo cache
                    if (!string.IsNullOrWhiteSpace(CacheFilePath))
                        FinalBitmap.Save(CacheFilePath);

                }
                catch (Exception e)
                {
                    // Elmah.ErrorSignal.FromCurrentContext().Raise(e);
                }
                finally
                {
                    if (FinalBitmap != null)
                        FinalBitmap.Dispose();
                }
            }
        }
    }
}