using Microsoft.Extensions.Hosting;

namespace CakeSystem.Application.Utils
{
    public class RootPath
    {
        public string Path { get; }

        public RootPath(IHostEnvironment env)
            => Path = new DirectoryInfo(env.ContentRootPath).Parent.Parent.Parent.FullName;
    }
}
