using Library;
using Library.Exceptions;
using System;
using System.Threading.Tasks;

using RegType       = MTUComm.MemoryMap.MemoryMap.RegType;
using REGISTER_TYPE = MTUComm.MemoryMap.AMemoryMap.REGISTER_TYPE;

namespace MTUComm.MemoryMap
{
    /// <summary>
    /// Special read-only structure created not to represent n bytes of the physical memory of the MTU,
    /// as with <see cref="MemoryRegister"/>, but to calculate and format the returned value,
    /// referencing and working with one or more registers ( <see cref="MemoryRegister"/> and <see cref="MemoryOverload"/> ).
    /// </summary>
    public class MemoryOverload<T> : IEquatable<MemoryOverload<T>>
    {
        #region Constants

        /// <summary>
        /// Types of custom values that can be set in
        /// the CustomGet tag in the memory maps XML entries.
        /// <para>
        /// <code>
        /// <Overload> <!-- OK -->
        ///     <Id>TiltTamperStatus</Id>
        ///     <Description>Tilt Tamper Status</Description>
        ///     <Type>string</Type>
        ///     <Registers> // MemoryRegister or MemoryOverload IDs
        ///         <Register>TiltAlarm</Register>
        ///         <Register>TiltTamper</Register>
        ///     </Registers>
        ///     <CustomGet>method:TamperStatus_Get</CustomGet> // Method with specific ID
        /// </Overload>
        /// <Overload>
        ///     <Id>MtuSoftVersion</Id>
        ///     <Description>Mtu software/firmware version</Description>
        ///     <Type>int</Type>
        ///     <Registers>
        ///         <Register>MtuSoftVersionLegacy</Register>
        ///         <Register>MtuSoftVersionNew</Register>
        ///     </Registers>
        ///     <CustomGet>method</CustomGet> // References to the method MtuSoftVersion_Get
        /// </Overload>
        /// <Overload>
        ///     <Id>XmitInterval</Id>
        ///     <Description>Transmit Interval</Description>
        ///     <Type>string</Type>
        ///     <Registers>
        ///         <Register>MessageOverlapCount</Register>
        ///         <Register>ReadIntervalMinutes</Register>
        ///     </Registers>
        ///     <CustomGet>_2_ - _1_</CustomGet> // Inline operation
        /// </Overload>
        /// </code>
        /// </para>
        /// <para>&#160;</para>
        /// </para>
        /// <list type="CUSTOM_TYPE">
        /// <item>
        ///     <term>CUSTOM_TYPE.METHOD</term>
        ///     <description>Set to use a custom method, using "method", and the associated
        /// method should have the suffix _Get or _Set, or an specific name ( >method:...</ )</description>
        /// </item>
        /// <item>
        ///     <term>CUSTOM_TYPE.OPERATION</term>
        ///     <description>Inline operation using "_index_" that will be replaced by the specified register value, working in base one</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        /// <remarks>
        /// NOTE: The following arithmetic operators are supported in expressions: +  -  *  /  %
        /// </remarks>
        private enum CUSTOM_TYPE { OPERATION, METHOD }

        #endregion

        #region Attributes

        public Func<Task<T>> funcGet;
        public string id { get; }
        public string description { get; }
        public RegType valueType { get; }
        public string[] registerIds { get; }
        public string custom_Get { get; }
        public string methodId { get; }
        private CUSTOM_TYPE customType;
        public REGISTER_TYPE registerType { get; }

        #endregion

        #region Properties

        private bool _HasCustomMethod
        {
            get { return this.custom_Get.ToLower ().StartsWith ( MemoryMap.METHOD ); }
        }

        private bool _HasCustomMethodId
        {
            get { return this.custom_Get.ToLower ().StartsWith (MemoryMap.METHOD_KEY ); }
        }

        private bool _HasCustomOperation
        {
            get { return ! this._HasCustomMethod      &&
                           this.valueType < RegType.CHAR &&
                         ! string.IsNullOrEmpty ( this.custom_Get ); }
        }

        /// <summary>
        /// Indicates whether the overload is configured with a method ( using "method"
        /// or an specific method ID ) in the custom get method tag.
        /// </summary>
        public bool HasCustomMethod
        {
            get { return this.customType == CUSTOM_TYPE.METHOD; }
        }

        /// <summary>
        /// Indicates whether the overload is configured with an
        /// inline operation in the custom get method tag.
        /// </summary>
        public bool HasCustomOperation
        {
            get { return this.customType == CUSTOM_TYPE.OPERATION; }
        }

        /// <summary>
        /// Returns asynchronously the value calculated and formated working with the list of specified registers,
        /// applying or not ( raw ) some post-process to the value of each register depending on their configuration.
        /// <para>
        /// See <see cref="MemoryRegister.HasCustomMethod_Get"/> to know if the value of a <see cref="MemoryRegister"/> will be used in raw format.
        /// </para>
        /// </summary>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.
        /// <para>
        /// Current value stored in the register in register format or byte array format-
        /// </para>
        /// </returns>
        public async Task<T> GetValue ()
        {
            return await this.funcGet ();
        }

        #endregion

        #region Initialization

        /// <param name="id">Identifier of the register</param>
        /// <param name="type">Type of the register ( int, uint, ulong, bool or string )</param>
        /// <param name="description">Additional information about the register and its purpose</param>
        /// <param name="registerIds">List of registers ( <see cref="MemoryRegister"/> and <see cref="MemoryOverload"/> ) to work with</param>
        /// <param name="custom">Identifier of the custom method used recovering the value of the overload</param>
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
            this.custom_Get       = custom.Replace ( " ", string.Empty );
            this.registerType = REGISTER_TYPE.OVERLOAD;

            if      ( this._HasCustomMethod    ) this.customType = CUSTOM_TYPE.METHOD;
            else if ( this._HasCustomOperation ) this.customType = CUSTOM_TYPE.OPERATION;
            else
            {
                // Selected dynamic member not exists
                Utils.Print ( "Get " + id + ": Error - Overload registers need custom field" );

                if ( ! MemoryMap.isUnityTest )
                    throw new OverloadEmptyCustomException ( id );
            }

            if ( this.HasCustomMethod )
            {
                if ( this._HasCustomMethodId )
                     this.methodId = this.custom_Get.Substring ( MemoryMap.METHOD_KEY.Length );
                else this.methodId = this.id + MemoryMap.METHOD_SUFFIX_GET;
            }
        }

        #endregion

        #region Compare

        /// <summary>
        /// Compares two <see cref="MemoryOverload"/>s, used to know if two <see cref="MemoryMap"/>s
        /// are the same and which ones have been modified from one to another.
        /// </summary>
        /// <remarks>
        /// NOTE: The value is not calculated nor recovered, only comparing the
        /// overload data ( ID, description, method ID and list of register IDs ).
        /// </remarks>
        /// <param name="other">Other <see cref="MemoryOverload"/> to compare with this</param>
        /// <returns><see langword="true"/> if both registers have the same values.</returns>
        public bool Equals ( MemoryOverload<T> other )
        {
            if ( other == null )
                return false;

            if ( this.registerIds.Length == other.registerIds.Length )
                for ( int i = this.registerIds.Length - 1; i >= 0; i-- )
                    if ( ! string.Equals ( this.registerIds[ i ], other.registerIds[ i ] ) )
                        return false;

            bool ok_id          = string.Equals ( this.id, other.id );
            bool ok_description = string.Equals ( this.description, other.description );
            bool ok_methodId    = string.Equals ( this.methodId, other.methodId );

            return ok_id          &&
                   ok_description &&
                   ok_methodId;
        }

        #endregion
    }
}
