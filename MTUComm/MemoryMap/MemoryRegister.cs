using System;
using System.Linq;
using System.Threading.Tasks;
using Library;
using Xml;

using REGISTER_TYPE = MTUComm.MemoryMap.AMemoryMap.REGISTER_TYPE;
using RegType       = MTUComm.MemoryMap.MemoryMap.RegType;

namespace MTUComm.MemoryMap
{
    public class MemoryRegister<T>
    {
        #region Constants

        private enum CUSTOM_TYPE { EMPTY, METHOD, OPERATION }

        #endregion

        #region Attributes

        public Func<Task<T>> funcGet;                 // MemoryRegister.Value{get}
        public Func<T> funcGetMap;              // MemoryRegister.Value{get}
        public Func<Task<T>> funcGetCustom;           // Only use working dynamically ( IMemoryRegister.Get )
        public Func<bool,byte[]> funcGetByteArray;   // 
        public Action<T> funcSet;               // MemoryRegister.Value{set}
        public Func<dynamic,Task<dynamic>> funcSetCustom;         // MemoryRegister.Value{set}
        public Action<string> funcSetString;    // MemoryRegister.Value{set}
        public Action<byte[]> funcSetByteArray; // MemoryRegister.Value{set}
        public Func<Task<T>> funcGetFromMtu;
        public string id { get; }
        public string description { get; }
        public RegType valueType { get; }
        public int address { get; }
        public int size { get; }    // By default size and sizeGet are equal but in some cases ( e.g. EncryptionKey )
        public int sizeGet { get; } // the size writing is different from the size used reading
        public bool write { get; }
        private string custom_Get { get; }
        private string custom_Set { get; }
        public string methodId_Get { get; }
        public string methodId_Set { get; }
        private CUSTOM_TYPE customType_Get;
        private CUSTOM_TYPE customType_Set;
        public REGISTER_TYPE registerType { get; }
        public bool used;   // Flag is used to know what registers should be written in the MTU
        public bool readedFromMtu; // Loaded at least one time reading from the MTU
        private Lexi.Lexi lexi;
        
        public byte[] lastRead;

        #endregion

        #region Properties

        // Value size ( number of consecutive bytes ) is also used for bit with bool type
        public int bit { get { return this.size; } }

        public string mathExpression_Get { get { return this.custom_Get; } }
        public string mathExpression_Set { get { return this.custom_Set; } }

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
            get { return ! this._HasCustomMethod_Get     &&            // Is not a custom method ( "method" or "method:..." )
                           this.valueType < RegType.BOOL &&            // Is a register of numeric type
                         ! string.IsNullOrEmpty ( this.custom_Get ); } // And is not an empty string
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

        #endregion

        // Read and write without processing data, raw info
        public T ValueRaw
        {
            get
            {
                return this.funcGetMap ();
            }
        }
        
        public byte[] ValueByteArrayRaw
        {
            get
            {
                return this.funcGetByteArray ( true );
            }
        }

        // Recover bytes without processing data, raw info
        public async Task<byte[]> GetValueByteArray (
            bool useSizeGet = true )
        {
            // Read value from MTU if is necessary
            await this.funcGet ();
        
            return this.funcGetByteArray ( useSizeGet );
        }
        
        public async Task<dynamic> GetValueFromMtu (
            bool returnByteArray = false )
        {
            Utils.PrintDeep ( Environment.NewLine + "------LOAD_FROM_MTU------" );
            Utils.Print ( "Register -> GetValueFromMtu -> " + this.id + " [ " + returnByteArray + " ]" );
        
            // Reset flag that will be used in funcGet to invoke funcGetFromMtu
            this.readedFromMtu = false;
            var result = await this.GetValue ();
            
            Utils.PrintDeep ( "---LOAD_FROM_MTU_FINISH--" + Environment.NewLine );
            
            if ( ! returnByteArray )
            	 return result;
          	else return this.lastRead;
        }
        
