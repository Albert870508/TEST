using Microsoft.AspNetCore.Mvc;

namespace TEST.Api.Route
{
    /// <summary>
    /// V1
    /// 路由规则
    /// </summary>
    public class RouteV1 : CustomRouteBase
    {
        /// <summary>
        /// V1
        /// 路由规则
        /// </summary>
        /// <param name="domain">领域 没有领域可以不写默认值为空</param>
        public RouteV1(string domain = "") : base(1, domain) { }
    }

    /// <summary>
    /// 自定义Router特性基类
    /// </summary>
    public class CustomRouteBase : RouteAttribute
    {
        /// <summary>
        /// 自定义Router特性基类
        /// </summary>
        /// <param name="v">版本</param>
        /// <param name="domain">领域 强制全是[controller]/[action]</param>
        public CustomRouteBase(int v, string domain) : base($"api/v{v.ToString()}/{domain}{(domain == "" ? "" : "/")}[controller]/[action]") { }
    }
}
