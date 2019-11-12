using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Library;
using Library.Exceptions;
using Xml;
using Xml.UnitTest;

/// <summary>
/// LOGIC:
/// - 1. In constructor loads current MTU family XML file and iterate over all registers
/// - 2. For each register creates an instance of the corresponding specialized RegisterObj type
/// - 3. Creates Get and Set methods for each register, leaving all parameters setted but value ( in set case )
///      CreateProperty_Get|Set_XXX methods create function/action delegates to simulate property, but logic
///      of new properties is inside GetXXXFromMem and SetXXXToMem methods, working with memory bytes[]
/// - 4. Pass Set and Get methods references to each object, that will be used on Value property inside objects ( registers.get|set_id )
/// - 5. Creates new ExpandoObject member with register name, asociating Value property to them ( registers.id )
/// - 6. Adds instances to registersObjs dictionary that will be filtered to recover only modified registers
/// - 7. Registers ExpandoObject 'PropertyChanged' event, inside AMemoryMap class in IMemoryMap.cs file, to avoid override
///      member when using asignment operator ( "=" / set ). The main problem working with ExpandoObject type, is that use
///      property get block or invoke a method works perfect, but assign ( set ) always overrides dynamic member. The trick
///      is to create dictionary copy, after have created all registers methods and properties, and use PropertyChanged event
///      to get new value assigned and restore dynamic member, put it pointing again to corresponding Value property
///      ----
///      dynamicObject.member = reference to property;
///      property = 2;
///      Utils.Print ( dynamicObject.member ); // 2 and member continues being reference to property
///      dynamicObject.member = 33;
///      Utils.Print ( dynamicObject.member ); // 33 but member now is only an integer, not previously referenced property
///      ----
/// - 8. New register properties simulated using ExpandoObject have to be used as normal class members
///      "memoryMap.registers.idProperty" to get value, and "memoryMap.registers.idProperty = value" to set
///      Never need to call directly to registers.get_idProperty nor registers.set_idProperty, because
///      Value property do it for us, invoking funcGet and funcSet methods
///      
/// HEADERS:
/// - Registers
///   - Get. public T RegisterId_Get|CustomId ( MemoryRegister<T> memoryRegister )
///   - Set. public T RegisterId_Get|CustomId ( MemoryRegister<T> memoryRegister, dynamic inputValue )
/// - Overloads
///   - Get. public T RegisterId_Get|CustomId ( MemoryOverload<T> memoryOverload, dynamic memoryRegisters ) -> ExpandoObject
///   - Get. public T RegisterId_Get|CustomId ( MemoryOverload<T> memoryOverload, dynamic[] memoryRegisters )
/// </summary>
namespace MTUComm.MemoryMap
{
    /// <summary>
    /// Representation of a memory map of an MTU with all the information required
    /// to be able to simulate all the memory registers of the physical memory of the MTU,
    /// in a human readable way.
    /// </summary>
    public partial class MemoryMap : AMemoryMap
    {
        #region Constants

        /// <summary>
        /// Available types to set the "Type" tag in the entries present in the XML memory map, that
        /// will be used to generate the <see cref="MemoryRegister{T}"/>s.
        /// <para>&#160;</para>
        /// </para>
        /// <list type="RegType">
        /// <item>
        ///     <term>RegType.INT</term>
        ///     <description>For ( signed ) integer values</description>
        /// </item>
        /// <item>
        ///     <term>RegType.UINT</term>
        ///     <description>For unsigned integer values</description>
        /// </item>
        /// <item>
        ///     <term>RegType.ULONG</term>
        ///     <description>For unsigned long values</description>
        /// </item>
        /// <item>
        ///     <term>RegType.BOOL</term>
        ///     <description>For boolean values, using only one bit of an specific byte</description>
        /// </item>
        /// <item>
        ///     <term>RegType.CHAR</term>
        ///     <description>For character values</description>
        /// </item>
        /// <item>
        ///     <term>RegType.STRING</term>
        ///     <description>For string/array of characters values</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        public enum RegType
        {
            INT,
            UINT,
            ULONG,
            BOOL,
            CHAR,
            STRING
        }

        private const string XML_PREFIX         = "family_";
        private const string XML_EXTENSION      = ".xml";

        private const string METHODS_GET_PREFIX = "get_";
        
        private const string METHODS_SET_PREFIX = "set_";
        private const string METHODS_GET_CUSTOM_PREFIX = "getCustom_";
        private const string METHODS_SET_CUSTOM_PREFIX = "setCustom_";
        private const string METHODS_GET_MAP_PREFIX    = METHODS_GET_PREFIX + "map_";
        private const string METHODS_GET_BYTE_PREFIX   = METHODS_GET_PREFIX + "byte_";
        private const string METHODS_SET_STRING_PREFIX = METHODS_SET_PREFIX + "string_";
        private const string METHODS_SET_BYTE_PREFIX   = METHODS_SET_PREFIX + "byte_";
        private const string METHODS_SET_MTU_PREFIX    = METHODS_SET_PREFIX + "mtu_";

        public  const string METHOD              = "method";
        public  const string METHOD_KEY          = METHOD + ":";
        public  const string METHOD_SUFIX_SET    = "_Set";
        public  const string METHOD_SUFIX_GET    = "_Get";
        
        private const string REGISTER_OP         = "_val_";
        private const string OVERLOAD_OP_SIGN    = "#";
        private const string OVERLOAD_OP         = "_" + OVERLOAD_OP_SIGN + "_";

        private const string HEX_PREFIX          = "0x";

        private const string EXCEP_SET_LIM_INT   = "Argument value is outside int limits";
        private const string EXCEP_SET_LIM_UINT  = "Argument value is outside uint limits";
        private const string EXCEP_SET_LIM_ULONG = "Argument value is outside ulong limits";
        public  const string EXCEP_SET_USED      = "The specified record has not been mapped";
        public  const string EXCEP_SET_READONLY  = "The specified record is readonly";
        public  const string EXCEP_OVE_READONLY  = "All overloads are readonly";
        //private const string EXCEP_REGI_METHOD   = "Custom register method '#' is not present in MTU family class";
        private const string EXCEP_OVER_METHOD   = "Custom overload method '#' is not present in MTU family class";

        #endregion

        public int Length
        {
            get { return this.memory.Length; }
        }
        
        public string Family
        {
            get { return this.family; }
        }

        #region Attributes

        public static bool isUnityTest { get; private set; }

        public  byte[] memory { private set; get; }
        private Dictionary<string,dynamic> registersObjs;
        private Lexi.Lexi lexi;
        private bool readFromMtuOnlyOnce;
        private string family;

        #endregion

        #region Initialization

