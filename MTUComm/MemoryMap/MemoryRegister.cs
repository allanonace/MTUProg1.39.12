using System;
using System.Linq;
using System.Threading.Tasks;
using Library;
using Library.Exceptions;
using Xml;

using REGISTER_TYPE = MTUComm.MemoryMap.AMemoryMap.REGISTER_TYPE;
using RegType       = MTUComm.MemoryMap.MemoryMap.RegType;

namespace MTUComm.MemoryMap
{
    /// <summary>
    /// Representation of a memory register with all the information required
    /// to be able to simulate n bytes of the physical memory of the MTU, in
    /// a human readable way.
    /// </summary>
    public class MemoryRegister<T>
    {
        #region Constants

        /// <summary>
        /// Types of custom values that can be set in the CustomGet
        /// and CustomSet tags in the memory maps XML entries.
        /// <para>
        /// <code>
        /// <Register>
        ///     <Id>P1MeterId</Id>
        ///     <Description>P1 meter ID</Description>
        ///     <Type>ulong</Type>
        ///     <Address>34</Address>
        ///     <Size>6</Size>
        ///     <Write>true</Write>
        ///     <CustomGet>method:BcdToULong</CustomGet> // Method with specific ID
        ///     <CustomSet>method:ULongToBcd</CustomSet>
        /// </Register>
        /// <Register>
        ///     <Id>MtuMiliVoltageBattery</Id>
        ///     <Description>Battery voltage</Description>
        ///     <Type>int</Type>
        ///     <Address>113</Address>
        ///     <Size>1</Size>
        ///     <Write>false</Write>
        ///     <CustomGet>(_val_ * 9.766 * 2) + 250</CustomGet> // Inline operation
        ///     <CustomSet/> // Empty = No custom method nor operation
        /// </Register>
        /// <Register>
        ///     <Id>ReadIntervalMinutes</Id>
        ///     <Description>Read interval in minutes</Description>
        ///     <Type>int</Type>
        ///     <Address>26</Address>
        ///     <Size>2</Size>
        ///     <Write>true</Write>
        ///     <CustomGet/>
        ///     <CustomSet>method</CustomSet> // References to the method ReadIntervalMinutes_Set
        /// </Register>
        /// </code>
        /// </para>
        /// <para>&#160;</para>
        /// </para>
        /// <list type="CUSTOM_TYPE">
        /// <item>
        ///     <term>CUSTOM_TYPE.EMPTY</term>
        ///     <description>No custom method nor inline operation</description>
        /// </item>
        /// <item>
        ///     <term>CUSTOM_TYPE.METHOD</term>
        ///     <description>Set to use a custom method, using "method", and the associated
        /// method should have the suffix _Get or _Set, or an specific name ( >method:...</ )</description>
        /// </item>
        /// <item>
        ///     <term>CUSTOM_TYPE.OPERATION</term>
        ///     <description>Inline operation using "_var_" string that will be replaced by the register value</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        /// <remarks>
        /// NOTE: The following arithmetic operators are supported in expressions: +  -  *  /  %
        /// </remarks>
        private enum CUSTOM_TYPE { EMPTY, METHOD, OPERATION }

        #endregion

        #region Attributes

