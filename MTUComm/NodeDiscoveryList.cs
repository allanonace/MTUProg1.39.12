using System;
using System.Collections.Generic;
using System.Linq;
using Library;

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

        private const short MIN_RSSI = -128; // ~0% probability
        private const short MAX_RSSI = -84;  // 100% probability

        /// <summary>
        /// Table with pre-calculated values to easiest know the probability
        /// of establish a good transmission channel, based on signal strength ( RSSI ).
        /// </summary>
        public readonly static (short RSSI,decimal Probability)[] RSSI_and_Probability =
        {
            ( -128,   0.00m ), // ~0% probability
            ( -127,  20.57m ),
            ( -126,  36.90m ),
            ( -125,  49.88m ),
            ( -124,  60.19m ),
            ( -123,  68.38m ),
            ( -122,  74.88m ),
            ( -121,  80.05m ),
            ( -120,  84.15m ),
            ( -119,  87.41m ),
            ( -118,  90.00m ),
            ( -117,  92.06m ),
            ( -116,  93.69m ),
            ( -115,  94.99m ),
            ( -114,  96.02m ),
            ( -113,  96.84m ),
            ( -112,  97.49m ),
            ( -111,  98.00m ),
            ( -110,  98.42m ),
            ( -109,  98.74m ),
            ( -108,  99.00m ),
            ( -107,  99.21m ),
            ( -106,  99.37m ),
            ( -105,  99.50m ),
            ( -104,  99.60m ),
            ( -103,  99.68m ),
            ( -102,  99.75m ),
            ( -101,  99.80m ),
            ( -100,  99.84m ),
            (  -99,  99.87m ),
            (  -98,  99.90m ),
            (  -97,  99.92m ),
            (  -96,  99.94m ),
            (  -95,  99.95m ),
            (  -94,  99.96m ),
            (  -93,  99.97m ),
            (  -92,  99.97m ),
            (  -91,  99.98m ),
            (  -90,  99.98m ),
            (  -89,  99.98m ),
            (  -88,  99.99m ),
            (  -87,  99.99m ),
            (  -86,  99.99m ),
            (  -85,  99.99m ),
            (  -84, 100.00m )  // 100% probability
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
            GeneralInfo,
            NextRead,
            LastRead,
            Empty
        }

        private const int  BYTE_RESULT = 2;
        private const byte HAS_DATA    = 0x00;

        #endregion

        #region Attributes

        private List<NodeDiscovery> entries;
        private List<List<NodeDiscovery>> attempts;
        private NodeType nodeType;

        #endregion

        #region Properties

        public NodeType NodeType
        {
            get { return this.nodeType; }
        }

        /// <summary>
        /// Full list with all nodes/DCUs detected.
        /// <para>
        /// See <see cref="NodeDiscovery"/> for the representation of the info associated to the detected nodes/DCUs.
        /// </para>
        /// </summary>
        public List<List<NodeDiscovery>> AllAttempts
        {
            get
            {
                List<List<NodeDiscovery>> mix = new List<List<NodeDiscovery>> ( this.attempts );
                mix.Add ( this.entries );

                return mix;
            }
        }

        public NodeDiscovery[] CurrentAttemptEntries
        {
            get { return this.entries.ToArray (); }
        }

        public bool HasCurrentAttemptEntries ()
        {
            return this.entries.Count > 1;
        }

        /// <summary>
        /// The total number of nodes/DCUs that should be recovered.
        /// </summary>
        public uint CurrentAttemptTotalEntries
        {
            get
            {
                if ( this.entries.Count > 0 )
                    return this.entries[ 0 ].TotalEntries;
                return 0;
            }
        }

        /// <summary>
        /// Returns the last node/DCU detecten, used during the <see cref="MTUComm.NodeDiscovery"/> process.
        /// <para>
        /// See <see cref="NodeDiscovery"/> for the representation of the info associated to the detected nodes/DCUs.
        /// </para>
        /// </summary>
        /// </summary>
        public NodeDiscovery CurrentAttemptLastEntry
        {
            get
            {
                if ( this.entries.Count > 0 )
                    return this.entries[ this.entries.Count - 1 ];
                return null;
            }
        }

        public List<NodeDiscovery> CurrentAttempt
        {
            get { return this.entries; }
        }

        private IEnumerable<IGrouping<uint,NodeDiscovery>> UniqueNodesValidated
        {
            get
            {
                return this.AllAttempts
                    // Converts a multi-dimensional array into a flat list with only validated nodes
                    .SelectMany ( list => list
                        .Where ( entry => entry.IsValidated )
                    )
                    // Avoids counting the same node multiple times
                    .GroupBy ( entry => entry.NodeId );
            }
        }

        /// <summary>
        /// The number of the validated nodes/DCUs only.
        /// </summary>
        /// <remarks>
        /// Only count once each DCU ( node ID ), not once per DCU channel ( F1 and F2 ).
        /// <para>
        /// The returned value should be equal or lower than <see cref="CountUniqueNodes"/>.
        /// </para>
        /// </remarks>
        public int CountUniqueNodesValidated
        {
            get { return this.UniqueNodesValidated.Count (); }
        }

        #endregion

        #region Initialization

        public NodeDiscoveryList (
            NodeType nodeType )
        {
            this.entries  = new List<NodeDiscovery> ();
            this.attempts = new List<List<NodeDiscovery>> ();
            this.nodeType = nodeType;
        }

        #endregion

        #region Logic

        public void StartNewAttempt ()
        {
            if ( this.entries.Count > 0 )
            {
                List<NodeDiscovery> copy = new List<NodeDiscovery> ();
                foreach ( NodeDiscovery node in this.entries )
                    copy.Add ( node.Clone () as NodeDiscovery );
                
                this.attempts.Add ( copy );
            }

            this.entries.Clear ();
        }

        public ( NodeDiscoveryQueryResult Result, uint Index ) TryToAdd (
            byte[] response,
            ref bool ok )
        {
            NodeDiscovery node = null;
        
            switch ( response[ BYTE_RESULT ] )
            {
                // ACK without node entry
                case byte val when val != 0x00:
                    return ( NodeDiscoveryQueryResult.Empty, 0 );

                // ACK with node entry
                case HAS_DATA:
                    // NOTE: It happened once LExI returned an array of bytes without the required amount of data
                    if ( ! ( ok = ( response.Length == NodeDiscovery.BYTES_REQUIRED_DATA_1 ) ||
                                    response.Length == NodeDiscovery.BYTES_REQUIRED_DATA_2 ) )
                        return ( NodeDiscoveryQueryResult.Empty, 0 );

                    node = new NodeDiscovery ( response );

                    // Repeating entry
                    if ( this.entries.Count >= node.Index )
                    {
                        Utils.Print ( "Node Discovery: Processing already retrieved response/node" );

                        // Clear previous responses
                        this.entries.RemoveRange ( ( int )node.Index - 1, this.entries.Count - ( ( int )node.Index - 1 ) );
                    }

                    // New entry
                    this.entries.Add ( node );
                    break;
            }

            bool last = this.CurrentAttemptLastEntry.IsLast;

            if ( this.entries.Count == 1 )
                return ( last ?
                    NodeDiscoveryQueryResult.Empty : NodeDiscoveryQueryResult.GeneralInfo,
                    node.Index );

            return ( last ?
                NodeDiscoveryQueryResult.LastRead : NodeDiscoveryQueryResult.NextRead,
                node.Index );
        }
    
        /// <summary>
        /// Calculates the probability of establish a good transmission channel
        /// between the MTU and a node/DCU, based on signal strength ( RSSI ).
        /// </summary>
        /// <remarks>
        /// The signal strength MAX_RSSI is very difficult to achieve, because it
        /// is the perfect signal, which allows a perfectly estable transfer.
        /// <para>
        /// By default, the minimum acceptable probability for channel F1 ( slow ) and channel F2 ( fast ) is 0.70.
        /// </para>
        /// <para>
        /// By default, the desired probability for channel F1 ( slow ) and channel F2 ( fast ) is 0.90.
        /// </para>
        /// </remarks>
        /// <param name="rssi">Signal strength</param>
        /// <returns>Normalized probability [0-1]</returns>
        public decimal GetProbability (
            short rssi )
        {
            ( short RSSI, decimal Probability )[] entries = RSSI_and_Probability
                .Where ( entry => entry.RSSI == rssi )
                .ToArray ();
            
            // Normalize probability [0,1]
            for ( int i = 0; i < entries.Length; i++ )
            {
                var entry = entries[ i ];
                entry.Probability /= 100m;
                entries[ i ] = entry;
            }

            if ( entries.Length > 0 )
                return entries[ 0 ].Probability;
            else
            {
                if ( rssi >= MAX_RSSI )
                     return 1m; // 100% probability because the signal strength is very good 
                else return 0m; // rssi <= MIN_RSSI, 0% probability because the signal strength is very small
            }
        }
    
        public decimal CalculateMtuSuccess (
            bool isF1 )
        {
            decimal acumRssi;
            short   averageRssi;
            decimal acumProb = 1m;
            foreach ( IGrouping<uint,NodeDiscovery> group in this.NodesValidatedForFreq ( isF1 ) )
            {
                acumRssi = 0;
                
                // Sum RSSI values to calculate the average value for each DCU
                foreach ( NodeDiscovery entry in group )
                    acumRssi += entry.RSSIRequest;

                // Rounds the average RSSI because RSSI values in the table are shorts
                averageRssi = ( short )Math.Round ( acumRssi / group.Count (), 0 );

                // Calculate half of P( MTU TX Success ) operation
                acumProb *= ( 1 - this.GetProbability ( averageRssi ) );
            }

            if ( isF1 )
                Library.Utils.Print ( "ND: F1 MtuTxSuccess " + ( 1 - acumProb ) +
                    " | AcumProb " + acumProb );

            // P( MTU TX Success )
            return 1 - acumProb;
        }

        public decimal CalculateTwoWaySuccess (
            short bestRssiResponse )
        {
            // P( MTU TX Success )
            decimal mtuTxSuccess = this.CalculateMtuSuccess ( false ); // false indicates F2

            // P( TWO WAY ) = 100% - ( 100% - P( DCU TX Success ) * P( MTU TX Success ) ) ^ 3
            decimal precalc = 1 - this.GetProbability ( bestRssiResponse ) * mtuTxSuccess;

            Library.Utils.Print ( "ND: F2 MtuTxSuccess " + mtuTxSuccess +
                " | BestRSSI " + bestRssiResponse +
                " | Prob BestRSSI " + this.GetProbability ( bestRssiResponse ) +
                " | Precalc " + precalc +
                " | Result " + ( 1 - precalc * precalc * precalc ) );

            return 1 - precalc * precalc * precalc;
        }

        private IEnumerable<IGrouping<uint,NodeDiscovery>> NodesValidatedForFreq (
            bool isF1 )
        {
            return this.AllAttempts
                .SelectMany ( list => list
                    .Where ( entry =>
                        entry.IsValidated &&
                        entry.IsF1 == isF1 )
                ).GroupBy ( entry => entry.NodeId );
        }

        #endregion
    }
}
