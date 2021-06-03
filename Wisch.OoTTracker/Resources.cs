using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace Wisch.OoTTracker
{
    /// <summary>
    /// This singleton caches and returns embedded images and icons from the assembly.
    /// </summary>
    class Resources
    {
        public static Resources Instance { get; } = new Resources();

        private readonly Assembly assembly;
        private readonly string[] resourceNames;

        private readonly Dictionary<string, Image> images = new Dictionary<string, Image>();

        private Resources()
        {
            assembly = Assembly.GetCallingAssembly();
            resourceNames = assembly.GetManifestResourceNames();
        }

        /// <summary>
        /// Finds the name of the resource by a name part.
        /// </summary>
        private string Find(string name)
        {
            return resourceNames.FirstOrDefault(x => x.Contains(name));
        }

        /// <summary>
        /// Returns an image from cache or loads an image into caches and returns it.
        /// </summary>
        public Image GetImage(string name)
        {
            if (!images.ContainsKey(name))
            {
                images[name] = LoadImage($"{name}.png");
            }
            return images[name];
        }

        /// <summary>
        /// Returns an icon from the assembly resources.
        /// </summary>
        public Icon GetIcon(string name)
        {
            return LoadIcon($"{name}.ico");
        }

        /// <summary>
        /// Loads an image from the assembly resources.
        /// </summary>
        private Image LoadImage(string name)
        {
            string resourceName = Find(name);

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                return Image.FromStream(stream);
            }
        }

        /// <summary>
        /// Loads an icon from the assembly resources.
        /// </summary>
        private Icon LoadIcon(string name)
        {
            string resourceName = Find(name);

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                return new Icon(stream);
            }
        }

        /// <summary>
        /// Indexer variant of <see cref="GetImage(string)"/> <br />
        /// Returns an image from cache or loads an image into caches and returns it.
        /// </summary>
        public Image this[string key]
        {
            get
            {
                return GetImage(key);
            }
        }
    }
}
