using System.Collections.Generic;
using UTK.Utilities;
using System.Xml;
namespace UTK.Utilities
{
    public static partial class Save
    {
        public static void ToXML(IReadableFormatter obj, string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("\t");

            using (XmlWriter writer = XmlWriter.Create(filename, settings))
            {
                writer.WriteStartDocument();

                ReadableEntry entry = obj.ToReadableFormat();

                WriteXMLEntry(entry, writer, System.IO.Path.GetFileNameWithoutExtension(filename));

                writer.WriteEndDocument();
            }
        }

        static void WriteXMLEntry(ReadableEntry entry, XmlWriter writer, string name, string nameSpace = "")
        {
            if (nameSpace != "")
            {
                writer.WriteStartElement(name, nameSpace);
            }
            else
            {
                writer.WriteStartElement(name);
            }

            foreach (KeyValuePair<string, List<string>> valueEntry in entry.Entries)
            {
                if (valueEntry.Value.Count == 1)
                {
                    writer.WriteElementString(valueEntry.Key, valueEntry.Value[0]);
                }
                else
                {
                    int memberIdx = 1;
                    foreach(string value in valueEntry.Value)
                    {
                        writer.WriteElementString(valueEntry.Key, memberIdx++.ToString(), value);
                    }
                }
            }

            if (entry.SubEntries.Count > 0)
            {
                foreach(KeyValuePair<string,List<ReadableEntry>> subEntry in entry.SubEntries)
                {
                    //Single subentry case
                    if (subEntry.Value.Count == 1)
                    {
                        if (subEntry.Value[0] != null)
                        {
                            WriteXMLEntry(subEntry.Value[0], writer, subEntry.Key);
                        }
                    }
                    //Multi-Subentry List
                    else
                    {
                        int memberIdx = 1;
                        foreach (ReadableEntry subentry in subEntry.Value)
                        {
                            //writer.WriteElementString(valueEntry.Key, memberIdx++.ToString(), value);
                            WriteXMLEntry(subentry, writer, subEntry.Key, memberIdx++.ToString());
                        }
                    }
                }
            }
            writer.WriteEndElement();
        }
    }
}
