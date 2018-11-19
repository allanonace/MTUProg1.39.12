using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
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
/// 
/// TODO: 
/// - De momento se crearan set y get para todo registro pero luego se modificara
///   para que en el xml de la familia se indique si cada registro tendra set
/// 
/// - Cuando en el campo custom se indique "Logic" de forma automatica, usando
///   reflexion, se generara la propiedad getter asociada a un metodo llamado
///   igual que el registro ( identificador ) mas el sufijo "_Logic"
///   
/// - Cambiar el planteamiento para que todos los campos que requieran algun calculo especial,
///   como ocurre ahora con los numericos y el elemento "custom", invoquen un metodo con el mismo
///   nombre que el identificador del registro con el sufijo "_Logic", para homogeneizar todo
///
/// - Añadir nuevos tipos de registro ( overload ) a los xml de las familias de los MTUs,
///   que representaran combinaciones de varios registros ( register )
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
        private const string METHODS_CUSTOM_SUFIX = "_Logic";
        private const string METHODS_GET_PREFIX = "get_";
        private const string METHODS_GET_CUSTOM_PREFIX = "getCustom_";
        private const string METHODS_SET_PREFIX = "set_";
        private const string EXCEP_SET_INT      = "String argument can't be casted to int";
        private const string EXCEP_SET_UINT     = "String argument can't be casted to uint";
        private const string EXCEP_SET_ULONG    = "String argument can't be casted to ulong";
        public  const string EXCEP_SET_USED     = "The specified record has not been mapped";
        public  const string EXCEP_SET_READONLY = "The specified record is readonly";

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
                MemRegisterList list;
                try {
                    list = s.Deserialize(reader) as MemRegisterList;
                }
                catch (Exception e)
                {
                    return;
                }

                

                #region Registers
                if (list.Registers != null)
                {
                    foreach (MemRegister xmlRegister in list.Registers)
                    {
                        RegType type = (RegType)Enum.Parse(typeof(RegType), xmlRegister.Type.ToUpper());
                        Type SysType = typeof(System.Object);

                        switch (type)
                        {
                            case RegType.INT: SysType = typeof(int); break;
                            case RegType.UINT: SysType = typeof(uint); break;
                            case RegType.ULONG: SysType = typeof(ulong); break;
                            case RegType.BOOL: SysType = typeof(bool); break;
                            case RegType.CHAR: SysType = typeof(char); break;
                            case RegType.STRING: SysType = typeof(string); break;
                        }

                        // Creates an instance of the generic class
                        dynamic memoryRegister = Activator.CreateInstance(typeof(MemoryRegister<>).MakeGenericType(SysType),
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
                        this.CreateProperty_Get(memoryRegister);
                        if (xmlRegister.Write)
                            this.CreateProperty_Set(memoryRegister);

                        // References to methods to use in property ( .Value )
                        dynamic get = base.registers[METHODS_GET_PREFIX + memoryRegister.id];
                        dynamic getC = (memoryRegister.HasCustomMethod) ? base.registers[METHODS_GET_CUSTOM_PREFIX + memoryRegister.id] : null;
                        dynamic set = (memoryRegister.write) ? base.registers[METHODS_SET_PREFIX + memoryRegister.id] : null;
                        TypeCode tc = Type.GetTypeCode(SysType.GetType());
                        switch (type)
                        {
                            case RegType.INT:
                                memoryRegister.funcGet = (Func<int>)get;
                                memoryRegister.funcGetCustom = (Func<int>)getC;
                                memoryRegister.funcSet = (Action<int>)set;
                                break;
                            case RegType.UINT:
                                memoryRegister.funcGet = (Func<uint>)get;
                                memoryRegister.funcGetCustom = (Func<uint>)getC;
                                memoryRegister.funcSet = (Action<uint>)set;
                                break;
                            case RegType.ULONG:
                                memoryRegister.funcGet = (Func<ulong>)get;
                                memoryRegister.funcGetCustom = (Func<ulong>)getC;
                                memoryRegister.funcSet = (Action<ulong>)set;
                                break;
                            case RegType.BOOL:
                                memoryRegister.funcGet = (Func<bool>)get;
                                memoryRegister.funcGetCustom = (Func<bool>)getC;
                                memoryRegister.funcSet = (Action<bool>)set;
                                break;
                            case RegType.CHAR:
                                memoryRegister.funcGet = (Func<char>)get;
                                memoryRegister.funcGetCustom = (Func<char>)getC;
                                memoryRegister.funcSet = (Action<char>)set;
                                break;
                            case RegType.STRING:
                                memoryRegister.funcGet = (Func<string>)get;
                                memoryRegister.funcGetCustom = (Func<string>)getC;
                                memoryRegister.funcSet = (Action<string>)set;
                                break;
                        }

                        // BAD: Reference to property itself
                        // OK : Reference to register object and use TrySet|GetMember methods
                        //      to override set and get logic, avoiding ExpandoObject problems
                        // NOTA: No se puede usar "base." porque parece ser invalidaria el comportamiento dinamico
                        AddProperty(memoryRegister);

                        // Add new object to collection where will be
                        // filtered to only recover modified registers
                        this.registersObjs.Add(xmlRegister.Id, memoryRegister);
                    }
                }

                #endregion
                #region Overloads

                // Overloads
                if (list.Overloads != null)
                {
                    foreach (MemOverload xmlOverload in list.Overloads)
                    {
                        RegType type = (RegType)Enum.Parse(typeof(RegType), xmlOverload.Type.ToUpper());
                        Type SysType = typeof(System.Object);

                        switch (type)
                        {
                            case RegType.INT: SysType = typeof(int); break;
                            case RegType.UINT: SysType = typeof(uint); break;
                            case RegType.ULONG: SysType = typeof(ulong); break;
                            case RegType.BOOL: SysType = typeof(bool); break;
                            case RegType.CHAR: SysType = typeof(char); break;
                            case RegType.STRING: SysType = typeof(string); break;
                        }

                        // Creates an instance of the generic class
                        dynamic memoryOverload = Activator.CreateInstance(typeof(MemoryOverload<>).MakeGenericType(SysType),
                            xmlOverload.Id,
                            type,
                            xmlOverload.Description,
                            xmlOverload.Registers.Select(o => o.Id).ToArray(),
                            xmlOverload.Custom);

                        this.CreateOverload_Get(memoryOverload);

                        dynamic get = base.registers[METHODS_GET_PREFIX + memoryOverload.id];
                        TypeCode tc = Type.GetTypeCode(SysType.GetType());
                        switch (type)
                        {
                            case RegType.INT: memoryOverload.funcGet = (Func<int>)get; break;
                            case RegType.UINT: memoryOverload.funcGet = (Func<uint>)get; break;
                            case RegType.ULONG: memoryOverload.funcGet = (Func<ulong>)get; break;
                            case RegType.BOOL: memoryOverload.funcGet = (Func<bool>)get; break;
                            case RegType.CHAR: memoryOverload.funcGet = (Func<char>)get; break;
                            case RegType.STRING: memoryOverload.funcGet = (Func<string>)get; break;
                        }

                        AddProperty(memoryOverload);
                    }
                }

                #endregion
            }

            #region Tests

            // TEST: Diferentes opciones campo custom ( metodo y operacion matematica )
            //Console.WriteLine ( "Test custom operation: " + base.registers.BatteryVoltage );
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
            //Console.WriteLine ( base.registers.Overload_Method + "" );
            //Console.WriteLine ( base.registers.Overload_Operation + "" );

            #endregion
        }

        #endregion

        #region Create Property Get

        /*
        private void CreateProperty_Get_Custom<T> ( Func<T> function )
        {
            string id = function.Method.Name;

            base.dictionary.Add( METHODS_GET_PREFIX + id, new Func<T>(() => {
                return function.Invoke ();
            }));
        }
        */

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
                        case TypeCode.String : result = ( object )this.GetStringFromMem(memoryRegister.address, memoryRegister.custom); break;
                    }

                    // Numeric field with operation to evaluate
                    if ( memoryRegister.HasCustomOperation )
                        return this.GetOperation<T> ( result, memoryRegister.custom );

                    // String field with format to apply
                    else if ( memoryRegister.HasCustomFormat )
                        return ( T )( object )this.GetStringFormatted ( ( string )result, memoryRegister.custom );

                    // Only return readed value
                    return ( T )result;
                }));

            // But only someone have special get block/method defined on MTU family classes
            if ( memoryRegister.HasCustomMethod )
            {
                MethodInfo custoMethod = this.GetType().GetMethod (
                    memoryRegister.id + METHODS_CUSTOM_SUFIX,
                    new Type[] { typeof( MemoryRegister<T> ) } );

                base.AddMethod ( METHODS_GET_CUSTOM_PREFIX + memoryRegister.id,
                    new Func<T>(() =>
                    {
                        return ( T )custoMethod.Invoke ( this, new object[] { memoryRegister } );
                    }));
            }
        }

        private void CreateOverload_Get<T> ( MemoryOverload<T> memoryOverload )
        {
            MethodInfo custoMethod = ( ! memoryOverload.HasCustomMethod ) ?
                null : this.GetType().GetMethod (
                    memoryOverload.id + METHODS_CUSTOM_SUFIX,
                    new Type[] { typeof ( MemoryOverload<T> ), typeof( ExpandoObject ) } );

            dynamic registersToUse = new ExpandoObject ();
            IDictionary<string,dynamic> dictionary = registersToUse;
            foreach ( string id in memoryOverload.registerIds )
                dictionary[ id ] = base.registers[ id ];

            // Overloads only have get block ( are readonly )
            base.AddMethod ( METHODS_GET_PREFIX + memoryOverload.id,
                new Func<T>(() =>
                {
                    // Use custom method
                    if ( memoryOverload.HasCustomMethod )
                        return ( T )custoMethod.Invoke ( this, new object[] { memoryOverload, registersToUse } );

                    return default( T );

                    // Operation to evaluate
                    //else
                    //    return this.GetOperation<T> ( result, memoryRegister.custom );
                }));
        }

        #endregion

        #region Create Property Set

        public void CreateProperty_Set<T>( MemoryRegister<T> regObj )
        {
            base.AddMethod ( METHODS_SET_PREFIX + regObj.id,
                new Action<T>((_value) =>
                {
                    switch (Type.GetTypeCode(typeof(T)))
                    {
                        case TypeCode.Int32  : this.SetIntToMem   ((int   )(object)_value, regObj.address, regObj.size); break;
                        case TypeCode.UInt32 : this.SetUIntToMem  ((uint  )(object)_value, regObj.address, regObj.size); break;
                        case TypeCode.UInt64 : this.SetULongToMem ((ulong )(object)_value, regObj.address, regObj.size); break;
                        case TypeCode.Boolean: this.SetBoolToMem  ((bool  )(object)_value, regObj.address, regObj.size); break;
                        case TypeCode.Char   : this.SetCharToMem  ((char  )(object)_value, regObj.address); break;
                        case TypeCode.String : this.SetStringToMem((string)(object)_value, regObj.address); break;
                    }
                }));
        }

        #endregion

        #region Get value

        private T GetOperation<T> ( object value, string operation )
        {
            // The following arithmetic operators are supported in expressions: +, -, *, / y %
            // NOTA: No se puede hacer la conversion directa de un double a un entero generico
            //return ( T )( new DataTable ().Compute ( operation.Replace ( "_val_", value.ToString () ), null ) );

            object result = new DataTable().Compute(operation.Replace("_val_", value.ToString()), null );

            switch ( Type.GetTypeCode( typeof( T )) )
            {
                case TypeCode.Int32  : result = Convert.ToInt32  ( result ); break;
                case TypeCode.UInt32 : result = Convert.ToUInt32 ( result ); break;
                case TypeCode.UInt64 : result = Convert.ToInt64  ( result ); break;
            }

            //Console.WriteLine ( "GetOperation: " + operation + " | " + value );

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

        protected string GetStringFromMem(int address, string format)
        {
            return string.Format(format, memory[address]);
        }

        #endregion

        #region Set value

        protected void SetIntToMem (int value, int address, int size = MemRegister.DEF_SIZE)
        {
            for ( int b = 0; b < size; b++ )
                this.memory[address + b] = ( byte )( value >> (b * 8) );
        }

        protected void SetIntToMem (string value, int address, int size = MemRegister.DEF_SIZE)
        {
            int vCasted;
            if ( int.TryParse(value, out vCasted) )
                throw new SetMemoryFormatException ( EXCEP_SET_INT + ": " + value);
            else
                for (int b = 0; b < size; b++)
                    this.memory[address + b] = (byte)(vCasted >> (b * 8));
        }

        protected void SetUIntToMem (uint value, int address, int size = MemRegister.DEF_SIZE)
        {
            for (int b = 0; b < size; b++)
                this.memory[address + b] = (byte)(value >> (b * 8));
        }

        protected void SetUIntToMem (string value, int address, int size = MemRegister.DEF_SIZE)
        {
            uint vCasted;
            if (uint.TryParse(value, out vCasted))
                throw new SetMemoryFormatException ( EXCEP_SET_UINT + ": " + value);
            else
                for (int b = 0; b < size; b++)
                    this.memory[address + b] = (byte)(vCasted >> (b * 8));
        }

        protected void SetULongToMem (ulong value, int address, int size = MemRegister.DEF_SIZE)
        {
            for (int b = 0; b < size; b++)
                this.memory[address + b] = (byte)(this.ULongToBcd(value.ToString ()) >> (b * 8));
        }

        protected void SetULongToMem (string value, int address, int size = MemRegister.DEF_SIZE)
        {
            ulong vCasted;
            if (ulong.TryParse(value, out vCasted))
                throw new SetMemoryFormatException ( EXCEP_SET_ULONG + ": " + value);
            else
                for (int b = 0; b < size; b++)
                    this.memory[address + b] = (byte)( this.ULongToBcd(value) >> (b * 8));
        }

        protected void SetBoolToMem (bool value, int address, int bit_index = MemRegister.DEF_BIT)
        {
            memory[address] = ( byte ) ( memory[address] | (1 << bit_index) );
        }

        protected void SetCharToMem (char value, int address)
        {
            this.memory[address] = (byte)value;
        }

        protected void SetStringToMem (string value, int address)
        {
            this.memory[address] = (byte)Int32.Parse(value);
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

        #region CommonmeThods

        public string DailySnap_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
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

        public string MtuStatus_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return MemoryRegisters.Shipbit.Value ? "OFF" : "ON";
        }

        public string ReadInterval_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return timeFormatter(MemoryRegisters.ReadIntervalMinutes.Value);
        }

        public string XmitInterval_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return timeFormatter(MemoryRegisters.ReadIntervalMinutes.Value * MemoryRegisters.MessageOverlapCount.Value);
        }

        public string PCBNumber_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return string.Format("{0}-{1:000000000}-{2}",
                Convert.ToChar(MemoryRegisters.PCBSupplierCode.Value),
                MemoryRegisters.PCBCoreNumber.Value,
                Convert.ToChar(MemoryRegisters.PCBProductRevision.Value));
        }

        public string MtuSoftware_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return string.Format("Version {0:00}", MemoryRegisters.MTUFirmwareVersionFormatFlag.Value);
        }

        public string Encryption_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return MemoryRegisters.Encrypted.Value ? "Yes" : "No";
        }

        public string MtuVoltageBattery_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return ((MemoryRegisters.MtuMiliVoltageBattery.Value * 1.0) / 1000).ToString("0.00 V");
        }

        public string P1ReadingError_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return translateErrorCodes(MemoryRegisters.P1ReadingErrorCode.Value);
        }

        public string P2ReadingError_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return translateErrorCodes(MemoryRegisters.P2ReadingErrorCode.Value);
        }
        #endregion

        #region AuxiliaryFunctions


        private string translateErrorCodes(int encoderErrorcode)
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

        private string timeFormatter(int time)
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

        public string GetTemperStatus(bool alarm, bool temper)
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

        public string InterfaceTamperStatus_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return GetTemperStatus(MemoryRegisters.P1PciAlarm.Value, MemoryRegisters.ProgrammingCoilInterfaceTamper.Value);
        }

        public string TiltTamperStatus_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return GetTemperStatus(MemoryRegisters.P1TiltAlarm.Value, MemoryRegisters.TiltTamper.Value);
        }

        public string MagneticTamperStatus_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return GetTemperStatus(MemoryRegisters.P1MagneticAlarm.Value, MemoryRegisters.MagneticTamper.Value);
        }

        public string RegisterCoverTamperStatus_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return GetTemperStatus(MemoryRegisters.P1RegisterCoverAlarm.Value, MemoryRegisters.RegisterCoverTamper.Value);
        }

        public string ReverseFlowTamperStatus_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {

            return GetTemperStatus(MemoryRegisters.P1ReverseFlowAlarm.Value, MemoryRegisters.ReverseFlowTamper.Value);
        }

        public string F12WAYRegister1_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return "0x" + MemoryRegisters.F12WAYRegister1Int.Value.ToString("X8");
        }

        public string F12WAYRegister10_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return "0x" + MemoryRegisters.F12WAYRegister10Int.Value.ToString("X8");
        }

        public string F12WAYRegister14_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return "0x" + MemoryRegisters.F12WAYRegister14Int.Value.ToString("X8");
        }


        public string BackFlowState_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
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


        public string DaysOfNoFlow_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
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

        public string LeakDetection_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
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

        public string DaysOfLeak_Logic(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
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
    }
}