        public Func<Task<T>> funcGet;
        public Func<T> funcGetMap;
        public Func<Task<T>> funcGetCustom;
        public Func<bool,byte[]> funcGetByteArray;
        public Action<T> funcSet;
        public Func<dynamic,Task<dynamic>> funcSetCustom;
        public Action<string> funcSetString;
        public Action<byte[]> funcSetByteArray;
        public Func<Task<T>> funcGetFromMtu;
        /// <summary>
        /// Identifier of the memory register.
        /// </summary>
        public string id { get; }
        /// <summary>
        /// Additional information about the register and its purpose.
        /// </summary>
        public string description { get; }
        /// <summary>
        /// Type of the register ( int, uint, ulong, bool or string ).
        /// <para>
        /// See <see cref="MTUComm.MemoryMap.MemoryMap.RegType"/> for available types.
        /// </para>
        /// </summary>
        public RegType valueType { get; }
        /// <summary>
        /// Additional information about the register and its purpose.
        /// </summary>
        public int address { get; }
        /// <summary>
        /// Number of consecutive bytes from the initial <see cref="address"/> that takes the register.
        /// </summary>
        public int size { get; }
        /// <summary>
        /// Some special cases ( e.g. Encryption Key ) have a different size writing than reading from the MTU.
        /// </summary>
        public int sizeGet { get; }
        /// <summary>
        /// Indicates if the memory register is a read read-only field, which does not allow writing to the MTU.
        /// </summary>
        public bool write { get; }
        private string custom_Get { get; }
        private string custom_Set { get; }
        public string methodId_Get { get; }
        public string methodId_Set { get; }
        private CUSTOM_TYPE customType_Get;
        private CUSTOM_TYPE customType_Set;
        public REGISTER_TYPE registerType { get; }
        /// <summary>
        /// Indicates whether the value of the memory register has been modified/set and if it should be written to the MTU.
        /// </summary>
        /// <remarks>
        /// NOTE: This flag only indicates that the memory register has updated its value, not if it has been written in the MTU.
        /// </remarks>
        public bool modified { private set; get; }
        /// <summary>
        /// Indicates whether the memory register has obtained its value from the MTU at least once or not.
        /// </summary>
        public bool readedFromMtu;
        public bool finalRead;
        private Lexi.Lexi lexi;
        public byte[] lastRead;
        public bool onlyOneLexiAttempt { private set; get; }

        #endregion

        #region Properties

        // Value size ( number of consecutive bytes ) is also used for bit with bool type
        public int bit { get { return this.size; } }

        #region Custom Operation

        public string mathExpression_Get { get { return this.custom_Get; } }
        public string mathExpression_Set { get { return this.custom_Set; } }

        #endregion

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

        /// <summary>
        /// Indicates whether the register value will be recovered in raw format, without performing any post-process.
        /// <para>
        /// If <see langword="true"/>, the value will be processed before returned, modifying the value read from the physical memory of the MTU.
        /// </para>
        /// </summary>
        /// <remarks>
        /// NOTE: If <see cref="HasCustomMethod_Get"/> and <see cref="HasCustomOperation_Get"/> are <see langword="false"/>, the
        /// value will be recovered in raw format, without applying any post-process.
        /// </remarks>
        /// <seealso cref="HasCustomOperation_Get"/>
        public bool HasCustomMethod_Get
        {
            get { return this.customType_Get == CUSTOM_TYPE.METHOD; }
        }

        /// <summary>
        /// Indicates whether the register value will be calculated used the inline operation defined.
        /// <para>
        /// If <see langword="true"/>, the value will be processed before returned, modifying the value read from the physical memory of the MTU.
        /// </para>
        /// </summary>
        /// <remarks>
        /// NOTE: If <see cref="HasCustomMethod_Get"/> and <see cref="HasCustomOperation_Get"/> are <see langword="false"/>, the
        /// value will be recovered in raw format, without applying any post-process.
        /// </remarks>
        /// <seealso cref="HasCustomMethod_Get"/>
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

        /// <summary>
        /// Returns immediately the value stored in the register without
        /// applying any post-process to the data ( raw ) in register format.
        /// <para>
        /// See <see cref="ValueByteArrayRaw"/> to recover raw data in byte array format.
        /// </para>
        /// </summary>
        /// <remarks>
        /// NOTE: Properties can not be used asynchronously ( await.. ) because the return must be immediate.
        /// </remarks>
        /// <returns></returns>
        /// <seealso cref="GetValueFromMtu(bool)"/>
        /// <seealso cref="GetValueByteArray(bool)"/>
        /// <seealso cref="GetValue"/>
        public T ValueRaw
        {
            get
            {
                return this.funcGetMap ();
            }
        }
        
        /// <summary>
        /// Returns immediately the value stored in the register without
        /// applying any post-process to the data ( raw ) in byte array format.
        /// <para>
        /// See <see cref="ValueRaw"/> to recover raw data in in register format.
        /// </para>
        /// </summary>
        /// <remarks>
        /// NOTE: Properties can not be used asynchronously ( await.. ) because the return must be immediate.
        /// </remarks>
        /// <returns></returns>
        /// <seealso cref="GetValueFromMtu(bool)"/>
        /// <seealso cref="GetValueByteArray(bool)"/>
        /// <seealso cref="GetValue"/>
        public byte[] ValueByteArrayRaw
        {
            get
            {
                return this.funcGetByteArray ( true );
            }
        }

