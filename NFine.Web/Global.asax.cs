using NFine.Code;
using System.Web.Mvc;
using System.Web.Routing;

namespace NFine.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 启动应用程序
        /// </summary>
        protected void Application_Start()
        {
            NFine.Application.AutoMapper.Configuration.Configure();//注册DTO映射(AutoMapper)
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //注册缓存依赖的服务代码：在vs命令行输入：aspnet_regsql -S . -U sa -P 123 -ed -d NFineBase -et -t Web_Content
            //-S localhost：数据库地址
            //-U sa：数据库登录名
            //-P 123456：数据库登录密码
            //-d wcfDemo：数据库的名称
            //-t user：表名称(小写)
        }
    }
}