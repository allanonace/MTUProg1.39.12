using System;
using System.Collections.Generic;
using System.Text;

using System.Threading.Tasks;

namespace Lexi.Interfaces
{
    /// <summary>
    /// Interface implemented by <see cref="ble_library.BleSerial"/> to make ensure that
    /// the bluetooth communication implementation complies with the LExI protocol.
    /// </summary>
    public interface ISerial
    {
        /// <summary>
        /// Reads a number of characters from the input buffer and writes them into an array of characters at a given offset.
        /// </summary>
        /// <param name="buffer">Bytes to write</param>
        /// <param name="offset">Zero based byte offset</param>
        /// <param name="count">The maximum number of bytes to read</param>
        /// <returns>The number of bytes read.</returns>
        int Read(byte[] buffer, int offset, int count);

        /// <summary>
        /// Writes a specific number of characters to the serial port using data from a buffer.
        /// </summary>
        /// <param name="buffer">Bytes to write</param>
        /// <param name="offset">Zero based byte offset</param>
        /// <param name="count">Number of bytes to write</param>
        Task Write(byte[] buffer, int offset, int count);

        /// <summary>
        /// Closes the port connection, sets the <c>IsOpen</c> property to false, and disposes of the internal Stream object.
        /// </summary>
        void Close();

        /// <summary>
        /// Indicates if the connection status is open or closed.
        /// </summary>
        /// <returns><see langword="true"/> if connection status is open.</returns>
        Boolean IsOpen();

        /// <summary>
        /// Opens a new serial port connection.
        /// </summary>
        /// <remarks></remarks>
        void Open();

        /// <summary>
        /// Returns the number of bytes of data received stored in the buffer.
        /// </summary>
        int BytesReadCount();

        /// <summary>
        /// Buffer of bytes of data received.
        /// </summary>
        byte[] BytesRead ();

        Boolean isEcho();

        byte[] GetBatteryLevel ();
    }
}
