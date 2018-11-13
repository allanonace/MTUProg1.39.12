using System;
using System.Collections.Generic;
using System.Dynamic;

namespace MTUComm.MemoryMap
{
    public interface IMemoryMap
    {
        int MtuType { get; }
        int MtuId { get; }

        bool Shipbit { get; }

        int DailyRead { get; }
        string DailySnap { get; } // Hard...

        int MessageOverlapCount { get; }
        int ReadInterval { get; }

        int MtuMiliVoltageBattery { get; }

        int MtuFirmwareVersionFormatFlag { get; }
        string MtuFirmwareVersion { get; }

        string PcbNumber { get; } // Hard...

        int P1MeterType { get; }
        int P2MeterType { get; }

        ulong P1MeterId { get; }
        ulong P2MeterId { get; }

        int P1Reading { get; } // Hard...
        int P2Reading { get; } // Hard...

        string P1ReadingError { get; }
        string P2ReadingError { get; }

        int P1Scaler { get; }
        int P2Scaler { get; }
    }

    public abstract class AMemoryMap : DynamicObject, IMemoryMap
    {
        #region Constants

        public enum REGISTER_TYPE { REGISTER, OVERLOAD }

        #endregion

        #region Attributes

        // Can't put directly <string,MemoryRegister>
        private Dictionary<string, dynamic> dictionary;
        protected dynamic registers { get; }

        #endregion

        #region Indexer

        // Return generated objects ( registers, overloads and methods ), without call TryGetMember
        public dynamic this[ string id ]
        {
            get
            {
                if ( this.dictionary.ContainsKey ( id ) )
                    return this.dictionary[ id ];

                // Selected dynamic member not exists
                Console.WriteLine ( "Get " + id + ": Error - Selected register is not loaded" );
                throw new MemoryRegisterNotExistException ( MemoryMap.EXCEP_SET_USED + ": " + id );
            }
        }

        #endregion

        #region Initialization

        public AMemoryMap ()
        {
            // Will contain MemoryRegister objects but thanks to TryGetMember
            // the returned values always will be property get block returned value
            // TryGetMember is only called using dot operator ( registers.id_register )
            this.dictionary = new Dictionary<string, dynamic> ();

            // En la clase MemoryMap no se puede usar directamente "this|base.idPropiedad", habiendo
            // de declarar esta variable dinamica mediante la cual acceder a los miembros dinamicos
            this.registers = this;
        }

        #endregion

        #region Methods

        // Add dynamic member of type MemoryRegister or MemoryOverload
        protected void AddProperty ( dynamic register )
        {
            if ( ! this.dictionary.ContainsKey ( register.id ) )
                this.dictionary[ register.id ] = register;
        }

        // Add dynamic member of type Func<> or Action<>
        protected void AddMethod ( string id, dynamic method )
        {
            if ( ! this.dictionary.ContainsKey ( id ) )
                this.dictionary[id] = method;
        }

        // Ask if a dynamic member exists in the object
        protected bool ContainsMember ( string id )
        {
            return this.dictionary.ContainsKey ( id );
        }

        #region Using dot operator

        public override bool TrySetMember ( SetMemberBinder binder, object value )
        {
            return this.Set ( binder.Name, value );
        }

        private bool Set ( string id, object value )
        {
            // Selected dynamic member exists
            if ( this.dictionary.ContainsKey ( id ) )
            {
                // En el get se puede no castear y devolver object que automaticamente se
                // transformara en el tipo esperado, pero si aqui se hace this.dictionary[id].Value = value
                // el programa se queda bloqueado no pudiendo completar la accion
                switch (Type.GetTypeCode( value.GetType () ))
                {
                    case TypeCode.Int32  : (this.dictionary[id] as MemoryRegister<int>   ).Value = (int   )value; break;
                    case TypeCode.UInt32 : (this.dictionary[id] as MemoryRegister<uint>  ).Value = (uint  )value; break;
                    case TypeCode.Int64  : (this.dictionary[id] as MemoryRegister<ulong> ).Value = (ulong )value; break;
                    case TypeCode.Boolean: (this.dictionary[id] as MemoryRegister<bool>  ).Value = (bool  )value; break;
                    case TypeCode.Char   : (this.dictionary[id] as MemoryRegister<char>  ).Value = (char  )value; break;
                    case TypeCode.String : (this.dictionary[id] as MemoryRegister<string>).Value = (string)value; break;
                }

                this.dictionary[id].used = true;

                return true;
            }

            // Selected dynamic member not exists
            Console.WriteLine ( "Set " + id + ": Error - Selected register is not loaded" );
            throw new MemoryRegisterNotExistException ( MemoryMap.EXCEP_SET_USED + ": " + id );
        }

        public override bool TryGetMember ( GetMemberBinder binder, out object result )
        {
            return this.Get ( binder.Name, out result );
        }

        private bool Get ( string id, out object result )
        {
            if ( this.dictionary.ContainsKey ( id ) )
            {
                dynamic register = this.dictionary[id];

                if ( register.registerType == REGISTER_TYPE.REGISTER )
                {
                    // Some registers have customized get method
                    if ( ! register.HasCustomMethod )
                         result = ( object )this.dictionary[ id ].Value;
                    else result = ( object )this.dictionary[ id ].funcGetCustom ();
                }
                else // Overload
                    result = ( object )this.dictionary[ id ].Value;

                return true;
            }

            // Selected dynamic member not exists
            Console.WriteLine ( "Get " + id + ": Error - Selected register is not loaded" );
            throw new MemoryRegisterNotExistException ( MemoryMap.EXCEP_SET_USED + ": " + id );
        }

