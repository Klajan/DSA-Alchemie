using System.IO;
using System.Reflection;

namespace Alchemie.FileHandling
{
    class EmbeddedHandling
    {
        static public Stream GetEmbeddedRecourceStream(string resource)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);
        }
    }
}

