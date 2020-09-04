// Create By: Oleg Gelezcov                        (olegg )
// Project: PacketReader     File: P0Packet.cs    Created at 2020/09/04/10:18 PM
// All rights reserved, for personal using only
// 

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using PacketReader.Packets.Interfaces;

namespace PacketReader.Packets
{
    /// <summary>
    ///     Packet of data P0
    /// </summary>
    public class P0Packet : IDataPacket
    {
        /// <summary>
        ///     Packet's data value
        /// </summary>
        public short Value { get; private set; }

        /// <summary>
        ///     Packet type name
        /// </summary>
        public PacketTypeName TypeName => PacketTypeName.P0;

        /// <summary>
        ///     Packet data size
        /// </summary>
        public int Size => sizeof(short);


        /// <inheritdoc />
        public async Task ReadAsync(Stream input, CancellationToken cancellationToken)
        {
            var i16Buffer = await input.ReadPacketAsync(Size, cancellationToken).ConfigureAwait(false);
            Value = BitConverter.ToInt16(i16Buffer);
        }

        /// <inheritdoc />
        public byte[] ToBytes()
        {
            var valueBytes = BitConverter.GetBytes(Value);
            var result = new byte[valueBytes.Length + 1];
            result[0] = (byte) TypeName;
            for (var i = 1; i < result.Length; i++)
            {
                result[i] = valueBytes[i - 1];
            }

            return result;
        }
    }
}