        /// <summary>
        /// </para>
        /// Families of the MTUs
        /// <list type="Families">
        /// <item>
        ///     <term>31xx32xx</term>
        ///     <description>Memory map file name is 'family_31xx32xx.xml'</description>
        /// </item>
        /// <item>
        ///     <term>33xx</term>
        ///     <description>Memory map is name 'family_33xx.xml'</description>
        /// </item>
        /// <item>
        ///     <term>342x</term>
        ///     <description>Memory map is name 'family_342x.xml'</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="memory">The memory map can be initialized with a specific memory or empty</param>
        /// <param name="family">Family of the MTU to load the correct XML memory map</param>
        /// <param name="readFromMtuOnlyOnce">By default, to get a register value the physical
        /// memory of the MTU is read, but sometimes it is preferable
        /// to only read once and cache the data</param>
        /// <param name="pathUnityTest">Only for debug purposes</param>
        public MemoryMap ( byte[] memory, string family, bool readFromMtuOnlyOnce = false )
        {
            this.lexi   = Singleton.Get.Lexi;
            this.family = family;
            
            // The third argument is used because is impossible to know if a byte
            // in the memory argument was read from the MTU or is only an empty byte
            this.readFromMtuOnlyOnce = readFromMtuOnlyOnce;

            // Read MTU family XML and prepare setters and getters
            Configuration config     = Singleton.Get.Configuration;
            XmlSerializer serializer = new XmlSerializer ( typeof ( MemRegisterList ) );

            // Prepares the memory buffer with the length indicates in the interface
            if ( memory == null )
                memory = new byte[ config.Interfaces.GetMemoryLengthByFamily ( family ) ];
            this.memory = memory;

            this.registersObjs = new Dictionary<string, dynamic>();

            using ( TextReader reader = Utils.GetResourceStreamReader ( XML_PREFIX + family + XML_EXTENSION ) )
            {
                MemRegisterList list = Validations.DeserializeXml<MemRegisterList> ( reader );

                #region Registers

                if ( list.Registers != null )
                    foreach ( MemRegister xmlRegister in list.Registers )
                    {
                        try
                        {
                            // TEST: PARA PODER CAPTURAR LA EJECUCION EN UN REGISTRO CONCRETO
                            //if ( string.Equals ( xmlRegister.Id, "P1MeterId" ) )
                            //{
                            //    { }
                            //}
    
                            RegType type = ( RegType )Enum.Parse ( typeof( RegType ), xmlRegister.Type.ToUpper () );
                            Type SysType = typeof(System.Object);
    
                            switch ( type )
                            {
                                case RegType.INT   : SysType = typeof ( int    ); break;
                                case RegType.UINT  : SysType = typeof ( uint   ); break;
                                case RegType.ULONG : SysType = typeof ( ulong  ); break;
                                case RegType.BOOL  : SysType = typeof ( bool   ); break;
                                case RegType.CHAR  : SysType = typeof ( char   ); break;
                                case RegType.STRING: SysType = typeof ( string ); break;
                            }
    
                            // Creates an instance of the generic class
                            dynamic memoryRegister = Activator.CreateInstance ( typeof ( MemoryRegister<> )
                                .MakeGenericType ( SysType ),
                                    xmlRegister.Id,
                                    type,
                                    xmlRegister.Description,
                                    xmlRegister.Address,
                                    xmlRegister.Size,
                                    xmlRegister.SizeGet,
                                    xmlRegister.WriteAsBool,
                                    xmlRegister.Custom_Get,
                                    xmlRegister.Custom_Set );
    
                            // Is not possible to use SysType as Type to invoke generic
                            // method like CreateProperty_Get<T> and is necesary to use Reflection
                            //typeof( MemoryMap ).GetMethod("CreateProperty_Get").MakeGenericMethod(SysType).Invoke(null, new object[] { regObj });
                            //typeof( MemoryMap ).GetMethod("CreateProperty_Set").MakeGenericMethod(SysType).Invoke(null, new object[] { regObj });
                            this.CreateProperty_Get ( memoryRegister );
                            this.CreateProperty_Get_ByteArray ( memoryRegister );
                            this.CreateProperty_Set_ByteArray ( memoryRegister );
                            if ( xmlRegister.WriteAsBool )
                            {
                                this.CreateProperty_Set ( memoryRegister );
                                this.CreateProperty_Set_String ( memoryRegister );
                            }

                            // References to write/set and get methods
                            bool w = memoryRegister.write;
                            dynamic get  =                                               base.registers[ METHODS_GET_PREFIX        + memoryRegister.id ];
                            dynamic getC = ( memoryRegister.HasCustomMethod_Get      ) ? base.registers[ METHODS_GET_CUSTOM_PREFIX + memoryRegister.id ] : null;
                            dynamic getM =                                               base.registers[ METHODS_GET_MAP_PREFIX    + memoryRegister.id ];
                            dynamic getB =                                               base.registers[ METHODS_GET_BYTE_PREFIX   + memoryRegister.id ];
                            dynamic set  = ( w                                       ) ? base.registers[ METHODS_SET_PREFIX        + memoryRegister.id ] : null;
                            dynamic setC = ( w && memoryRegister.HasCustomMethod_Set ) ? base.registers[ METHODS_SET_CUSTOM_PREFIX + memoryRegister.id ] : null;
                            dynamic setS = ( w                                       ) ? base.registers[ METHODS_SET_STRING_PREFIX + memoryRegister.id ] : null;
                            dynamic setB =                                               base.registers[ METHODS_SET_BYTE_PREFIX   + memoryRegister.id ];
                            dynamic setM =                                               base.registers[ METHODS_SET_MTU_PREFIX    + memoryRegister.id ];
                            TypeCode tc  = Type.GetTypeCode(SysType.GetType());
                            switch (type)
                            {
                                case RegType.INT:
                                    memoryRegister.funcGet        = (Func<Task<int>>)get;
                                    memoryRegister.funcGetMap     = (Func<int>)getM;
                                    memoryRegister.funcSet        = (Action<int>)set;
                                    memoryRegister.funcGetCustom  = (Func<Task<int>>)getC;
                                    memoryRegister.funcGetFromMtu = (Func<Task<int>>)setM;
                                    break;
                                case RegType.UINT:
                                    memoryRegister.funcGet        = (Func<Task<uint>>)get;
                                    memoryRegister.funcGetMap     = (Func<uint>)getM;
                                    memoryRegister.funcSet        = (Action<uint>)set;
                                    memoryRegister.funcGetCustom  = (Func<Task<uint>>)getC;
                                    memoryRegister.funcGetFromMtu = (Func<Task<uint>>)setM;
                                    break;
                                case RegType.ULONG:
                                    memoryRegister.funcGet        = (Func<Task<ulong>>)get;
                                    memoryRegister.funcGetMap     = (Func<ulong>)getM;
                                    memoryRegister.funcSet        = (Action<ulong>)set;
                                    memoryRegister.funcGetCustom  = (Func<Task<ulong>>)getC;
                                    memoryRegister.funcGetFromMtu = (Func<Task<ulong>>)setM;
                                    break;
                                case RegType.BOOL:
                                    memoryRegister.funcGet        = (Func<Task<bool>>)get;
                                    memoryRegister.funcGetMap     = (Func<bool>)getM;
                                    memoryRegister.funcSet        = (Action<bool>)set;
                                    memoryRegister.funcGetCustom  = (Func<Task<bool>>)getC;
                                    memoryRegister.funcGetFromMtu = (Func<Task<bool>>)setM;
                                    break;
                                case RegType.CHAR:
                                    memoryRegister.funcGet        = (Func<Task<char>>)get;
                                    memoryRegister.funcGetMap     = (Func<char>)getM;
                                    memoryRegister.funcSet        = (Action<char>)set;
                                    memoryRegister.funcGetCustom  = (Func<Task<char>>)getC;
                                    memoryRegister.funcGetFromMtu = (Func<Task<char>>)setM;
                                    break;
                                case RegType.STRING:
                                    memoryRegister.funcGet        = (Func<Task<string>>)get;
                                    memoryRegister.funcGetMap     = (Func<string>)getM;
                                    memoryRegister.funcSet        = (Action<string>)set;
                                    memoryRegister.funcGetCustom  = (Func<Task<string>>)getC;
                                    memoryRegister.funcGetFromMtu = (Func<Task<string>>)setM;
                                    break;
                            }
                            
                            memoryRegister.funcSetCustom = (Func<dynamic,Task<dynamic>>)setC;
    
                            // All register have this three functions
                            memoryRegister.funcGetByteArray = (Func<bool,byte[]>)getB;
                            memoryRegister.funcSetString    = (Action<string>)setS;
                            memoryRegister.funcSetByteArray = (Action<byte[]>)setB;
    
                            // BAD: Reference to property itself
                            // OK : Reference to register object and use TrySet|GetMember methods
                            //      to override set and get logic, avoiding ExpandoObject problems
                            // NOTA: No se puede usar "base." porque parece invalidar el comportamiento dinamico
                            AddProperty ( memoryRegister );
    
                            // Add new object to collection where will be
                            // filtered to only recover modified registers
                            this.registersObjs.Add(xmlRegister.Id, memoryRegister);
                        }
                        catch ( Exception e )
                        {
                            Utils.PrintDeep("ERROR! " + xmlRegister.Id + " -> " + e.Message + " " + e.InnerException);
                            throw new MemoryMapParseXmlException ( xmlRegister.Id );
                        }
                    }

                #endregion

                #region Overloads

                if ( list.Overloads != null )
                    foreach ( MemOverload xmlOverload in list.Overloads )
                    {
                        try {

                        RegType type = ( RegType )Enum.Parse ( typeof( RegType ), xmlOverload.Type.ToUpper () );
                        Type SysType = typeof(System.Object);

                        switch ( type )
                        {
                            case RegType.INT   : SysType = typeof ( int    ); break;
                            case RegType.UINT  : SysType = typeof ( uint   ); break;
                            case RegType.ULONG : SysType = typeof ( ulong  ); break;
                            case RegType.BOOL  : SysType = typeof ( bool   ); break;
                            case RegType.CHAR  : SysType = typeof ( char   ); break;
                            case RegType.STRING: SysType = typeof ( string ); break;
                        }

                        // Creates an instance of the generic class
                        dynamic memoryOverload = Activator.CreateInstance(typeof(MemoryOverload<>).MakeGenericType(SysType),
                            xmlOverload.Id,
                            type,
                            xmlOverload.Description,
                            xmlOverload.Registers.Select ( o => o.Id ).ToArray (),
                            xmlOverload.CustomGet );

                        this.CreateOverload_Get ( memoryOverload );

                        dynamic get = base.registers[ METHODS_GET_PREFIX + memoryOverload.id ];
                        TypeCode tc = Type.GetTypeCode ( SysType.GetType() );
                        switch (type)
                        {
                            case RegType.INT   : memoryOverload.funcGet = (Func<Task<int>>   )get; break;
                            case RegType.UINT  : memoryOverload.funcGet = (Func<Task<uint>>  )get; break;
                            case RegType.ULONG : memoryOverload.funcGet = (Func<Task<ulong>> )get; break;
                            case RegType.BOOL  : memoryOverload.funcGet = (Func<Task<bool>>  )get; break;
                            case RegType.CHAR  : memoryOverload.funcGet = (Func<Task<char>>  )get; break;
                            case RegType.STRING: memoryOverload.funcGet = (Func<Task<string>>)get; break;
                        }

                        AddProperty ( memoryOverload );
                        

                        }
                        catch ( Exception e )
                        {
                            Utils.PrintDeep("ERROR! " + xmlOverload.Id + " -> " + e.Message + " " + e.InnerException);
                            throw new MemoryMapParseXmlException ( xmlOverload.Id );
                        }
                        
                    }

                #endregion
            }
        }

