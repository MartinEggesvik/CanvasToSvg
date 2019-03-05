using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CanvasToSvg
{
    class Program
    {
        static void Main(string[] args)
        {

            XDocument srcTree = XDocument.Load("XMLToParse.xml");

            var paths = srcTree.Descendants().Where(n => n.Name.ToString().Equals("Path")).ToList();

            string svgPaths = "";
            XDocument SVG = new XDocument(new XElement("svg"));
            
            foreach (XElement path in paths)
            {
                var pathElement = new XElement("path");
                if (path.Attribute("Fill") != null)
                {
                    string color = path.Attribute("Fill").Value;
                    StringBuilder sb = new StringBuilder(color);
                    sb.Remove(1, 2);
                    pathElement.Add(new XAttribute("fill", sb.ToString()));
                }

                if (path.Attribute("Stroke") != null)
                {
                    string color = path.Attribute("Stroke").Value;
                    StringBuilder sb = new StringBuilder(color);
                    sb.Remove(1, 2);
                    pathElement.Add(new XAttribute("stroke", sb.ToString()));
                }

                if (path.Attribute("StrokeThickness") != null)
                {
                    string width = path.Attribute("StrokeThickness").Value;
                    pathElement.Add(new XAttribute("stroke-width", width));
                }

                if (path.Attribute("Data") != null)
                {
                    string data = path.Attribute("Data").Value;
                    StringBuilder sb = new StringBuilder(data);

                    while (true)
                    {
                        if(sb[0] == null || sb[0] == 'M' || sb[0] == 'm')
                        {
                            break;
                        }

                        sb.Remove(0,1);
                    }

                    pathElement.Add(new XAttribute("d", sb.ToString()));
                }



                SVG.Root.Add(pathElement);
            }
            Console.WriteLine(SVG.ToString());

            Console.ReadLine();
        }
    }
}
