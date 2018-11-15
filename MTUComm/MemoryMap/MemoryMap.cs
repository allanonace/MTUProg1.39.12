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
/// CURRENT:
/// - Seems that get methods are created ok but when call CreateProperty_Set_XXX breaks
///   Inside CreateProperty_Set_Int logic sentence is commented
/// > Also breaks when try to set funcGet|Set references to the methods
///   - El problema estaba en que no se puede castear Func<object> a Func<int> y lo mismo para Action
///   - Pero el siguiente problema es que si se usa una coleccion del tipo padre de los registros ( RegisterObj )
///     se usaran las referencias de funciones de la clase padre, no invocandose los metodos hijos por mucho que se
///     haya usado 'new' para ocultar los miembros de la clase padre. La solucion vuelve a ser usar un tipo dinamico,
///     en este caso para la coleccion de registros, convirtiendo ademas la clase padre en generica ( RegisterObj<T> ),
///     ademas de los metodos CreatePropertySet|Get y los delegados Func|Action<T>
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
        private const string METHODS_GET_CUSTOM_PREFIX = "getCustom_";
        private const string METHODS_SET_PREFIX = "set_";
        private const string METHODS_SET_STRING_PREFIX = METHODS_SET_PREFIX + "string_";
        private const string REGISTER_OP        = "_val_";
        private const string OVERLOAD_OP_SIGN   = "#";
        private const string OVERLOAD_OP        = "_" + OVERLOAD_OP_SIGN + "_";
        private const string EXCEP_SET_INT      = "String argument can't be casted to int";
        private const string EXCEP_SET_UINT     = "String argument can't be casted to uint";
        private const string EXCEP_SET_ULONG    = "String argument can't be casted to ulong";
        public  const string EXCEP_SET_USED     = "The specified record has not been mapped";
        public  const string EXCEP_SET_READONLY = "The specified record is readonly";
        private const string EXCEP_CUST_METHOD  = "Method '#' is not present in MTU family class";

        #endregion

        #region Attributes

        public byte[] memory { private set; get; }
        private Dictionary<string,dynamic> registersObjs;

        #endregion

        #region Initialization

        public MemoryMap ( byte[] memory, string family )
        {
            this.memory = memory;
            this.registersObjs = new Dictionary<string,dynamic>();

            // Read MTU family XML and prepare setters and getters
            Configuration config = Configuration.GetInstance();
            XmlSerializer s = new XmlSerializer ( typeof ( MemRegisterList ) );
            using (TextReader reader = new StreamReader(Path.Combine(config.GetBasePath(), XML_PREFIX + family + XML_EXTENSION)))
            {
                MemRegisterList list = s.Deserialize(reader) as MemRegisterList;

                #region Registers

                if ( list.Registers != null )
                    foreach ( MemRegister xmlRegister in list.Registers )
                    {
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
                                xmlRegister.Custom);

                        // Is not possible to use SysType as Type to invoke generic
                        // method like CreateProperty_Get<T> and is necesary to use Reflection
                        //typeof( MemoryMap ).GetMethod("CreateProperty_Get").MakeGenericMethod(SysType).Invoke(null, new object[] { regObj });
                        //typeof( MemoryMap ).GetMethod("CreateProperty_Set").MakeGenericMethod(SysType).Invoke(null, new object[] { regObj });
                        this.CreateProperty_Get ( memoryRegister );
                        if ( xmlRegister.Write )
                        {
                            this.CreateProperty_Set ( memoryRegister );
                            this.CreateProperty_Set_String ( memoryRegister );
                        }

                        // References to methods to use in property ( .Value )
                        dynamic get  = base.registers[ METHODS_GET_PREFIX + memoryRegister.id ];
                        dynamic getC = ( memoryRegister.HasCustomMethod ) ? base.registers[ METHODS_GET_CUSTOM_PREFIX + memoryRegister.id ] : null;
                        dynamic set  = ( memoryRegister.write ) ? base.registers[ METHODS_SET_PREFIX + memoryRegister.id ] : null;
                        dynamic setS = ( memoryRegister.write ) ? base.registers[ METHODS_SET_STRING_PREFIX + memoryRegister.id ] : null;
                        TypeCode tc  = Type.GetTypeCode(SysType.GetType());
                        switch (type)
                        {
                            case RegType.INT:
                                memoryRegister.funcGet       = (Func<int>)get;
                                memoryRegister.funcGetCustom = (Func<int>)getC;
                                memoryRegister.funcSet       = (Action<int>)set;
                                break;
                            case RegType.UINT:
                                memoryRegister.funcGet       = (Func<uint>)get;
                                memoryRegister.funcGetCustom = (Func<uint>)getC;
                                memoryRegister.funcSet       = (Action<uint>)set;
                                break;
                            case RegType.ULONG:
                                memoryRegister.funcGet       = (Func<ulong>)get;
                                memoryRegister.funcGetCustom = (Func<ulong>)getC;
                                memoryRegister.funcSet       = (Action<ulong>)set;
                                break;
                            case RegType.BOOL:
                                memoryRegister.funcGet       = (Func<bool>)get;
                                memoryRegister.funcGetCustom = (Func<bool>)getC;
                                memoryRegister.funcSet       = (Action<bool>)set;
                                break;
                            case RegType.CHAR:
                                memoryRegister.funcGet       = (Func<char>)get;
                                memoryRegister.funcGetCustom = (Func<char>)getC;
                                memoryRegister.funcSet       = (Action<char>)set;
                                break;
                            case RegType.STRING:
                                memoryRegister.funcGet       = (Func<string>)get;
                                memoryRegister.funcGetCustom = (Func<string>)getC;
                                memoryRegister.funcSet       = (Action<string>)set;
                                break;
                        }

                        memoryRegister.funcSetString = (Action<string>)setS;

                        // BAD: Reference to property itself
                        // OK : Reference to register object and use TrySet|GetMember methods
                        //      to override set and get logic, avoiding ExpandoObject problems
                        // NOTA: No se puede usar "base." porque parece ser invalidaria el comportamiento dinamico
                        AddProperty ( memoryRegister );

                        // Add new object to collection where will be
                        // filtered to only recover modified registers
                        this.registersObjs.Add(xmlRegister.Id, memoryRegister);
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
                            xmlOverload.Custom );

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

            #region Tests

            // TEST: Diferentes opciones campo custom ( metodo y operacion matematica )
            //Console.WriteLine ( "Test operation register: " + base.registers.BatteryVoltage );
            //Console.WriteLine ( "Test custom format: " + base.registers.DailyRead );

            // TEST: Separacion entre Value.get y funGetCustom
            //dynamic mInt = this.GetProperty_Int ( "DailyRead" );
            //Console.WriteLine ( base.registers.DailyRead + " == " + mInt.Value );
            //mInt.Value = 123;
            //Console.WriteLine ( base.registers.DailyRead + " == " + mInt.Value );

            // TEST: Registros de solo lectura
            //Console.WriteLine ( "Test lectura 1: " + base.registers.MtuType);
            //this.registers.MtuType = 12321; // Not allow. Error!

            // TEST: Recuperar registros modificados
            //this.SetRegisterModified ( "MtuType"   );
            //this.SetRegisterModified ( "Shipbit"   );
            //this.SetRegisterModified ( "DailyRead" );
            //MemoryRegisterDictionary regs = this.GetModifiedRegisters ();

            // TEST: Recuperar objetos registro
            //dynamic             reg1 = this.GetProperty      ( "MtuType" );
            //MemoryRegister<int> reg2 = this.GetProperty<int> ( "MtuType" );
            //MemoryRegister<int> reg3 = this.GetProperty_Int  ( "MtuType" );
            //Console.WriteLine ( "Registro MtuType: " +
            //    reg1.Value + " " + reg2.Value + " " + reg3.Value );

            // TEST: Trabajar con overloads
            //Console.WriteLine ( "Test metodo overload: "       + base.registers.Overload_Method );
            //Console.WriteLine ( "Test metodo reuse overload: " + base.registers.Overload_Method_Reuse );
            //Console.WriteLine ( "Test metodo array overload: " + base.registers.Overload_Method_Array );
            //Console.WriteLine ( "Test operation overload: "    + base.registers.Overload_Operation );

            #endregion
        }

        #endregion

        #region Create Property Get

        private void CreateProperty_Get<T> ( MemoryRegister<T> memoryRegister )
        {
            // All register have normal get block
            base.AddMethod ( METHODS_GET_PREFIX + memoryRegister.id,
                new Func<T>(() =>
                {
                    object result = default ( T );
                    switch (Type.GetTypeCode(typeof(T)))
                    {
                        case TypeCode.Int32  : result = ( object )this.GetIntFromMem   (memoryRegister.address, memoryRegister.size);   break;
                        case TypeCode.UInt32 : result = ( object )this.GetUIntFromMem  (memoryRegister.address, memoryRegister.size);   break;
                        case TypeCode.UInt64 : result = ( object )this.GetULongFromMem (memoryRegister.address, memoryRegister.size);   break;
                        case TypeCode.Boolean: result = ( object )this.GetBoolFromMem  (memoryRegister.address, memoryRegister.bit);    break;
                        case TypeCode.Char   : result = ( object )this.GetCharFromMem  (memoryRegister.address);                        break;
                        case TypeCode.String : result = ( object )this.GetStringFromMem(memoryRegister.address, memoryRegister.size); break;
                    }

                    // Numeric field with operation to evaluate
                    if ( memoryRegister.HasCustomOperation )
                        return this.GetOperation<T> ( memoryRegister.custom, result );

                    // String field with format to apply
                    else if ( memoryRegister.HasCustomFormat )
                        return ( T )( object )this.GetStringFormatted ( ( string )result, memoryRegister.custom );

                    // Only return readed value
                    return ( T )result;
                }));

            // But only someone have special get block/method defined on MTU family classes
            if ( memoryRegister.HasCustomMethod )
            {
                MethodInfo customMethod = this.GetType().GetMethod (
                    memoryRegister.methodId,
                    new Type[] { typeof( MemoryRegister<T> ) } );

                // Method is not present in MTU family class
                if ( customMethod == null )
                {
                    string strError = EXCEP_CUST_METHOD.Replace ( "#", memoryRegister.methodId );
                    Console.WriteLine ( "Create Custom Get " + memoryRegister.id + ": Error - " + strError );
                    throw new CustomMethodNotExistException ( strError + ": " + memoryRegister.id );
                }

                base.AddMethod ( METHODS_GET_CUSTOM_PREFIX + memoryRegister.id,
                    new Func<T>(() =>
                    {
                        return ( T )customMethod.Invoke ( this, new object[] { memoryRegister } );
                    }));
            }
        }

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
                        string strError = EXCEP_CUST_METHOD.Replace ( "#", memoryOverload.methodId );
                        Console.WriteLine ( "Create Custom Get " + memoryOverload.id + ": Error - " + strError );
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

                        return this.GetOperation<T> ( memoryOverload.custom, values );
                    }
                }));
        }

        #endregion

        #region Create Property Set

        public void CreateProperty_Set<T>( MemoryRegister<T> regObj )
        {
            base.AddMethod ( METHODS_SET_PREFIX + regObj.id,
                new Action<T>((_value) =>
                {
                    switch ( Type.GetTypeCode(typeof(T)) )
                    {
                        case TypeCode.Int32  : this.SetIntToMem   ((int   )(object)_value, regObj.address, regObj.size); break;
                        case TypeCode.UInt32 : this.SetUIntToMem  ((uint  )(object)_value, regObj.address, regObj.size); break;
                        case TypeCode.UInt64 : this.SetULongToMem ((ulong )(object)_value, regObj.address, regObj.size); break;
                        case TypeCode.Boolean: this.SetBoolToMem  ((bool  )(object)_value, regObj.address, regObj.size); break;
                        case TypeCode.Char   : this.SetCharToMem  ((char  )(object)_value, regObj.address); break;
                        //case TypeCode.String : this.SetStringToMem(TypeCode.String, (string)(object)_value, regObj.address); break;
                    }
                }));
        }

        public void CreateProperty_Set_String<T> ( MemoryRegister<T> regObj )
        {
            base.AddMethod ( METHODS_SET_STRING_PREFIX + regObj.id,
                new Action<string>((_value) =>
                {
                    this.SetStringToMem(Type.GetTypeCode(typeof(T)), (string)(object)_value, regObj.address,regObj.size);
                }));
        }

        #endregion

        #region Get value

        private T GetOperation<T> ( string operation, object value )
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

        private T GetOperation<T> ( string operation, params object[] values )
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

        private string GetStringFormatted ( string value, string mask )
        {
            return value; // string.Format ( mask, value );
        }

        protected int GetIntFromMem(int address, int size = MemRegister.DEF_SIZE)
        {
            int value = 0;
            for (int b = 0; b < size; b++)
                value += memory[address + b] << (b * 8);

            return value;
        }

        protected uint GetUIntFromMem(int address, int size = MemRegister.DEF_SIZE)
        {
            uint value = 0;
            for (int b = 0; b < size; b++)
                value += (uint)(memory[address + b] << (b * 8));

            return value;
        }

        protected ulong GetULongFromMem(int address, int size = MemRegister.DEF_SIZE)
        {
            ulong value = 0;
            for (int b = 0; b < size; b++)
                value += (ulong)memory[address + b] << (b * 8);

            return BcdToULong(value);
        }

        protected bool GetBoolFromMem(int address, int bit_index = MemRegister.DEF_BIT)
        {
            return (((memory[address] >> bit_index) & 1) == 1);
        }

        protected char GetCharFromMem(int address)
        {
            return Convert.ToChar(memory[address]);
        }

        protected string GetStringFromMem(int address, int size = MemRegister.DEF_SIZE)
        {
            byte[] dataRead = new byte[size];
            Array.Copy(memory, address, dataRead, 0, size);
            return Encoding.Default.GetString(dataRead);
        }

        #endregion

        #region Set value

        protected void SetIntToMem(int value, int address, int size = MemRegister.DEF_SIZE)
        {
            for (int b = 0; b < size; b++)
                this.memory[address + b] = (byte)(value >> (b * 8));
        }

        private void SetIntToMem(string value, int address, int size = MemRegister.DEF_SIZE)
        {
            int vCasted;
            if (!int.TryParse(value, out vCasted))
                throw new SetMemoryFormatException(EXCEP_SET_INT + ": " + value);
            else
                for (int b = 0; b < size; b++)
                    this.memory[address + b] = (byte)(vCasted >> (b * 8));
        }

        protected void SetUIntToMem(uint value, int address, int size = MemRegister.DEF_SIZE)
        {
            for (int b = 0; b < size; b++)
                this.memory[address + b] = (byte)(value >> (b * 8));
        }

        private void SetUIntToMem(string value, int address, int size = MemRegister.DEF_SIZE)
        {
            uint vCasted;
            if (!uint.TryParse(value, out vCasted))
                throw new SetMemoryFormatException(EXCEP_SET_UINT + ": " + value);
            else
                for (int b = 0; b < size; b++)
                    this.memory[address + b] = (byte)(vCasted >> (b * 8));
        }

        protected void SetULongToMem(ulong value, int address, int size = MemRegister.DEF_SIZE)
        {
            for (int b = 0; b < size; b++)
                this.memory[address + b] = (byte)(this.ULongToBcd(value.ToString()) >> (b * 8));
        }

        private void SetULongToMem(string value, int address, int size = MemRegister.DEF_SIZE)
        {
            ulong vCasted;
            if (!ulong.TryParse(value, out vCasted))
                throw new SetMemoryFormatException(EXCEP_SET_ULONG + ": " + value);
            else
                for (int b = 0; b < size; b++)
                    this.memory[address + b] = (byte)(this.ULongToBcd(value) >> (b * 8));
        }

        protected void SetBoolToMem (bool value, int address, int bit_index = MemRegister.DEF_BIT)
        {
            memory[address] = ( byte ) ( memory[address] | (1 << bit_index) );
        }

        protected void SetCharToMem (char value, int address)
        {
            this.memory[address] = (byte)value;
        }

        protected void SetStringToMem ( TypeCode registerType, string value, int address, int size = MemRegister.DEF_SIZE)
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

        #endregion

        #region Get register

        public dynamic GetProperty ( string id )
        {
            if ( base.ContainsMember ( id ) )
                return base.registers[ id ];

            // Selected dynamic member not exists
            Console.WriteLine("Set " + id + ": Error - Selected register is not loaded");
            throw new MemoryRegisterNotExistException(MemoryMap.EXCEP_SET_USED + ": " + id);
        }

        public MemoryRegister<T> GetProperty<T>(string id)
        {
            if ( base.ContainsMember ( id ) )
                return (MemoryRegister<T>)base.registers[ id ];

            // Selected dynamic member not exists
            Console.WriteLine("Set " + id + ": Error - Selected register is not loaded");
            throw new MemoryRegisterNotExistException(MemoryMap.EXCEP_SET_USED + ": " + id);
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

        #region BCD

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
            return ulong.Parse ( value, System.Globalization.NumberStyles.HexNumber );
        }

        #endregion

        #region Used

        public void SetRegisterModified ( string id )
        {
            if ( this.registersObjs.ContainsKey ( id ) )
                this.registersObjs[ id ].used = true;
            else
                throw new MemoryRegisterNotExistException ( EXCEP_SET_USED + ": " + id );
        }

        public void SetRegisterNotModified ( string id )
        {
            if ( this.registersObjs.ContainsKey ( id ) )
                this.registersObjs[ id ].used = false;
            else
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
                        if ( ( RegType )register.type == RegType.INT )
                            changes.AddElement<int> ( register );
                        break;
                    case TypeCode.UInt32:
                        if ( ( RegType )register.type == RegType.UINT )
                            changes.AddElement<uint> ( register );
                        break;
                    case TypeCode.Int64:
                        if ( ( RegType )register.type == RegType.ULONG )
                            changes.AddElement<ulong> ( register );
                        break;
                    case TypeCode.Boolean:
                        if ( ( RegType )register.type == RegType.BOOL )
                            changes.AddElement<bool> ( register );
                        break;
                    case TypeCode.Char:
                        if ( ( RegType )register.type == RegType.CHAR )
                            changes.AddElement<char> ( register );
                        break;
                    case TypeCode.String:
                        if ( ( RegType )register.type == RegType.STRING )
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
    }
}