        #endregion

        #region Initialization

        public MemoryRegister () { }

        /// <param name="id">Identifier of the register</param>
        /// <param name="type">Type of the register ( int, uint, ulong, bool or string )</param>
        /// <param name="description">Additional information about the register and its purpose</param>
        /// <param name="address">First byte in the MTU memory used by the register</param>
        /// <param name="size">Number of consecutive bytes from the initial <see cref="address"/> that takes the register</param>
        /// <param name="sizeGet">Some special cases ( e.g. Encryption Key ) have a different <see cref="size"/> writing than reading from the MTU</param>
        /// <param name="write"><see langword="false"/> if it is read-only register</param>
        /// <param name="custom_Get">Identifier of the custom method used recovering the value of the register</param>
        /// <param name="custom_Set">Identifier of the custom method used writing a new value in the register</param>
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
                else this.methodId_Get = this.id + MemoryMap.METHOD_SUFFIX_GET;
            }

            // Custom Set
            if      ( this._HasCustomMethod_Set    ) this.customType_Set = CUSTOM_TYPE.METHOD;
            else if ( this._HasCustomOperation_Set ) this.customType_Set = CUSTOM_TYPE.OPERATION;
            else                                     this.customType_Set = CUSTOM_TYPE.EMPTY;

            if ( this.HasCustomMethod_Set )
            {
                if ( this._HasCustomMethodId_Set )
                     this.methodId_Set = this.custom_Set.Substring ( MemoryMap.METHOD_KEY.Length );
                else this.methodId_Set = this.id + MemoryMap.METHOD_SUFFIX_SET;
            }
        }

        #endregion
    
        #region Logic

        #region Get

        /// <summary>
        /// Returns asynchronously the value stored in the register without applying
        /// any post-process to the data ( raw ) in byte array format.
        /// <para>
        /// See <see cref="GetValueFromMtu(bool)"/> to recover data in register format.
        /// </para>
        /// </summary>
        /// <remarks>
        /// TODO: Rename to "GetValueByteArrayFromMtu"
        /// </remarks>
        /// <param name="useSizeGet"><see langword="false"/> to recover register data using the same size as writing</param>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.
        /// <para>
        /// Current value stored in the register in byte array format.
        /// </para>
        /// </returns>
        /// <seealso cref="ValueRaw"/>
        /// <seealso cref="ValueByteArrayRaw"/>
        /// <seealso cref="GetValue"/>
        public async Task<byte[]> GetValueByteArray (
            bool useSizeGet = true )
        {
            // Read value from MTU if is necessary
            await this.funcGet ();
        
            return this.funcGetByteArray ( useSizeGet );
        }

        /// <summary>
        /// Returns asynchronously the value stored in the register without
        /// applying any post-process to the data ( raw ).
        /// <para>
        /// See <see cref="GetValueByteArray(bool)"/> to recover data in byte array format.
        /// </para>
        /// </summary>
        /// <param name="returnByteArray"><see langword="true"/> to recover data in byte array format</param>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.
        /// <para>
        /// Current value stored in the register in register format or byte array format-
        /// </para>
        /// </returns>
        /// <seealso cref="ValueRaw"/>
        /// <seealso cref="ValueByteArrayRaw"/>
        /// <seealso cref="GetValue"/>
        public async Task<dynamic> GetValueFromMtu (
            bool returnByteArray = false,
            bool onlyOneLexiAttempt = false )
        {
            Utils.PrintDeep ( Environment.NewLine + "------LOAD_FROM_MTU------" );
            Utils.PrintDeep ( "Register -> GetValueFromMtu -> " + this.id + " [ Return byte array: " + returnByteArray + " ]" );
        
            // Reset flag that will be used in funcGet to invoke funcGetFromMtu
            this.readedFromMtu = false;
            var result = await this.GetValue ( onlyOneLexiAttempt );
            
            Utils.PrintDeep ( "---LOAD_FROM_MTU_FINISH--" + Environment.NewLine );
            
            if ( ! returnByteArray )
            	 return result;
          	else return this.lastRead;
        }

        /// <summary>
        /// Returns asynchronously the value cached in the register, without accesing
        /// the physical memory of the MTU, at least for this register because using
        /// custom methods could be necessary to recover other registers from the MTU.
        /// </summary>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.
        /// <para>
        /// Current ( cached ) value of the register.
        /// </para>
        /// </returns>
        /// <seealso cref="ValueRaw"/>
        /// <seealso cref="ValueByteArrayRaw"/>
        /// <seealso cref="GetValueFromMtu(bool)"/>
        /// <seealso cref="GetValueByteArray(bool)"/>
        public async Task<T> GetValue (
            bool onlyOneLexiAttempt = false )
        {
            this.onlyOneLexiAttempt = onlyOneLexiAttempt;

            // If register has not customized get method, use normal/direct get raw value
            if ( ! this.HasCustomMethod_Get )
                return await this.funcGet ();

            return await this.funcGetCustom ();
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

        #region Set

        /// <summary>
        /// Writes passed value first in the register and then in the physical memory of the MTU.
        /// <para>
        /// If no argument is passed, the current ( cached ) value of the register
        /// will be used to write to the physical memory of the MTU.
        /// </para>
        /// <para>
        /// See <see cref="SetBitToMtu"/> to modify only the bit pointed by the register in the physical memory of the MTU.
        /// </para>
        /// </summary>
        /// <param name="value">New value that will be set to the register and then written to the physical memory of the MTU</param>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.</returns>
        /// <seealso cref="ResetByteAndSetValueToMtu"/>
        /// <seealso cref="SetValue"/>
        public async Task SetValueToMtu (
            dynamic value = null )
        {
            Utils.PrintDeep ( Environment.NewLine + "------WRITE_TO_MTU-------" );
            Utils.PrintDeep ( "Register -> SetValueToMtu -> " + this.id + " = " + ( ( value != null ) ? value : this.ValueRaw ) );
        
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
                await this.lexi.Write (
                    ( uint )this.address,
                    this.funcGetByteArray ( false ) );
            }
            
            Utils.PrintDeep ( "---WRITE_TO_MTU_FINISH---" + Environment.NewLine );
        }

        /// <summary>
        /// Writes current value of the register in the physical memory of the MTU, only by modifying the specified bit.
        /// </summary>
        /// <remarks>
        /// NOTE: This method should only be used working with bool registers.
        /// </remarks>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.</returns>
        /// <seealso cref="SetValueToMtu"/>
        /// <seealso cref="ResetByteAndSetValueToMtu"/>
        private async Task SetBitToMtu ()
        {
            // Read current value
            byte systemFlags = ( await this.lexi.Read ( ( uint )this.address, 1, true ) )[ 0 ];

            Utils.PrintDeep ( "Register -> ValueWriteToMtu_Bit -> Current value map: " + this.id + " -> " + this.ValueRaw );
            Utils.PrintDeep ( "Register -> ValueWriteToMtu_Bit -> Current value MTU: " + this.id + " -> " + Utils.ByteToBits ( systemFlags ) + " [ Hex: " + systemFlags.ToString ( "D3" ) + " ]" );

            bool valueInMap = ( bool )( object )this.ValueRaw;

            // Modify bit and write to MTU
            if ( valueInMap )
                 systemFlags = ( byte ) ( systemFlags |    1 << ( int )bit   );
            else systemFlags = ( byte ) ( systemFlags & ~( 1 << ( int )bit ) );
            
            Utils.PrintDeep ( "Register -> ValueWriteToMtu_Bit -> Write full byte to MTU: " + this.id + " -> " + Utils.ByteToBits ( systemFlags ) + " [ Hex: " + systemFlags.ToString ( "D3" ) + " ] to bit: " + bit );
            
            await this.lexi.Write (
                ( uint )this.address,
                new byte[] { systemFlags } );
        }

        /// <summary>
        /// Updates only the memory register value, but does not write it in the physical memory of the MTU.
        /// </summary>
        /// <param name="value">Value that will be set to the register</param>
        /// <param name="force">Used to allow fill in a memory map while executing unit tests</param>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.</returns>
        /// <seealso cref="SetValueToMtu"/>
        /// <seealso cref="SetBitToMtu"/>
        /// <seealso cref="ResetByteAndSetValueToMtu"/>
        public async Task SetValue (
            dynamic value,
            bool force = false )
        {
            // Register with read and write
            if ( this.write || force )
            {
                Utils.Print ( "Register -> SetValue: " + this.id + " = " + value );
            
                try
                {
                    // Method will modify passed value before set in byte array
                    // If XML custom field is "method" or "method:id"
                    if ( this.HasCustomMethod_Set )
                    {
                        value = await this.funcSetCustom ( value );

                        Utils.Print ( "Register -> Value is null? " + ( value == null || value is byte[] && value.Length == 0 ) );

                        if ( value is byte[] )
                        {
                            System.Text.StringBuilder stb = new System.Text.StringBuilder ();
		                    foreach ( byte b in value )
			                    stb.Append( b.ToString("X2") );
                            Utils.Print ( "Register -> Value customized [ Bytes ]: " + stb.ToString () );
                        }
                        else Utils.Print ( "Register -> Value customized: " + value );
                    }
    
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
                    this.modified = true;
                }
                catch ( Exception e ) when ( Data.SaveIfDotNetAndContinue ( e ) )
                {
                    Utils.Print ( "Register -> SetValue [ ERROR ]: " + this.id + " -> " + e.Message );

                    // Is not own exception
                    if ( ! Errors.IsOwnException ( e ) )
                         throw new MemoryRegisterSetValueException ();
                    else throw e;
                }
            }
            // Register is readonly
            else
            {
                Utils.PrintDeep( "Set " + id + ": Error - Can't write to this register because is readonly" );

                if ( ! MemoryMap.isUnityTest )
                    throw new MemoryRegisterNotAllowWrite ( MemoryMap.EXCEP_SET_READONLY + ": " + id );
            }
        }

        /// <summary>
        /// Set to zero/0 the value of the register, reseting all its bits.
        /// <para>
        /// If the argument is not null, set the bit referenced by the register to desired value and
        /// finally writes current byte value to the physical memory of the MTU.
        /// </para>
        /// <para>
        /// See <see cref="SetValueToMtu"/> to modify the value of the register and write it to the physical memory of the MTU.
        /// </para>
        /// </summary>
        /// <remarks>
        /// NOTE: This method should only be used working with bool registers.
        /// <para>
        /// TODO: Rename to "ResetByteAndSetBitToMtu"
        /// </para>
        /// </remarks>
        /// <param name="value">New value that will be set to the bit pointed by the register and then written to the physical memory of the MTU</param>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.</returns>
        /// <seealso cref="SetValue"/>
        /// <seealso cref="SetBitToMtu"/>
        public async Task ResetByteAndSetValueToMtu (
            dynamic value = null )
        {
            if ( this.valueType == RegType.BOOL )
            {
                Utils.PrintDeep( "Register -> ResetByte -> " + this.id + ( ( value != null ) ? " = " + value : "" ) );
            
                // Reset full byte ( all flags to zero/disable )
                await this.lexi.Write (
                    ( uint )this.address,
                    new byte[] { default ( byte ) } );
                
                // Write flag for this register
                await this.SetValueToMtu ( value );
            }
        }

        #endregion

        #endregion

        #region Compare

        /// <summary>
        /// Compares two <see cref="MemoryRegister"/>s, used to know if two <see cref="MemoryMap"/>s are the
        /// same and which ones have been modified from one to another.
        /// </summary>
        /// <param name="other">Other <see cref="MemoryRegister"/> to compare with this</param>
        /// <returns><see langword="true"/> if both registers have the same values.</returns>
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
                if ( this.valueType != RegType.BOOL )
                {
                    valLocal = this.ValueByteArrayRaw;
                    valOther = await other.GetValueByteArray ();
                    
                    ok_value = valLocal.SequenceEqual ( valOther );
                    
                    Utils.PrintDeep( "Equals: " + this.id + " -> " +
                    Utils.ByteArrayToString ( valLocal ) + " == " +
                    Utils.ByteArrayToString ( valOther ) + " = " +
                    ok_value );
                }
                else
                {
                    T bitLocal = this.ValueRaw;
                    T bitOther = await other.GetValueFromMtu ();
                
                    ok_value = ( bool.Equals ( bitLocal, bitOther ) );
                    
                    Utils.PrintDeep( "Equals: " + this.id + " -> " +
                        bitLocal + " == " +
                        bitOther + " = " +
                        ok_value );
                }
            }
            catch ( Exception )
            {
                return false;
            }

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
