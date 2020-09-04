// Create By: Oleg Gelezcov                        (olegg )
// Project: PacketReader     File: PacketTypeNameExtensions.cs    Created at 2020/09/04/10:13 PM
// All rights reserved, for personal using only
// 

using System;
using System.Collections.Generic;
using System.Linq;
using PacketReader.Packets;

namespace PacketReader
{
    /// <summary>
    ///     Convenience extensions methods for packet type name conversions
    /// </summary>
    public static class PacketTypeNameExtensions
    {
        private static readonly IDictionary<PacketTypeName, Type> PacketTypeMap = new Dictionary<PacketTypeName, Type>
        {
            [PacketTypeName.P0] = typeof(P0Packet),
            [PacketTypeName.P1] = typeof(P1Packet),
            [PacketTypeName.P2] = typeof(P2Packet)
        };

        /// <summary>
        ///     Converts byte value to concrete packet type
        /// </summary>
        /// <param name="val">Byte value which represents packet type</param>
        /// <returns>Actual packet type</returns>
        public static Type GetPacketType(this byte val)
        {
            return PacketTypeMap.ContainsKey((PacketTypeName) val)
                ? PacketTypeMap[(PacketTypeName) val]
                : throw new ArgumentException(nameof(val));
        }

        /// <summary>
        ///     Convert type to packet type name
        /// </summary>
        /// <param name="type">Concrete packet type</param>
        /// <returns>Packet type name for concrete packet type</returns>
        public static PacketTypeName GetPacketTypeName(this Type type)
        {
            return PacketTypeMap.Where(pair => pair.Value == type).Select(pair => pair.Key).First();
        }
    }
}