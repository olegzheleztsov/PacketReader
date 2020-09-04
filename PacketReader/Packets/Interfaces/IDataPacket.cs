// Create By: Oleg Gelezcov                        (olegg )
// Project: PacketReader     File: IDataPacket.cs    Created at 2020/09/04/10:17 PM
// All rights reserved, for personal using only
// 

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PacketReader.Packets.Interfaces
{
    /// <summary>
    ///     Implemented by data packets
    /// </summary>
    public interface IDataPacket : IStreamSerializable
    {
        /// <summary>
        ///     Data packet name
        /// </summary>
        PacketTypeName TypeName { get; }

        /// <summary>
        ///     Size of data packet in bytes
        /// </summary>
        int Size { get; }

        /// <summary>
        ///     Read data packet from stream asynchronously
        /// </summary>
        /// <param name="input">Input data stream</param>
        /// <param name="cancellationToken">Cancellation token to cancel operation</param>
        Task ReadAsync(Stream input, CancellationToken cancellationToken);
    }
}