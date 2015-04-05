using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Enterprise.Invoicing.Web
{

    public class ImageRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new ImageHandler(requestContext);
        }
    }

    public class ImageHandler : IHttpHandler
    {
        public ImageHandler(RequestContext context)
        {
            ProcessRequest(context);
        }

        private static void ProcessRequest(RequestContext requestContext)
        {
            var response = requestContext.HttpContext.Response;
            var request = requestContext.HttpContext.Request;
            var server = requestContext.HttpContext.Server;
            var validRequestFile = requestContext.RouteData.Values["filename"].ToString();
            const string invalidRequestFile = "thief.gif";
            var path = server.MapPath("~/graphics/");

            response.Clear();
            response.ContentType = GetContentType(request.Url.ToString());

            if (request.ServerVariables["HTTP_REFERER"] != null &&
                request.ServerVariables["HTTP_REFERER"].Contains("mikesdotnetting.com"))
            {
                response.TransmitFile(path + validRequestFile);
            }
            else
            {
                response.TransmitFile(path + invalidRequestFile);
            }
            response.End();
        }

        private static string GetContentType(string url)
        {
            switch (Path.GetExtension(url))
            {
                case ".gif":
                    return "Image/gif";
                case ".jpg":
                    return "Image/jpeg";
                case ".png":
                    return "Image/png";
                default:
                    break;
            }
            return null;
        }

        public void ProcessRequest(HttpContext context)
        {
        }

        public bool IsReusable
        {
            get { return false; }
        }
    } 
}