using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Xml;

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
///      Console.WriteLine ( dynamicObject.member ); // 2 and member continues being reference to property
///      dynamicObject.member = 33;
///      Console.WriteLine ( dynamicObject.member ); // 33 but member now is only an integer, not previously referenced property
///      ----
/// - 8. New register properties simulated using ExpandoObject have to be used as normal class members
///      "memoryMap.registers.idProperty" to get value, and "memoryMap.registers.idProperty = value" to set
///      Never need to call directly to registers.get_idProperty nor registers.set_idProperty, because
///      Value property do it for us, invoking funcGet and funcSet methods
///      
/// HEADERS:
/// - Registers
///   - Get. public T RegisterId_Get|CustomId ( MemoryRegister<T> MemoryRegister )
///   - Set. public T RegisterId_Get|CustomId ( MemoryRegister<T> MemoryRegister, dynamic inputValue )
/// - Overloads
///   - Get. public T RegisterId_Get|CustomId ( MemoryOverload<T> MemoryOverload, dynamic MemoryRegisters ) -> ExpandoObject
///   - Get. public T RegisterId_Get|CustomId ( MemoryOverload<T> MemoryOverload, dynamic[] MemoryRegisters )
/// </summary>

namespace MTUComm.MemoryMap
{
    public class MemoryMap : AMemoryMap
    {
        #region Constants

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
        private const string METHODS_SET_STRING_PREFIX = METHODS_SET_PREFIX + "string_";
        private const string METHODS_SET_BYTE_PREFIX   = METHODS_SET_PREFIX + "byte_";

        public  const string METHOD             = "method";
        public  const string METHOD_KEY         = METHOD + ":";
        public  const string METHOD_SUFIX_SET   = "_Set";
        public  const string METHOD_SUFIX_GET   = "_Get";
        
        private const string REGISTER_OP        = "_val_";
        private const string OVERLOAD_OP_SIGN   = "#";
        private const string OVERLOAD_OP        = "_" + OVERLOAD_OP_SIGN + "_";

        private const string EXCEP_SET_INT      = "String argument can't be casted to int";
        private const string EXCEP_SET_UINT     = "String argument can't be casted to uint";
        private const string EXCEP_SET_ULONG    = "String argument can't be casted to ulong";
        public  const string EXCEP_SET_USED     = "The specified record has not been mapped";
        public  const string EXCEP_SET_READONLY = "The specified record is readonly";
        private const string EXCEP_REGI_METHOD  = "Custom register method '#' is not present in MTU family class";
        private const string EXCEP_OVER_METHOD  = "Custom overload method '#' is not present in MTU family class";

        #endregion

        #region Attributes

        public static bool isUnityTest { get; private set; }

        public byte[] memory { private set; get; }
        private Dictionary<string,dynamic> registersObjs;

        #endregion

        #region Initialization

