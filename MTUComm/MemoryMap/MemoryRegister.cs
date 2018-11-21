using System;
using Xml;

using REGISTER_TYPE = MTUComm.MemoryMap.AMemoryMap.REGISTER_TYPE;
using RegType       = MTUComm.MemoryMap.MemoryMap.RegType;

namespace MTUComm.MemoryMap
{
    public class MemoryRegister<T>
    {
        #region Constants

        private enum CUSTOM_TYPE { EMPTY, METHOD, OPERATION, FORMAT }

        #endregion

        #region Attributes

        public Func<T> funcGet;                 // MemoryRegister.Value{get}
        public Func<T> funcGetCustom;           // Only use working dynamically ( IMemoryRegister.Get )
        public Action<T> funcSet;               // MemoryRegister.Value{set}
        public Func<dynamic,dynamic> funcSetCustom;         // MemoryRegister.Value{set}
        public Action<string> funcSetString;    // MemoryRegister.Value{set}
        public Action<byte[]> funcSetByteArray; // MemoryRegister.Value{set}
        public string id { get; }
        public string description { get; }
        public RegType valueType { get; }
        public int address { get; }
        public int size { get; }
        public bool write { get; }
        private string custom_Get { get; }
        private string custom_Set { get; }
        public string methodId_Get { get; }
        public string methodId_Set { get; }
        private CUSTOM_TYPE customType_Get;
        private CUSTOM_TYPE customType_Set;
        public bool used;
        public REGISTER_TYPE registerType { get; }

        #endregion

        #region Properties

        // Value size ( number of consecutive bytes ) is also used for bit with bool type
        public int bit { get { return this.size; } }

        public string mathExpression_Get { get { return this.custom_Get; } }
        public string mathExpression_Set { get { return this.custom_Set; } }
        public string format_Get { get { return this.custom_Get; } }
        public string format_Set { get { return this.custom_Set; } }

        #region Custom Get

        private bool _HasCustomMethod_Get
        {
            get { return this.custom_Get.ToLower ().StartsWith ( MemoryMap.METHOD ); }
        }

        private bool _HasCustomMethodId_Get
        {
            get { return this.custom_Get.ToLower ().StartsWith ( MemoryMap.METHOD_KEY ); }
        }

        private bool _HasCustomOperation_Get
        {
            get { return ! this._HasCustomMethod_Get     &&
                           this.valueType < RegType.CHAR &&
                         ! string.IsNullOrEmpty ( this.custom_Get ); }
        }

        private bool _HasCustomFormat_Get
        {
            get { return ! this._HasCustomMethod_Get        &&
                           this.valueType == RegType.STRING &&
                         ! string.IsNullOrEmpty ( this.custom_Get ); }
        }

        // - MemoryMap.CreateProperty_Get<T>
        // - MemoryRegister{Constructor}
        // - IMemoryMap.TryGetMember|Get
        public bool HasCustomMethod_Get
        {
            get { return this.customType_Get == CUSTOM_TYPE.METHOD; }
        }

        // - MemoryMap.CreateProperty_Get<T>
        public bool HasCustomOperation_Get
        {
            get { return this.customType_Get == CUSTOM_TYPE.OPERATION; }
        }

        // - MemoryMap.CreateProperty_Get<T>
        public bool HasCustomFormat_Get
        {
            get { return this.customType_Get == CUSTOM_TYPE.FORMAT; }
        }

        #endregion

        #region Custom Set

        private bool _HasCustomMethod_Set
        {
            get { return this.custom_Set.ToLower ().StartsWith ( MemoryMap.METHOD ); }
        }

        private bool _HasCustomMethodId_Set
        {
            get { return this.custom_Set.ToLower ().StartsWith ( MemoryMap.METHOD_KEY ); }
        }

        private bool _HasCustomOperation_Set
        {
            get { return ! this._HasCustomMethod_Set     &&
                           this.valueType < RegType.CHAR &&
                         ! string.IsNullOrEmpty ( this.custom_Set ); }
        }

        private bool _HasCustomFormat_Set
        {
            get { return ! this._HasCustomMethod_Set        &&
                           this.valueType == RegType.STRING &&
                         ! string.IsNullOrEmpty ( this.custom_Set ); }
        }

