// Create By: Oleg Gelezcov                        (olegg )
// Project: PacketReader     File: Connection.cs    Created at 2020/09/04/10:12 PM
// All rights reserved, for personal using only
// 

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using PacketReader.Packets.Interfaces;

namespace PacketReader
{
    /// <summary>
    ///     Connection class with purpose to read packets from the stream
    /// </summary>
    public class Connection
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Stream _stream;

        /// <summary>
        ///     Creates instance of the connection
        /// </summary>
        /// <param name="stream">Stream that is source of data</param>
        public Connection(Stream stream)
        {
            _stream = stream;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        ///     Synchronously read packet from the stream
        /// </summary>
        /// <typeparam name="T">Packet type</typeparam>
        /// <param name="timeoutMilliseconds">Timeout, when timeout will expire exception throws</param>
        /// <returns>Packet from stream</returns>
        public T Read<T>(int timeoutMilliseconds = 0) where T : IDataPacket, new()
        {
            var t = ReadAsync<T>(timeoutMilliseconds == 0
                ? TimeSpan.Zero
                : TimeSpan.FromMilliseconds(timeoutMilliseconds));
            return t.Result;
        }

        private async Task<T> ReadAsync<T>(TimeSpan timeout) where T : IDataPacket, new()
        {
            var cancellationToken = _cancellationTokenSource.Token;
            if (!await IsValidPacketTypeAsync(_stream, typeof(T), cancellationToken).ConfigureAwait(false))
            {
                throw new InvalidOperationException($"Expected packet : {typeof(T).GetPacketTypeName()}");
            }

            var packet = new T();
            ScheduleCancelling(timeout);
            await packet.ReadAsync(_stream, cancellationToken).ConfigureAwait(false);
            return packet;
        }

        private static async Task<bool> IsValidPacketTypeAsync(Stream stream, Type expectedType,
            CancellationToken cancellationToken)
        {
            var packetTypeBuffer = await stream.ReadPacketAsync(1, cancellationToken).ConfigureAwait(false);
            var realType = packetTypeBuffer[0].GetPacketType();
            return realType == expectedType;
        }

        private void ScheduleCancelling(TimeSpan timeout)
        {
            if (timeout == TimeSpan.Zero)
            {
                return;
            }

            _cancellationTokenSource.CancelAfter(timeout);
        }
    }
}