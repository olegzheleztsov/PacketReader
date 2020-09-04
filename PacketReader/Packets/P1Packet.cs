// Create By: Oleg Gelezcov                        (olegg )
// Project: PacketReader     File: P1Packet.cs    Created at 2020/09/04/10:19 PM
// All rights reserved, for personal using only
// 

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using PacketReader.Packets.Interfaces;

namespace PacketReader.Packets
{
    /// <summary>
    ///     Packet data of P1
    /// </summary>
    public class P1Packet : IDataPacket
    {
        /// <summary>
        ///     Packet's value
        /// </summary>
        public byte Value { get; private set; }

        /// <inheritdoc />
        public PacketTypeName TypeName => PacketTypeName.P1;

        /// <inheritdoc />
        public int Size => sizeof(byte);

        /// <inheritdoc />
        public async Task ReadAsync(Stream input, CancellationToken cancellationToken)
        {
            var i8Buffer = await input.ReadPacketAsync(Size, cancellationToken).ConfigureAwait(false);
            Value = i8Buffer[0];
        }

        /// <inheritdoc />
        public byte[] ToBytes()
        {
            return new[] {(byte) TypeName, Value};
        }
    }
}