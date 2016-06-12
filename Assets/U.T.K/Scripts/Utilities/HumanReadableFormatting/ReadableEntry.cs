using System;
using System.Collections.Generic;

namespace UTK.Utilities
{
    public class ReadableEntry
    {
        /// <summary>
        /// Contains all readable entries in string format.
        /// </summary>
        Dictionary<string, List<string>> readableEntries = new Dictionary<string, List<string>>();

        /// <summary>
        /// Contains all subObjects. Or any entry that is also a readable entry.
        /// </summary>
        Dictionary<string, List<ReadableEntry>> readableObjEntries = new Dictionary<string, List<ReadableEntry>>();

        string _objectName;

        /// <summary>
        /// Returns the dictionary containing all of the subentries in this entry.
        /// </summary>
        public Dictionary<string, List<ReadableEntry>> SubEntries
        {
            get
            {
                return readableObjEntries;
            }
        }

        /// <summary>
        /// Returns the dictionary containing all standard XML values found at this entry level
        /// </summary>
        public Dictionary<string,List<string>> Entries
        {
            get
            {
                return readableEntries;
            }
        }

        /// <summary>
        /// Returns the name of this object.
        /// </summary>
        public string ObjectName
        {
            get
            {
                return _objectName;
            }
        }

        /// <summary>
        /// Default constructor necessary for parameterless constructor. 
        /// </summary>
        public ReadableEntry()
        {

        }

        /// <summary>
        /// Allows readable entry to be given a name
        /// </summary>
        /// <param name="objectName"></param>
        public ReadableEntry(string objectName)
        {
            _objectName = objectName;
        }

        /// <summary>
        /// Allows any type that inherits IReadableFormatter to be decomposed automatically
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddEntry<T>(string name, T value)
            where T : IReadableFormatter
        {
            if (typeof(T).IsEnum)
            {
                readableEntries.Add(name, new List<string>() { value.ToString() });
            }
            else
            {
                if (value != null)
                {
                    readableObjEntries.Add(name, new List<ReadableEntry>() { value.ToReadableFormat() });
                }
            }
        }

        /// <summary>
        /// Adds an entry 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddEntry(string name, int value)
        {
            AddEntrySingle(name, value);
        }

        public void AddEntry(string name, float value)
        {
            AddEntrySingle(name, value);
        }

        public void AddEntry(string name, double value)
        {
            AddEntrySingle(name, value);
        }

        public void AddEntry(string name, uint value)
        {
            AddEntrySingle(name, value);
        }

        public void AddEntry(string name, short value)
        {
            AddEntrySingle(name, value);
        }

        public void AddEntry(string name, byte value)
        {
            AddEntrySingle(name, value);
        }

        public void AddEntry(string name, string value)
        {
            AddEntrySingle(name, value);
        }

        public void AddEntry(string name, List<double> value)
        {
            AddEntryMulti(name, value);
        }

        public void AddEntry(string name, List<string> value)
        {
            AddEntryMulti(name, value);
        }

        public void AddEntry(string name, List<float> value)
        {
            AddEntryMulti(name, value);
        }

        public void AddEntry(string name, List<int> value)
        {
            AddEntryMulti(name, value);
        }

        public void AddEntry(string name, List<short> value)
        {
            AddEntryMulti(name, value);
        }

        public void AddEntry(string name, List<byte> value)
        {
            AddEntryMulti(name, value);
        }

        public void AddEntry(string name, List<uint> value)
        {
            AddEntryMulti(name, value);
        }

        /// <summary>
        /// Adds a single entry by first checking if the key exists in the dictionary. If stackable is enabled,
        /// then multiple values can be added to the same key. Otherwise, the key may only hold a single entry.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="isStackable"></param>
        internal void AddEntrySingle(string name, object value, bool isStackable = false)
        {
            if (!readableEntries.ContainsKey(name))
            {
                readableEntries.Add(name, new List<string>() { value.ToString() });
            }
            else if (isStackable)
            {
                readableEntries[name].Add(value.ToString());
            }
            else
            {
                throw new InvalidOperationException("Key already exists at this object's level, unable to place valid entry: " + name);
            }
        }

