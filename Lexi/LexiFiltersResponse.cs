using System;

namespace Lexi
{
    public class LexiFiltersResponse
    {
        public readonly LexiFilterResponse[] Entries;

        public int Count
        {
            get { return this.Entries.Length; }
        }

        public LexiFilterResponse this[ int index ]
        {
            get
            {
                if ( Entries.Length > index )
                    return Entries[ index ];
                return null;
            }
        }

        public LexiFiltersResponse (
            ( int responseBytes, int indexByte, byte value )[] data )
        {
            int count = data.Length;

            this.Entries = new LexiFilterResponse[ count ];
            for ( int i = 0; i < count; i++ )
            {
                var entryData = data[ i ];
                this.Entries[ i ] = new LexiFilterResponse (
                    entryData.responseBytes, entryData.indexByte, entryData.value );
            }
        }
    }
}
