using System;

using RegType = MTUComm.MemoryMap.MemoryMap.RegType;
using REGISTER_TYPE = MTUComm.MemoryMap.AMemoryMap.REGISTER_TYPE;

namespace MTUComm.MemoryMap
{
    public class MemoryOverload<T>
    {
        private enum CUSTOM_TYPE { OPERATION, METHOD }

        private const string METHOD       = "method";
        private const string METHOD_KEY   = METHOD + ":";
        private const string METHOD_SUFIX = "_Logic";
        private const string EXCEP_OVER_CUSTOM = "Custom field is empty";

        public Func<T> funcGet;
        public string id { get; }
        public string description { get; }
        public RegType valueType { get; }
        public string[] registerIds { get; }
        public string custom { get; }
        public string methodId { get; }
        private CUSTOM_TYPE customType;
        public REGISTER_TYPE registerType { get; }

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

        public bool HasCustomMethod
        {
            get { return this.customType == CUSTOM_TYPE.METHOD; }
        }

        public bool HasCustomOperation
        {
            get { return this.customType == CUSTOM_TYPE.OPERATION; }
        }

        public MemoryOverload (
            string id,
            RegType type,
            string description,
            string[] registerIds,
            string custom )
        {
            this.id           = id;
            this.valueType    = type;
            this.description  = description;
            this.registerIds  = registerIds;
            this.custom       = custom;
            this.registerType = REGISTER_TYPE.OVERLOAD;

            if      ( this._HasCustomMethod    ) this.customType = CUSTOM_TYPE.METHOD;
            else if ( this._HasCustomOperation ) this.customType = CUSTOM_TYPE.OPERATION;
            else
            {
                // Selected dynamic member not exists
                Console.WriteLine ( "Get " + id + ": Error - Overload registers need custom field" );
                throw new OverloadEmptyCustomException ( EXCEP_OVER_CUSTOM + ": " + id );
            }

            if ( this.HasCustomMethod )
            {
                if ( this._HasCustomMethodId )
                     this.methodId = this.custom.Substring ( METHOD_KEY.Length );
                else this.methodId = this.id + METHOD_SUFIX;
            }
        }

        public T Value
        {
            get { return (T)this.funcGet(); }
        }
    }
}
