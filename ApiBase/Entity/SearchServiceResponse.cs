using System.Collections.Generic;

namespace ApiBase.Entity
{
    public sealed class SearchServiceResponse
    {
        public string Method { get; set; }

        public string RelativePath { get; set; }

        public List<ServiceAttribute> ServiceAttributes { get; set; }
    }

    public sealed class ServiceAttribute
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }
    }
}