        #endregion

        #region Create Property Get

        // Custom header format: public T RegisterId_Get|CustomId ( MemoryRegister<T> MemoryRegister )
        private void CreateProperty_Get<T> ( MemoryRegister<T> memoryRegister )
        {
            #region Raw Get

            // Always only from memory map
            base.AddMethod ( METHODS_GET_MAP_PREFIX + memoryRegister.id,
                new Func<T>(() =>
                {
                    // Get value from local memory map ( not using/reading the MTU )
                    return this.PropertyGet_Logic ( memoryRegister );
                }));

            #endregion

            #region Normal Get

            // All register have normal get block
            base.AddMethod ( METHODS_GET_PREFIX + memoryRegister.id,
                new Func<Task<T>>( async () =>
                {
                    // The register has not be loaded yet from the MTU or is a
                    // readonly registers that in any moment could change their value
                    if ( ! this.readFromMtuOnlyOnce &&       // Always for not read yet or readonly registers
                         ( ! memoryRegister.readedFromMtu ||
                           ! memoryRegister.write ) ||
                         this.readFromMtuOnlyOnce &&
                         ! memoryRegister.readedFromMtu )    // Only the first time for not read yet registers
                        return await memoryRegister.funcGetFromMtu ();

                    // Get value from local memory map ( not using/reading the MTU )
                    T result = this.PropertyGet_Logic ( memoryRegister );

                    Utils.PrintDeep ( "Map -> Value cache: " + memoryRegister.id + " = " + result );

                    return result;
                }));

            #endregion

            #region Get ( and set ) From MTU

            // Read from the MTU and update local memory map previous to return the value
            base.AddMethod ( METHODS_SET_MTU_PREFIX + memoryRegister.id,
                new Func<Task<T>> ( async () =>
                {
                    Utils.PrintDeep ( "Map -> Read from MTU: " + memoryRegister.id +
                                        " | dir: " + memoryRegister.address +
                                        " | Size Get: " + memoryRegister.sizeGet +
                                        " | Size Set: " + memoryRegister.size +
                                        " | Bit: " + memoryRegister.bit +
                                        " -> " + ( ( ! memoryRegister.write ) ? "Readonly" : "Not loaded" ) );
                
                    // Read current value from the MTU
                    byte[] read = await lexi.Read ( ( uint )memoryRegister.address, ( uint )memoryRegister.sizeGet );
                    
                    memoryRegister.lastRead = read;
                    
                    Utils.PrintDeep ( "Map -> From MTU value: " + memoryRegister.id + " = " + Utils.ByteArrayToString ( read ) + " [ " + read + " ]" );
                    
                    // Convert byte array to desired format
                    object value = default ( T );
                    switch ( Type.GetTypeCode ( typeof( T ) ) )
                    {
                        case TypeCode.Int32  : value = ( object )this.GetIntFromMem_Logic    ( read ); break;
                        case TypeCode.UInt32 : value = ( object )this.GetUIntFromMem_Logic   ( read ); break;
                        case TypeCode.UInt64 : value = ( object )this.GetULongFromMem_Logic  ( read ); break;
                        case TypeCode.Boolean: value = ( object )this.GetBoolFromMem_Logic   ( read[ 0 ], memoryRegister.bit ); break;
                        case TypeCode.Char   : value = ( object )this.GetCharFromMem_Logic   ( read[ 0 ] ); break;
                        case TypeCode.String : value = ( object )this.GetStringFromMem_Logic ( read ); break;
                    }
                    
                    Utils.PrintDeep ( "Map -> Converted value: " + memoryRegister.id + " = " + value );
                    
                    // When sizeGet is different to size, no setting is performed, only recover
                    if ( memoryRegister.size == memoryRegister.sizeGet ||
                         memoryRegister.valueType == RegType.BOOL )
                    {
                        // Write converted value in the memory map
                        switch ( Type.GetTypeCode( typeof( T ) ) )
                        {
                            case TypeCode.Int32  : this.SetIntToMem   ( ( int   )( object )value, memoryRegister.address, memoryRegister.sizeGet ); break;
                            case TypeCode.UInt32 : this.SetUIntToMem  ( ( uint  )( object )value, memoryRegister.address, memoryRegister.sizeGet ); break;
                            case TypeCode.UInt64 : this.SetULongToMem ( ( ulong )( object )value, memoryRegister.address, memoryRegister.sizeGet ); break;
                            case TypeCode.Boolean: this.SetBoolToMem  ( ( bool  )( object )value, memoryRegister.address, memoryRegister.bit     ); break;
                            case TypeCode.Char   : this.SetCharToMem  ( ( char  )( object )value, memoryRegister.address ); break;
                            case TypeCode.String : this.SetStringToMem<String> ( ( string )( object )value, memoryRegister.address, memoryRegister.sizeGet ); break;
                        }
                        
                        value = this.PropertyGet_Logic ( memoryRegister );
                        
                        Utils.PrintDeep ( "Map -> Read from map: " + memoryRegister.id + " = " + value );
                    }
                    
                    // Set flag to know that at least the register read one time its value from the MTU
                    memoryRegister.readedFromMtu = true;
                    
                    return ( T )value;
                }));

            #endregion

            #region Custom Get that internally uses Normal Get

            // But only someone have special get block/method defined on MTU family classes
            if ( memoryRegister.HasCustomMethod_Get )
            {
                MethodInfo customMethod = this.GetType().GetMethod (
                    memoryRegister.methodId_Get,
                    new Type[] { typeof( MemoryRegister<T> ) } );

                // Method is not present in the MemoryMap_CustomMethods class
                if ( customMethod == null )
                {
                    Utils.Print ( "ERROR: Create Custom Get method " + memoryRegister.id );

                    throw new CustomMethodNotExistException ( memoryRegister.methodId_Get );
                }

                base.AddMethod ( METHODS_GET_CUSTOM_PREFIX + memoryRegister.id,
                    new Func<Task<T>>( async () =>
                    {
                        Task<T> result = ( Task<T> )customMethod.Invoke ( this, new object[] { memoryRegister } );
                        
                        return await result;
                    }));
            }

            #endregion
        }

