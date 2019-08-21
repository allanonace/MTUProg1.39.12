using System;
using System.Collections.Generic;
using System.Linq;

using NodeType = Lexi.Lexi.NodeType;

namespace MTUComm
{
    /// <summary>
    /// Auxiliar class that will contain all the nodes/DCUs detected by the MTU during the
    /// <see cref="MTUComm.NodeDiscovery"/> process.
    /// <para>
    /// It is used also to calculate the probability of establish
    /// a good transmission channel between both devices ( MTU-DCU ).
    /// </para>
    /// </summary>
    /// <seealso cref="NodeDiscovery"/>
    public class NodeDiscoveryList
    {
        #region Constants

        /// <summary>
        /// Table with pre-calculated values to easiest know the probability
        /// of establish a good transmission channel, based on signal strength ( RSSI ).
        /// </summary>
        public static (int RSSI,decimal Probability)[] RSSI_and_Probability =
        {
            ( -115, 0.002m  ), // ~0% probability
            ( -114, 0.0154m ),
            ( -113, 0.0288m ),
            ( -112, 0.0422m ),
            ( -111, 0.0556m ),
            ( -110, 0.069m  ),
            ( -109, 0.1134m ),
            ( -108, 0.1578m ),
            ( -107, 0.2022m ),
            ( -106, 0.2466m ),
            ( -105, 0.291m  ),
            ( -104, 0.3426m ),
            ( -103, 0.3942m ),
            ( -102, 0.4458m ),
            ( -101, 0.4974m ),
            ( -100, 0.549m  ),
            (  -99, 0.5894m ),
            (  -98, 0.6298m ),
            (  -97, 0.6702m ),
            (  -96, 0.7106m ),
            (  -95, 0.751m  ),
            (  -94, 0.7776m ),
            (  -93, 0.8042m ),
            (  -92, 0.8308m ),
            (  -91, 0.8574m ),
            (  -90, 0.884m  ),
            (  -89, 0.8962m ),
            (  -88, 0.9084m ),
            (  -87, 0.9206m ),
            (  -86, 0.9328m ),
            (  -85, 0.945m  ),
            (  -84, 0.9506m ),
            (  -83, 0.9562m ),
            (  -82, 0.9618m ),
            (  -81, 0.9674m ),
            (  -80, 0.973m  ),
            (  -79, 0.9758m ),
            (  -78, 0.9786m ),
            (  -77, 0.9814m ),
            (  -76, 0.9842m ),
            (  -75, 0.987m  ),
            (  -74, 0.9882m ),
            (  -73, 0.9894m ),
            (  -72, 0.9906m ),
            (  -71, 0.9918m ),
            (  -70, 0.993m  ),
            (  -69, 0.9938m ),
            (  -68, 0.9946m ),
            (  -67, 0.9954m ),
            (  -66, 0.9962m ),
            (  -65, 0.997m  ),
            (  -64, 0.9972m ),
            (  -63, 0.9974m ),
            (  -62, 0.9976m ),
            (  -61, 0.9978m ),
            (  -60, 0.998m  ),
            (  -59, 0.9982m ),
            (  -58, 0.9984m ),
            (  -57, 0.9986m ),
            (  -56, 0.9988m ),
            (  -55, 0.999m  ),
            (  -54, 0.999m  ),
            (  -53, 0.999m  ),
            (  -52, 0.999m  ),
            (  -51, 0.999m  ),
            (  -50, 0.999m  ),
            (  -49, 0.9992m ),
            (  -48, 0.9994m ),
            (  -47, 0.9996m ),
            (  -46, 0.9998m ),
            (  -45, 1m      )  // 100% probability
        };

        /// <summary>
        /// Result based on the last message received from the MTU
        /// during the <see cref="MTUComm.NodeDiscovery"/> process.
        /// <para>&#160;</para>
        /// </para>
        /// <list type="NodeDiscoveryQueryResult">
        /// <item>
        ///     <term>NodeDiscoveryQueryResult.NextRead</term>
        ///     <description>A new node has been received and it is not the last</description>
        /// </item>
        /// <item>
        ///     <term>NodeDiscoveryQueryResult.LastRead</term>
        ///     <description>A new node has been received and it is the last</description>
        /// </item>
        /// <item>
        ///     <term>NodeDiscoveryQueryResult.Empty</term>
        ///     <description>The last response is empty and indicates that no node/DCUs was detected</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        public enum NodeDiscoveryQueryResult
        {
            NextRead,
            LastRead,
            Empty
        }

        private const int  BYTE_RESULT = 2;
        private const byte HAS_DATA    = 0x00;

        #endregion

        #region Attributes

        private List<NodeDiscovery> entries;
        private NodeType nodeType;
        private int uniqueNodeIDs;

        #endregion

        #region Properties

