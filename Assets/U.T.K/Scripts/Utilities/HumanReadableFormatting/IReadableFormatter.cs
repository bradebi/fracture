using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UTK.Utilities
{
    public interface IReadableFormatter
    {
        /// <summary>
        /// This should return a byte-format form of the class 
        /// </summary>
        /// <returns></returns>
        ReadableEntry ToReadableFormat();

        /// <summary>
        /// This should internally construct the class from a set of bytes
        /// </summary>
        void FromReadableFormat(ReadableEntry data);
    }
}
