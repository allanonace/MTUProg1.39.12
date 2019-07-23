using System;

namespace Lexi
{
    public class LexiPackage
    {
        public readonly string Header;
        public readonly string Cmd;
        public readonly string StartAddress;
        public readonly string Checksum;
        public readonly byte[] CRC;

        public LexiPackage (
            string header,
            string cmd,
            string startAddress,
            string checksum,
            byte[] crc )
        {
            this.Header       = header;
            this.Cmd          = cmd;
            this.StartAddress = startAddress;
            this.Checksum     = checksum;
            this.CRC          = crc;
        }
    }
}
