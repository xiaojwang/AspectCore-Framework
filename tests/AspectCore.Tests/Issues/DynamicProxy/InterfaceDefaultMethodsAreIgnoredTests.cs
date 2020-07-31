using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using AspectCore.Tests.DynamicProxy;
using Xunit;

#if NETCOREAPP3_0
namespace AspectCore.Tests.Issues.DynamicProxy
{
    // https://github.com/dotnetcore/AspectCore-Framework/issues/223
    public class InterfaceDefaultMethodsAreIgnoredTests : DynamicProxyTestBase
    {
        public class Interceptor : AbstractInterceptorAttribute
        {
            public override async Task Invoke(AspectContext context, AspectDelegate next)
            {
                await context.Invoke(next);
                context.ReturnValue = "interceptor";
            }
        }

        public interface IService
        {
            [Interceptor]
            public string From()
            {
                return "interface";
            }
        }

        public class Service : IService
        {
            [Interceptor]
            public virtual string From()
            {
                return "class";
            }
        }

        [Fact]
        public void InterfaceProxy_Test()
        {
            var service = ProxyGenerator.CreateClassProxy<Service>();
            {
                var output = service.From();
                Assert.Equal("interceptor", output);
            }
            {
                var output = ((IService)service).From();
                Assert.Equal("interceptor", output);
            }
        }
    }
}
#endif