        private T PropertyGet_Logic<T> ( MemoryRegister<T> memoryRegister )
        {
            object result = default ( T );
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Int32  : result = ( object )this.GetIntFromMem    ( memoryRegister.address, memoryRegister.sizeGet ); break;
                case TypeCode.UInt32 : result = ( object )this.GetUIntFromMem   ( memoryRegister.address, memoryRegister.sizeGet ); break;
                case TypeCode.UInt64 : result = ( object )this.GetULongFromMem  ( memoryRegister.address, memoryRegister.sizeGet ); break;
                case TypeCode.Boolean: result = ( object )this.GetBoolFromMem   ( memoryRegister.address, memoryRegister.bit     ); break;
                case TypeCode.Char   : result = ( object )this.GetCharFromMem   ( memoryRegister.address );                         break;
                case TypeCode.String : result = ( object )this.GetStringFromMem ( memoryRegister.address, memoryRegister.sizeGet ); break;
            }

            // Numeric field with operation to evaluate
            if ( memoryRegister.HasCustomOperation_Get )
                return this.ExecuteOperation<T> ( memoryRegister.mathExpression_Get, result );

            // Only return readed value
            return ( T )result;
        }

        // Custom header format 1: public T RegisterId_Get|CustomId ( MemoryOverload<T> MemoryOverload, dynamic MemoryRegisters ) -> ExpandoObject
        // Custom header format 2: public T RegisterId_Get|CustomId ( MemoryOverload<T> MemoryOverload, dynamic[] MemoryRegisters )
        private void CreateOverload_Get<T> ( MemoryOverload<T> memoryOverload )
        {
            bool useParamArray = false;
            MethodInfo customMethod = null;

            if ( memoryOverload.HasCustomMethod )
            {
                // First try to retrieve method with header ( MemoryOverload<T> MemoryOverload, dynamic MemoryRegisters )
                customMethod = this.GetType().GetMethod (
                                    memoryOverload.methodId,
                                    new Type[] { typeof ( MemoryOverload<T> ), typeof( ExpandoObject ) } );

                // If method not, try to retrieve method with header ( MemoryOverload<T> MemoryOverload, dynamic[] MemoryRegisters )
                if ( customMethod == null )
                {
                    useParamArray = true;
                    customMethod  = this.GetType().GetMethod (
                                        memoryOverload.methodId,
                                        new Type[] { typeof ( MemoryOverload<T> ), typeof( dynamic[] ) } );

                    // If both options are not present, thow an exception
                    if ( customMethod == null )
                    {
                        Utils.Print ( "ERROR: Create Custom Get method " + memoryOverload.id );

                        throw new CustomMethodNotExistException ( memoryOverload.methodId );
                    }
                }
            }

            dynamic registersToUse = new ExpandoObject ();
            IDictionary<string,dynamic> dictionary = registersToUse;
            dynamic[] registersToUseArray = new dynamic[ memoryOverload.registerIds.Length ];

            int i = 0;
            foreach ( string id in memoryOverload.registerIds )
            {
                dictionary[ id ] = base.registers[ id ];
                registersToUseArray[ i++ ] = dictionary[ id ];
            }
            
            // Overloads only have get block ( are readonly )
            base.AddMethod ( METHODS_GET_PREFIX + memoryOverload.id,
                new Func<Task<T>>( async () =>
                {
                    // Use custom method
                    if ( memoryOverload.HasCustomMethod )
                    {
                        Task<T> result = null;
                    
                        if ( ! useParamArray )
                             result = ( Task<T> )customMethod.Invoke ( this, new object[] { memoryOverload, registersToUse      } );
                        else result = ( Task<T> )customMethod.Invoke ( this, new object[] { memoryOverload, registersToUseArray } );
                        
                        return await result;
                    }

                    // Operation to evaluate
                    else
                    {
                        i = 0;
                        object[] values = new object[ dictionary.Count ];
                        foreach ( string id in dictionary.Keys )
                            values[ i++ ] = await base[ id ].GetValue ();

                        return this.ExecuteOperation<T> ( memoryOverload.custom_Get, values );
                    }
                }));
        }

        public void CreateProperty_Get_ByteArray<T> ( MemoryRegister<T> memoryRegister )
        {
            base.AddMethod ( METHODS_GET_BYTE_PREFIX + memoryRegister.id,
                new Func<bool,byte[]> ( ( bool useSizeGet ) =>
                {
                    return this.GetByteArrayFromMem ( memoryRegister.address, useSizeGet ? memoryRegister.sizeGet : memoryRegister.size );
                }));
        }

