using System;

using RegType = MTUComm.MemoryMap.MemoryMap.RegType;
using REGISTER_TYPE = MTUComm.MemoryMap.AMemoryMap.REGISTER_TYPE;

namespace MTUComm.MemoryMap
{
    public class MemoryOverload<T>
    {
        private enum CUSTOM_TYPE { OPERATION, METHOD }

        private const string STR_CUSTOM = "method";

        public Func<T> funcGet;
        public string id { get; }
        public string description { get; }
        public RegType valueType { get; }
        public string[] registerIds { get; }
        public string custom { get; }
        private CUSTOM_TYPE customType;
        public REGISTER_TYPE registerType { get; }

        public bool HasCustomMethod
        {
            get { return string.Equals ( this.custom.ToLower (), STR_CUSTOM ); }
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

            this.customType = ( this.HasCustomMethod ) ? CUSTOM_TYPE.METHOD : CUSTOM_TYPE.OPERATION;
        }

        public T Value
        {
            get { return (T)this.funcGet(); }
        }
    }
}
