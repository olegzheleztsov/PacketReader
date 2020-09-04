// Create By: Oleg Gelezcov                        (olegg )
// Project: PacketReader.Tests     File: PacketTypeNameExtensionsTests.cs    Created at 2020/09/05/12:00 AM
// All rights reserved, for personal using only
// 

using System;
using System.Runtime.InteropServices;
using PacketReader.Packets;
using Xunit;

namespace PacketReader.Tests
{
    public class PacketTypeNameExtensionsTests
    {
        [Theory]
        [InlineData((byte) PacketTypeName.P0, typeof(P0Packet))]
        [InlineData((byte) PacketTypeName.P1, typeof(P1Packet))]
        [InlineData((byte) PacketTypeName.P2, typeof(P2Packet))]
        public void Should_Return_Valid_Packet_Types(byte typeByte, Type packetType)
        {
            Assert.Equal(packetType, typeByte.GetPacketType());
        }

        [Fact]
        public void Should_Throws_When_Invalid_Byte_Passed()
        {
            const byte val = 128;
            Assert.Throws<ArgumentException>(() => val.GetPacketType());
        }

        [Theory]
        [InlineData(typeof(P0Packet), PacketTypeName.P0)]
        [InlineData(typeof(P1Packet), PacketTypeName.P1)]
        [InlineData(typeof(P2Packet), PacketTypeName.P2)]
        public void Should_Return_Valid_Packet_Names_For_Packet_Types(Type packetType, PacketTypeName packetTypeName)
        {
            Assert.Equal(packetTypeName, packetType.GetPacketTypeName());
        }

        [Fact]
        public void Should_Throws_For_Invalid_Packet_Type()
        {
            Assert.Throws<InvalidOperationException>(() => typeof(string).GetPacketTypeName());
        }
    }
}