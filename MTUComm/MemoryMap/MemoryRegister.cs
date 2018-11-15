using System;
using Xml;

using RegType = MTUComm.MemoryMap.MemoryMap.RegType;
using REGISTER_TYPE = MTUComm.MemoryMap.AMemoryMap.REGISTER_TYPE;
using System.Text;

namespace MTUComm.MemoryMap
{
    public class MemoryRegister<T>
    {
        #region Constants

        private enum CUSTOM_TYPE { EMPTY, METHOD, OPERATION, FORMAT }

        private const string METHOD       = "method";
        private const string METHOD_KEY   = METHOD + ":";
        private const string METHOD_SUFIX = "_Logic";

        #endregion

        #region Attributes

        public Func<T> funcGet;
        public Func<T> funcGetCustom;
        public Action<T> funcSet;
        public Action<string> funcSetString;
        public string id { get; }
        public string description { get; }
        public RegType valueType { get; }
        public int address { get; }
        public int size { get; }
        public bool write { get; }
        public string custom { get; }
        public string methodId { get; }
        private CUSTOM_TYPE customType;
        public bool used;
        public REGISTER_TYPE registerType { get; }

        #endregion

        #region Properties

        // Value size ( number of consecutive bytes ) is also used for bit with bool type
        public int bit { get { return this.size; } }

        private bool _HasCustomMethod
        {
            get { return this.custom.ToLower ().StartsWith ( METHOD ); }
        }

        private bool _HasCustomMethodId
        {
            get { return this.custom.ToLower ().StartsWith ( METHOD_KEY ); }
        }

        private bool _HasCustomOperation
        {
            get { return ! this._HasCustomMethod      &&
                           this.valueType < RegType.CHAR &&
                         ! string.IsNullOrEmpty ( this.custom ); }
        }

        private bool _HasCustomFormat
        {
            get { return ! this._HasCustomMethod         &&
                           this.valueType == RegType.STRING &&
                         ! string.IsNullOrEmpty ( this.custom ); }
        }

        public bool HasCustomMethod
        {
            get { return this.customType == CUSTOM_TYPE.METHOD; }
        }

        public bool HasCustomOperation
        {
            get { return this.customType == CUSTOM_TYPE.OPERATION; }
        }

        public bool HasCustomFormat
        {
            get { return this.customType == CUSTOM_TYPE.FORMAT; }
        }

        public dynamic Value
        {
            get { return (T)this.funcGet(); }
            set
            {
                // Register with read and write
                if ( this.write )
                {
                    // Try to set string value after read form control
                    if ( value is string )
                        this.funcSetString(value);
                    
                    // Try to set string but using byte array ( AES )
                    else if ( value is byte[] )
                        this.funcSetString(Encoding.Default.GetString(value));

                    // Try to set value of waited type
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
            string custom = "" )
        {
            this.id           = id;
            this.valueType    = type;
            this.description  = description;
            this.address      = address;
            this.size         = size;
            this.write        = write;
            this.custom       = custom;
            this.registerType = REGISTER_TYPE.REGISTER;

            if      ( this._HasCustomMethod    ) this.customType = CUSTOM_TYPE.METHOD;
            else if ( this._HasCustomOperation ) this.customType = CUSTOM_TYPE.OPERATION;
            else if ( this._HasCustomFormat    ) this.customType = CUSTOM_TYPE.FORMAT;
            else                                 this.customType = CUSTOM_TYPE.EMPTY;

            if ( this.HasCustomMethod )
            {
                if ( this._HasCustomMethodId )
                     this.methodId = this.custom.Substring ( METHOD_KEY.Length + 1 );
                else this.methodId = this.id + METHOD_SUFIX;
            }   
        }

        #endregion
    }
}