        public NodeType NodeType
        {
            get { return this.nodeType; }
        }

        /// <summary>
        /// The total number of nodes/DCUs detected.
        /// </summary>
        /// <value></value>
        public int Count
        {
            get { return this.entries.Count; }
        }

        /// <summary>
        /// Full list with all nodes/DCUs detected.
        /// <para>
        /// See <see cref="NodeDiscovery"/> for the representation of the info associated to the detected nodes/DCUs.
        /// </para>
        /// </summary>
        /// <value></value>
        public NodeDiscovery[] Entries
        {
            get { return this.entries.ToArray (); }
        }

        /// <summary>
        /// List of the validated nodes/DCUs only.
        /// <para>
        /// See <see cref="NodeDiscovery"/> for the representation of the info associated to the detected nodes/DCUs.
        /// </para>
        /// </summary>
        /// <value></value>
        public NodeDiscovery[] EntriesValidated
        {
            get { return this.entries.Where ( entry => entry.IsValidated ).ToArray (); }
        }

        /// <summary>
        /// The number of the validated nodes/DCUs only.
        /// </summary>
        /// <value></value>
        public int CountEntriesValidated
        {
            get { return this.entries.Where ( entry => entry.IsValidated ).Count (); }
        }

        /// <summary>
        /// The total number of nodes/DCUs that should be recovered.
        /// </summary>
        /// <value></value>
        public int TotalEntries
        {
            get
            {
                if ( this.entries.Count > 0 )
                    return this.entries[ 0 ].TotalEntries;
                return -1;
            }
        }

        /// <summary>
        /// Returns the last node/DCU detecten, used during the <see cref="MTUComm.NodeDiscovery"/> process.
        /// <para>
        /// See <see cref="NodeDiscovery"/> for the representation of the info associated to the detected nodes/DCUs.
        /// </para>
        /// </summary>
        /// </summary>
        /// <value></value>
        public NodeDiscovery LastNode
        {
            get
            {
                if ( this.entries.Count > 0 )
                    return this.entries[ this.entries.Count - 1 ];
                return null;
            }
        }

        public int CountUniqueNodes
        {
            get { return this.uniqueNodeIDs; }
        }

        #endregion

        #region Initialization

        public NodeDiscoveryList (
            NodeType nodeType )
        {
            this.entries  = new List<NodeDiscovery> ();
            this.nodeType = nodeType;
            this.uniqueNodeIDs = 0;
        }

        #endregion

        public ( NodeDiscoveryQueryResult Result, int Index ) TryToAdd (
            byte[] response )
        {
            NodeDiscovery node = null;
        
            switch ( response[ BYTE_RESULT ] )
            {
                // ACK without node entry
                case byte val when val != 0x00:
                    return ( NodeDiscoveryQueryResult.Empty, -1 );

                // ACK with node entry
                case HAS_DATA:
                    node = new NodeDiscovery ( response );
                    // Repeating entry
                    if ( this.entries.Count >= node.Index )
                         this.entries[ node.Index - 1 ] = node;
                    // New entry
                    else this.entries.Add ( node );
                    break;
            }

            // Check if it is a new DCU
            if ( node.NodeId > 0 &&
                 ! this.entries.Any ( n => n.NodeId == node.NodeId ) )
                uniqueNodeIDs++;

            return ( ( this.entries[ this.entries.Count - 1 ].IsLast ) ?
                NodeDiscoveryQueryResult.LastRead : NodeDiscoveryQueryResult.NextRead, node.Index );
        }
    
        /// <summary>
        /// Calculates the probability of establish a good transmission channel
        /// between the MTU and a node/DCU, based on signal strength ( RSSI ).
        /// </summary>
        /// <remarks>
        /// The signal strength -45 is very difficult to achieve, because it
        /// is the perfect signal, which allows a perfectly estable transfer.
        /// <para>
        /// By default, the minimum acceptable probability for channel F1 ( slow ) is 0.75 and for channel F2 ( fast ) is 0.5;
        /// </para>
        /// <para>
        /// By default, the desired probability for channel F1 ( slow ) is 0.985 and for channel F2 ( fast ) is 0.75.
        /// </para>
        /// </remarks>
        /// <param name="rssi">Signal strength</param>
        /// <returns>Normalized probability [0-1]</returns>
        public decimal GetProbability (
            int rssi )
        {
            ( int RSSI, decimal Probability )[] entries = RSSI_and_Probability
                .Where ( entry => entry.RSSI == rssi ).ToArray ();

            if ( entries.Length > 0 )
                return entries[ 0 ].Probability;
            else
            {
                if ( rssi >= -45 )
                     return 1m; // 100% probability because the signal strength is very good 
                else return 0m; // rssi <= -115, 0% probability because the signal strength is very small
            }
        }
    }
}