        public MemoryMap ( byte[] memory, string family, string pathUnityTest = "" )
        {
            this.memory = memory;
            this.registersObjs = new Dictionary<string,dynamic>();

            isUnityTest = ! string.IsNullOrEmpty ( pathUnityTest );

            // Read MTU family XML and prepare setters and getters
            Configuration config = Configuration.GetInstance ( isUnityTest, pathUnityTest );
            XmlSerializer s = new XmlSerializer ( typeof ( MemRegisterList ) );

            // Parameter "family" when testing is full path to use
            string path = ( ! isUnityTest ) ? Path.Combine(config.GetBasePath(), XML_PREFIX + family + XML_EXTENSION) : pathUnityTest + family + XML_EXTENSION;

            using (TextReader reader = new StreamReader ( path ))
            {
                MemRegisterList list = s.Deserialize(reader) as MemRegisterList;

                #region Registers

                if ( list.Registers != null )
                    foreach ( MemRegister xmlRegister in list.Registers )
                    {
                        try {

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
                        dynamic memoryRegister = Activator.CreateInstance(typeof(MemoryRegister<>)
                            .MakeGenericType(SysType),
                                xmlRegister.Id,
                                type,
                                xmlRegister.Description,
                                xmlRegister.Address,
                                xmlRegister.Size,
                                xmlRegister.Write,
                                xmlRegister.Custom_Get,
                                xmlRegister.Custom_Set );

                        // Is not possible to use SysType as Type to invoke generic
                        // method like CreateProperty_Get<T> and is necesary to use Reflection
                        //typeof( MemoryMap ).GetMethod("CreateProperty_Get").MakeGenericMethod(SysType).Invoke(null, new object[] { regObj });
                        //typeof( MemoryMap ).GetMethod("CreateProperty_Set").MakeGenericMethod(SysType).Invoke(null, new object[] { regObj });
                        this.CreateProperty_Get ( memoryRegister );
                        if ( xmlRegister.Write )
                        {
                            this.CreateProperty_Set ( memoryRegister );
                            this.CreateProperty_Set_String ( memoryRegister );
                            this.CreateProperty_Set_ByteArray ( memoryRegister );
                        }

                        // References to write/set and get methods
                        bool w = memoryRegister.write;
                        dynamic get  =                                               base.registers[ METHODS_GET_PREFIX        + memoryRegister.id ];
                        dynamic getC = ( memoryRegister.HasCustomMethod_Get      ) ? base.registers[ METHODS_GET_CUSTOM_PREFIX + memoryRegister.id ] : null;
                        dynamic set  = ( w                                       ) ? base.registers[ METHODS_SET_PREFIX        + memoryRegister.id ] : null;
                        dynamic setC = ( w && memoryRegister.HasCustomMethod_Set ) ? base.registers[ METHODS_SET_CUSTOM_PREFIX + memoryRegister.id ] : null;
                        dynamic setS = ( w                                       ) ? base.registers[ METHODS_SET_STRING_PREFIX + memoryRegister.id ] : null;
                        dynamic setB = ( w                                       ) ? base.registers[ METHODS_SET_BYTE_PREFIX   + memoryRegister.id ] : null;
                        TypeCode tc  = Type.GetTypeCode(SysType.GetType());
                        switch (type)
                        {
                            case RegType.INT:
                                memoryRegister.funcGet       = (Func<int>)get;
                                memoryRegister.funcSet       = (Action<int>)set;
                                memoryRegister.funcGetCustom = (Func<int>)getC;
                                break;
                            case RegType.UINT:
                                memoryRegister.funcGet       = (Func<uint>)get;
                                memoryRegister.funcSet       = (Action<uint>)set;
                                memoryRegister.funcGetCustom = (Func<uint>)getC;
                                break;
                            case RegType.ULONG:
                                memoryRegister.funcGet       = (Func<ulong>)get;
                                memoryRegister.funcSet       = (Action<ulong>)set;
                                memoryRegister.funcGetCustom = (Func<ulong>)getC;
                                break;
                            case RegType.BOOL:
                                memoryRegister.funcGet       = (Func<bool>)get;
                                memoryRegister.funcSet       = (Action<bool>)set;
                                memoryRegister.funcGetCustom = (Func<bool>)getC;
                                break;
                            case RegType.CHAR:
                                memoryRegister.funcGet       = (Func<char>)get;
                                memoryRegister.funcSet       = (Action<char>)set;
                                memoryRegister.funcGetCustom = (Func<char>)getC;
                                break;
                            case RegType.STRING:
                                memoryRegister.funcGet       = (Func<string>)get;
                                memoryRegister.funcSet       = (Action<string>)set;
                                memoryRegister.funcGetCustom = (Func<string>)getC;
                                break;
                        }
                        
                        memoryRegister.funcSetCustom = (Func<dynamic,dynamic>)setC;

                        // All register have this two functions
                        memoryRegister.funcSetString    = (Action<string>)setS;
                        memoryRegister.funcSetByteArray = (Action<byte[]>)setB;

                        // BAD: Reference to property itself
                        // OK : Reference to register object and use TrySet|GetMember methods
                        //      to override set and get logic, avoiding ExpandoObject problems
                        // NOTA: No se puede usar "base." porque parece ser invalidaria el comportamiento dinamico
                        AddProperty ( memoryRegister );

                        // Add new object to collection where will be
                        // filtered to only recover modified registers
                        this.registersObjs.Add(xmlRegister.Id, memoryRegister);

                        }
                        catch ( Exception e )
                        {
                            Console.WriteLine ( "ERROR! " + xmlRegister.Id + " -> " + e.Message + " " + e.InnerException );
                        }
                    }

                #endregion
                #region Overloads

                if ( list.Overloads != null )
                    foreach ( MemOverload xmlOverload in list.Overloads )
                    {
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
                            case RegType.INT   : memoryOverload.funcGet = (Func<int>   )get; break;
                            case RegType.UINT  : memoryOverload.funcGet = (Func<uint>  )get; break;
                            case RegType.ULONG : memoryOverload.funcGet = (Func<ulong> )get; break;
                            case RegType.BOOL  : memoryOverload.funcGet = (Func<bool>  )get; break;
                            case RegType.CHAR  : memoryOverload.funcGet = (Func<char>  )get; break;
                            case RegType.STRING: memoryOverload.funcGet = (Func<string>)get; break;
                        }

                        AddProperty ( memoryOverload );
                    }

                #endregion
            }
        }

