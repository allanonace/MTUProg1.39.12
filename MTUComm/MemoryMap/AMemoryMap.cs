using System.Collections.Generic;
using System.Dynamic;
using Library;
using Library.Exceptions;
using System.Threading.Tasks;

namespace MTUComm.MemoryMap
{
    public abstract class AMemoryMap : DynamicObject
    {
        #region Constants

        /// <summary>
        /// Types of elements present in the XML memory map, associated with
        /// <see cref="MemoryRegister{T}"/>,
        /// <see cref="MemoryOverload{T}"/>
        /// respectively
        /// <para>&#160;</para>
        /// </para>
        /// <list type="REGISTER_TYPE">
        /// <item>
        ///     <term>REGISTER_TYPE.REGISTER</term>
        ///     <description>Memory register</description>
        /// </item>
        /// <item>
        ///     <term>REGISTER_TYPE.OVERLOAD</term>
        ///     <description>Overload that uses N memory registers to format and return a value</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        public enum REGISTER_TYPE { REGISTER, OVERLOAD }

        #endregion

        #region Attributes

        /// <summary>
        /// Dictionary used by the dynamic object to register dynamic members.
        /// <para>
        /// NOTE: Should not be used directly, instead use TrySetMember and TryGetMember methods,
        /// invoked transparently to the user who only needs to use the dot operator ( "." ).
        /// </para>
        /// <para>
        /// See <see cref="TrySetMember" /> to add a new member dynamically to the object.
        /// </para>
        /// </summary>
        private Dictionary<string, dynamic> dictionary;
        protected dynamic registers { get; }

        #endregion

        #region Indexer

        /// <summary>
        /// Easy way to recover members registered in the dynamic object, in this case
        /// <see cref="MemoryRegister{T}"/>,
        /// <see cref="MemoryOverload{T}"/> and
        /// the associated methods.
        /// </summary>
        /// <remarks>
        /// NOTE: The recovered object using this indexer or using the dot operator ( "." ) is the same.
        /// <para>
        /// <code>
        /// MemoryRegister<int> mreg1 = map[ "MtuType" ];
        /// MemoryRegister<int> mreg2 = map.MtuType;
        /// bool same = ( mreg1 == mreg2 ); // true
        /// </code>
        /// <para>
        /// </remarks>
        /// <value></value>
        public dynamic this[ string id ]
        {
            get
            {
                if ( this.dictionary.ContainsKey ( id ) )
                    return this.dictionary[ id ];

                // Selected dynamic member not exists
                Utils.Print ( "Get " + id + ": Error - " + MemoryMap.EXCEP_SET_USED );
                throw new MemoryRegisterNotExistException ( id + ".Indexer" );
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// This is the abstract class used as base to generate the dynamic memory map for interact with the physical memory of the MTUs.
        /// </summary>
        public AMemoryMap ()
        {
            // Will contain MemoryRegister objects but thanks to TryGetMember
            // the returned values always will be property get block returned value
            // TryGetMember is only called using dot operator ( registers.id_register )
            this.dictionary = new Dictionary<string,dynamic> ();

            // En la clase MemoryMap no se puede usar directamente "this|base.idPropiedad", habiendo
            // de declarar esta variable dinamica mediante la cual acceder a los miembros dinamicos
            this.registers = this;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a dynamic member to the memory map of type
        /// <see cref="MemoryRegister{T}"/>
        /// or <see cref="MemoryOverload{T}"/>.
        /// </summary>
        /// <param name="register">Instance of the element to add to the memory map</param>
        protected void AddProperty ( dynamic register )
        {
            if ( ! this.dictionary.ContainsKey ( register.id ) )
                this.dictionary.Add ( register.id, register );
        }

        /// <summary>
        /// Add a dynamic member to the memory map of type Func<> or Action<>
        /// </summary>
        /// <param name="id">Name of the method to be registered</param>
        /// <param name="method">Reference to the method</param>
        protected void AddMethod ( string id, dynamic method )
        {
            if ( ! this.dictionary.ContainsKey ( id ) )
                this.dictionary.Add ( id, method );
        }

        /// <summary>
        /// Check if the memory map contains/has registered a member with an specific name.
        /// </summary>
        /// <param name="id">Name of the element to search</param>
        /// <returns><see langword="true"/> if the member is present in the memory map</returns>
        public bool ContainsMember ( string id )
        {
            return this.dictionary.ContainsKey ( id );
        }

        #region Dot operator

        /// <summary>
        /// Method inherited from DynamicObject base class, that allows to set
        /// new dynamic members using the dot operator ( "." ).
        /// </summary>
        /// <remarks>
        /// NOTE: This method can not be converted into an asynchronous method.
        /// </remarks>
        /// <param name="binder">Use binder.Name property to recover the member ID/name specified</param>
        /// <param name="value">Value to be associated to the new generated member</param>
        /// <returns><see langword="true"/> if the new member is generated correctly.</returns>
        public override bool TrySetMember ( SetMemberBinder binder, object value )
        {
            return this.Set ( binder.Name, value ).Result;
        }

        private async Task<bool> Set ( string id, object value )
        {
            // Selected dynamic member exists
            if ( this.dictionary.ContainsKey ( id ) )
            {
                dynamic register = this.dictionary[id];

                // Overloads are readonly
                if ( register.GetType().GetGenericTypeDefinition() == typeof( MemoryOverload<> ) )
                    throw new MemoryOverloadsAreReadOnly ( MemoryMap.EXCEP_OVE_READONLY + ": " + id );

                await this.dictionary[id].SetValue ( value );

                return true;
            }

            // Selected dynamic member not exists
            Utils.Print ( "Set " + id + ": Error - Selected register is not loaded" );
            throw new MemoryRegisterNotExistException ( id + ".Set" );
        }

        /// <summary>
        /// Method inherited from DynamicObject base class, that allows to get/recover
        /// registered dynamic members using the dot operator ( "." ).
        /// </summary>
        /// <remarks>
        /// NOTE: This method can not be converted into an asynchronous method.
        /// </remarks>
        /// <param name="binder">Use binder.Name property to recover the member ID/name specified</param>
        /// <param name="result">It will be the reference to the recovered member</param>
        /// <returns><see langword="true"/> if the member is present in the dictionary and it is recovered correctly.</returns>
        public override bool TryGetMember ( GetMemberBinder binder, out object result )
        {
            return this.Get ( binder.Name, out result );
        }

        private bool Get ( string id, out object result )
        {
            if ( this.dictionary.ContainsKey ( id ) )
            {
                dynamic register = this.dictionary[id];

                result = this.dictionary[ id ];

                /*
                if ( register.registerType == REGISTER_TYPE.REGISTER )
                {
                    // Some registers have customized get method
                    if ( ! register.HasCustomMethod_Get )
                         result = ( object )this.dictionary[ id ].GetValueRaw (); // Invokes funGet method
                    else result = ( object )this.dictionary[ id ].GetValue ();    // Invokes funGetCustom method
                }
                else // Overload
                    result = ( object )this.dictionary[ id ].GetValue ();
                */

                return true;
            }

            // Selected dynamic member not exists
            Utils.Print ( "Get " + id + ": Error - Selected register is not loaded" );
            throw new MemoryRegisterNotExistException ( id + ".Get" );
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

        /*
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
        */

        #endregion
    }
}
