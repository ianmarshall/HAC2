using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HAC.Models;
using HAC.Models.Notifications;
using System.Threading;
using HAC.Controllers;
using HAC.Models.Repositories;

namespace HAC.Controllers
{


    public class CommentController : BaseController
    {

        [Authorize]
        public JsonResult Create(FormCollection formValues)
        {
            Comment c=new Comment();
            c.ID = System.Guid.NewGuid().ToString();
            c.Email = HttpContext.User.Identity.Name;

            if (TryUpdateModel(c, formValues))
            {
                Session["name"] = c.Author;
                c.TimeStamp = DateTime.Now;

                GalleryRepository rep = this.GetGalleryRepository();
                rep.AddComment(formValues["ImageVPath"], c);

                if (Configuration.GetConfiguration().Notifications.Enabled)
                {
                    try
                    {
                        NotificationConfig config = Configuration.GetConfiguration().Notifications;
                        string emailFrom = config.EmailFrom;
                        new Thread(() =>
                        {
                         //   PostmarkNotification notification = new PostmarkNotification(config);
                          //  notification.NotifyComment(emailFrom, c);

                        }).Start();
                    }
                    catch (Exception ex)
                    {
                     //  Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    }
                }

                return Json(new { status = "ok", Comments = rep.GetImageFromVPath(formValues["ImageVPath"]).Comments.OrderByDescending(com => com.TimeStamp) });
            }
            else {
              //  Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception("Error deleting comment: " + ModelState.Values));
                Response.StatusCode = 400;
                return Json(new {error=ModelState.Values});
            }
        }

        
        public JsonResult Latest(int top)
        {
            GalleryRepository rep = this.GetGalleryRepository();
            return Json(new { status = "ok", Comments = rep.GetLatestComments(top)}, JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        [HttpPost]
        public JsonResult Delete(FormCollection formValues)
        {
           GalleryRepository rep = this.GetGalleryRepository();
           string id = formValues["ID"];
           Comment c = rep.GetComment(id);           
           if (c == null)
           {
               Response.StatusCode = 400;
               return Json(new { error = "Comment " + id + " doesn't exist" });
           }
           else
           {
               if (!c.CanDeleteComment){
                   Response.StatusCode = 400;
                   return Json(new { error = "Permission denied" });
               }
               else
               {
                   rep.DeleteComment(c);
                   //return Json(new { status = "ok", Comments = rep.GetImageFromVPath(c.ImageVPath).Comments.OrderByDescending(com => com.TimeStamp) });
                   return Json(new { status = "ok" });
               }
           }
        }
    }
}
