// Create By: Oleg Gelezcov                        (olegg )
// Project: PacketReader.Tests     File: ConnectionTests.cs    Created at 2020/09/05/12:13 AM
// All rights reserved, for personal using only
// 

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using PacketReader.Packets;
using Xunit;

namespace PacketReader.Tests
{
    public class ConnectionTests
    {
        [Fact]
        public void Should_Read_Full_Data_Normally()
        {
            var p0Bytes = new byte[] {0x00, 0x10, 0x20};
            var p1Bytes = new byte[] {0x01, 0x44};
            var p2Bytes = new byte[] {0x02, 0x0f, 0x0b, 0x24, 0x21, 0x01};
            var inputBytes = new List<byte>();
            inputBytes.AddRange(p0Bytes);
            inputBytes.AddRange(p1Bytes);
            inputBytes.AddRange(p2Bytes);
            using var stream = new MemoryStream(inputBytes.ToArray());
            stream.Flush();

            var connection = new Connection(stream);
            var p0Packet = connection.Read<P0Packet>();
            var p1Packet = connection.Read<P1Packet>();
            var p2Packet = connection.Read<P2Packet>();
            Assert.Equal(p0Bytes, p0Packet.ToBytes());
            Assert.Equal(p1Bytes, p1Packet.ToBytes());
            Assert.Equal(p2Bytes, p2Packet.ToBytes());
        }

        [Fact]
        public async Task Should_Read_Packets_Normally_When_Data_Comes_With_A_Time()
        {
            var part0 = new byte[] { 0x00, 0x10, };
            var part1 = new byte[] { 0x20,  0x01, 0x44 };
            var part2 = new byte[] { 0x02, 0x0f, 0x0b };
            var part3 = new byte[] {0x24, 0x21};
            var part4 = new byte[] {0x01};
            var stream = new EmulateNetworkBehaviorStream();
            var connection = new Connection(stream);

            _ = Task.Run(async () =>
            {
                await stream.WriteAsync(part0, CancellationToken.None).ConfigureAwait(false);
                await stream.FlushAsync().ConfigureAwait(false);
                await Task.Delay(TimeSpan.FromMilliseconds(150)).ConfigureAwait(false);

                await stream.WriteAsync(part1, CancellationToken.None).ConfigureAwait(false);
                await stream.FlushAsync().ConfigureAwait(false);
                await Task.Delay(TimeSpan.FromMilliseconds(250)).ConfigureAwait(false);

                await stream.WriteAsync(part2, CancellationToken.None).ConfigureAwait(false);
                await stream.FlushAsync().ConfigureAwait(false);
                await Task.Delay(TimeSpan.FromMilliseconds(10)).ConfigureAwait(false);

                await stream.WriteAsync(part3, CancellationToken.None).ConfigureAwait(false);
                await stream.FlushAsync().ConfigureAwait(false);
                await Task.Delay(TimeSpan.FromMilliseconds(200)).ConfigureAwait(false);

                await stream.WriteAsync(part4, CancellationToken.None).ConfigureAwait(false);
                await stream.FlushAsync().ConfigureAwait(false);
            });

            var p0Packet = connection.Read<P0Packet>();
            var p1Packet = connection.Read<P1Packet>();
            var p2Packet = connection.Read<P2Packet>();
            Assert.Equal(new byte[]{ 0x00, 0x10, 0x20 }, p0Packet.ToBytes());
            Assert.Equal(new byte[] { 0x01, 0x44 }, p1Packet.ToBytes());
            Assert.Equal(new byte[] { 0x02, 0x0f, 0x0b, 0x24, 0x21, 0x01 }, p2Packet.ToBytes());
        }

        [Fact]
        public void Should_Throws_When_Invalid_Data_Comes()
        {
            var unknownBytes = new byte[] { 0x0A, 0x10, 0x20 };
            using var stream = new MemoryStream(unknownBytes);
            stream.Flush();
            var connection = new Connection(stream);
            Assert.Throws<AggregateException>(() => connection.Read<P0Packet>());
        }

        [Fact]
        public void Should_Throws_When_Timeout_Expires()
        {
            var part0 = new byte[] { 0x00, 0x10};
            var part1 = new byte[] {0x20};
            var stream = new EmulateNetworkBehaviorStream();
            var connection = new Connection(stream);
            _ = Task.Run(async () =>
            {
                await stream.WriteAsync(part0, CancellationToken.None).ConfigureAwait(false);
                await stream.FlushAsync().ConfigureAwait(false);
                await Task.Delay(TimeSpan.FromMilliseconds(300)).ConfigureAwait(false);

                await stream.WriteAsync(part1, CancellationToken.None).ConfigureAwait(false);
                await stream.FlushAsync().ConfigureAwait(false);
                await Task.Delay(TimeSpan.FromMilliseconds(300)).ConfigureAwait(false);
            });
            Assert.Throws<AggregateException>(() => connection.Read<P0Packet>(150));
        }
    }
}