        #endregion

        #region Create Property Set

        // Using custom method we can't be assured of the type used for parameter, but yes about return value
        // Custom header format: public T RegisterId_Get|CustomId ( MemoryRegister<T> MemoryRegister, dynamic inputValue )
        public void CreateProperty_Set<T>( MemoryRegister<T> memoryRegister )
        {
            #region Normal Set

            base.AddMethod ( METHODS_SET_PREFIX + memoryRegister.id,
                new Action<T>((_value) =>
                {
                    this.PropertySet_Logic ( memoryRegister, _value );
                }));
                
            //Utils.Print ( "-> Set: " + memoryRegister.id + " - " + ( base.registers[ METHODS_SET_PREFIX + memoryRegister.id ] == null ) );

            #endregion

            #region Custom Set previous to Normal Set

            // But only someone have special set block/method defined on MTU family classes
            if ( memoryRegister.HasCustomMethod_Set )
            {
                MethodInfo customMethod = this.GetType().GetMethod (
                    memoryRegister.methodId_Set,
                    BindingFlags.Instance     |
                    BindingFlags.IgnoreReturn |
                    BindingFlags.NonPublic    |
                    BindingFlags.Public );
                    //new Type[] { typeof( MemoryRegister<T> ), typeof ( dynamic } );

                // Method is not present in MTU family class
                if ( customMethod == null )
                {
                    Utils.Print ( "ERROR: Create Custom Set method " + memoryRegister.id );

                    throw new CustomMethodNotExistException ( memoryRegister.methodId_Set );
                }

                base.AddMethod ( METHODS_SET_CUSTOM_PREFIX + memoryRegister.id,
                    new Func<dynamic,Task<dynamic>>( async (_value) =>
                    {
                        // This method allows to use concrete return value type ( ex. Task<bool> )
                        dynamic awaitable = customMethod.Invoke ( this, new object[] { memoryRegister, _value } );
                        await   awaitable;
                        return  awaitable.GetAwaiter ().GetResult ();
                    
                        // This method requires that references methods use Task<dynamic> as return value type
                        //Task<dynamic> result = ( Task<dynamic> )customMethod.Invoke ( this, new object[] { memoryRegister, _value } );
                        //return await result;
                    }));
                
                //Utils.Print ( "-> Set Custom: " + memoryRegister.id + " - " + ( base.registers[ METHODS_SET_CUSTOM_PREFIX + memoryRegister.id ] == null ) );
            }

            #endregion
        }

        private void PropertySet_Logic<T> ( MemoryRegister<T> memoryRegister, dynamic value )
        {
            // Numeric field with operation to evaluate
            if ( memoryRegister.HasCustomOperation_Set )
                value = this.ExecuteOperation<T> ( memoryRegister.mathExpression_Set, value );

            switch ( Type.GetTypeCode( typeof(T)) )
            {
                case TypeCode.Int32  : this.SetIntToMem   ((int  )(object)value, memoryRegister.address, memoryRegister.size); break;
                case TypeCode.UInt32 : this.SetUIntToMem  ((uint )(object)value, memoryRegister.address, memoryRegister.size); break;
                case TypeCode.UInt64 : this.SetULongToMem ((ulong)(object)value, memoryRegister.address, memoryRegister.size); break;
                case TypeCode.Boolean: this.SetBoolToMem  ((bool )(object)value, memoryRegister.address, memoryRegister.size); break;
                case TypeCode.Char   : this.SetCharToMem  ((char )(object)value, memoryRegister.address); break;
                //case TypeCode.String : this.SetStringToMem(TypeCode.String, (string)(object)_value, regObj.address); break;
            }
        }

        public void CreateProperty_Set_String<T> ( MemoryRegister<T> memoryRegister )
        {
            base.AddMethod ( METHODS_SET_STRING_PREFIX + memoryRegister.id,
                new Action<string>((_value) =>
                {
                    // Boolean register need to be forced to use one byte, because size could be zero ( for first bit )
                    this.SetStringToMem<T> ( _value, memoryRegister.address, ( memoryRegister.size <= 0 ) ? 1 : memoryRegister.size );
                }));
        }

        public void CreateProperty_Set_ByteArray<T> ( MemoryRegister<T> memoryRegister )
        {
            base.AddMethod ( METHODS_SET_BYTE_PREFIX + memoryRegister.id,
                new Action<byte[]>((_value) =>
                {
                    // Boolean register need to be forced to use one byte, because size could be zero ( for first bit )
                    this.SetByteArrayToMem ( _value, memoryRegister.address, ( memoryRegister.size <= 0 ) ? 1 : memoryRegister.size );
                }));
        }

        #endregion

        #region Validations

        private bool ValidateNumeric<T> ( dynamic value, int size )
        {
            return true;
        
            return ( Validations.NumericBytesLimit<T> ( value, size ) &&
                     Validations.NumericTypeLimit <T> ( value ) );
        }

        /// <summary>
        /// Compares two memory maps only taking into account the <see cref="MemoryRegister{T}"/> that have been
        /// modified ( set/write ) and returns an string array with the identifiers of the registers with differences.
        /// <para>
        /// See <see cref="MemoryRegister.Equals(MemoryRegister{T})"/> to compare two <see cref="MemoryRegister{T}"/>.
        /// </para>
        /// <para>
        /// See <see cref="ValidateModifiedRegisters(MemoryMap)"/> to know if two <see cref="MemoryMap"/>s are equal.
        /// </para>
        /// </summary>
        /// <param name="otherMap">Other <see cref="MemoryMap"/> to compare with this</param>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.
        /// <para>
        /// List of identifiers of differente registers.
        /// </para>
        /// </returns>
        public async Task<string[]> GetModifiedRegistersDifferences ( MemoryMap otherMap )
        {
            List<string> difs = new List<string> ();

            // Only check modified registers
            List<dynamic> modifiedRegisters = this.GetModifiedRegisters ().GetAllElements ();
            for ( int i = 0; i < modifiedRegisters.Count; i++ )
            {
                dynamic register = modifiedRegisters[ i ];
                string  name     = register.id;
                
                Utils.Print ( "Check MTU write: " + name +
                    " [ Size: " + register.size +
                    ", SizeGet: " + register.sizeGet +
                    ", Other contains: " + otherMap.ContainsMember ( name ) + " ]" );
                
                if ( ( register.size == register.sizeGet ||
                       register.valueType == RegType.BOOL ) &&
                     ( ! otherMap.ContainsMember ( name ) ||                // Register not present in other memory map
                       ! await base[ name ].Equals ( otherMap[ name ] ) ) ) // Both registers are not equal
                {
                    Utils.Print ( "Equals: " + name + " -> NO" );
                
                    difs.Add ( name );
                    continue;
                }
                else Utils.PrintDeep( "Equals: " + name + " -> OK" );
            }

            return difs.ToArray ();
        }

