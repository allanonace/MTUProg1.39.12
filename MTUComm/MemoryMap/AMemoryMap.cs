using System;
using System.Collections.Generic;
using System.Dynamic;

namespace MTUComm.MemoryMap
{
    public abstract class AMemoryMap : DynamicObject
    {
        #region Constants

        public enum REGISTER_TYPE { REGISTER, OVERLOAD }

        private const string ID_MTUTYPE      = "MtuType";
        private const string ID_MTUID        = "MtuId";
        private const string ID_SHIPTBIT     = "Shipbit";
        private const string ID_DAILYREAD    = "DailyRead";
        private const string ID_DAILYSNAP    = "DailySnap";
        private const string ID_MSGOVERLAP   = "MessageOverlapCount";
        private const string ID_READINTERVAL = "ReadInterval";
        private const string ID_MTUVOLTAGE   = "MtuMiliVoltageBattery";
        private const string ID_MTUFIRMFMT   = "MtuFirmwareVersionFormatFlag";
        private const string ID_MTUFIRM      = "MtuFirmwareVersion";
        private const string ID_PCBNUMBER    = "PcbNumber";
        private const string ID_P1METERTYPE  = "P1MeterType";
        private const string ID_P2METERTYPE  = "P2MeterType";
        private const string ID_P1METERID    = "P1MeterId";
        private const string ID_P2METERID    = "P2MeterId";
        private const string ID_P1READING    = "P1Reading";
        private const string ID_P2READING    = "P2Reading";
        private const string ID_P1SCALER     = "P1Scaler";
        private const string ID_P2SCALER     = "P2Scaler";
        private const string ID_P1READERROR  = "P1ReadingError";
        private const string ID_P2READERROR  = "P2ReadingError";

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
                Console.WriteLine ( "Get " + id + ": Error - " + MemoryMap.EXCEP_SET_USED );
                throw new MemoryRegisterNotExistException ( MemoryMap.EXCEP_SET_USED + " [ Indexer ]: " + id );
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
                this.dictionary.Add ( register.id, register );
        }

        // Add dynamic member of type Func<> or Action<>
        protected void AddMethod ( string id, dynamic method )
        {
            if ( ! this.dictionary.ContainsKey ( id ) )
                this.dictionary.Add ( id, method );
        }

        // Ask if a dynamic member exists in the object
        protected bool ContainsMember ( string id )
        {
            return this.dictionary.ContainsKey ( id );
        }

        #region Dot operator

        public override bool TrySetMember ( SetMemberBinder binder, object value )
        {
            return this.Set ( binder.Name, value );
        }

        private bool Set ( string id, object value )
        {
            // Selected dynamic member exists
            if ( this.dictionary.ContainsKey ( id ) )
            {
                dynamic register = this.dictionary[id];

                // Overloads are readonly
                if ( register.GetType().GetGenericTypeDefinition() == typeof( MemoryOverload<> ) )
                    throw new MemoryOverloadsAreReadOnly ( MemoryMap.EXCEP_OVE_READONLY + ": " + id );

                this.dictionary[id].Value = value;
                this.dictionary[id].used  = true;

                return true;
            }

            // Selected dynamic member not exists
            Console.WriteLine ( "Set " + id + ": Error - Selected register is not loaded" );
            throw new MemoryRegisterNotExistException ( MemoryMap.EXCEP_SET_USED + " [ Set ]: " + id );
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
                    if ( ! register.HasCustomMethod_Get )
                         result = ( object )this.dictionary[ id ].ValueRaw; // Invokes funGet method
                    else result = ( object )this.dictionary[ id ].Value;    // Invokes funGetCustom method
                }
                else // Overload
                    result = ( object )this.dictionary[ id ].Value;