        #endregion

        #region Create Property Get

        // Custom header format: public T RegisterId_Get|CustomId ( MemoryRegister<T> MemoryRegister )
        private void CreateProperty_Get<T> ( MemoryRegister<T> memoryRegister )
        {
            #region Normal Get

            // All register have normal get block
            base.AddMethod ( METHODS_GET_PREFIX + memoryRegister.id,
                new Func<T>(() =>
                {
                    object result = default ( T );
                    switch (Type.GetTypeCode(typeof(T)))
                    {
                        case TypeCode.Int32  : result = ( object )this.GetIntFromMem   (memoryRegister.address, memoryRegister.size); break;
                        case TypeCode.UInt32 : result = ( object )this.GetUIntFromMem  (memoryRegister.address, memoryRegister.size); break;
                        case TypeCode.UInt64 : result = ( object )this.GetULongFromMem (memoryRegister.address, memoryRegister.size); break;
                        case TypeCode.Boolean: result = ( object )this.GetBoolFromMem  (memoryRegister.address, memoryRegister.bit);  break;
                        case TypeCode.Char   : result = ( object )this.GetCharFromMem  (memoryRegister.address);                      break;
                        case TypeCode.String : result = ( object )this.GetStringFromMem(memoryRegister.address, memoryRegister.size); break;
                    }

                    // Numeric field with operation to evaluate
                    if ( memoryRegister.HasCustomOperation_Get )
                        return this.ExecuteOperation<T> ( memoryRegister.mathExpression_Get, result );

                    // String field with format to apply
                    else if ( memoryRegister.HasCustomFormat_Get )
                        return ( T )( object )this.ApplyFormat ( ( string )result, memoryRegister.format_Get );

                    // Only return readed value
                    return ( T )result;
                }));

            #endregion

            #region Custom Get that internally uses Normal Get

            // But only someone have special get block/method defined on MTU family classes
            if ( memoryRegister.HasCustomMethod_Get )
            {
                MethodInfo customMethod = this.GetType().GetMethod (
                    memoryRegister.methodId_Get,
                    new Type[] { typeof( MemoryRegister<T> ) } );

                // Method is not present in MTU family class
                if ( customMethod == null )
                {
                    string strError = EXCEP_REGI_METHOD.Replace ( "#", memoryRegister.methodId_Get );
                    Console.WriteLine ( "Create Custom Get " + memoryRegister.id + ": Error - " + strError );

                    if ( ! isUnityTest )
                        throw new CustomMethodNotExistException ( strError + ": " + memoryRegister.id );
                }

                base.AddMethod ( METHODS_GET_CUSTOM_PREFIX + memoryRegister.id,
                    new Func<T>(() =>
                    {
                        return ( T )customMethod.Invoke ( this, new object[] { memoryRegister } );
                    }));
            }

            #endregion
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
                        string strError = EXCEP_OVER_METHOD.Replace ( "#", memoryOverload.methodId );
                        Console.WriteLine ( "Create Custom Get " + memoryOverload.id + ": Error - " + strError );

                        if ( ! isUnityTest )
                            throw new CustomMethodNotExistException ( strError + ": " + memoryOverload.id );
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
                new Func<T>(() =>
                {
                    // Use custom method
                    if ( memoryOverload.HasCustomMethod )
                    {
                        if ( ! useParamArray )
                             return ( T )customMethod.Invoke ( this, new object[] { memoryOverload, registersToUse } );
                        else return ( T )customMethod.Invoke ( this, new object[] { memoryOverload, registersToUseArray } );
                    }

                    // Operation to evaluate
                    else
                    {
                        i = 0;
                        object[] values = new object[ dictionary.Count ];
                        foreach ( string id in dictionary.Keys )
                            values[ i++ ] = base[ id ].Value;

                        return this.ExecuteOperation<T> ( memoryOverload.custom_Get, values );
                    }
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
                    // Numeric field with operation to evaluate
                    if ( memoryRegister.HasCustomOperation_Set )
                        _value = this.ExecuteOperation<T> ( memoryRegister.mathExpression_Set, _value );

                    switch ( Type.GetTypeCode(typeof(T)) )
                    {
                        case TypeCode.Int32  : this.SetIntToMem   ((int  )(object)_value, memoryRegister.address, memoryRegister.size); break;
                        case TypeCode.UInt32 : this.SetUIntToMem  ((uint )(object)_value, memoryRegister.address, memoryRegister.size); break;
                        case TypeCode.UInt64 : this.SetULongToMem ((ulong)(object)_value, memoryRegister.address, memoryRegister.size); break;
                        case TypeCode.Boolean: this.SetBoolToMem  ((bool )(object)_value, memoryRegister.address, memoryRegister.size); break;
                        case TypeCode.Char   : this.SetCharToMem  ((char )(object)_value, memoryRegister.address); break;
                        //case TypeCode.String : this.SetStringToMem(TypeCode.String, (string)(object)_value, regObj.address); break;
                    }
                }));

