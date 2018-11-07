using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Xamarin.Forms;
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
        private const string METHODS_SET_PREFIX = "set_";
        private const string EXCEP_SET_INT      = "String argument can't be casted to int";
        private const string EXCEP_SET_UINT     = "String argument can't be casted to uint";
        private const string EXCEP_SET_ULONG    = "String argument can't be casted to ulong";
        public  const string EXCEP_SET_USED     = "The specified record has not been mapped";
        public  const string EXCEP_SET_READONLY = "The specified record is readonly";

        #endregion

        #region Attributes

        protected byte[] memory { private set; get; }
        private Dictionary<string,dynamic> registersObjs;

        #endregion

        #region Properties

        // Hides parent inherited member
        //public new dynamic registers { get { return base.registers; } }

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

                    // Creates an instance of the generic class RegisterObj of type selected
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
                    this.CreateProperty_Get ( memoryRegister );
                    if ( xmlRegister.Write )
                        this.CreateProperty_Set ( memoryRegister );

                    // References to methods to use in property ( .Value )
                    dynamic get = base.dictionary[ METHODS_GET_PREFIX + memoryRegister.id ];
                    dynamic set = ( xmlRegister.Write ) ? base.dictionary[ METHODS_SET_PREFIX + memoryRegister.id ] : null;
                    TypeCode tc = Type.GetTypeCode(SysType.GetType());
                    switch (type)
                    {
                        case RegType.INT:
                            memoryRegister.funcGet = (Func<int>)get;
                            if ( xmlRegister.Write )
                                memoryRegister.funcSet = (Action<int>)set;
                            break;
                        case RegType.UINT:
                            memoryRegister.funcGet = (Func<uint>)get;
                            if ( xmlRegister.Write )
                                memoryRegister.funcSet = (Action<uint>)set;
                            break;
                        case RegType.ULONG:
                            memoryRegister.funcGet = (Func<ulong>)get;
                            if ( xmlRegister.Write )
                                memoryRegister.funcSet = (Action<ulong>)set;
                            break;
                        case RegType.BOOL:
                            memoryRegister.funcGet = (Func<bool>)get;
                            if ( xmlRegister.Write )
                                memoryRegister.funcSet = (Action<bool>)set;
                            break;
                        case RegType.CHAR:
                            memoryRegister.funcGet = (Func<char>)get;
                            if ( xmlRegister.Write )
                                memoryRegister.funcSet = (Action<char>)set;
                            break;
                        case RegType.STRING:
                            memoryRegister.funcGet = (Func<string>)get;
                            if ( xmlRegister.Write )
                                memoryRegister.funcSet = (Action<string>)set;
                            break;
                    }

                    // BAD: Reference to property itself
                    // OK : Reference to register object and use TrySet|GetMember methods
                    //      to override set and get logic, avoiding ExpandoObject problems
                    // NOTA: No se puede usar "base." porque parece ser invalidaria el comportamiento dinamico
                    AddProperty ( memoryRegister );

                    // Add new object to collection where will be
                    // filtered to only recover modified registers
                    this.registersObjs.Add(xmlRegister.Id, memoryRegister);
                }
            }

            /*
            Console.WriteLine ( "--------------------" );

            Console.WriteLine ( "Test custom methods: " + base.registers.DailyRead );
            Console.WriteLine ( "Test custom operation: " + base.registers.BatteryVoltage );
            Console.WriteLine ( "Test custom format: " + base.registers.MtuFirmwareVersion );

            Console.WriteLine ( "Test lectura 1: " + base.registers.MtuType);
            this.registers.MtuType = 12321; // Not allow
            Console.WriteLine ( "Test lectura 2: " + this.registers.MtuType);
            */
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
            // Register uses normal get block
            if ( ! memoryRegister.HasCustomMethod )
            {
                base.dictionary.Add( METHODS_GET_PREFIX + memoryRegister.id,
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
            }
            // Register uses special get block/method defined on MTU family classes
            else
            {
                MethodInfo custoMethod = this.GetType().GetMethod ( memoryRegister.id + METHODS_CUSTOM_SUFIX );

                base.dictionary.Add( METHODS_GET_PREFIX + memoryRegister.id,
                    new Func<T>(() =>
                    {
                        return ( T )custoMethod.Invoke ( this, null );
                    }));
            }
        }

        #endregion

        #region Create Property Set

        public void CreateProperty_Set<T>( MemoryRegister<T> regObj )
        {
            base.dictionary.Add( METHODS_SET_PREFIX + regObj.id,
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

        #region Get

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

        #region Set

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

        /*
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
        */

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
            List<MemoryRegister<T>> list = new List<MemoryRegister<T>> ();

            foreach ( dynamic register in mixedRegisters )
            {
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
