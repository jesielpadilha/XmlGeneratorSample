using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace XmlSample
{
  public class XmlGeneratorSample
  {
    private static void CreateXml(string filePath, IList<Person> people, bool appendFile = false)
    {
      if (!appendFile)
      {
        var xmlSettings = new XmlWriterSettings
        {
          Indent = true,
          Encoding = Encoding.UTF8
        };
        using (var writer = XmlWriter.Create(filePath, xmlSettings))
        {
          writer.WriteStartDocument();
          writer.WriteStartElement("people");
          foreach (var person in people)
          {
            writer.WriteStartElement("person");
            writer.WriteElementString("Name", person.Name);
            writer.WriteElementString("Age", person.Age.ToString());
            writer.WriteEndElement();
          }
          writer.WriteEndElement();
          writer.Flush();
        };
      }
      else
      {
        var xDocument = XDocument.Load(filePath);
        if (xDocument == null) return;

        var root = xDocument.Element("people");
        if (root == null) return;

        foreach (var person in people)
        {
          XElement newRow = root.Descendants("person").Last();
          newRow.AddAfterSelf(
            new XElement("person",
              new XElement("Name", person.Name),
              new XElement("Age", person.Age.ToString())
            )
          );
        }

        xDocument.Save(filePath);
      }
    }

    private static void CreateXmlStream(Stream stream, IList<Person> people)
    {
      var xmlSettings = new XmlWriterSettings
      {
        Indent = true,
        Encoding = Encoding.UTF8
      };
      using (var writer = XmlWriter.Create(stream, xmlSettings))
      {
        writer.WriteStartDocument();
        writer.WriteStartElement("people");
        foreach (var person in people)
        {
          writer.WriteStartElement("person");
          writer.WriteElementString("Name", person.Name);
          writer.WriteElementString("Age", person.Age.ToString());
          writer.WriteEndElement();
        }
        writer.WriteEndElement();
        writer.Flush();
      };
    }

    private static void CreateXml(string filePath, IList<Person> people)
    {
      var xmlSettings = new XmlWriterSettings
      {
        Indent = true,
        Encoding = Encoding.UTF8
      };
      using (var writer = XmlWriter.Create(filePath, xmlSettings))
      {
        writer.WriteStartDocument();
        writer.WriteStartElement("people");
        foreach (var person in people)
        {
          writer.WriteStartElement("person");
          writer.WriteElementString("Name", person.Name);
          writer.WriteElementString("Age", person.Age.ToString());
          writer.WriteEndElement();
        }
        writer.WriteEndElement();
        writer.Flush();
      };
    }
  }
}