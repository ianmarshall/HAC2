using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using PostmarkDotNet;
using System.Collections.Specialized;

namespace HAC.Models.Notifications
{
    public class PostmarkNotification : INotificationService
    {
        private NotificationConfig _config = null;
        
        public PostmarkNotification(NotificationConfig config) {
            _config = config;
        }

        public void NotifyComment(string emailTo, Comment c)
        {
            string body = "Hi there! <strong>\"" + c.Author  + "\"</strong> just posted a comment on the picture:";
            body += "<p><a href=\"" + c.MainPageVirtualUrl + "\">" + c.ImageVPath + "</a>";
            body += "on " + c.TimeStamp.ToLongDateString() + " at " + c.TimeStamp.ToLongTimeString() + "</p>";
            body += "<p>Email:" + c.Email+ "</p>";
            body += "<p>Comment:<pre>" + c.Body + "</pre></p>";

            //PostmarkMessage message = new PostmarkMessage
            //{
            //    From = _config.EmailFrom,
            //    To = emailTo,
            //    Subject = "New comment from " + c.Author,
            //    HtmlBody = body,
            //    TextBody = body,                
            //    Headers = new NameValueCollection { { "CUSTOM-HEADER", "value" } }
            //};

            //PostmarkClient client = new PostmarkClient(_config.PostmarkKey);

            //PostmarkResponse response = client.SendMessage(message);

            //if (response.Status != PostmarkStatus.Success)
            //{
            //    Console.WriteLine("Response was: " + response.Message);
            //}
        }
    }
}
