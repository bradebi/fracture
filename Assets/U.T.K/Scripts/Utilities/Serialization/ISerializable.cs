using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UTK.Utilities
{
    /// <summary>
    /// Implements the global class for serializing and deserializing data
    /// Requires that a ToBytes method and a FromBytes method be implemented to 
    /// handle conversion to and from bytes.
    /// Note: Naturally, serialization and deserialization MUST happen in the same order.
    /// </summary>
    public interface ISerializable
    {
        /// <summary>
        /// This should return a byte-format form of the class 
        /// </summary>
        /// <returns></returns>
        byte [] ToBytes();

        /// <summary>
        /// This should internally construct the class from a set of bytes
        /// </summary>
        void FromBytes( byte [] data);
    }
}