// Create By: Oleg Gelezcov                        (olegg )
// Project: PacketReader.Tests     File: EmulateNetworkBehaviorStream.cs    Created at 2020/09/04/11:29 PM
// All rights reserved, for personal using only
// 

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PacketReader.Tests
{
    public class EmulateNetworkBehaviorStream : MemoryStream
    {
        private static readonly object SyncRoot = new object();

        public long ReadPosition { get; set; }

        public long WritePosition { get; set; }


        /// <inheritdoc />
        public override ValueTask WriteAsync(ReadOnlyMemory<byte> source,
            CancellationToken cancellationToken = new CancellationToken())
        {
            lock (SyncRoot)
            {
                Position = WritePosition;
                var result = base.WriteAsync(source, cancellationToken);
                WritePosition = Position;
                return result;
            }
        }

        /// <inheritdoc />
        // ReSharper disable once TooManyArguments
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            lock (SyncRoot)
            {
                Position = ReadPosition;
                var result = base.ReadAsync(buffer, offset, count, cancellationToken);
                ReadPosition = Position;
                return result;
            }
        }
    }
}