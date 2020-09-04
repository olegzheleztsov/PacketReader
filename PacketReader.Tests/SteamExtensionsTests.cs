using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PacketReader.Tests
{
    public class StreamExtensionsTests
    {
        [Theory]
        [InlineData(new byte[] {0x00, 0x10, 0x20})]
        [InlineData(new byte[] {0x01, 0x44})]
        [InlineData(new byte[] {0x02, 0x0f, 0x0b, 0x24, 0x21, 0x01})]
        public async Task Should_Read_Stream_With_Available_Data_Normally(byte[] inputBytes)
        {
            //Arrange
            await using var stream = new MemoryStream(inputBytes);
            await stream.FlushAsync().ConfigureAwait(false);
            stream.Position = 0;

            //Act
            var result = await stream.ReadPacketAsync(inputBytes.Length, CancellationToken.None).ConfigureAwait(false);

            //Assert
            Assert.Equal(inputBytes, result);
        }

        [Theory]
        [InlineData(new byte[] {0x00, 0x10, 0x20})]
        [InlineData(new byte[] {0x01, 0x44})]
        [InlineData(new byte[] {0x02, 0x0f, 0x0b, 0x24, 0x21, 0x01})]
        public async Task Should_Read_Stream_Normally_If_Data_Gets_By_Parts(byte[] inputBytes)
        {
            //Arrange
            var stream = new EmulateNetworkBehaviorStream();

            _ = Task.Run(async () =>
            {
                foreach (var inputByte in inputBytes)
                {
                    await stream.WriteAsync(new[] {inputByte}, CancellationToken.None).ConfigureAwait(false);
                    await stream.FlushAsync(CancellationToken.None).ConfigureAwait(false);
                    await Task.Delay(TimeSpan.FromMilliseconds(200)).ConfigureAwait(false);
                }
            });


            //Act
            var result = await stream.ReadPacketAsync(inputBytes.Length, CancellationToken.None).ConfigureAwait(false);

            //Assert
            Assert.Equal(inputBytes, result);
        }

        [Fact]
        public async Task Should_Throw_Exception_When_Cancelling_Operation()
        {
            //Arrange
            var stream = new MemoryStream(new byte[] {1, 2, 3});
            await stream.FlushAsync().ConfigureAwait(false);
            var cancellationTokenSource = new CancellationTokenSource();

            //Act
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(2));

            //Assert
            await Assert
                .ThrowsAsync<TaskCanceledException>(() => stream.ReadPacketAsync(10, cancellationTokenSource.Token))
                .ConfigureAwait(false);
        }
    }
}