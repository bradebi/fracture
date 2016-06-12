using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UTK.Utilities
{
    public static partial class Save
    {
        public static bool SaveToLocation(ISerializable data, string location, string fileNameAndExtension)
        {
            System.IO.Directory.CreateDirectory(location);

            using (System.IO.FileStream _FileStream = new System.IO.FileStream(location + "/" + fileNameAndExtension, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                try
                {
                    byte[] dataBytes = data.ToBytes();
                    _FileStream.Write(dataBytes, 0, dataBytes.Length);

                    _FileStream.Close();

                    return true;
                }
                catch (Exception _Exception)
                {
                    Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());

                    return false;
                }
            }
        }
    }
}
