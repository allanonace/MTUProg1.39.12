using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Xamarin.Forms;
using Xml;

/// <summary>
/// LOGIC:
/// - 1. In constructor loads current MTU family XML file and iterate over all registers
/// - 2. For each register creates an instance of the corresponding specialized RegisterObj type
/// - 3. Creates Get and Set methods for each register, leaving all parameters setted but value to be setted
///      CreateProperty_Get|Set_XXX methods create function/action delegates to simulate property, but logic
///      of new properties is inside GetXXXFromMem and SetXXXToMem methods, working with memory bytes[]
/// - 4. Pass Set and Get methods references to each object, that will be used on Value property inside objects ( registers.get|set_id )
/// - 5. Creates new ExpandoObject member with register name, asociating Value property to them ( registers.id )
/// - 6. Adds instances to registersObjs dictionary that will be filtered to recover only modified registers
/// - 7. Registers ExpandoObject PropertyChanged event, inside AMemoryMap class in IMemoryMap.cs file, to avoid override
///      member when using asignment operator ( "=" / set ). The main problem working with ExpandoObject type, is that use
///      property get block or invoke a method works perfect, but assign ( set ) always overrides dynamic member. The trick
///      is to create dictionary copy, after have created all registers methods and properties, and use PropertyChanged
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
///      Never need to to call directly to registers.get_idProperty nor registers.set_idProperty, because
///      Value property do it for us, invoking funcGet and funcSet methods
///      
/// CURRENT:
/// - Seems that get methods are created ok but when call CreateProperty_Set_XXX breaks
///   Inside CreateProperty_Set_Int logic sentence is commented
/// - Also breaks when try to set funcGet|Set references to the methods
/// 
/// TODO: 
/// - De momento se crearan set y get para todo registro pero luego se modificara
///   para que en el xml de la familia se indique si cada registro tendra set
/// 
/// - Cuando en el campo custom se indique "Logic" de forma automatica, usando
///   reflexion, se generara la propiedad getter asociada a un metodo llamado
///   igual que el registro ( identificador ) mas el sufijo "_Logic"
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

        private const string XML_PREFIX = "family_";
        private const string XML_EXTENSION = ".xml";
        private const string METHODS_GET_PREFIX = "get_";
        private const string METHODS_SET_PREFIX = "set_";
        private const string EXCEP_SET_INT = "String argument can't be casted to int";
        private const string EXCEP_SET_UINT = "String argument can't be casted to uint";
        private const string EXCEP_SET_ULONG = "String argument can't be casted to ulong";
        private const string EXCEP_SET_USED = "The specified record has not been mapped";

        #endregion

        #region Nested class

        public class RegisterObj
        {
            public Func<object> funcGet;
            public Action<object> funcSet;
            private byte[] _memory;
            public string id { get; }
            public string description { get; }
            public RegType type { get; }
            public int address { get; }
            public int size { get; }
            public string custom { get; }
            public bool used;

            public ref byte[] memory
            {
                get { return ref this._memory; }
            }

            // Size element is also used for bit with bool type
            public int bit { get { return this.size; } }

            public RegisterObj (
                ref byte[] memory,
                string id,
                RegType type,
                string description,
                int address,
                int size = 1,
                string special = "" )
            {
                this._memory = memory;
                this.id = id;
                this.type = type;
                this.description = description;
                this.address = address;
                this.size = size;
                this.custom = special;
            }

            public virtual object Value
            {
                get
                {
                    Console.WriteLine("RegisterObj: Get");

                    return this.funcGet ();
                }
                set
                {
                    Console.WriteLine("RegisterObj: Set");

                    this.funcSet ( value );
                }
            }
        }

        #region Specialized classes

        private class RegisterObj_Int : RegisterObj
        {
            public new int Value
            {
                get
                {
                    Console.WriteLine("RegisterObj: Get Int");

                    return ( int )base.funcGet();
                }
                set
                {
                    Console.WriteLine("RegisterObj: Set Int");

                    base.funcSet ( value );
                }
            }

            public RegisterObj_Int (
                ref byte[] memory,
                string id,
                RegType type,
                string description,
                int address,
                int size = 1,
                string special = "") : base ( ref memory, id, type, description, address, size, special ) {}
        }

        private class RegisterObj_UInt : RegisterObj
        {
            public new uint Value
            {
                get
                {
                    Console.WriteLine("RegisterObj: Get UInt");

                    return (uint)base.funcGet();
                }
                set
                {
                    Console.WriteLine("RegisterObj: Set UInt");

                    base.funcSet(value);
                }
            }

            public RegisterObj_UInt(
                ref byte[] memory,
                string id,
                RegType type,
                string description,
                int address,
                int size = 1,
                string special = "") : base(ref memory, id, type, description, address, size, special) { }
        }

        private class RegisterObj_ULong : RegisterObj
        {
            public new ulong Value
            {
                get
                {
                    Console.WriteLine("RegisterObj: Get ULong");

                    return (ulong)base.funcGet();
                }
                set
                {
                    Console.WriteLine("RegisterObj: Set ULong");

                    base.funcSet(value);
                }
            }

            public RegisterObj_ULong(
                ref byte[] memory,
                string id,
                RegType type,
                string description,
                int address,
                int size = 1,
                string special = "") : base(ref memory, id, type, description, address, size, special) { }
        }

        private class RegisterObj_Bool : RegisterObj
        {
            public new bool Value
            {
                get
                {
                    Console.WriteLine("RegisterObj: Get Bool");

                    return (bool)base.funcGet();
                }
                set
                {
                    Console.WriteLine("RegisterObj: Set Bool");

                    base.funcSet(value);
                }
            }

            public RegisterObj_Bool(
                ref byte[] memory,
                string id,
                RegType type,
                string description,
                int address,
                int size = 1,
                string special = "") : base(ref memory, id, type, description, address, size, special) { }
        }

        private class RegisterObj_Char : RegisterObj
        {
            public new char Value
            {
                get
                {
                    Console.WriteLine("RegisterObj: Get Char");

                    return (char)base.funcGet();
                }
                set
                {
                    Console.WriteLine("RegisterObj: Set Char");

                    base.funcSet(value);
                }
            }

            public RegisterObj_Char(
                ref byte[] memory,
                string id,
                RegType type,
                string description,
                int address,
                int size = 1,
                string special = "") : base(ref memory, id, type, description, address, size, special) { }
        }

        private class RegisterObj_String : RegisterObj
        {
            public new string Value
            {
                get
                {
                    Console.WriteLine("RegisterObj: Get String");

                    return (string)base.funcGet();
                }
                set
                {
                    Console.WriteLine("RegisterObj: Set String");

                    base.funcSet(value);
                }
            }

            public RegisterObj_String(
                ref byte[] memory,
                string id,
                RegType type,
                string description,
                int address,
                int size = 1,
                string special = "") : base(ref memory, id, type, description, address, size, special) { }
        }

        #endregion

        #endregion

        #region Attributes

        protected byte[] memory { private set; get; }
        private Dictionary<string,RegisterObj> registersObjs;

        #endregion

        #region Properties

        // Hides parent inherited member
        public new dynamic registers
        {
            get { return base.registers; }
        }

        #endregion

        #region Initialization

        public MemoryMap ( byte[] memory, string family )
        {
            this.memory = memory;
            this.registersObjs = new Dictionary<string, RegisterObj>();

            // Read MTU family XML and prepare setters and getters
            var xml_documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (Device.RuntimePlatform == Device.Android)
                xml_documents = xml_documents.Replace("/data/user/0/", "/storage/emulated/0/Android/data/");

            RegisterObj regObj = null;
            XmlSerializer s = new XmlSerializer ( typeof ( MemRegisterList ) );
            using (TextReader reader = new StreamReader(Path.Combine(xml_documents, XML_PREFIX + family + XML_EXTENSION)))
            {
                MemRegisterList list = s.Deserialize(reader) as MemRegisterList;
                foreach ( MemRegister reg in list.Registers )
                {
                    RegType type = ( RegType )Enum.Parse ( typeof( RegType ), reg.Type.ToUpper () );

                    Console.WriteLine ( "New register: " + reg.Id + " <" + type + ">" );

                    // Can be created RegisterObj class as generic because could not define
                    // registersObjs dictionary using object, and each register type need their
                    // own RegisterObj specialized class, only overriding Value property
                    switch ( type )
                    {
                        case RegType.INT:
                            regObj = new RegisterObj_Int (
                                ref memory,
                                reg.Id,
                                type,
                                reg.Description,
                                reg.Address,
                                reg.Size,
                                reg.Custom );
                            this.CreateProperty_Get_Int ( regObj );
                            this.CreateProperty_Set_Int ( regObj );
                            break;
                        case RegType.UINT:
                            regObj = new RegisterObj_UInt (
                                ref memory,
                                reg.Id,
                                type,
                                reg.Description,
                                reg.Address,
                                reg.Size,
                                reg.Custom );
                            this.CreateProperty_Get_UInt(regObj);
                            this.CreateProperty_Set_UInt(regObj);
                            break;
                        case RegType.ULONG:
                            regObj = new RegisterObj_ULong (
                                ref memory,
                                reg.Id,
                                type,
                                reg.Description,
                                reg.Address,
                                reg.Size,
                                reg.Custom );
                            this.CreateProperty_Get_ULong(regObj);
                            this.CreateProperty_Set_ULong(regObj);
                            break;
                        case RegType.BOOL:
                            regObj = new RegisterObj_Bool (
                                ref memory,
                                reg.Id,
                                type,
                                reg.Description,
                                reg.Address,
                                reg.Size,
                                reg.Custom );
                            this.CreateProperty_Get_Bool(regObj);
                            this.CreateProperty_Set_Bool(regObj);
                            break;
                        case RegType.CHAR:
                            regObj = new RegisterObj_Char (
                                ref memory,
                                reg.Id,
                                type,
                                reg.Description,
                                reg.Address,
                                reg.Size,
                                reg.Custom );
                            this.CreateProperty_Get_Char(regObj);
                            this.CreateProperty_Set_Char(regObj);
                            break;
                        case RegType.STRING:
                            regObj = new RegisterObj_String (
                                ref memory,
                                reg.Id,
                                type,
                                reg.Description,
                                reg.Address,
                                reg.Size,
                                reg.Custom );
                            this.CreateProperty_Get_String(regObj);
                            this.CreateProperty_Set_String(regObj);
                            break;
                    }

                    Console.WriteLine ( "Exist get method? " + this.dictionary.ContainsKey ( METHODS_GET_PREFIX + regObj.id ) );
                    Console.WriteLine ( "Exist set method? " + this.dictionary.ContainsKey ( METHODS_SET_PREFIX + regObj.id ) );

                    // References to methods to use in property ( .Value )
                    regObj.funcGet = ( Func<object> )this.dictionary[ METHODS_GET_PREFIX + regObj.id ];
                    regObj.funcSet = ( Action<object> )this.dictionary[ METHODS_SET_PREFIX + regObj.id ];

                    // Reference to property itself
                    base.dictionary[ regObj.id ] = regObj.Value;

                    // Add new object to collection where will be
                    // filtered to only recover modified registers
                    this.registersObjs.Add(reg.Id, regObj);
                }
            }

            // Register event that allow to overpass ExpandoObject limitation
            // using asignment operator that override class member with pass value
            base.AddModifyEvent ();

            Console.WriteLine ( "Test lectura 1: " + this.registers.MtuType );
            this.registers.MtuType = 33;
            Console.WriteLine ( "Test lectura 2: " + this.registers.MtuType );
        }

        #endregion

        #region Create Property Get

        protected void CreateProperty_Get_Custom<TResul> ( Func<TResul> function )
        {
            string id = function.Method.Name;

            Console.WriteLine( "MemoryMap -> Create Property Custom [ GET ]: " + id);

            this.dictionary.Add( METHODS_GET_PREFIX + id, new Func<TResul>(() => {
                return function.Invoke ();
            }));
        }

        private void CreateProperty_Get_Int ( RegisterObj regObj )
        {
            Console.WriteLine("MemoryMap -> Create Property Int [ GET ]: " + METHODS_GET_PREFIX + regObj.id );
            
            this.dictionary.Add( METHODS_GET_PREFIX + regObj.id,
                new Func<int>(() =>
                {
                    Console.WriteLine("MemoryMap -> Property Int [ GET ]: " + regObj.id);

                    return this.GetIntFromMem ( regObj.address, regObj.size );
                }));
        }

        private void CreateProperty_Get_UInt ( RegisterObj regObj )
        {
            Console.WriteLine("MemoryMap -> Create Property UInt [ GET ]: " + regObj.id);

            this.dictionary.Add( METHODS_GET_PREFIX + regObj.id, new Func<uint>(() => {
                return this.GetUIntFromMem( regObj.address, regObj.size );
            }));
        }

        private void CreateProperty_Get_ULong ( RegisterObj regObj )
        {
            Console.WriteLine("MemoryMap -> Create Property ULong [ GET ]: " + regObj.id);

            this.dictionary.Add( METHODS_GET_PREFIX + regObj.id, new Func<ulong>(() => {
                return this.GetULongFromMem( regObj.address, regObj.size );
            }));
        }

        private void CreateProperty_Get_Bool ( RegisterObj regObj )
        {
            Console.WriteLine("MemoryMap -> Create Property Bool [ GET ]: " + regObj.id);

            this.dictionary.Add( METHODS_GET_PREFIX + regObj.id, new Func<bool>(() => {
                return this.GetBoolFromMem( regObj.address, regObj.bit );
            }));
        }

        private void CreateProperty_Get_Char ( RegisterObj regObj )
        {
            Console.WriteLine("MemoryMap -> Create Property Char [ GET ]: " + regObj.id);

            this.dictionary.Add( METHODS_GET_PREFIX + regObj.id, new Func<char>(() => {
                return this.GetCharFromMem(regObj.address);
            }));
        }

        private void CreateProperty_Get_String ( RegisterObj regObj )
        {
            Console.WriteLine("MemoryMap -> Create Property String [ GET ]: " + regObj.id);

            this.dictionary.Add( METHODS_GET_PREFIX + regObj.id, new Func<string>(() => {
                return this.GetStringFromMem(regObj.address, regObj.custom);
            }));
        }

        #endregion

        #region Create Property Set

        public void CreateProperty_Set_Int ( RegisterObj regObj )
        {
            Console.WriteLine("MemoryMap -> Create Property Int [ SET ]: " + METHODS_SET_PREFIX + regObj.id );

            this.dictionary.Add( METHODS_SET_PREFIX + regObj.id,
                new Action<int>((_value) =>
                {
                    Console.WriteLine("MemoryMap -> Property Int [ SET ]: " + regObj.id + " Value: " + _value );

                    //this.SetIntToMem(_value, regObj.address, regObj.size );
                }));
        }

        public void CreateProperty_Set_UInt ( RegisterObj regObj )
        {
            Console.WriteLine("MemoryMap -> Create Property UInt [ SET ]: " + regObj.id);

            this.dictionary.Add( METHODS_SET_PREFIX + regObj.id,
                new Action<uint>((_value) =>
                {
                    this.SetUIntToMem(_value, regObj.address, regObj.size);
                }));
        }

        public void CreateProperty_Set_ULong ( RegisterObj regObj )
        {
            Console.WriteLine("MemoryMap -> Create Property ULong [ SET ]: " + regObj.id);

            this.dictionary.Add( METHODS_SET_PREFIX + regObj.id,
                new Action<ulong>((_value) =>
                {
                    this.SetULongToMem(_value, regObj.address, regObj.size);
                }));
        }

        public void CreateProperty_Set_Bool ( RegisterObj regObj )
        {
            Console.WriteLine("MemoryMap -> Create Property Bool [ SET ]: " + regObj.id);

            this.dictionary.Add( METHODS_SET_PREFIX + regObj.id,
                new Action<bool>((_value) =>
                {
                    this.SetBoolToMem(_value, regObj.address, regObj.bit);
                }));
        }

        public void CreateProperty_Set_Char ( RegisterObj regObj )
        {
            Console.WriteLine("MemoryMap -> Create Property Char [ SET ]: " + regObj.id);

            this.dictionary.Add( METHODS_SET_PREFIX + regObj.id,
                new Action<char>((_value) =>
                {
                    this.SetCharToMem(_value, regObj.address);
                }));
        }

        public void CreateProperty_Set_String ( RegisterObj regObj )
        {
            Console.WriteLine("MemoryMap -> Create Property String [ SET ]: " + regObj.id);

            this.dictionary.Add( METHODS_SET_PREFIX + regObj.id,
                new Action<string>((_value) =>
                {
                    this.SetStringToMem(_value, regObj.address);
                }));
        }

        #endregion

        #region Get

        protected int GetIntFromMem(int address, int size = 1)
        {
            int value = 0;
            for (int b = 0; b < size; b++)
                value += memory[address + b] << (b * 8);

            return value;
        }

        protected uint GetUIntFromMem(int address, int size = 1)
        {
            uint value = 0;
            for (int b = 0; b < size; b++)
                value += (uint)(memory[address + b] << (b * 8));

            return value;
        }

        protected ulong GetULongFromMem(int address, int size = 1)
        {
            ulong value = 0;
            for (int b = 0; b < size; b++)
                value += (ulong)memory[address + b] << (b * 8);

            return BcdToULong(value);
        }

        protected bool GetBoolFromMem(int address, int bit_index = 0)
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

        protected override void SetValueAfterAvoidOverride ( string id, object value )
        {
            RegisterObj obj = this.registersObjs[ id ];
            switch ( obj.type )
                {
                    case RegType.INT:
                        (( Action<int> )this.dictionary[ METHODS_SET_PREFIX + id ]) ( (int)value );
                        break;
                    case RegType.UINT:
                        (( Action<uint> )this.dictionary[ METHODS_SET_PREFIX + id ]) ( (uint)value );
                        break;
                    case RegType.ULONG:
                        (( Action<ulong> )this.dictionary[ METHODS_SET_PREFIX + id ]) ( (ulong)value );
                        break;
                    case RegType.BOOL:
                        (( Action<bool> )this.dictionary[ METHODS_SET_PREFIX + id ]) ( (bool)value );
                        break;
                    case RegType.CHAR:
                        (( Action<char> )this.dictionary[ METHODS_SET_PREFIX + id ]) ( (char)value );
                        break;
                    case RegType.STRING:
                        (( Action<string> )this.dictionary[ METHODS_SET_PREFIX + id ]) ( (string)value );
                        break;
                }
        }

        protected void SetIntToMem (int value, int address, int size = 1)
        {
            for ( int b = 0; b < size; b++ )
                this.memory[address + b] = ( byte )( value >> (b * 8) );
        }

        protected void SetIntToMem (string value, int address, int size = 1)
        {
            int vCasted;
            if ( int.TryParse(value, out vCasted) )
                throw new SetMemoryFormatException ( EXCEP_SET_INT + ": " + value);
            else
                for (int b = 0; b < size; b++)
                    this.memory[address + b] = (byte)(vCasted >> (b * 8));
        }

        protected void SetUIntToMem (uint value, int address, int size = 1)
        {
            for (int b = 0; b < size; b++)
                this.memory[address + b] = (byte)(value >> (b * 8));
        }

        protected void SetUIntToMem (string value, int address, int size = 1)
        {
            uint vCasted;
            if (uint.TryParse(value, out vCasted))
                throw new SetMemoryFormatException ( EXCEP_SET_UINT + ": " + value);
            else
                for (int b = 0; b < size; b++)
                    this.memory[address + b] = (byte)(vCasted >> (b * 8));
        }

        protected void SetULongToMem (ulong value, int address, int size = 1)
        {
            for (int b = 0; b < size; b++)
                this.memory[address + b] = (byte)(this.ULongToBcd(value.ToString ()) >> (b * 8));
        }

        protected void SetULongToMem (string value, int address, int size = 1)
        {
            ulong vCasted;
            if (ulong.TryParse(value, out vCasted))
                throw new SetMemoryFormatException ( EXCEP_SET_ULONG + ": " + value);
            else
                for (int b = 0; b < size; b++)
                    this.memory[address + b] = (byte)( this.ULongToBcd(value) >> (b * 8));
        }

        protected void SetBoolToMem (bool value, int address, int bit_index = 0)
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

        public RegisterObj[] GetModifiedRegisters ()
        {
            return this.registersObjs.Where(reg => reg.Value.used)
                .ToDictionary(reg => reg.Key, reg => reg.Value)
                .Values.ToArray<RegisterObj> ();
        }

        #endregion
    }
}
