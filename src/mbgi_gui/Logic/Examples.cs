using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace mbgi_gui.Logic
{
    public class Examples
    {
        /// <summary>
        /// Stores the examples
        /// </summary>
        private static List<ExampleItem> examples = new List<ExampleItem>();

        /// <summary>
        /// Loads the examples and returns the examples
        /// </summary>
        /// <returns>Return a list of examples</returns>
        public static List<ExampleItem> LoadExamples()
        {
            examples.Clear();
            examples.Add(
                new ExampleItem()
                {
                    Name = "example1",
                    Description = "Example 1 - Blocks Random to Output",
                    MbgiFile = LoadFromResource("mbgi_gui.Examples.example1.mbgi"),
                    CsFile = LoadFromResource("mbgi_gui.Examples.example1.user.txt")
                });

            examples.Add(
                new ExampleItem()
                {
                    Name = "example2",
                    Description = "Example 2 - Type Random to Output",
                    MbgiFile = LoadFromResource("mbgi_gui.Examples.example2.mbgi"),
                    CsFile = LoadFromResource("mbgi_gui.Examples.example2.user.txt")
                });

            examples.Add(
                new ExampleItem()
                {
                    Name = "Body Acceleration",
                    Description = "Accelerates a body",
                    MbgiFile = LoadFromResource("mbgi_gui.Examples.body_acceleration.mbgi"),
                    CsFile = LoadFromResource("mbgi_gui.Examples.body_acceleration.user.txt")
                });

            examples.Add(
                new ExampleItem()
                {
                    Name = "Body Braking",
                    Description = "Accelerates a body, like a vehicle, down to standstill",
                    MbgiFile = LoadFromResource("mbgi_gui.Examples.body_braking.mbgi"),
                    CsFile = LoadFromResource("mbgi_gui.Examples.body_braking.user.txt")
                });

            return examples;
        }

        public static string LoadFromResource(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = filename;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