        /// <summary>
        /// Compares two memory maps only taking into account the <see cref="MemoryRegister{T}"/> that have been
        /// modified ( set/write ).
        /// <para>
        /// See <see cref="MemoryRegister.Equals(MemoryRegister{T})"/> to compare two <see cref="MemoryRegister{T}"/>.
        /// </para>
        /// </summary>
        /// <param name="otherMap">Other <see cref="MemoryMap"/> to compare with this</param>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.
        /// <para>
        /// Boolean that indicates if both <see cref="MemoryMap"/>s are equal.
        /// </para>
        /// </returns>
        public async Task<bool> ValidateModifiedRegisters ( MemoryMap otherMap )
        {
            return ( ( await this.GetModifiedRegistersDifferences ( otherMap ) ).Length == 0 );
        }

        #endregion

        #region Operations

        private T ExecuteOperation<T> ( string operation, object value )
        {
            // The following arithmetic operators are supported in expressions: +, -, *, / and %
            // NOTA: No se puede hacer la conversion directa de un double a un entero generico
            //return ( T )( new DataTable ().Compute ( operation.Replace ( "_val_", value.ToString () ), null ) );

            object result = new DataTable().Compute(operation.Replace ( REGISTER_OP, value.ToString()), null );

            switch ( Type.GetTypeCode( typeof( T )) )
            {
                case TypeCode.Int32  : result = Convert.ToInt32  ( result ); break;
                case TypeCode.UInt32 : result = Convert.ToUInt32 ( result ); break;
                case TypeCode.UInt64 : result = Convert.ToInt64  ( result ); break;
            }

            Utils.PrintDeep ( "GetOperation: " + operation + " | " + value + " = " + result );

            return ( T )result;
        }

        private T ExecuteOperation<T> ( string operation, params object[] values )
        {
            int i = 1;
            foreach ( object value in values )
                operation = operation.Replace ( OVERLOAD_OP.Replace ( OVERLOAD_OP_SIGN, ( i++ ).ToString () ), value.ToString () );

            object result = new DataTable().Compute ( operation, null );

            switch ( Type.GetTypeCode( typeof( T )) )
            {
                case TypeCode.Int32  : result = Convert.ToInt32  ( result ); break;
                case TypeCode.UInt32 : result = Convert.ToUInt32 ( result ); break;
                case TypeCode.UInt64 : result = Convert.ToInt64  ( result ); break;
            }

            Utils.PrintDeep ( "GetOperation: " + operation + " = " + result );

            return ( T )result;
        }

        #endregion

        #region Get value

        // NOTE: C# for the moment does not allow to use mathematical operators with T as one of the operands

        private int GetIntFromMem ( int address, int size = MemRegister.DEF_SIZE )
        {
            byte[] data = new byte[ size ];
            Array.Copy ( this.memory, address, data, 0, size );

            return this.GetIntFromMem_Logic ( data );
        }

        private uint GetUIntFromMem ( int address, int size = MemRegister.DEF_SIZE )
        {
            byte[] data = new byte[ size ];
            Array.Copy ( this.memory, address, data, 0, size );
        
            return this.GetUIntFromMem_Logic ( data );
        }

        private ulong GetULongFromMem ( int address, int size = MemRegister.DEF_SIZE )
        {
            byte[] data = new byte[ size ];
            Array.Copy ( this.memory, address, data, 0, size );
            
            return this.GetULongFromMem_Logic ( data );
        }

        private bool GetBoolFromMem ( int address, int bit_index = MemRegister.DEF_BIT )
        {
            return this.GetBoolFromMem_Logic ( this.memory[ address ], bit_index );
        }

        private char GetCharFromMem ( int address )
        {
            return this.GetCharFromMem_Logic ( this.memory[ address ] );
        }

        private string GetStringFromMem ( int address, int size = MemRegister.DEF_SIZE )
        {
            byte[] data = new byte[ size ];
            Array.Copy ( this.memory, address, data, 0, size );

            return this.GetStringFromMem_Logic ( data );
        }

        private int GetIntFromMem_Logic ( byte[] data )
        {
            int value = 0;
            for ( int i = 0; i < data.Length; i++ )
                value += data[ i ] << ( i * 8 );
            
            return value;
        }
        
        private uint GetUIntFromMem_Logic ( byte[] data )
        {
            uint value = 0;
            for ( int i = 0; i < data.Length; i++ )
                value += ( uint )( data[ i ] << ( i * 8 ) );

            return value;
        }
        
        private ulong GetULongFromMem_Logic ( byte[] data )
        {
            ulong value = 0;
            for ( int i = 0; i < data.Length; i++ )
                value += ( ulong )data[ i ] << ( i * 8 );

            return value;
        }
        
        private bool GetBoolFromMem_Logic ( int data, int bit_index )
        {
            return ( ( ( data >> bit_index ) & 1 ) == 1 );
        }

        private char GetCharFromMem_Logic ( int data )
        {
            return Convert.ToChar ( data );
        }

        private string GetStringFromMem_Logic ( byte[] data )
        {
            bool trimRight = true;
            List<byte> bytes = new List<byte> ();
            for ( int i = data.Length - 1; i >= 0; i-- )
            {
                if ( trimRight && data[ i ] == 0 )
                    continue;

                // Only removes consecutive empty bytes on the right
                trimRight = false;

                bytes.Add ( data[ i ] );
            }
            bytes.Reverse ();

            return Encoding.UTF8.GetString ( bytes.ToArray () ).Trim ();
        }

        private byte[] GetByteArrayFromMem ( int address, int size = MemRegister.DEF_SIZE )
        {
            byte[] dataRead = new byte[ size ];
            Array.Copy ( memory, address, dataRead, 0, size );
            
            return dataRead;
        }

        #endregion

        #region Set value

        private void SetIntToMem (
            int value,
            int address,
            int size = MemRegister.DEF_SIZE )
        {
            if ( this.ValidateNumeric<int> ( value, size ) )
                this.SetNumToMem_Logic ( value, address, size );
            else
                throw new SetMemoryTypeLimitException ( value + ".Int." + address + "+" + size );
        }

        private void SetIntToMem (
            string value,
            int address,
            int size = MemRegister.DEF_SIZE )
        {
            int vCasted;
            if (!int.TryParse(value, out vCasted))
                throw new SetMemoryFormatException ( value + ".Int" );
            else
            {
                if ( this.ValidateNumeric<int> ( value, size ) )
                    this.SetNumToMem_Logic ( vCasted, address, size );
                else
                    throw new SetMemoryTypeLimitException ( value + ".Int." + address + "+" + size );
            }
        }

        private void SetUIntToMem (
            uint value,
            int address,
            int size = MemRegister.DEF_SIZE )
        {
            if ( this.ValidateNumeric<uint> ( value, size ) )
                this.SetNumToMem_Logic ( value, address, size );
            else
                throw new SetMemoryTypeLimitException ( value + ".UInt." + address + "+" + size );
        }

        private void SetUIntToMem (
            string value,
            int address,
            int size = MemRegister.DEF_SIZE )
        {
            uint vCasted;
            if (!uint.TryParse(value, out vCasted))
                throw new SetMemoryFormatException ( value + ".UInt" );
            else
            {
                if ( this.ValidateNumeric<uint> ( value, size ) )
                    this.SetNumToMem_Logic ( vCasted, address, size );
                else
                    throw new SetMemoryTypeLimitException ( value + ".UInt." + address + "+" + size );
            }
        }

