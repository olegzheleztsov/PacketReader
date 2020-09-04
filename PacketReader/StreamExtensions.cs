// Create By: Oleg Gelezcov                        (olegg )
// Project: PacketReader     File: StreamExtensions.cs    Created at 2020/09/04/10:14 PM
// All rights reserved, for personal using only
// 

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PacketReader
{
    /// <summary>
    /// Convenience methods for stream
    /// </summary>
    public static class StreamExtensions
    {
        private const int WAIT_INTERVAL = 50;

        /// <summary>
        /// Most important method, core of the reading procedure. So I don't use any specific stream class like
        /// NetworkStream from TCPClient, FileStream etc. As consequence I don't use any specific properties of these
        /// streams. There are not much possibilities remain, I ise polling procedure asking in loop for new data.
        /// If data available I read them until all data will be read. Operation can be cancelled by CancellationToken
        /// </summary>
        /// <param name="stream">Data source stream</param>
        /// <param name="packetSize">Packet size to read</param>
        /// <param name="cancellationToken">Cancellation token to terminate operation</param>
        /// <returns>Byte array which can be deserialized to packet object</returns>
        public static async Task<byte[]> ReadPacketAsync(this Stream stream, int packetSize,
            CancellationToken cancellationToken)
        {
            var result = new byte[packetSize];
            var totalRead = 0;
            while (totalRead < packetSize)
            {
                var nRead = await stream.ReadAsync(result, totalRead, packetSize - totalRead, cancellationToken)
                    .ConfigureAwait(false);
                if (nRead == 0)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(WAIT_INTERVAL), cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    totalRead += nRead;
                }
            }

            return result;
        }
    }
}