        public async Task SetValueToMtu (
            dynamic value = null )
        {
            Utils.PrintDeep ( Environment.NewLine + "------WRITE_TO_MTU-------" );
            Utils.Print ( "Register -> SetValueToMtu -> " + this.id + ( ( value != null ) ? " = " + value : "" ) );
        
            // Set value in temporary memory map before write it to the MTU
            if ( value != null )
                await this.SetValue ( value );
            
            // Write value set in memory map to the MTU
            if ( valueType == RegType.BOOL )
                await this.SetBitToMtu ();
            else
            {
                // Recover byte array with length equals to the value to set,
                // not the length ( sizeGet ) that will be used to recover/get
                await this.lexi.Write ( ( uint )this.address, await this.GetValueByteArray ( false ) );
            }
            
            Utils.PrintDeep ( "---WRITE_TO_MTU_FINISH---" + Environment.NewLine );
        }
        
        private async Task SetBitToMtu ()
        {
            Utils.PrintDeep ( "Register -> ValueWriteToMtu_Bit -> " + this.id );
        
            // Read current value
            byte systemFlags = ( await this.lexi.Read ( ( uint )this.address, 1 ) )[ 0 ];

            Utils.PrintDeep ( "Register -> ValueWriteToMtu_Bit -> Current value map: " + this.id + " -> " + this.ValueRaw );
            Utils.PrintDeep ( "Register -> ValueWriteToMtu_Bit -> Current value MTU: " + this.id + " -> " + Utils.ByteToBits ( systemFlags ) + " [ Hex: " + systemFlags + " ]" );

            var valueInMap = ( bool )( object )this.ValueRaw;

            // Modify bit and write to MTU
            if ( valueInMap )
                 systemFlags = ( byte ) ( systemFlags |    1 << ( int )bit   );
            else systemFlags = ( byte ) ( systemFlags & ~( 1 << ( int )bit ) );
            
            Utils.PrintDeep ( "Register -> ValueWriteToMtu_Bit -> Write bit to MTU: " + this.id + " -> " + Utils.ByteToBits ( systemFlags ) + " [ Hex: " + systemFlags + " ] to bit: " + bit );
            
            await this.lexi.Write ( ( uint )this.address, new byte[] { systemFlags } );
        }

        // Register Value property always returns value from byte array without any modification
        // This behaviour is mandatory to be able to use original value inside custom get methods,
        // avoiding to create infinite loop: Custom Get -> Value -> Custom Get -> Value...
        public async Task<T> GetValue ()
        {
            // If register has not customized get method, use normal/direct get raw value
            if ( ! this.HasCustomMethod_Get )
                return await this.funcGet ();

            return await this.funcGetCustom ();
        }

        // Use custom methods if them are registered
        public async Task SetValue (
            dynamic value )
        {
            // Register with read and write
            if ( this.write )
            {
                Utils.Print ( "Register -> SetValue: " + this.id + " = " + value );
            
                try
                {
                    // Method will modify passed value before set in byte array
                    // If XML custom field is "method" or "method:id"
                    if ( this.HasCustomMethod_Set )
                        value = await this.funcSetCustom ( value );
    
                    // Try to set string value after read form control
                    // If XML custom field is...
                    if ( value is string )
                        this.funcSetString ( value ); 
    
                    // Try to set string but using byte array ( AES )
                    else if ( value is byte[] )
                        this.funcSetByteArray ( value );
    
                    // Try to set value of waited type
                    // If XML custom field is a math expression ( e.g. _val_ * 2 / 5 )
                    else
                        this.funcSet ( ( T )value );
                    
                    // Flag is used to know what registers should be written in the MTU
                    this.used = true;
                }
                catch ( Exception e )
                {
                     Console.WriteLine ( "Register -> SetValue [ ERROR ]: " + e.Message );
                     Console.WriteLine ( e.StackTrace );
                }
            }
            // Register is readonly
            else
            {
                Utils.Print ( "Set " + id + ": Error - Can't write to this register because is readonly" );

                if ( ! MemoryMap.isUnityTest )
                    throw new MemoryRegisterNotAllowWrite ( MemoryMap.EXCEP_SET_READONLY + ": " + id );
            }
        }