        internal void AddEntryReadableEntry(string name, ReadableEntry entry)
        {
            if (!readableEntries.ContainsKey(name))
            {
                readableObjEntries.Add(name, new List<ReadableEntry>() { entry });
            }
            else
            {
                readableObjEntries[name].Add(entry);
            }
        }

        private void AddEntryMulti<T>(string name, IEnumerable<T> value)
        {
            if(value == null)
            {
                return;
            }

            if (!readableEntries.ContainsKey(name))
            {
                List<string> entryList = new List<string>();
                readableEntries.Add(name, entryList);
                foreach (T obj in value)
                {
                    entryList.Add(obj.ToString());
                }
            }
        }

        public void GetEntry(string name, out int value)
        {
            int.TryParse(GetEntrySingle(name), out value);
        }

        public void GetEntry(string name, out float value)
        {
            float.TryParse(GetEntrySingle(name), out value);
        }

        public void GetEntry(string name, out double value)
        {
            double.TryParse(GetEntrySingle(name), out value);
        }

        public void GetEntry(string name, out uint value)
        {
            uint.TryParse(GetEntrySingle(name), out value);
        }

        public void GetEntry(string name, out short value)
        {
            short.TryParse(GetEntrySingle(name), out value);
        }

        public void GetEntry(string name, out byte value)
        {
            byte.TryParse(GetEntrySingle(name), out value);
        }

        public void GetEntry(string name, out string value)
        {
            value = GetEntrySingle(name);
        }

        public void GetEntry(string name, out List<int> valueList)
        {
            int intValue = 0;
            valueList = new List<int>();
            foreach (string value in GetEntryMulti(name))
            {
                int.TryParse(value, out intValue);
                valueList.Add(intValue);
            }
        }

        public void GetEntry(string name, out List<float> valueList)
        {
            float floatValue = 0;
            valueList = new List<float>();
            foreach (string value in GetEntryMulti(name))
            {
                float.TryParse(value, out floatValue);
                valueList.Add(floatValue);
            }
        }

        public void GetEntry(string name, out List<double> valueList)
        {
            double doubleValue = 0;
            valueList = new List<double>();
            foreach (string value in GetEntryMulti(name))
            {
                double.TryParse(value, out doubleValue);
                valueList.Add(doubleValue);
            }
        }

        public void GetEntry(string name, out List<uint> valueList)
        {
            uint uintValue = 0;
            valueList = new List<uint>();
            foreach (string value in GetEntryMulti(name))
            {
                uint.TryParse(value, out uintValue);
                valueList.Add(uintValue);
            }
        }

        public void GetEntry(string name, out List<short> valueList)
        {
            short shortValue = 0;
            valueList = new List<short>();
            foreach (string value in GetEntryMulti(name))
            {
                short.TryParse(value, out shortValue);
                valueList.Add(shortValue);
            }
        }

        public void GetEntry(string name, out List<byte> valueList)
        {
            byte byteValue = 0;
            valueList = new List<byte>();
            foreach (string value in GetEntryMulti(name))
            {
                byte.TryParse(value, out byteValue);
                valueList.Add(byteValue);
            }
        }

        public void GetEntry(string name, out List<string> valueList)
        {
            valueList = GetEntryMulti(name);
        }

        private string GetEntrySingle(string name)
        {
            if (readableEntries.ContainsKey(name))
            {
                return readableEntries[name][0];
            }
            else
            {
                return "";
            }
        }

        private List<string> GetEntryMulti(string name)
        {
            if (readableEntries.ContainsKey(name))
            {
                return readableEntries[name];
            }
            else
            {
                return new List<string>();
            }
        }

        public void GetEntryEnum<T>(string name, out T value)
            where T : struct, IConvertible
        {
            if (typeof(T).IsEnum)
            {
                value = (T)Enum.Parse(typeof(T), GetEntrySingle(name));
            }
            else
            {
                value = default(T);
            }
        }

        public void GetEntry<T>(string name, out T value)
            where T : IReadableFormatter, new()
        {
            if(!readableObjEntries.ContainsKey(name))
            {
                value = default(T);
            }
            else if (readableObjEntries[name] == null)
            {
                value = default(T);
            }
            else
            {
                ReadableEntry entry = readableObjEntries[name][0];
                value = new T();
                value.FromReadableFormat(entry);
            }
        }
    }
}