                return true;
            }

            // Selected dynamic member not exists
            Console.WriteLine ( "Get " + id + ": Error - Selected register is not loaded" );
            throw new MemoryRegisterNotExistException ( MemoryMap.EXCEP_SET_USED + " [ Get ]: " + id );
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

        public dynamic MtuType
        {
            get
            {
                object result;
                this.Get ( ID_MTUTYPE, out result );
                return ( int )result;
            }
            set
            {
                this.Set ( ID_MTUTYPE, value );
            }
        }

        public dynamic MtuId
        {
            get
            {
                object result;
                this.Get(ID_MTUID, out result);
                return (int)result;
            }
            set
            {
                this.Set(ID_MTUID, value);
            }
        }

        public dynamic Shipbit
        {
            get
            {
                object result;
                this.Get(ID_SHIPTBIT, out result);
                return (bool)result;
            }
            set
            {
                this.Set(ID_SHIPTBIT, value);
            }
        }

        public dynamic DailyRead
        {
            get
            {
                object result;
                this.Get(ID_DAILYREAD, out result);
                return (int)result;
            }
            set
            {
                this.Set(ID_DAILYREAD, value);
            }
        }

        public dynamic DailySnap
        {
            get
            {
                object result;
                this.Get(ID_DAILYSNAP, out result);
                return (String)result;
            }
            set
            {
                this.Set(ID_DAILYSNAP, value);
            }
        }

        public dynamic MessageOverlapCount
        {
            get
            {
                object result;
                this.Get(ID_MSGOVERLAP, out result);
                return (int)result;
            }
            set
            {
                this.Set(ID_MSGOVERLAP, value);
            }
        }

        public dynamic ReadInterval
        {
            get
            {
                object result;
                this.Get(ID_READINTERVAL, out result);
                return ( string )result;
            }
            set
            {
                this.Set(ID_READINTERVAL, value);
            }
        }

        public dynamic MtuMiliVoltageBattery
        {
            get
            {
                object result;
                this.Get(ID_MTUVOLTAGE, out result);
                return (int)result;
            }
            set
            {
                this.Set(ID_MTUVOLTAGE, value);
            }
        }

        public dynamic MtuFirmwareVersionFormatFlag
        {
            get
            {
                object result;
                this.Get(ID_MTUFIRMFMT, out result);
                return (int)result;
            }
            set
            {
                this.Set(ID_MTUFIRMFMT, value);
            }
        }

        public dynamic MtuFirmwareVersion
        {
            get
            {
                object result;
                this.Get(ID_MTUFIRM, out result);
                return (string)result;
            }
            set
            {
                this.Set(ID_MTUFIRM, value);
            }
        }

        public dynamic PcbNumber
        {
            get
            {
                object result;
                this.Get(ID_PCBNUMBER, out result);
                return (string)result;
            }
            set
            {
                this.Set(ID_PCBNUMBER, value);
            }
        }

        public dynamic P1MeterType
        {
            get
            {
                object result;
                this.Get(ID_P1METERTYPE, out result);
                return (int)result;
            }
            set
            {
                this.Set(ID_P1METERTYPE, value);
            }
        }

        public dynamic P2MeterType
        {
            get
            {
                object result;
                this.Get(ID_P2METERTYPE, out result);
                return (int)result;
            }
            set
            {
                this.Set(ID_P2METERTYPE, value);
            }
        }

        public dynamic P1MeterId
        {
            get
            {
                object result;
                this.Get(ID_P1METERID, out result);
                return (ulong)result;
            }
            set
            {
                this.Set(ID_P1METERID, value);
            }
        }

        public dynamic P2MeterId
        {
            get
            {
                object result;
                this.Get(ID_P2METERID, out result);
                return (ulong)result;
            }
            set
            {
                this.Set(ID_P2METERID, value);
            }
        }

        public dynamic P1Reading
        {
            get
            {
                object result;
                this.Get(ID_P1READING, out result);
                return (ulong)result;
            }
            set
            {
                this.Set(ID_P1READING, value);
            }
        }

        public dynamic P2Reading
        {
            get
            {
                object result;
                this.Get(ID_P2READING, out result);
                return (ulong)result;
            }
            set
            {
                this.Set(ID_P2READING, value);
            }
        }

        public dynamic P1Scaler
        {
            get
            {
                object result;
                this.Get(ID_P1SCALER, out result);
                return (int)result;
            }
            set
            {
                this.Set(ID_P1SCALER, value);
            }
        }

        public dynamic P2Scaler
        {
            get
            {
                object result;
                this.Get(ID_P2SCALER, out result);
                return (int)result;
            }
            set
            {
                this.Set(ID_P2SCALER, value);
            }
        }

        public dynamic P1ReadingError
        {
            get
            {
                object result;
                this.Get(ID_P1READERROR, out result);
                return (string)result;
            }
            set
            {
                this.Set(ID_P1READERROR, value);
            }
        }

        public dynamic P2ReadingError
        {
            get
            {
                object result;
                this.Get(ID_P2READERROR, out result);
                return (string)result;
            }
            set
            {
                this.Set(ID_P2READERROR, value);
            }
        }

        #endregion
    }
}
