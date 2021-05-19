
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace EMPMGMT.Web.Infrastructure
{
    //public class SubdomainRoute : RouteBase
    //{

    //    public override RouteData GetRouteData(HttpContextBase httpContext)
    //    {
    //        var host = httpContext.Request.Url.Host;
    //        if (!string.IsNullOrEmpty(host))
    //        {
    //            if (host.IndexOf(":") > 0)
    //            {
    //                host = host.Substring(0, host.IndexOf(":"));
    //            }
    //        }
    //        else
    //        {
    //            host = httpContext.Request.Url.Host;
    //        }
    //        var index = host.IndexOf(".");
    //        string[] segments = httpContext.Request.Url.PathAndQuery.Split('/');

    //        if (index < 0)
    //            return null;

    //        var subdomain = host.Substring(0, index);
    //        string controller = "Employee";//(segments.Length > 0) ? segments[0] : "User";
    //        string action = "Login";//(segments.Length > 1) ? segments[1] : "Login";

    //        var routeData = new RouteData(this, new MvcRouteHandler());
    //        routeData.Values.Add("controller", controller); //Goes to the relevant Controller  class
    //        routeData.Values.Add("action", action); //Goes to the relevant action method on the specified Controller
    //        //routeData.Values.Add("subdomain", subdomain); //pass subdomain as argument to action method
    //        return routeData;
    //    }

    //    public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
    //    {
    //        //Implement your formating Url formating here
    //        return null;
    //    }
    //}
    public class DomainRoute : Route
    {
        private Regex domainRegex;
        private Regex pathRegex;

        public string Domain { get; set; }

        public DomainRoute(string domain, string url, RouteValueDictionary defaults)
            : base(url, defaults, new MvcRouteHandler())
        {
            Domain = domain;
        }

        public DomainRoute(string domain, string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
            Domain = domain;
        }

        public DomainRoute(string domain, string url, object defaults)
            : base(url, new RouteValueDictionary(defaults), new MvcRouteHandler())
        {
            Domain = domain;
        }

        public DomainRoute(string domain, string url, object defaults, IRouteHandler routeHandler)
            : base(url, new RouteValueDictionary(defaults), routeHandler)
        {
            Domain = domain;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            // Build regex
            domainRegex = CreateRegex(Domain);
            pathRegex = CreateRegex(Url);

            // Request information
            string requestDomain = httpContext.Request.Headers["host"];
            if (!string.IsNullOrEmpty(requestDomain))
            {
                if (requestDomain.IndexOf(":") > 0)
                {
                    requestDomain = requestDomain.Substring(0, requestDomain.IndexOf(":"));
                }
            }
            else
            {
                requestDomain = httpContext.Request.Url.Host;
            }
            string requestPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) + httpContext.Request.PathInfo;

            // Match domain and route
            Match domainMatch = domainRegex.Match(requestDomain);
            Match pathMatch = pathRegex.Match(requestPath);

            // Route data
            RouteData data = null;
            if (domainMatch.Success && pathMatch.Success && requestDomain.ToLower() != "tg.local" && requestDomain.ToLower() != "tg.terrasynq.net" && requestDomain.ToLower() != "www.townsgossip.com" && requestDomain.ToLower() != "townsgossip.com")
            {
                data = new RouteData(this, RouteHandler);

                // Add defaults first
                if (Defaults != null)
                {
                    foreach (KeyValuePair<string, object> item in Defaults)
                    {
                        data.Values[item.Key] = item.Value;
                    }
                }

                // Iterate matching domain groups
                for (int i = 1; i < domainMatch.Groups.Count; i++)
                {
                    Group group = domainMatch.Groups[i];
                    if (group.Success)
                    {
                        string key = domainRegex.GroupNameFromNumber(i);

                        if (!string.IsNullOrEmpty(key) && !char.IsNumber(key, 0))
                        {
                            if (!string.IsNullOrEmpty(group.Value))
                            {
                                data.Values[key] = group.Value;
                            }
                        }
                    }
                }

                // Iterate matching path groups
                for (int i = 1; i < pathMatch.Groups.Count; i++)
                {
                    Group group = pathMatch.Groups[i];
                    if (group.Success)
                    {
                        string key = pathRegex.GroupNameFromNumber(i);

                        if (!string.IsNullOrEmpty(key) && !char.IsNumber(key, 0))
                        {
                            if (!string.IsNullOrEmpty(group.Value))
                            {
                                data.Values[key] = group.Value;
                            }
                        }
                    }
                }
            }

            return data;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return base.GetVirtualPath(requestContext, RemoveDomainTokens(values));
        }

        public DomainData GetDomainData(RequestContext requestContext, RouteValueDictionary values)
        {
            // Build hostname
            string hostname = Domain;
            foreach (KeyValuePair<string, object> pair in values)
            {
                hostname = hostname.Replace("{" + pair.Key + "}", pair.Value.ToString());
            }

            // Return domain data
            return new DomainData
            {
                Protocol = "http",
                HostName = hostname,
                Fragment = ""
            };
        }

        private Regex CreateRegex(string source)
        {
            // Perform replacements
            source = source.Replace("/", @"\/?");
            source = source.Replace(".", @"\.?");
            source = source.Replace("-", @"\-?");
            source = source.Replace("{", @"(?<");
            source = source.Replace("}", @">([a-zA-Z0-9_\-]*))");

            return new Regex("^" + source + "$");
        }

        private RouteValueDictionary RemoveDomainTokens(RouteValueDictionary values)
        {
            Regex tokenRegex = new Regex(@"({[a-zA-Z0-9_\-]*})*\.?\/?({[a-zA-Z0-9_\-]*})*\.?\/?({[a-zA-Z0-9_\-]*})*\.?\/?({[a-zA-Z0-9_\-]*})*\.?\/?({[a-zA-Z0-9_\-]*})*\.?\/?({[a-zA-Z0-9_\-]*})*\.?\/?({[a-zA-Z0-9_\-]*})*\.?\/?({[a-zA-Z0-9_\-]*})*\.?\/?({[a-zA-Z0-9_\-]*})*\.?\/?({[a-zA-Z0-9_\-]*})*\.?\/?({[a-zA-Z0-9_\-]*})*\.?\/?({[a-zA-Z0-9_\-]*})*\.?\/?");
            Match tokenMatch = tokenRegex.Match(Domain);
            for (int i = 0; i < tokenMatch.Groups.Count; i++)
            {
                Group group = tokenMatch.Groups[i];
                if (group.Success)
                {
                    string key = group.Value.Replace("{", "").Replace("}", "");
                    if (values.ContainsKey(key))
                        values.Remove(key);
                }
            }

            return values;
        }
    }
    public class DomainData
    {
        public string Protocol { get; set; }
        public string HostName { get; set; }
        public string Fragment { get; set; }
    }
}