        public async Task<string> GetValueXMask (
            string xMask,
            int digits )
        {
            string value = ( await this.GetValue () ).ToString ();

            // Ejemplo: num 1234 mask X00 digits 6
            // 1. 4 < 6
            // 2. 6 - 4 == 3 - 1
            if ( value.Length < digits &&
                 digits - value.Length == xMask.Length - 1 )
            {
                value = xMask.Substring ( 1, xMask.Length - 1 ) + value;
            }

            throw new Exception ();
        }

        #endregion

        #region Initialization

        public MemoryRegister () { }

        public MemoryRegister (
            string id,
            RegType type,
            string description,
            int address,
            int size = MemRegister.DEF_SIZE,
            int sizeGet = MemRegister.DEF_SIZE,
            bool write = MemRegister.DEF_WRITE,
            string custom_Get = "",
            string custom_Set = "" )
        {
            this.id           = id;
            this.valueType    = type;
            this.description  = description;
            this.address      = address;
            this.size         = size;
            this.sizeGet      = ( sizeGet > 1 || type == RegType.BOOL ) ? sizeGet : size; // sizeGet by default is 1
            this.write        = write;
            this.custom_Get   = custom_Get.Replace ( " ", string.Empty );
            this.custom_Set   = custom_Set.Replace ( " ", string.Empty );
            this.registerType = REGISTER_TYPE.REGISTER;
            this.lexi         = Singleton.Get.Lexi;

            // Custom Get
            if      ( this._HasCustomMethod_Get    ) this.customType_Get = CUSTOM_TYPE.METHOD;
            else if ( this._HasCustomOperation_Get ) this.customType_Get = CUSTOM_TYPE.OPERATION;
            else                                     this.customType_Get = CUSTOM_TYPE.EMPTY;

            if ( this.HasCustomMethod_Get )
            {
                if ( this._HasCustomMethodId_Get )
                     this.methodId_Get = this.custom_Get.Substring ( MemoryMap.METHOD_KEY.Length );
                else this.methodId_Get = this.id + MemoryMap.METHOD_SUFIX_GET;
            }

            // Custom Set
            if      ( this._HasCustomMethod_Set    ) this.customType_Set = CUSTOM_TYPE.METHOD;
            else if ( this._HasCustomOperation_Set ) this.customType_Set = CUSTOM_TYPE.OPERATION;
            else                                     this.customType_Set = CUSTOM_TYPE.EMPTY;

            if ( this.HasCustomMethod_Set )
            {
                if ( this._HasCustomMethodId_Set )
                     this.methodId_Set = this.custom_Set.Substring ( MemoryMap.METHOD_KEY.Length );
                else this.methodId_Set = this.id + MemoryMap.METHOD_SUFIX_SET;
            }
        }

        #endregion

        #region Compare

        public async Task<bool> Equals ( MemoryRegister<T> other )
        {
            if ( other == null )
                return false;

            bool ok_id          = string.Equals ( this.id, other.id );
            bool ok_description = string.Equals ( this.description, other.description );
            bool ok_address     = ( this.address == other.address );
            bool ok_size        = ( this.size    == other.size    );
            bool ok_write       = ( this.write   == other.write   );
            
            byte[] valLocal = new byte[] { };
            byte[] valOther = new byte[] { };
            
            bool ok_value = true;
            try
            {
                valLocal = this.ValueByteArrayRaw;
                valOther = await other.GetValueByteArray ();
            
                ok_value = valLocal.SequenceEqual ( valOther );
            }
            catch ( Exception e )
            {
                ok_value = false;
            }

            Utils.Print ( "Equals: " + this.id + " -> " +
                Utils.ByteArrayToString ( valLocal ) + " == " +
                Utils.ByteArrayToString ( valOther ) + " = " +
                ok_value );

            return ok_id          &&
                   ok_description &&
                   ok_address     &&
                   ok_size        &&
                   ok_write       &&
                   ok_value;
        }

        #endregion
    }
}
