using System;
using System.Collections.Generic;
using System.Text;

namespace aclara_meters
{
    public interface IPathService
    {
        string InternalFolder { get; }
        string PublicExternalFolder { get; }
        string PrivateExternalFolder { get; }
    }
}
