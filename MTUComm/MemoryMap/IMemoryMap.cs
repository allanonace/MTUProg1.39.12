using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        int BatteryVoltage { get; }

        int MtuFirmwareVersionFormatFlag { get; }
        string MtuFirmwareVersion { get; }

        string PcbNumber { get; } // Hard...

        int P1MeterType { get; }
        int P2MeterType { get; }

        ulong P1MeterId { get; }
        ulong P2MeterId { get; }

        uint P1Reading { get; } // Hard...
        int P2Reading { get; } // Hard...

        int P1Scaler { get; }
        int P2Scaler { get; }
    }

    public abstract class AMemoryMap : IMemoryMap
    {
        #region Attributes

        public dynamic registers;
        protected IDictionary<string, object> dictionary { private set; get; }
        private ReadOnlyDictionary<string, object> backupDictionary;

        #endregion

        #region Initialization

        public AMemoryMap ()
        {
            this.registers = new ExpandoObject();
            this.dictionary = (IDictionary<string, object>)this.registers;
        }

        #endregion

        #region Events

        protected void AddModifyEvent()
        {
            // Dictionary backup after have been added all family registers, for avoid
            // that ExpandoObject type overrides members when using asignment operator
            this.backupDictionary = new ReadOnlyDictionary<string, object>(this.dictionary);

            ((INotifyPropertyChanged)this.registers).PropertyChanged +=
                new PropertyChangedEventHandler(HandlePropertyChanges);
        }

        protected void RemoveModifyEvent()
        {
            ((INotifyPropertyChanged)this.registers).PropertyChanged -=
                new PropertyChangedEventHandler(this.HandlePropertyChanges);
        }

        private void HandlePropertyChanges(
            object sender, PropertyChangedEventArgs e)
        {
            string id = e.PropertyName;

            Console.WriteLine ( "Handler: " + id );

            // Recuperar valor asignado
            object value = this.dictionary[ id ];

            // Restaurar el miembro anterior (  )
            dictionary[ id ] = this.backupDictionary[ id ];
            //dictionary[ e.PropertyName ] = value; // Crearia un bucle y no invocaria nunca set

            this.SetValue ( id, value );
        }

        protected abstract void SetValue(string id, object value);

        #endregion

        #region Shared properties

        public int MtuType
        {
            get { return registers.MtuType; }
        }

        public int MtuId
        {
            get { return registers.MtuId; }
        }

        public bool Shipbit
        {
            get { return this.registers.Shipbit; }
        }

        public int DailyRead
        {
            get { return this.registers.DailyRead; }
        }

        public String DailySnap
        {
            get { return this.registers.DailySnap; }
        }

        public int MessageOverlapCount
        {
            get { return this.registers.MessageOverlapCount; }
        }

        public int ReadInterval
        {
            get { return this.registers.ReadInterval; }
        }

        public int BatteryVoltage
        {
            get { return this.registers.BatteryVoltage; }
        }

        public int MtuFirmwareVersionFormatFlag
        {
            get { return this.registers.MtuFirmwareVersionFormatFlag; }
        }

        public string MtuFirmwareVersion
        {
            get { return this.registers.MtuFirmwareVersion; }
        }

        public string PcbNumber
        {
            get { return this.registers.PcbNumber; }
        }

        public int P1MeterType
        {
            get { return this.registers.P1MeterType; }
        }

        public int P2MeterType
        {
            get { return this.registers.P2MeterType; }
        }

        public ulong P1MeterId
        {
            get { return this.registers.P1MeterId; }
        }

        public ulong P2MeterId
        {
            get { return this.registers.P2MeterId; }
        }

        public uint P1Reading
        {
            get { return this.registers.P1Reading; }
        }

        public int P2Reading
        {
            get { return this.registers.P2Reading; }
        }

        public int P1Scaler
        {
            get { return this.registers.P1Scaler; }
        }

        public int P2Scaler
        {
            get { return this.registers.P2Scaler; }
        }

        #endregion
    }
}
