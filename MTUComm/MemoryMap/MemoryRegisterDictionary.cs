using System;
using System.Collections.Generic;

using RegType = MTUComm.MemoryMap.MemoryMap.RegType;

namespace MTUComm.MemoryMap
{
    public class MemoryRegisterDictionary
    {
        Dictionary<RegType,dynamic> dictionary;

        /// <summary>
        /// Custom dictionary to store memory registers separated by type.
        /// <para>
        /// See <see cref="MemoryMap.RegType"/> for a list of available types.
        /// </para>
        /// </summary>
        public MemoryRegisterDictionary ()
        {
            this.dictionary = new Dictionary<RegType,dynamic> ();
            this.dictionary.Add ( RegType.INT,    new List<MemoryRegister<int   >> () );
            this.dictionary.Add ( RegType.UINT,   new List<MemoryRegister<uint  >> () );
            this.dictionary.Add ( RegType.ULONG,  new List<MemoryRegister<ulong >> () );
            this.dictionary.Add ( RegType.BOOL,   new List<MemoryRegister<bool  >> () );
            this.dictionary.Add ( RegType.CHAR,   new List<MemoryRegister<char  >> () );
            this.dictionary.Add ( RegType.STRING, new List<MemoryRegister<string>> () );
        }

        /// <summary>
        /// Add a new memory register to the dictionary.
        /// <para>
        /// See <see cref="MemoryMap.RegType"/> for a list of available types.
        /// </para>
        /// </summary>
        /// <param name="register">Instance of the memory register to add to the dictionary</param>
        /// <typeparam name="T">Type of the register</typeparam>
        public void AddElement<T> ( MemoryRegister<T> register )
        {
            switch ( Type.GetTypeCode ( typeof ( T ) ) )
            {
                case TypeCode.Int32  : this.dictionary[ RegType.INT    ].Add ( register ); break;
                case TypeCode.UInt32 : this.dictionary[ RegType.UINT   ].Add ( register ); break;
                case TypeCode.UInt64 : this.dictionary[ RegType.ULONG  ].Add ( register ); break;
                case TypeCode.Boolean: this.dictionary[ RegType.BOOL   ].Add ( register ); break;
                case TypeCode.Char   : this.dictionary[ RegType.CHAR   ].Add ( register ); break;
                case TypeCode.String : this.dictionary[ RegType.STRING ].Add ( register ); break;
            }

            Console.WriteLine ( "Add Modified Register: " + register.id );
        }

        /// <summary>
        /// Returns the memory registers of ( signed ) integer type stored in the dictionary.
        /// <para>
        /// See <see cref="MemoryMap.RegType"/>.INT used to create integer memory registers.
        /// </para>
        /// </summary>
        /// <returns>List of memory registers.</returns>
        public List<MemoryRegister<int>> GetElements_Int ()
        {
            return this.dictionary[ RegType.INT ];
        }

        /// <summary>
        /// Returns the memory registers of unsigned integer type stored in the dictionary.
        /// <para>
        /// See <see cref="MemoryMap.RegType"/>.UINT used to create integer memory registers.
        /// </para>
        /// </summary>
        /// <returns>List of memory registers.</returns>
        public List<MemoryRegister<uint>> GetElements_UInt ()
        {
            return this.dictionary[ RegType.UINT ];
        }

        /// <summary>
        /// Returns the memory registers of unsigned long type stored in the dictionary.
        /// <para>
        /// See <see cref="MemoryMap.RegType"/>.ULONG used to create integer memory registers.
        /// </para>
        /// </summary>
        /// <returns>List of memory registers.</returns>
        public List<MemoryRegister<ulong>> GetElements_ULong ()
        {
            return this.dictionary[ RegType.ULONG ];
        }

        /// <summary>
        /// Returns the memory registers of boolean type stored in the dictionary.
        /// <para>
        /// See <see cref="MemoryMap.RegType"/>.BOOL used to create integer memory registers.
        /// </para>
        /// </summary>
        /// <returns>List of memory register.</returns>
        public List<MemoryRegister<bool>> GetElements_Bool ()
        {
            return this.dictionary[ RegType.BOOL ];
        }

        /// <summary>
        /// Returns the memory registers of char type stored in the dictionary.
        /// <para>
        /// See <see cref="MemoryMap.RegType"/>.CHAR used to create char memory registers.
        /// </para>
        /// </summary>
        /// <returns>List of memory registers.</returns>
        public List<MemoryRegister<char>> GetElements_Char ()
        {
            return this.dictionary[ RegType.CHAR ];
        }

        /// <summary>
        /// Returns the memory registers of string/char array type stored in the dictionary.
        /// <para>
        /// See <see cref="MemoryMap.RegType"/>.STRING used to create integer memory registers.
        /// </para>
        /// </summary>
        /// <returns>List of memory registers.</returns>
        public List<MemoryRegister<string>> GetElements_String ()
        {
            return this.dictionary[ RegType.STRING ];
        }

        /// <summary>
        /// Returns all memory registers stored in the dictionary concatenated in a single list.
        /// <para>&#160;</para>
        /// <para>
        /// Elements order
        /// <list type="Order">
        /// <item>
        ///     <term>int</term>
        ///     <description>RegType.INT</description>
        /// </item>
        /// <item>
        ///     <term>uint</term>
        ///     <description>RegType.UINT</description>
        /// </item>
        /// <item>
        ///     <term>ulong</term>
        ///     <description>RegType.ULONG</description>
        /// </item>
        /// <item>
        ///     <term>bool</term>
        ///     <description>RegType.BOOL</description>
        /// </item>
        /// <item>
        ///     <term>char</term>
        ///     <description>RegType.CHAR</description>
        /// </item>
        /// <item>
        ///     <term>string</term>
        ///     <description>RegType.STRING</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        /// <returns>List of all memory registers.</returns>
        /// <seealso cref="MemoryMap.RegType"/>
        public List<dynamic> GetAllElements ()
        {
            List<dynamic> list = new List<dynamic> ();
            list.AddRange ( this.dictionary[ RegType.INT    ] );
            list.AddRange ( this.dictionary[ RegType.UINT   ] );
            list.AddRange ( this.dictionary[ RegType.ULONG  ] );
            list.AddRange ( this.dictionary[ RegType.BOOL   ] );
            list.AddRange ( this.dictionary[ RegType.CHAR   ] );
            list.AddRange ( this.dictionary[ RegType.STRING ] );

            return list;
        }
    }
}
