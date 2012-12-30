using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using HAC.Domain;
using HAC.Models;
using HAC.Controllers;

namespace HAC
{

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AutoMapAttribute : ActionFilterAttribute
    {
        public Type SourceType { get; private set; }
        public Type DestType { get; private set; }

        public AutoMapAttribute(Type sourceType, Type destType)
        {
            SourceType = sourceType;
            DestType = destType;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            var model = filterContext.Controller.ViewData.Model;

            var userM = new UserMapper();
            var viewModel = userM.Map(model, SourceType, DestType);

            filterContext.Controller.ViewData.Model = viewModel;
        }
    }
}