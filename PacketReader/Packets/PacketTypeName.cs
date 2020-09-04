// Create By: Oleg Gelezcov                        (olegg )
// Project: PacketReader     File: PacketTypeName.cs    Created at 2020/09/04/10:15 PM
// All rights reserved, for personal using only
// 

namespace PacketReader.Packets
{
    /// <summary>
    ///     Packet type name
    /// </summary>
    public enum PacketTypeName : byte
    {
        /// <summary>
        ///     Type name for packets P0
        /// </summary>
        P0 = 0x00,

        /// <summary>
        ///     Type name for packets P1
        /// </summary>
        P1 = 0x01,

        /// <summary>
        ///     Type name for packets P2
        /// </summary>
        P2 = 0x02
    }
}