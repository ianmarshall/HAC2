using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAC.Models.Notifications
{
    interface INotificationService
    {
        void NotifyComment(string emailTo, Comment c);
    }
}