        // - MemoryMap.CreateProperty_Set<T>
        // - MemoryRegister{Constructor}
        // - MemoryRegister.Value{set}
        public bool HasCustomMethod_Set
        {
            get { return this.customType_Set == CUSTOM_TYPE.METHOD; }
        }

        // - MemoryMap.CreateProperty_Set<T>
        public bool HasCustomOperation_Set
        {
            get { return this.customType_Set == CUSTOM_TYPE.OPERATION; }
        }

        // - MemoryMap.CreateProperty_Set_String<T>
        public bool HasCustomFormat_Set
        {
            get { return this.customType_Set == CUSTOM_TYPE.FORMAT; }
        }

        #endregion

        public dynamic Value
        {
            // Register Value property always returns value from byte array without any modification
            // This behaviour is mandatory to be able to use original value inside custom get methods,
            // avoiding to create infinite loop Custom Get -> Value -> Custom Get -> Value...
            get { return (T)this.funcGet(); }
            set
            {
                // Register with read and write
                if ( this.write )
                {
                    // Method will modify passed value before set in byte array
                    // If XML custom field is "method" or "method:id"
                    if ( this.HasCustomMethod_Set )
                        value = this.funcSetCustom ( value );

                    // Try to set string value after read form control
                    // If XML custom field is...
                    if ( value is string )
                        this.funcSetString(value); 

                    // Try to set string but using byte array ( AES )
                    else if ( value is byte[] )
                        this.funcSetByteArray(value);

                    // Try to set value of waited type
                    // If XML custom field is a math expression ( e.g. _val_ * 2 / 5 )
                    else
                        this.funcSet((T)value);
                }

                // Register is readonly
                else
                {
                    Console.WriteLine ( "Set " + id + ": Error - Can't write to this register" );
                    throw new MemoryRegisterNotAllowWrite ( MemoryMap.EXCEP_SET_READONLY + ": " + id );
                }
            }
        }

        #endregion

        #region Initialization

        public MemoryRegister (
            string id,
            RegType type,
            string description,
            int address,
            int size = MemRegister.DEF_SIZE,
            bool write = MemRegister.DEF_WRITE,
            string custom_Get = "",
            string custom_Set = "" )
        {
            this.id           = id;
            this.valueType    = type;
            this.description  = description;
            this.address      = address;
            this.size         = size;
            this.write        = write;
            this.custom_Get   = custom_Get.Replace ( " ", string.Empty );
            this.custom_Set   = custom_Set.Replace ( " ", string.Empty );
            this.registerType = REGISTER_TYPE.REGISTER;

            // Custom Get
            if      ( this._HasCustomMethod_Get    ) this.customType_Get = CUSTOM_TYPE.METHOD;
            else if ( this._HasCustomOperation_Get ) this.customType_Get = CUSTOM_TYPE.OPERATION;
            else if ( this._HasCustomFormat_Get    ) this.customType_Get = CUSTOM_TYPE.FORMAT;
            else                                     this.customType_Get = CUSTOM_TYPE.EMPTY;

            if ( this.HasCustomMethod_Get )
            {
                if ( this._HasCustomMethodId_Get )
                     this.methodId_Get = this.custom_Get.Substring ( MemoryMap.METHOD_KEY.Length + 1 );
                else this.methodId_Get = this.id + MemoryMap.METHOD_SUFIX_GET;
            }

            // Custom Set
            if      ( this._HasCustomMethod_Set    ) this.customType_Set = CUSTOM_TYPE.METHOD;
            else if ( this._HasCustomOperation_Set ) this.customType_Set = CUSTOM_TYPE.OPERATION;
            else if ( this._HasCustomFormat_Set    ) this.customType_Set = CUSTOM_TYPE.FORMAT;
            else                                     this.customType_Set = CUSTOM_TYPE.EMPTY;

            if ( this.HasCustomMethod_Set )
            {
                if ( this._HasCustomMethodId_Set )
                     this.methodId_Set = this.custom_Set.Substring ( MemoryMap.METHOD_KEY.Length + 1 );
                else this.methodId_Set = this.id + MemoryMap.METHOD_SUFIX_SET;
            }  
        }

        #endregion
    }
}
