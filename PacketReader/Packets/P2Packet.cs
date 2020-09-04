// Create By: Oleg Gelezcov                        (olegg )
// Project: PacketReader     File: P2Packet.cs    Created at 2020/09/04/10:19 PM
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
    ///     Packet's data of P2
    /// </summary>
    public class P2Packet : IDataPacket
    {
        /// <summary>
        ///     Packet's bool value
        /// </summary>
        public bool BoolValue { get; private set; }

        /// <summary>
        ///     Packet's uint value
        /// </summary>
        public uint Value { get; private set; }

        /// <inheritdoc />
        public PacketTypeName TypeName => PacketTypeName.P2;

        /// <inheritdoc />
        public int Size => sizeof(uint) + 1;

        /// <inheritdoc />
        public async Task ReadAsync(Stream input, CancellationToken cancellationToken)
        {
            var i40Buffer = await input.ReadPacketAsync(Size, cancellationToken).ConfigureAwait(false);
            var uintBuffer = new byte[sizeof(uint)];
            Array.Copy(i40Buffer, uintBuffer, sizeof(uint));
            Value = BitConverter.ToUInt32(uintBuffer);
            BoolValue = i40Buffer[sizeof(uint)] != 0;
        }

        /// <inheritdoc />
        public byte[] ToBytes()
        {
            var uintBytes = BitConverter.GetBytes(Value);
            var result = new byte[uintBytes.Length + 2];
            result[0] = (byte) TypeName;
            for (var i = 1; i < 1 + uintBytes.Length; i++)
            {
                result[i] = uintBytes[i - 1];
            }

            result[^1] = (byte) (BoolValue ? 0x01 : 0x00);
            return result;
        }
    }
}