        #endregion

        #endregion

        #region Shared properties

        // Los miembros fijos/estaticos de la clase se gestionan por separado de los miembros dinamicos
        // Si la clase cuenta con una propiedad o atributo con identificador A y se añade un miembro dinamico A
        // del mismo tipo a la clase, al trabajar con A se estara usando el miembro estatico, que no el dinamico,
        // no realizandose sobreescritura alguna o sustitucion de un miembro por el otro
        // La unica forma que parece viable de asociar ambos miembros es trabajar con
        // el dinamico desde la propiedad estatica, a modo de wrapper

        public int MtuType
        {
            get
            {
                object result;
                this.Get ( "MtuType", out result );
                return ( int )result;
            }
            set
            {
                this.Set ( "MtuType", value );
            }
        }

        public int MtuId
        {
            get
            {
                object result;
                this.Get("MtuId", out result);
                return (int)result;
            }
            set
            {
                this.Set("MtuId", value);
            }
        }

        public bool Shipbit
        {
            get
            {
                object result;
                this.Get("Shipbit", out result);
                return (bool)result;
            }
            set
            {
                this.Set("Shipbit", value);
            }
        }

        public int DailyRead
        {
            get
            {
                object result;
                this.Get("DailyRead", out result);
                return (int)result;
            }
            set
            {
                this.Set("DailyRead", value);
            }
        }

        public String DailySnap
        {
            get
            {
                object result;
                this.Get("DailySnap", out result);
                return (String)result;
            }
            set
            {
                this.Set("DailySnap", value);
            }
        }

        public int MessageOverlapCount
        {
            get
            {
                object result;
                this.Get("MessageOverlapCount", out result);
                return (int)result;
            }
            set
            {
                this.Set("MessageOverlapCount", value);
            }
        }

        public int ReadInterval
        {
            get
            {
                object result;
                this.Get("ReadInterval", out result);
                return (int)result;
            }
            set
            {
                this.Set("ReadInterval", value);
            }
        }

        public int MtuMiliVoltageBattery
        {
            get
            {
                object result;
                this.Get("MtuMiliVoltageBattery", out result);
                return (int)result;
            }
            set
            {
                this.Set("MtuMiliVoltageBattery", value);
            }
        }

        public int MtuFirmwareVersionFormatFlag
        {
            get
            {
                object result;
                this.Get("MtuFirmwareVersionFormatFlag", out result);
                return (int)result;
            }
            set
            {
                this.Set("MtuFirmwareVersionFormatFlag", value);
            }
        }

        public string MtuFirmwareVersion
        {
            get
            {
                object result;
                this.Get("MtuFirmwareVersion", out result);
                return (string)result;
            }
            set
            {
                this.Set("MtuFirmwareVersion", value);
            }
        }

        public string PcbNumber
        {
            get
            {
                object result;
                this.Get("PcbNumber", out result);
                return (string)result;
            }
            set
            {
                this.Set("PcbNumber", value);
            }
        }

        public int P1MeterType
        {
            get
            {
                object result;
                this.Get("P1MeterType", out result);
                return (int)result;
            }
            set
            {
                this.Set("P1MeterType", value);
            }
        }

        public int P2MeterType
        {
            get
            {
                object result;
                this.Get("P2MeterType", out result);
                return (int)result;
            }
            set
            {
                this.Set("P2MeterType", value);
            }
        }

        public ulong P1MeterId
        {
            get
            {
                object result;
                this.Get("P1MeterId", out result);
                return (ulong)result;
            }
            set
            {
                this.Set("P1MeterId", value);
            }
        }

        public ulong P2MeterId
        {
            get
            {
                object result;
                this.Get("P2MeterId", out result);
                return (ulong)result;
            }
            set
            {
                this.Set("P2MeterId", value);
            }
        }

        public int P1Reading
        {
            get
            {
                object result;
                this.Get("P1Reading", out result);
                return (int)result;
            }
            set
            {
                this.Set("P1Reading", value);
            }
        }

        public int P2Reading
        {
            get
            {
                object result;
                this.Get("P2Reading", out result);
                return (int)result;
            }
            set
            {
                this.Set("P2Reading", value);
            }
        }

        public int P1Scaler
        {
            get
            {
                object result;
                this.Get("P1Scaler", out result);
                return (int)result;
            }
            set
            {
                this.Set("P1Scaler", value);
            }
        }

        public int P2Scaler
        {
            get
            {
                object result;
                this.Get("P2Scaler", out result);
                return (int)result;
            }
            set
            {
                this.Set("P2Scaler", value);
            }
        }

        public string P1ReadingError
        {
            get
            {
                object result;
                this.Get("P1ReadingError", out result);
                return (string)result;
            }
            set
            {
                this.Set("P1ReadingError", value);
            }
        }

        public string P2ReadingError
        {
            get
            {
                object result;
                this.Get("P2ReadingError", out result);
                return (string)result;
            }
            set
            {
                this.Set("P2ReadingError", value);
            }
        }

        #endregion
    }
}
