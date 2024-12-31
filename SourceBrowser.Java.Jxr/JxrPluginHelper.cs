using System.Xml.Linq;

namespace SourceBrowser.Java.Jxr
{
    public class JxrPluginHelper
    {
        public static void AddPluginToPom(string pomFileDirectory)
        {
            Console.WriteLine($"Adding jxr plugin to pom inside: {pomFileDirectory}");

            string pomFilePath = FindFirstPomFile(pomFileDirectory);
            if (pomFilePath == null)
            {
                Console.WriteLine("No pom.xml file found in the directory.");
                return;
            }

            AddPluginToPom(pomFilePath, "org.apache.maven.plugins", "maven-jxr-plugin", "3.6.0");
        }

        private static string FindFirstPomFile(string directory)
        {
            foreach (string file in Directory.EnumerateFiles(directory, "pom.xml", SearchOption.AllDirectories))
            {
                return file;
            }
            return null;
        }

        internal static void AddPluginToPom(string pomFilePath, string groupId, string artifactId, string version)
        {
            if (!File.Exists(pomFilePath))
            {
                throw new FileNotFoundException($"The file {pomFilePath} does not exist.");
            }

            // Load the document
            XDocument doc = XDocument.Load(pomFilePath);

            // Get the default namespace
            XNamespace ns = doc.Root.GetDefaultNamespace();

            // Ensure the <build> element exists
            XElement buildElement = doc.Root.Element(ns + "build");
            if (buildElement == null)
            {
                buildElement = new XElement(ns + "build");
                doc.Root.Add(buildElement);
            }

            // Ensure the <plugins> element exists within <build>
            XElement pluginsElement = buildElement.Element(ns + "plugins");
            if (pluginsElement == null)
            {
                pluginsElement = new XElement(ns + "plugins");
                buildElement.Add(pluginsElement);
            }

            // Check if the plugin already exists
            bool pluginExists = pluginsElement.Elements(ns + "plugin").Any(p =>
                (string)p.Element(ns + "groupId") == groupId &&
                (string)p.Element(ns + "artifactId") == artifactId);

            if (pluginExists)
            {
                Console.WriteLine("The plugin already exists in the pom.xml.");
                return;
            }

            // Create the new plugin element
            XElement pluginElement = new XElement(ns + "plugin",
                new XElement(ns + "groupId", groupId),
                new XElement(ns + "artifactId", artifactId),
                new XElement(ns + "version", version)
            );

            // Add the plugin to the <plugins> element
            pluginsElement.Add(pluginElement);

            Console.WriteLine($"Added plugin to: {pomFilePath}");

            // Save the updated pom.xml
            doc.Save(pomFilePath);
        }
    }
}
