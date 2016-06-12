using System;

namespace UTK.Utilities
{
    public static partial class Load
    {
        public static bool LoadFromLocation<T>(out T value, string location, string fileNameAndExtension)
            where T : ISerializable, new ()
        {
            try
            {
                byte[] dataBytes = System.IO.File.ReadAllBytes(location + fileNameAndExtension);

                value = new T();
                value.FromBytes(dataBytes);
                return true;
            }
            catch (Exception _Exception)
            {
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
                value = new T();
                return false;
            }
        }
    }
}