        private void SetULongToMem (
            ulong value,
            int address,
            int size = MemRegister.DEF_SIZE )
        {
            if ( this.ValidateNumeric<ulong> ( value, size ) )
                this.SetNumToMem_Logic ( value, address, size );
            else
                throw new SetMemoryTypeLimitException ( value + ".ULong." + address + "+" + size );
        }

        private void SetULongToMem (
            string value,
            int address,
            int size = MemRegister.DEF_SIZE )
        {
            ulong vCasted;
            if (!ulong.TryParse(value, out vCasted))
                throw new SetMemoryFormatException ( value + ".ULong" );
            else
            {
                if ( this.ValidateNumeric<ulong> ( value, size ) )
                    this.SetNumToMem_Logic ( vCasted, address, size );
                else
                    throw new SetMemoryTypeLimitException ( value + ".ULong." + address + "+" + size );
            }
        }

        private void SetBoolToMem (
            bool value,
            int address,
            int bit_index = MemRegister.DEF_BIT )
        {
            if ( value )
                 this.memory[address] = ( byte ) ( this.memory[address] |    1 << bit_index   );
            else this.memory[address] = ( byte ) ( this.memory[address] & ~( 1 << bit_index ) );
        }

        private void SetBoolToMem (
            string value,
            int address,
            int bit_index = MemRegister.DEF_BIT )
        {
            bool vCasted;
            if (!bool.TryParse(value, out vCasted)) // It is not case sensitive
                throw new SetMemoryFormatException ( value + ".Bool" );
            else
            {
                if ( vCasted )
                     this.memory[address] = ( byte ) ( this.memory[address] |    1 << bit_index   );
                else this.memory[address] = ( byte ) ( this.memory[address] & ~( 1 << bit_index ) );
            }
        }

        private void SetCharToMem (
            char value, 
            int address )
        {
            this.memory[address] = (byte)value;
        }

        private void SetStringToMem<T> (
            string value,
            int address,
            int size = MemRegister.DEF_SIZE )
        {
            TypeCode type = Type.GetTypeCode( typeof( T ) );

            // If value to set is a "real" string
            if ( type is TypeCode.String )
            {
                foreach(char c in value)
                    this.memory[address++] = (byte)c;
            }
            else
            {
                // If value to set is NOT to register of type string
                switch ( type )
                {
                    case TypeCode.Int32  : this.SetIntToMem   (value, address, size); break;
                    case TypeCode.UInt32 : this.SetUIntToMem  (value, address, size); break;
                    case TypeCode.UInt64 : this.SetULongToMem (value, address, size); break;
                    case TypeCode.Char   : this.SetCharToMem  (value[ 0 ], address);  break;
                    case TypeCode.Boolean: this.SetBoolToMem  (value, address, size); break;
                }
            }
        }

        private void SetByteArrayToMem (
            byte[] value,
            int address,
            int size = MemRegister.DEF_SIZE )
        {
            Utils.Print ( "Memory: size " + this.memory.Length + " | from: " + address + " to " + ( address + size ) + " [ " + size + " bytes ]" );
        
            for ( int i = 0; i < size; i++ )
                if ( value.Length > i )
                    this.memory[ address + i ] = value[ i ];
                
                // Allows to pass less data than the maximum bytes of the register
                else break;
        }

        private void SetNumToMem_Logic (
            dynamic value,
            int address,
            int size = MemRegister.DEF_SIZE )
        {
            for ( int b = 0; b < size; b++ )
                this.memory[ address + b ] = ( byte )( value >> ( b * 8 ) );
        }

        #endregion

        #region Get n bytes
        
        /// <summary>
        /// Loads a specific amount of data, reading from the physical the memory of the MTU,
        /// into the memory of the memory map, more commonly known as data dump.
        /// </summary>
        /// <remarks>
        /// NOTE: Used only for debug purposes.
        /// </remarks>
        /// <param name="address">Initial byte to start reading from the MTU</param>
        /// <param name="numBytes">Number of consecutive bytes from the initial <see cref="address"/></param>
        /// <param name="offset">Number of bytes of the result to avoid from the beginning</param>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.</returns>
        public async Task ReadFromMtu (
            int address,
            int numBytes,
            int offset = 0 )
        {
            if ( address + numBytes < this.memory.Length )
            {
                Utils.Print ( "Read " + numBytes + " from MTU and add to memory map buffer" );
                
                Buffer.BlockCopy ( await lexi.Read ( ( uint )address, ( uint )numBytes ), 0, this.memory, offset, numBytes );
            }
            else
                throw new Exception ();
        }
        
        #endregion

        #region Get register

        /// <summary>
        /// Returns the reference to a register dynamic member of the memory map, that can be a
        /// <see cref="MemoryRegister{T}"/> or a
        /// <see cref="MemoryOverload{T}"/>
        /// </summary>
        /// <param name="id">Identifier of the member to recover</param>
        /// <returns>The instance of the dynamic member stored in the memory map.</returns>
        public dynamic GetProperty ( string id )
        {
            if ( base.ContainsMember ( id ) )
                return base.registers[ id ];

            // Selected dynamic member not exists
            Utils.Print("Set " + id + ": Error - Selected register is not loaded");

            throw new MemoryRegisterNotExistException ( id + ".GetProperty" );
        }

        /// <summary>
        /// Returns the reference to the <see cref="MemoryRegister{T}"/> stored as
        /// a dynamic member in the memory map, with the specific identifier and of T type.
        /// <para>
        /// See <see cref="RegType"/> for a list of available types.
        /// </para>
        /// </summary>
        /// <param name="id">Identifier of the member to recover</param>
        /// <typeparam name="T">Type of the register</typeparam>
        /// <returns>The instance of the dynamic member stored in the memory map.</returns>
        public MemoryRegister<T> GetProperty<T>(string id)
        {
            if ( base.ContainsMember ( id ) )
                return (MemoryRegister<T>)base.registers[ id ];

            // Selected dynamic member not exists
            Utils.Print("Set " + id + ": Error - Selected register is not loaded");

            throw new MemoryRegisterNotExistException ( id + ".GetProperty<T>" );
        }

        /// <summary>
        /// Returns the reference to the <see cref="MemoryRegister{T}"/> stored as
        /// a dynamic member in the memory map, with the specific identifier and of integer type.
        /// </summary>
        /// <param name="id">Identifier of the member to recover</param>
        /// <returns>The instance of the dynamic member stored in the memory map.</returns>
        public MemoryRegister<int> GetProperty_Int(string id)
        {
            return this.GetProperty<int>(id);
        }

        /// <summary>
        /// Returns the reference to the <see cref="MemoryRegister{T}"/> stored as
        /// a dynamic member in the memory map, with the specific identifier and of unsigned integer type.
        /// </summary>
        /// <param name="id">Identifier of the member to recover</param>
        /// <returns>The instance of the dynamic member stored in the memory map.</returns>
        public MemoryRegister<uint> GetProperty_UInt(string id)
        {
            return this.GetProperty<uint>(id);
        }

        /// <summary>
        /// Returns the reference to the <see cref="MemoryRegister{T}"/> stored as
        /// a dynamic member in the memory map, with the specific identifier and of unsigned long type.
        /// </summary>
        /// <param name="id">Identifier of the member to recover</param>
        /// <returns>The instance of the dynamic member stored in the memory map.</returns>
        public MemoryRegister<ulong> GetProperty_ULong(string id)
        {
            return this.GetProperty<ulong>(id);
        }

