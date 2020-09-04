// Create By: Oleg Gelezcov                        (olegg )
// Project: PacketReader     File: IStreamSerializable.cs    Created at 2020/09/05/12:16 AM
// All rights reserved, for personal using only
// 

namespace PacketReader.Packets.Interfaces
{
    /// <summary>
    ///     Implemented by classes which can't be serialized to byte array
    /// </summary>
    public interface IStreamSerializable
    {
        /// <summary>
        ///     Convert object to byte array
        /// </summary>
        /// <returns>Serialized array from the object</returns>
        byte[] ToBytes();
    }
}