            #endregion

            #region Custom Set previous to Normal Set

            // But only someone have special set block/method defined on MTU family classes
            if ( memoryRegister.HasCustomMethod_Set )
            {
                MethodInfo customMethod = this.GetType().GetMethod (
                    memoryRegister.methodId_Set ); //,
                    //new Type[] { typeof( MemoryRegister<T> ), typeof ( dynamic } );

                // Method is not present in MTU family class
                if ( customMethod == null )
                {
                    string strError = EXCEP_REGI_METHOD.Replace ( "#", memoryRegister.methodId_Set );
                    Console.WriteLine ( "Create Custom Set " + memoryRegister.id + ": Error - " + strError );

                    if ( ! isUnityTest )
                        throw new CustomMethodNotExistException ( strError + ": " + memoryRegister.id );
                }

                base.AddMethod ( METHODS_SET_CUSTOM_PREFIX + memoryRegister.id,
                    new Func<dynamic,dynamic>((_value) =>
                    {
                        return customMethod.Invoke ( this, new object[] { memoryRegister, _value } );
                    }));
            }

            #endregion
        }

        public void CreateProperty_Set_String<T> ( MemoryRegister<T> memoryRegister )
        {
            base.AddMethod ( METHODS_SET_STRING_PREFIX + memoryRegister.id,
                new Action<string>((_value) =>
                {
                    // String field with format to apply
                    if ( memoryRegister.HasCustomFormat_Set )
                        _value = this.ApplyFormat ( _value, memoryRegister.format_Set );

                    this.SetStringToMem(Type.GetTypeCode(typeof(T)), _value, memoryRegister.address,memoryRegister.size);
                }));
        }

        public void CreateProperty_Set_ByteArray<T> ( MemoryRegister<T> regObj )
        {
            base.AddMethod ( METHODS_SET_BYTE_PREFIX + regObj.id,
                new Action<byte[]>((_value) =>
                {
                    this.SetByteArrayToMem ( _value, regObj.address, regObj.size );
                }));
        }

        #endregion

        #region Operations

        private T ExecuteOperation<T> ( string operation, object value )
        {
            // The following arithmetic operators are supported in expressions: +, -, *, / y %
            // NOTA: No se puede hacer la conversion directa de un double a un entero generico
            //return ( T )( new DataTable ().Compute ( operation.Replace ( "_val_", value.ToString () ), null ) );

            object result = new DataTable().Compute(operation.Replace ( REGISTER_OP, value.ToString()), null );

            switch ( Type.GetTypeCode( typeof( T )) )
            {
                case TypeCode.Int32  : result = Convert.ToInt32  ( result ); break;
                case TypeCode.UInt32 : result = Convert.ToUInt32 ( result ); break;
                case TypeCode.UInt64 : result = Convert.ToInt64  ( result ); break;
            }

            Console.WriteLine ( "GetOperation: " + operation + " | " + value );

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

            Console.WriteLine ( "GetOperation: " + operation );

            return ( T )result;
        }

        #endregion

        #region Format

        private string ApplyFormat ( string value, string mask )
        {
            return value; // string.Format ( mask, value );
        }

        #endregion

        #region Get value

        private int GetIntFromMem(int address, int size = MemRegister.DEF_SIZE)
        {
            int value = 0;
            for (int b = 0; b < size; b++)
                value += memory[address + b] << (b * 8);

            return value;
        }

        private uint GetUIntFromMem(int address, int size = MemRegister.DEF_SIZE)
        {
            uint value = 0;
            for (int b = 0; b < size; b++)
                value += (uint)(memory[address + b] << (b * 8));

            return value;
        }

        private ulong GetULongFromMem(int address, int size = MemRegister.DEF_SIZE)
        {
            ulong value = 0;
            for (int b = 0; b < size; b++)
                value += (ulong)memory[address + b] << (b * 8);

            return value;
        }

        private bool GetBoolFromMem(int address, int bit_index = MemRegister.DEF_BIT)
        {
            return (((memory[address] >> bit_index) & 1) == 1);
        }

        private char GetCharFromMem(int address)
        {
            return Convert.ToChar(memory[address]);
        }

        private string GetStringFromMem(int address, int size = MemRegister.DEF_SIZE)
        {
            byte[] dataRead = new byte[size];
            Array.Copy(memory, address, dataRead, 0, size);
            return Encoding.UTF8.GetString(dataRead);
        }

        #endregion

        #region Set value

        private void SetIntToMem(int value, int address, int size = MemRegister.DEF_SIZE)
        {
            for (int b = 0; b < size; b++)
                this.memory[address + b] = (byte)(value >> (b * 8));
        }

        private void SetIntToMem(string value, int address, int size = MemRegister.DEF_SIZE)
        {
            int vCasted;
            if (!int.TryParse(value, out vCasted))
            {
                if ( ! isUnityTest )
                    throw new SetMemoryFormatException(EXCEP_SET_INT + ": " + value);
            }
            else
                for (int b = 0; b < size; b++)
                    this.memory[address + b] = (byte)(vCasted >> (b * 8));
        }

        private void SetUIntToMem(uint value, int address, int size = MemRegister.DEF_SIZE)
        {
            for (int b = 0; b < size; b++)
                this.memory[address + b] = (byte)(value >> (b * 8));
        }

        private void SetUIntToMem(string value, int address, int size = MemRegister.DEF_SIZE)
        {
            uint vCasted;
            if (!uint.TryParse(value, out vCasted))
            {
                if ( ! isUnityTest )
                    throw new SetMemoryFormatException(EXCEP_SET_UINT + ": " + value);
            }
            else
                for (int b = 0; b < size; b++)
                    this.memory[address + b] = (byte)(vCasted >> (b * 8));
        }

        private void SetULongToMem(ulong value, int address, int size = MemRegister.DEF_SIZE)
        {
            for (int b = 0; b < size; b++)
                this.memory[address + b] = (byte)(value >> (b * 8));
        }

        private void SetULongToMem(string value, int address, int size = MemRegister.DEF_SIZE)
        {
            ulong vCasted;
            if (!ulong.TryParse(value, out vCasted))
            {
                if ( ! isUnityTest )
                    throw new SetMemoryFormatException(EXCEP_SET_ULONG + ": " + value);
            }
            else
                for (int b = 0; b < size; b++)
                    this.memory[address + b] = (byte)(vCasted >> (b * 8));
        }

        private void SetBoolToMem (bool value, int address, int bit_index = MemRegister.DEF_BIT)
        {
            memory[address] = ( byte ) ( memory[address] | (1 << bit_index) );
        }

        private void SetCharToMem (char value, int address)
        {
            this.memory[address] = (byte)value;
        }

        private void SetStringToMem ( TypeCode registerType, string value, int address, int size = MemRegister.DEF_SIZE)
        {
            // If value to set is "real" string
            if ( registerType is TypeCode.String )
            {
                foreach(char c in value)
                    this.memory[address++] = (byte)c;
            }
            else
            {
                // If value to set is NOT to register of type string
                switch ( registerType )
                {
                    case TypeCode.Int32  : this.SetIntToMem   (value, address, size); break;
                    case TypeCode.UInt32 : this.SetUIntToMem  (value, address, size); break;
                    case TypeCode.UInt64 : this.SetULongToMem (value, address, size); break;
                    case TypeCode.Char   : this.SetCharToMem  (value[ 0 ], address); break;
                }
            }
        }

        private void SetByteArrayToMem ( byte[] value, int address, int size = MemRegister.DEF_SIZE )
        {
            for ( int i = 0; i < size; i++ )
                this.memory[ address + i ] = value[ i ];
        }

        #endregion

        #region Get register

        public dynamic GetProperty ( string id )
        {
            if ( base.ContainsMember ( id ) )
                return base.registers[ id ];

            // Selected dynamic member not exists
            Console.WriteLine("Set " + id + ": Error - Selected register is not loaded");

            if ( ! isUnityTest )
                throw new MemoryRegisterNotExistException(MemoryMap.EXCEP_SET_USED + ": " + id);
            return null;
        }

        public MemoryRegister<T> GetProperty<T>(string id)
        {
            if ( base.ContainsMember ( id ) )
                return (MemoryRegister<T>)base.registers[ id ];

            // Selected dynamic member not exists
            Console.WriteLine("Set " + id + ": Error - Selected register is not loaded");

            if ( ! isUnityTest )
                throw new MemoryRegisterNotExistException(MemoryMap.EXCEP_SET_USED + ": " + id);
            return null;
        }

        public MemoryRegister<int> GetProperty_Int(string id)
        {
            return this.GetProperty<int>(id);
        }

        public MemoryRegister<uint> GetProperty_UInt(string id)
        {
            return this.GetProperty<uint>(id);
        }

        public MemoryRegister<ulong> GetProperty_ULong(string id)
        {
            return this.GetProperty<ulong>(id);
        }

        public MemoryRegister<bool> GetProperty_Bool(string id)
        {
            return this.GetProperty<bool>(id);
        }

        public MemoryRegister<char> GetProperty_Char(string id)
        {
            return this.GetProperty<char>(id);
        }

        public MemoryRegister<string> GetProperty_String(string id)
        {
            return this.GetProperty<string>(id);
        }

        #endregion

        #region Used

        public void SetRegisterModified ( string id )
        {
            if ( this.registersObjs.ContainsKey ( id ) )
                this.registersObjs[ id ].used = true;

            else if ( ! isUnityTest )
                throw new MemoryRegisterNotExistException ( EXCEP_SET_USED + ": " + id );
        }

        public void SetRegisterNotModified ( string id )
        {
            if ( this.registersObjs.ContainsKey ( id ) )
                this.registersObjs[ id ].used = false;

            else if ( ! isUnityTest )
                throw new MemoryRegisterNotExistException ( EXCEP_SET_USED + ": "  + id );
        }

        public MemoryRegisterDictionary GetModifiedRegisters ()
        {
            MemoryRegisterDictionary changes = new MemoryRegisterDictionary ();

            Dictionary<string,dynamic> mixedRegisters =
                this.registersObjs.Where ( reg => reg.Value.used )
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
                    case TypeCode.Int64:
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

            /*
            return this.registersObjs.Where(reg => reg.Value.used)
                .ToDictionary(reg => reg.Key, reg => reg.Value)
                .Values.ToArray<RegisterObj> ();
            */
        }

        #endregion

        #region Overloads

        public string DailySnap_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            int timeDiff = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
            int curTime = MemoryRegisters.DailyGMTHourRead.Value + timeDiff;

            if (curTime < 0)
                curTime = 24 + curTime;
            if (curTime == 0)
                return "MidNight";
            if (curTime <= 11)
                return curTime.ToString() + " AM";
            if (curTime == 12)
                return "Noon";
            if (curTime > 12 && curTime < 24)
                return (curTime - 12).ToString() + " PM";
            else
                return "Off";
        }

        public string MtuStatus_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return MemoryRegisters.Shipbit.Value ? "OFF" : "ON";
        }

        public string ReadInterval_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return TimeFormatter(MemoryRegisters.ReadIntervalMinutes.Value);
        }

        public string XmitInterval_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return TimeFormatter(MemoryRegisters.ReadIntervalMinutes.Value * MemoryRegisters.MessageOverlapCount.Value);
        }

        public string PCBNumber_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            string tempString = string.Empty;
            //ASCII RANGE FOR PCBSupplierCode
            if (MemoryRegisters.PCBSupplierCode.Value >=65 && MemoryRegisters.PCBSupplierCode.Value <= 90)
            {
                tempString = tempString + Convert.ToChar(MemoryRegisters.PCBSupplierCode.Value) + "-";
            }

            if(MemoryRegisters.PCBCoreNumber.Value >= 0)
            {
                tempString = tempString + string.Format("{0:000000000}", MemoryRegisters.PCBCoreNumber.Value);
            }

            if (MemoryRegisters.PCBProductRevision.Value >= 65 && MemoryRegisters.PCBProductRevision.Value <= 90)
            {
                tempString = tempString + "-" +Convert.ToChar(MemoryRegisters.PCBProductRevision.Value);
            }
            return tempString.Equals(String.Empty) ? "Not Available" : tempString;
        }

        public string MtuSoftware_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            //if (MemoryRegisters)
            if(MemoryOverload.registerIds.Length > 1)
            {
                return string.Format("Version {0:00}.{1:00}.{2:0000}", 
                    MemoryRegisters.MTUFirmwareVersionMaior.Value,
                    MemoryRegisters.MTUFirmwareVersionMinor.Value,
                    MemoryRegisters.MTUFirmwareVersionBuild.Value);
            }
            else
            {
                return string.Format("Version {0:00}", MemoryRegisters.MTUFirmwareVersionFormatFlag.Value);
            }
            
        }

        public string Encryption_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return MemoryRegisters.Encrypted.Value ? "Yes" : "No";
        }

        public string MtuVoltageBattery_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return ((MemoryRegisters.MtuMiliVoltageBattery.Value * 1.0) / 1000).ToString("0.00 V");
        }

        public string P1ReadingError_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return TranslateErrorCodes(MemoryRegisters.P1ReadingErrorCode.Value);
        }

        public string P2ReadingError_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return TranslateErrorCodes(MemoryRegisters.P2ReadingErrorCode.Value);
        }

        public string InterfaceTamperStatus_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return GetTemperStatus(MemoryRegisters.P1InterfaceAlarm.Value, MemoryRegisters.ProgrammingCoilInterfaceTamper.Value);
        }

        public string TiltTamperStatus_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return GetTemperStatus(MemoryRegisters.P1TiltAlarm.Value, MemoryRegisters.TiltTamper.Value);
        }

        public string MagneticTamperStatus_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return GetTemperStatus(MemoryRegisters.P1MagneticAlarm.Value, MemoryRegisters.MagneticTamper.Value);
        }

        public string RegisterCoverTamperStatus_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return GetTemperStatus(MemoryRegisters.P1RegisterCoverAlarm.Value, MemoryRegisters.RegisterCoverTamper.Value);
        }

        public string ReverseFlowTamperStatus(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return GetTemperStatus(MemoryRegisters.P1ReverseFlowAlarm.Value, MemoryRegisters.ReverseFlowTamper.Value);
        }

        public string FastMessagingMode_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return MemoryRegisters.Fast2Way.Value ? "Fast" : "Slow";
        }

        public string LastGasp_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return MemoryRegisters.LastGaspTamper.Value ? "Enabled" : "Triggered";
        }

        public string InsufficentMemory_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return MemoryRegisters.InsufficentMemoryTamper.Value ? "Enabled" : "Triggered";
        }

        public string P1Status_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return GetPortStatus(MemoryRegisters.P1StatusFlag.Value);
        }

        public string P2Status_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return GetPortStatus(MemoryRegisters.P2StatusFlag.Value);
        }

        public string F12WAYRegister1_Get(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return "0x" + MemoryRegisters.F12WAYRegister1Int.Value.ToString("X8");
        }

        public string F12WAYRegister10_Get(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return "0x" + MemoryRegisters.F12WAYRegister10Int.Value.ToString("X8");
        }

        public string F12WAYRegister14_Get(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return "0x" + MemoryRegisters.F12WAYRegister14Int.Value.ToString("X8");
        }

        public string ReverseFlowTamperStatus_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return MemoryOverload.Value;
        }

        #endregion

        #region Registers

        public int ReadIntervalMinutes_Set ( MemoryRegister<ulong> MemoryRegister, dynamic inputValue )
        {
            string[] readIntervalArray = ((string)inputValue).Split(' ');
            string readIntervalStr = readIntervalArray[0];
            string timeUnit = readIntervalArray[1];
            int timeIntervalMins = Int32.Parse(readIntervalStr);

            if (timeUnit is "Hours")
                timeIntervalMins = timeIntervalMins * 60;

            return timeIntervalMins;
        }

        // Use with <CustomGet>method:ULongToBcd</CustomGet>
        public ulong BcdToULong ( MemoryRegister<ulong> MemoryRegister )
        {
            return this.BcdToULong ( ( ulong )MemoryRegister.Value );
        }

        // Use with <CustomSet>method:ULongToBcd</CustomSet>
        public ulong ULongToBcd ( MemoryRegister<ulong> MemoryRegister, dynamic inputValue )
        {
            if ( inputValue is string )
                return this.ULongToBcd ( inputValue );
            return this.ULongToBcd ( ( ulong )inputValue );
        }

        #endregion

        #region e-Coder

        public string BackFlowState_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            string reply = string.Empty;
            string param = Convert.ToString(MemoryRegisters.FlowState.Value, 2).PadLeft(8, '0').Substring(6);
            switch (param)
            {
                case "00":
                    reply = "No reverse Flow Event in last 35 days";
                    break;
                case "01":
                    reply = "Small Reverse Flow Event in last 35 days";
                    break;
                case "10":
                    reply = "Large Reverse Flow Event in last 35 days";
                    break;
            }
            return reply;
        }

        public string DaysOfNoFlow_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            string reply = string.Empty;
            string param = Convert.ToString(MemoryRegisters.FlowState.Value, 2).PadLeft(8, '0').Substring(3, 3);
            switch (param)
            {
                case "000":
                    reply = "0";
                    break;
                case "001":
                    reply = "1-2";
                    break;
                case "010":
                    reply = "3-7";
                    break;
                case "011":
                    reply = "8-14";
                    break;
                case "100":
                    reply = "15-21";
                    break;
                case "101":
                    reply = "22-34";
                    break;
                case "110":
                    reply = "35 (ALL)";
                    break;
            }

            return reply + " days of no consumption";
        }

        public string LeakDetection_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            string reply = string.Empty;
            string param = Convert.ToString(MemoryRegisters.LeakState.Value, 2).PadLeft(8, '0').Substring(5, 2);
            switch (param)
            {
                case "00":
                    reply = "Less than 50 15-minute intervals";
                    break;
                case "01":
                    reply = "Between 50 and 95 15-minute intervals";
                    break;
                case "10":
                    reply = "Greater than 96 15-minute intervals";
                    break;
            }
            return reply;
        }

        public string DaysOfLeak_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            string reply = string.Empty;
            string param = Convert.ToString(MemoryRegisters.LeakState.Value, 2).PadLeft(8, '0').Substring(2, 3);
            switch (param)
            {
                case "000":
                    reply = "0";
                    break;
                case "001":
                    reply = "1-2";
                    break;
                case "010":
                    reply = "3-7";
                    break;
                case "011":
                    reply = "8-14";
                    break;
                case "100":
                    reply = "15-21";
                    break;
                case "101":
                    reply = "22-34";
                    break;
                case "110":
                    reply = "35 (ALL)";
                    break;
            }

            return reply + " days of leak detection";
        }

        #endregion

        #region AuxiliaryFunctions

        private string TranslateErrorCodes (int encoderErrorcode)
        {
            if (encoderErrorcode == 0xFF)
                return "ERROR - Check Meter";
            if (encoderErrorcode == 0xFE)
                return "ERROR - Bad Digits";
            if (encoderErrorcode == 0xFD)
                return "ERROR - Delta Overflow";
            if (encoderErrorcode == 0xFC)
                return "ERROR - Readings Purged";
            return "";
        }

        private string TimeFormatter (int time)
        {
            switch (time)
            {
                case 2880: return "48 Hrs";
                case 2160: return "36 Hrs";
                case 1440: return "24 Hrs";
                case 720: return "12 Hrs";
                case 480: return "8 Hrs";
                case 360: return "6 Hrs";
                case 240: return "4 Hrs";
                case 180: return "3 Hrs";
                case 120: return "2 Hrs";
                case 90: return "1 Hr 30 Min";
                case 60: return "1 Hr";
                case 30: return "30 Min";
                case 15: return "15 Min";
                case 10: return "10 Min";
                case 5: return "5 Min";
                default: // KG 3.10.2010 add HR-Min calc:
                    if (time % 60 == 0)
                        return (time / 60).ToString() + " Hrs";
                    else
                        if (time < 60)
                        return (time % 60).ToString() + " Min";
                    else if (time < 120)
                        return (time / 60).ToString() + " Hr " + (time % 60).ToString() + " Min";
                    else
                        return (time / 60).ToString() + " Hrs " + (time % 60).ToString() + " Min";
                    //return xMit.ToString() + " Min";//"BAD READ";

            }
        }

        private string GetTemperStatus (bool alarm, bool temper)
        {
            if (alarm)
            {
                if (temper)
                {
                    return "Triggered";
                }
                else
                {
                    return "Enabled";
                }
            }
            else
            {
                return "Disabled";
            }
        }

        private string GetPortStatus (bool status)
        {
            return status ? "Enabled" : "Disabled";
        }

        private ulong BcdToULong ( ulong valueInBCD )
        {
            // Define powers of 10 for the BCD conversion routines.
            ulong powers = 1;
            ulong outNum = 0;
            byte tempNum;

            for (int offset = 0; offset < 7; offset++)
            {
                tempNum = (byte)((valueInBCD >> offset * 8) & 0xff);
                if ((tempNum & 0x0f) > 9)
                {
                    break;
                }
                outNum += (ulong)(tempNum & 0x0f) * powers;
                powers *= 10;
                if ((tempNum >> 4) > 9)
                {
                    break;
                }
                outNum += (ulong)(tempNum >> 4) * powers;
                powers *= 10;
            }

            return outNum;
        }

        private ulong ULongToBcd ( string value )
        {
            return ulong.Parse(value, System.Globalization.NumberStyles.HexNumber);
        }

        private ulong ULongToBcd ( ulong value )
        {
            return this.ULongToBcd ( value.ToString () );
        }

        #endregion
    }
}
