using System.Xml;
using System.Linq;

namespace UTK.Utilities
{
    public static partial class Load
    {
        public static void FromXML<T>(out T obj, string filePath)
        where T : IReadableFormatter, new ()
        {
            XmlReader reader = XmlReader.Create(filePath);

            reader.MoveToContent();
            // Parse the file and display each of the nodes.
            ReadableEntry currentEntry = new ReadableEntry();

            ReadFromEntry(reader, currentEntry,"Root");
            obj = new T();
            obj.FromReadableFormat(currentEntry);
        }

        static void ReadFromEntry(XmlReader reader, ReadableEntry currentEntryLevel, string currentEntryName, string nextEntry = "")
        {
            string elementName = nextEntry;
            string currentValue = "";



            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        //If a previous element does exist, then that suggests that we are in a subentry. Make a new subentry and parse.
                        if (elementName != "")
                        {
                            //Check to see how many entries there are

                            //Find if the namespace indicates that this is part of a subentry array

                            currentEntryLevel.AddEntryReadableEntry(elementName, new ReadableEntry());
                            ReadFromEntry(reader, currentEntryLevel.SubEntries[elementName].Last(), elementName, reader.Name);
                        }
                        elementName = reader.Name;
                        break;
                    case XmlNodeType.Text:
                        currentValue = reader.Value;
                        break;
                    case XmlNodeType.EndElement:
                        //This suggests that there was a value to be read between element tags. Add the entry now.
                        if(currentValue != "")
                        {
                            currentEntryLevel.AddEntrySingle(reader.Name, currentValue, true);
                        }
                        //If there was no value read and the end element name matches the start element, then it must signal the end of the entry
                        else if(reader.Name == currentEntryName)
                        {
                            return;
                        }
                        //Reset the current value and element name
                        currentValue = "";
                        elementName = "";
                        break;
                }
            }
        }
    }
}