        /// <summary>
        /// Returns the reference to the <see cref="MemoryRegister{T}"/> stored as
        /// a dynamic member in the memory map, with the specific identifier and of boolean type.
        /// </summary>
        /// <param name="id">Identifier of the member to recover</param>
        /// <returns>The instance of the dynamic member stored in the memory map.</returns>
        public MemoryRegister<bool> GetProperty_Bool(string id)
        {
            return this.GetProperty<bool>(id);
        }

        /// <summary>
        /// Returns the reference to the <see cref="MemoryRegister{T}"/> stored as
        /// a dynamic member in the memory map, with the specific identifier and of char type.
        /// </summary>
        /// <param name="id">Identifier of the member to recover</param>
        /// <returns>The instance of the dynamic member stored in the memory map.</returns>
        public MemoryRegister<char> GetProperty_Char(string id)
        {
            return this.GetProperty<char>(id);
        }

        /// <summary>
        /// Returns the reference to the <see cref="MemoryRegister{T}"/> stored as
        /// a dynamic member in the memory map, with the specific identifier and of string/char array type.
        /// </summary>
        /// <param name="id">Identifier of the member to recover</param>
        /// <returns>The instance of the dynamic member stored in the memory map.</returns>
        public MemoryRegister<string> GetProperty_String(string id)
        {
            return this.GetProperty<string>(id);
        }

        #endregion

        #region Used

        /// <summary>
        /// Returns a dictionary with only the modified memory registers,
        /// those whose values have been modified.
        /// <para>
        /// See <see cref="MTUComm.WriteMtuModifiedRegisters"/> to write to the phisical memory of the MTU only the memory registers modified.
        /// </para>
        /// <para>
        /// See <see cref="MemoryRegister.modified"/> that is the flag modified when a memory register value is set.
        /// </para>
        /// </summary>
        /// <returns></returns>
        public MemoryRegisterDictionary GetModifiedRegisters ()
        {
            MemoryRegisterDictionary changes = new MemoryRegisterDictionary ();

            Dictionary<string,dynamic> mixedRegisters =
                this.registersObjs.Where ( reg => reg.Value.modified )
                .ToDictionary ( reg => reg.Key, reg => reg.Value );

            this.GetModifiedRegisters<int   > ( changes, mixedRegisters );
            this.GetModifiedRegisters<uint  > ( changes, mixedRegisters );
            this.GetModifiedRegisters<ulong > ( changes, mixedRegisters );
            this.GetModifiedRegisters<bool  > ( changes, mixedRegisters );
            this.GetModifiedRegisters<char  > ( changes, mixedRegisters );
            this.GetModifiedRegisters<string> ( changes, mixedRegisters );

            return changes;
        }

        private void GetModifiedRegisters<T> (
            MemoryRegisterDictionary changes,
            Dictionary<string,dynamic> mixedRegisters )
        {
            foreach ( KeyValuePair<string,dynamic> entry in mixedRegisters )
            {
                dynamic register = entry.Value;

                switch ( Type.GetTypeCode ( typeof ( T ) ) )
                {
                    case TypeCode.Int32:
                        if ( ( RegType )register.valueType == RegType.INT )
                            changes.AddElement<int> ( register );
                        break;
                    case TypeCode.UInt32:
                        if ( ( RegType )register.valueType == RegType.UINT )
                            changes.AddElement<uint> ( register );
                        break;
                    case TypeCode.UInt64:
                        if ( ( RegType )register.valueType == RegType.ULONG )
                            changes.AddElement<ulong> ( register );
                        break;
                    case TypeCode.Boolean:
                        if ( ( RegType )register.valueType == RegType.BOOL )
                            changes.AddElement<bool> ( register );
                        break;
                    case TypeCode.Char:
                        if ( ( RegType )register.valueType == RegType.CHAR )
                            changes.AddElement<char> ( register );
                        break;
                    case TypeCode.String:
                        if ( ( RegType )register.valueType == RegType.STRING )
                            changes.AddElement<string> ( register );
                        break;
                }
            }
        }

        #endregion
        
        /// <summary>
        /// Resets the memory registers flags that indicate that at least
        /// once they have been read from the physical memory of the MTU.
        /// </summary>
        public void ResetReadFlags ()
        {
            foreach ( dynamic register in this.registersObjs.Values )
                register.readedFromMtu = false;
        }
        
        /// <summary>
        /// Configures how to work with the MTU, caching data or always recovering
        /// from the physical memory of the MTU each time a memory register is read.
        /// </summary>
        /// <param name="ok"><see langword="true"/> to cache the first reading performed</param>
        public void SetReadFromMtuOnlyOnce (
            bool ok )
        {
            this.readFromMtuOnlyOnce = ok;
        }
    
        #region Unit Test

        public async Task FillMemory (
            UnitTest_DumpMemoryMap dump )
        {
            foreach ( UnitTest_Register xmlReg in dump.ListRegisters )
            {
                //Console.WriteLine ( xmlReg.Id );

                var mapReg = base[ xmlReg.Id ]; //.SetValue ( xmlReg.Value, true ); // Force to write

                byte[] ar = xmlReg.GetBytes ();

                try
                {
                    Array.Copy ( ar, 0, this.memory, mapReg.address,
                        ( mapReg.valueType != RegType.BOOL ) ? mapReg.size : 1 );

                    Console.WriteLine ( "UnitTest - FillMap: " + xmlReg.Id +
                        " <- " + BitConverter.ToString ( ar ).Replace ( "-", " " ) );
                }
                catch ( Exception )
                {
                    //Console.WriteLine ( e.Message );
                }
            }
        }

        public async Task LogFullMemory ()
        {
            try
            {
                StringBuilder str = new StringBuilder ();
                //const string sentence = "\t<Register id=\"#1#\" value=\"#2#\" bytes=\"#3#\" size=\"#4#\" readmtu=\"#5#\" modified=\"#6#\"/>";
                const string sentence = "\t<Register id=\"#1#\" value=\"#2#\" bytes=\"#3#\" size=\"#4#\"/>";

                str.AppendLine ( "<MemoryDump>" );
                foreach ( KeyValuePair<string,dynamic> entry in this.registersObjs )
                {
                    var reg = entry.Value;

                    if ( reg.readedFromMtu ||
                         reg.modified )
                    {
                        var    value   = await reg.GetValue ();
                        byte[] valueAr = await reg.GetValueByteArray ();

                        str.AppendLine (
                            sentence
                                .Replace ( "#1#", entry.Key )
                                .Replace ( "#2#", value.ToString () )
                                .Replace ( "#3#", BitConverter.ToString ( valueAr ).Replace ( "-", " " ) )
                                .Replace ( "#4#", reg.sizeGet.ToString () )
                                //.Replace ( "#5#", reg.readedFromMtu.ToString () )
                                //.Replace ( "#6#", reg.modified.ToString () )
                        );
                    }
                }
                str.AppendLine ( "</MemoryDump>" );
                Utils.Print ( str.ToString () );

                str.Clear ();
                str = null;
            }
            catch ( Exception )
            {

            }
        }

        #endregion
    }
}
