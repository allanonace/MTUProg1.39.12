﻿using System;
namespace aclara_meters.Resources
{
    public class XmlStrings
    {
        public static string GetMeterString()
        {

           
            string value = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<MeterTypes>
<FileVersion>4.14 All Meter Types</FileVersion>
<FileDate>2/29/2008</FileDate>
<Meter ID=""0"">
<Display>Default</Display>
<Type>R</Type>
<Vendor>Default</Vendor>
<Model>Default</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1"">
<Display>Schlum Generic ARB-V/4</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""2"">
<Display>SCHLUM GENERIC ARB-V/6 DIGIT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>GENERIC</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""3"">
<Display>SCHLUM GENERIC ProRead/4DIGIT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""4"">
<Display>SCHLUM GENERIC ProRead/6DIGIT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>GENERIC</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""5"">
<Display>ABB C-700 5/8x3/4 Scndr 1 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""6"">
<Display>ABB C-700 3/4 Scancoder 1 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""7"">
<Display>ABB C-700 1 Scancoder 1 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""8"">
<Display>ABB C-700 1 1/2 Scndr 10 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""9"">
<Display>ABB C-700 2 Scancoder 10 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""10"">
<Display>ABB T-3000 1 1/2 Scndr 10 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""11"">
<Display>ABB T-3000 2 Scancoder 10 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""12"">
<Display>ABB T-3000 3 Scancoder 100 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""13"">
<Display>ABB T-3000 4 Scancoder 100 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""14"">
<Display>ABB T-3000 6 Scndr 1000 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""15"">
<Display>ABB T-3000 8 Scndr 1000 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""16"">
<Display>ABB T-3000 10 Scndr 1000 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""17"">
<Display>ABB C3000 2-HiFlo Scndr 10 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""18"">
<Display>ABB C3000 2-LoFlo Scndr 1 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""19"">
<Display>ABB C3000 3-HiFlo Scndr 100 CuFt</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""20"">
<Display>ABB C3000 3-LoFlo Scndr 1 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""21"">
<Display>ABB C3000 4-HiFlo Scndr 100 CuFt</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""22"">
<Display>ABB C3000 4-LoFlo Scndr 1 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""23"">
<Display>ABB C3000 6-HiFlo Scndr 1000CuFt</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""24"">
<Display>ABB C3000 6-LoFlo Scndr 1 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""25"">
<Display>ABB C3000 8-HiFlo Scndr 1000CuFt</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""26"">
<Display>ABB C3000 8-LoFlo Scndr 10 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""27"">
<Display>ABB C-700 5/8x3/4 Scndr 10 Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""28"">
<Display>ABB C-700 3/4 Scancoder 10 Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""29"">
<Display>ABB C-700 1 Scancoder 10 Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""30"">
<Display>ABB C-700 1 1/2 Scndr 100 Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""31"">
<Display>ABB C-700 2 Scancoder 100 Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""32"">
<Display>ABB T-3000 1 1/2 Scndr 100 Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""33"">
<Display>ABB T-3000 2 Scancoder 100 Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""34"">
<Display>ABB T-3000 3 Scancoder 1000 Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""35"">
<Display>ABB T-3000 4 Scancoder 1000 Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""36"">
<Display>ABB T-3000 6 Scancoder 1000 Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""37"">
<Display>ABB T-3000 8 Scancoder 1000 Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""38"">
<Display>ABB T-3000 10 Scndr 10000 Gal</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>4</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""39"">
<Display>ABB C3000 2-HiFlo Scndr 100 Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""40"">
<Display>ABB C3000 2-LoFlo Scndr 10 Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""41"">
<Display>ABB C3000 3-HiFlo Scndr 1000Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""42"">
<Display>ABB C3000 3-LoFlo Scndr 10 Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""43"">
<Display>ABB C3000 4-HiFlo Scndr 1000Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""44"">
<Display>ABB C3000 4-LoFlo Scndr 10 Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""45"">
<Display>ABB C3000 6-HiFlo Scndr 1000 Gal</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""46"">
<Display>ABB C3000 6-LoFlo Scndr 10 Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""47"">
<Display>ABB C3000 8-HiFlo Scndr 1000 Gal</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""48"">
<Display>ABB C3000 8-LoFlo Scndr 100 Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""49"">
<Display>Sensus SRII 5/8x3/4 ECR4 100CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""50"">
<Display>Sensus SRII 3/4 ECR-4 100CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""51"">
<Display>Sensus SRII 1 ECR-4 100CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""52"">
<Display>Sensus SRII 5/8x3/4 ECR-6 1CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""53"">
<Display>Sensus SRII 3/4 ECR-6 1CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""54"">
<Display>Sensus SRII 1 ECR-6 1CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""55"">
<Display>Badger M25 5/8RTR PLS 6D CuFt/10</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>M-25</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""56"">
<Display>Badger M25 3/4RTR PLS 6D CuFt/10</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>M-25</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""57"">
<Display>Badger M35 3/4RTR PLS 6D CuFt/10</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>M-35</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""58"">
<Display>Badger M40 1 RTR PLS 6D CuFt/10</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>M-40</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""59"">
<Display>Badger M70 1 RTR PLS 6D CuFt/10</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>M-70</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""60"">
<Display>Badger M120 1.5 RTR 10CuFt/10</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>M-120</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""61"">
<Display>Badger Generic Plsgen 6D Cu.Ft/1</Display>
<Type>G</Type>
<Vendor>BADGER</Vendor>
<Model>M-25</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""62"">
<Display>Badger M25 5/8 Plsgen 6D 10 CuFt</Display>
<Type>G</Type>
<Vendor>BADGER</Vendor>
<Model>M-25</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""63"">
<Display>Badger M35 3/4 Plsgen 6D 10 CuFt</Display>
<Type>G</Type>
<Vendor>BADGER</Vendor>
<Model>M-35</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""64"">
<Display>Badger M70 1 Pulsegen 6D 10 CuFt</Display>
<Type>G</Type>
<Vendor>BADGER</Vendor>
<Model>M-70</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""65"">
<Display>Badger M120 1.5 Pgen 6D 100 CuFt</Display>
<Type>G</Type>
<Vendor>BADGER</Vendor>
<Model>M-120</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""66"">
<Display>Badger M170 2 Plsgen 6D 100 CuFt</Display>
<Type>G</Type>
<Vendor>BADGER</Vendor>
<Model>M-170</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""67"">
<Display>Trident Generic Gen. Cu.Ft./1</Display>
<Type>G</Type>
<Vendor>TRIDENT</Vendor>
<Model>GENERIC</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""68"">
<Display>Trident Generic Gen. Cu.Ft/10</Display>
<Type>G</Type>
<Vendor>TRIDENT</Vendor>
<Model>GENERIC</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""69"">
<Display>Rockwell Generic Gen. Cu.Ft/1</Display>
<Type>G</Type>
<Vendor>ROCKWELL</Vendor>
<Model>GENERIC</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""70"">
<Display>Rockwell Genric Gen. Cu.Ft./10</Display>
<Type>G</Type>
<Vendor>ROCKWELL</Vendor>
<Model>GENERIC</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""71"">
<Display>SchlumCmpd Genric Pri ARB-V/6</Display>
<Type>E</Type>
<Vendor>SCHLUM-PIT</Vendor>
<Model>GENERIC</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""72"">
<Display>SchlmCmpd Genric Bypss ARB-V/6</Display>
<Type>E</Type>
<Vendor>SCHLUM-PIT</Vendor>
<Model>GENERIC</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""73"">
<Display>Badger M40 1 Pulsegen 6D 10 CuFt</Display>
<Type>G</Type>
<Vendor>BADGER</Vendor>
<Model>M-40</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""74"">
<Display>Badger M70 1 Plsgen 6D 100 CuFt</Display>
<Type>G</Type>
<Vendor>BADGER</Vendor>
<Model>M-70</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""75"">
<Display>Badger M170 2 RTR PLS 10CuFt/10</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>M-170</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""76"">
<Display>Sensus SRII 1 1/2 ECR-6 10CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""77"">
<Display>Badger Mag 6 PLS 6D 1000GAL</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Magnetoflo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""78"">
<Display>ABB Magmeter 4 PLS 6D 1000GAL</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>Magmeter</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""79"">
<Display>Badger Mag 3 PLS 6D 1000GAL</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Magnetoflo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""80"">
<Display>ABB C-700 5/8x3/4 RS Pulsr 10 Gal</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""81"">
<Display>ABB C-700 3/4 RS Pulser 10 Gal</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""82"">
<Display>ABB C-700 1 RS Pulser 10 Gal</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""83"">
<Display>ABB C-700 1 1/2 RS Pulsr 100 Gal</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""84"">
<Display>ABB C-700 2 RS Pulser 100 Gal</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""85"">
<Display>ABB C-700 5/8x3/4 RSPulsr 1 CuFt</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""86"">
<Display>ABB C-700 3/4 RS Pulser 1 CuFt</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""87"">
<Display>ABB C-700 1 RS Pulser 1 CuFt</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""88"">
<Display>ABB C-700 1 1/2 RS Pulsr 10 CuFt</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""89"">
<Display>ABB C-700 2 RS Pulser 10 CuFt</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""90"">
<Display>ABB PSM-T 5/8 x 1/2 10 Gal</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>PSM-T</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""91"">
<Display>ABB PSM-T 5/8 x 3/4 10 Gal</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>PSM-T</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""92"">
<Display>ABB 5UM-20 3/4 HOT WATER 1 Gal</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>5UM-20</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""93"">
<Display>BADGER M20 5/8 DSI 6D PLSE 10GAL</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>M20 DSI</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""94"">
<Display>BADGER M25 5/8 DSI 6D PLSE 10GAL</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>M25 DSI</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""95"">
<Display>BADGER M25 3/4 DSI 6D PLSE 10GAL</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>M25 DSI</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""96"">
<Display>BADGER M35 3/4 DSI 6D PLSE 10GAL</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>M35 DSI</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""97"">
<Display>BADGER M40 1 DSI 6D PLSE 100GAL</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>M40 DSI</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""98"">
<Display>BADGER M120 1 1/2 DSI 6D 100GAL</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>M120 DSI</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""99"">
<Display>BADGER M170 2 DSI 6D 100GAL</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>M170 DSI</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""100"">
<Display>SCHLUM 5/8 ARB-V/6Digit 10 Gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""101"">
<Display>SCHLUM 3/4 ARB-V/6Digit 10 Gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""102"">
<Display>SCHLUM 1 ARB-V/6Digit 10 Gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""103"">
<Display>SCHLUM 5/8x3/4 PRO 6D 10 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""104"">
<Display>SCHLUM 3/4 PROREAD 6D 10 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""105"">
<Display>SCHLUM 1 PROREAD 6D 10 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""106"">
<Display>SCHLUM 1 1/2 PROREAD 6D 100 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""107"">
<Display>SCHLUM 2 PROREAD 6D 100 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""108"">
<Display>NEPT T10 1-1/2 ARBV 6D 100gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""109"">
<Display>NEPT T10 2 ARBV 6D 100 gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""110"">
<Display>SCHLUM 5/8X3/4 PRO 4D 1000 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""111"">
<Display>SCHLUM 3/4 PROREAD 4D 1000 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""112"">
<Display>SCHLUM 1 PROREAD 4D 1000 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""113"">
<Display>NEPT HPT 1 1/2 PRO6 1Cu Mtr</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""114"">
<Display>NEPT HPT 2 PRO6 1Cu Mtr</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""115"">
<Display>NEPT HPT 3 PRO6 1Cu Mtr</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""116"">
<Display>NEPT HPT 4 PRO6 1Cu Mtr</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""117"">
<Display>NEPT HPT 6 PRO6 10Cu Mtr</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""118"">
<Display>NEPT TT 3 ARBV 6D 10CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>TT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""119"">
<Display>NEPT T10 1ARBV 4D 1000CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""120"">
<Display>SCHLUM 2 CMPD PRO HI2 6D 100G</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""121"">
<Display>SCHLUM 2 CMPD PRO LO5/8 6D 10G</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""122"">
<Display>SCHLUM 3 CMPD PRO HI3 6D 100G</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""123"">
<Display>SCHLUM 3 CMPD PRO LO5/8 6D 10G</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""124"">
<Display>SCHLUM 4 CMPD PRO HI4 6D 100G</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""125"">
<Display>SCHLUM 4 CMPD PRO LO3/4 6D 10G</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""126"">
<Display>NEPT T10 5/8ARBV 4D 100CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""127"">
<Display>NEPT T10 3/4ARBV 4D 100CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""128"">
<Display>NEPT T10 1ARBV 4D 100CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""129"">
<Display>NEPT T10 1-1/2ARBV 4D 1000CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""130"">
<Display>NEPT T10 2ARBV 4D 1000CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""131"">
<Display>NEPT TT 3ARBV 4D 1000CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>TT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""132"">
<Display>NEPT TT 4ARBV 4D 1000CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>TT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""133"">
<Display>NEPT TT 6ARBV 4D 10000CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>TT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""134"">
<Display>NEPT TT 8ARBV 4D 10000CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>TT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""135"">
<Display>NEPT TT 10ARBV 4D 10000CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>TT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""136"">
<Display>NEPT TT 12ARBV 4D 100000CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>TT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""137"">
<Display>NEPT TT 4 ARBV 6D 10CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>TT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""138"">
<Display>SCHLUM 6 CMPD PRO HI6 6D 1000 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""139"">
<Display>SCHLUM 6 CMPD PRO LO1 6D 10 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""140"">
<Display>Sensus SRII 5/8x3/4 ECR4 1000gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""141"">
<Display>Sensus SRII 3/4 ECR-4 1000gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""142"">
<Display>Sensus SRII 1 ECR-4 1000gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""143"">
<Display>SCHLM HPT 1.5ARBV 4D 1000CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""144"">
<Display>SCHLUM HPT 2ARBV 4D 1000CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""145"">
<Display>SCHLUM HPT 3ARBV 4D 1000CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""146"">
<Display>SCHLUM HPT 4ARBV 4D 1000CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""147"">
<Display>SCHLUM HPT 6ARBV 4D 10000CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""148"">
<Display>SCHLUM HPT 8ARBV 4D 10000CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""149"">
<Display>SCHLUM HPT 10ARBV 4D 10000CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""150"">
<Display>Sensus 3 SRH CMPND 6 Dig 100Gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""151"">
<Display>Sensus 4 SRH CMPND 6 Dig 1000Gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""152"">
<Display>Sensus 6 SRH CMPND 6 Dig 1000Gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""153"">
<Display>Sensus 2 SRH CMPND 6 Dig 100Gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""154"">
<Display>INVNSYS SRII 5/8HiRes ECR4 100gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""155"">
<Display>INVNSYS SRII 1 Hi Res ECR4 100gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""156"">
<Display>INVNSYS SRII 1 1/2HiRes4d1000gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""157"">
<Display>INVNSYS SRII 2 Hi Res ECR4 1000gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""158"">
<Display>ABB Magmeter PLS 6D 1000CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>Magmeter</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""159"">
<Display>Sensus SRII 2 ECR-4 1000CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""160"">
<Display>Sensus 3 W-Turbo 6 Digit 100Gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""161"">
<Display>Sensus 4 W-Turbo 6 Dig 1000Gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""162"">
<Display>Sensus 6 W-Turbo 6 Dig 1000Gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""163"">
<Display>Sensus 8 W-Turbo 6 Dig 1000Gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""164"">
<Display>SENSUS SRII 5/8 HIRES 4D 10 CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""165"">
<Display>SENSUS SRII 1 HIRES 4D 10 CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""166"">
<Display>SENSUS SRII 1 1/2 HIRES4D 100CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""167"">
<Display>SENSUS SRII 2 HIRES 4D 100 CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""168"">
<Display>SCHLM CMPDTT 2HIARBV4D1000CFT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>CMPD TT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""169"">
<Display>SCHLM CMPDTT 2LOARBV4D100CFT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>CMPD TT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""170"">
<Display>SCHLM CMPDTT 3HIARBV4D1000CFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>CMPD TT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""171"">
<Display>SCHLM CMPDTT 3LOARBV4D100CFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>CMPD T-10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""172"">
<Display>SCHLM CMPDTT 4HIARBV4D1000CFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>CMPD TT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""173"">
<Display>SCHLM CMPDTT 4LOARBV4D100CFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>CMPD T-10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""174"">
<Display>SCHLM CMPDTT6HIARBV4D10000CFT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>CMPD TT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""175"">
<Display>SCHLM CMPDTT 6LOARBV4D100CFT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>CMPD TT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""176"">
<Display>ABB T-3000 6 Scndr 1000 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""177"">
<Display>ABB T-3000 8 Scndr 1000 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""178"">
<Display>ABB T-3000 10 Scndr 1000 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""180"">
<Display>Badger M25 5/8 Plsgen 6D 100Gal</Display>
<Type>G</Type>
<Vendor>BADGER</Vendor>
<Model>M-25</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""181"">
<Display>Badger M35 3/4 Plsgen 6D 100Gal</Display>
<Type>G</Type>
<Vendor>BADGER</Vendor>
<Model>M-35</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""182"">
<Display>Badger M40 1 Plsgen 6D 100Gal</Display>
<Type>G</Type>
<Vendor>BADGER</Vendor>
<Model>M-40</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""183"">
<Display>Badger M70 1 Plsgen 6D 100Gal</Display>
<Type>G</Type>
<Vendor>BADGER</Vendor>
<Model>M-70</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""184"">
<Display>Badger M70 1 Plsgen 6D 1000Gal</Display>
<Type>G</Type>
<Vendor>BADGER</Vendor>
<Model>M-70</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""185"">
<Display>Badger M120 1 1/2Plsgen1000Gal</Display>
<Type>G</Type>
<Vendor>BADGER</Vendor>
<Model>M-120</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""186"">
<Display>Badger M170 2 Plsgen 6D 1000Gal</Display>
<Type>G</Type>
<Vendor>BADGER</Vendor>
<Model>M-170</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""187"">
<Display>Badger RTR M55 1 PLS 6D 10Gal</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>M-55</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""188"">
<Display>Nept Cmpd LO5/8 Plsgen 100Gal</Display>
<Type>G</Type>
<Vendor>Schlum/Neptune</Vendor>
<Model>Cmpnd T-10</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""189"">
<Display>Nept HPT 2 Plsgen1000Gal</Display>
<Type>G</Type>
<Vendor>Schlum/Neptune</Vendor>
<Model>Cmpnd HPT</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""190"">
<Display>NEPT T10 5/8 ARBV 6D 1CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""191"">
<Display>NEPT T10 3/4 ARBV 6D 1CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""192"">
<Display>NEPT T10 1 ARBV 6D 1CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""193"">
<Display>NEPT T10 1-1/2 ARBV 6D 10CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""194"">
<Display>NEPT T10 2 ARBV 6D 10CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""195"">
<Display>Neptune T10 5/8 Plsgen 6D 100Gal</Display>
<Type>G</Type>
<Vendor>Schlum/Neptune</Vendor>
<Model>T-10</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""196"">
<Display>Neptune T10 3/4 Plsgen 6D 100Gal</Display>
<Type>G</Type>
<Vendor>Schlum/Neptune</Vendor>
<Model>T-10</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""197"">
<Display>Neptune T10 1 Plsgen 6D 100Gal</Display>
<Type>G</Type>
<Vendor>Schlum/Neptune</Vendor>
<Model>T-10</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""198"">
<Display>Neptune T10 1 1/2Plsgen1000Gal</Display>
<Type>G</Type>
<Vendor>Schlum/Neptune</Vendor>
<Model>T-10</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""199"">
<Display>Neptune T10 2 Plsgen1000Gal</Display>
<Type>G</Type>
<Vendor>Schlum/Neptune</Vendor>
<Model>T-10</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""200"">
<Display>GE ELEC I70S Teldata /100</Display>
<Type>R</Type>
<Vendor>GE</Vendor>
<Model>I70S</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>0</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""201"">
<Display>Riotronics ge 170s /1KWH</Display>
<Type>R</Type>
<Vendor>Riotronics</Vendor>
<Model></Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""202"">
<Display>OCS Electric WL-50</Display>
<Type>R</Type>
<Vendor>OCS</Vendor>
<Model>WL-50</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>0</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""203"">
<Display>OCS Electric AB-120</Display>
<Type>R</Type>
<Vendor>OCS</Vendor>
<Model>AB-120</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>0</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""204"">
<Display>NEPT HPT 8 PRO6 10Cu Mtr</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""205"">
<Display>NEPT HPT 10 PRO6 10Cu Mtr</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""206"">
<Display>NEPT TT 3 ARBV 6D 1 Cu. Mtr.</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>TT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""207"">
<Display>NEPT TT 4 ARBV 6D 1 Cu. Mtr.</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>TT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""208"">
<Display>NEPT TT 6 ARBV 6D 1 Cu. Mtr.</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>TT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""209"">
<Display>NEPT HPT 1 1/2 ARBV 6D 1 CuMtr</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""210"">
<Display>Osaki Elec 9124-SK1 1KWH</Display>
<Type>R</Type>
<Vendor>OSAKI</Vendor>
<Model>9124-SK1</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""211"">
<Display>OSAKI ELEC AEX46 1KWH</Display>
<Type>R</Type>
<Vendor>OSAKI</Vendor>
<Model>AEX46</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>56</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""212"">
<Display>NEPT HPT 2 ARBV 6D 1 CuMtr</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""213"">
<Display>NEPT HPT 3 ARBV 6D 1 CuMtr</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""214"">
<Display>NEPT HPT 4 ARBV 6D 1 CuMtr</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""215"">
<Display>IMS Watthour Meter 1 KWH</Display>
<Type>R</Type>
<Vendor>IMS</Vendor>
<Model>DUAL</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>56</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""216"">
<Display>NEPT HPT 6 ARBV 6D 10CuMtr</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""217"">
<Display>NEPT HPT 8 ARBV 6D 10CuMtr</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""218"">
<Display>NEPT HPT 10 ARBV 6D 10CuMtr</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""219"">
<Display>NEPT 5/8 ARBV 6D .5CuMtr</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""220"">
<Display>GE 1-70-S/2 Rr=13 8/9</Display>
<Type>L</Type>
<Vendor>GE</Vendor>
<Model>1-70-S/2</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<PreScaler>0</PreScaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>0</ProvingHandFactor>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""221"">
<Display>Westinghouse 4DS Rr=13 8/9</Display>
<Type>L</Type>
<Vendor>Westinghouse</Vendor>
<Model>4DS</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<PreScaler>0</PreScaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>0</ProvingHandFactor>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""222"">
<Display>Landis Gyr Rr=27 7/9</Display>
<Type>L</Type>
<Vendor>Landis &amp; Gyr</Vendor>
<Model>Electric</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<PreScaler>0</PreScaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>0</ProvingHandFactor>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""223"">
<Display>Sangamo J5S Rr=27 7/9</Display>
<Type>L</Type>
<Vendor>SANGAMO</Vendor>
<Model>J5S</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<PreScaler>0</PreScaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>0</ProvingHandFactor>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""224"">
<Display>Sangamo J4S Rr=27 7/9</Display>
<Type>L</Type>
<Vendor>SANGAMO</Vendor>
<Model>J4S</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<PreScaler>0</PreScaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>0</ProvingHandFactor>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""225"">
<Display>ABB AB1 Rr=13 8/9</Display>
<Type>L</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AB1</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<PreScaler>0</PreScaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>0</ProvingHandFactor>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""226"">
<Display>Carlon JSJ100 1 100gal</Display>
<Type>R</Type>
<Vendor>Carlon</Vendor>
<Model>JSJ 100</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""227"">
<Display>Carlon JSJ150 1 1/2 100gal</Display>
<Type>R</Type>
<Vendor>Carlon</Vendor>
<Model>JSJ 150</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""228"">
<Display>NEPT 3/4 ARBV 6D .5CuMtr</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""229"">
<Display>NEPT 1 ARBV 6D .5CuMtr</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""230"">
<Display>SANGAMO J5 ELECTRIC /1KWH</Display>
<Type>R</Type>
<Vendor>SANGAMO</Vendor>
<Model>J-5</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""231"">
<Display>SCHLUM 6CMPD ARBV LO1 6D10CF</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>CMPD T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""232"">
<Display>SCHLUM T10 1 PRO6 10CFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""233"">
<Display>NEPT T10 1 ARBV 6D 10CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""234"">
<Display>NEPT TT 6ARBV 6D 100CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>TT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""235"">
<Display>NEPT T10 5/8 ARBV 4D 1000 gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""236"">
<Display>NEPT T10 3/4 ARBV 4D 1000 gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""237"">
<Display>NEPT T10 1 ARBV 4D 1000 gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""238"">
<Display>NEPT T10 1 1/2 ARBV 4D 10000 gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""239"">
<Display>NEPT T10 2 ARBV 4D 10000 gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""240"">
<Display>NEPT/SCHLUM T10 5/8 PRO6 1CFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""241"">
<Display>NEPT/SCHLUM T10 3/4 PRO6 1CFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""242"">
<Display>NEPT/SCHLUM T10 1 PRO6 1CFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""243"">
<Display>NEPT/SCHLUM T10 1-1/2 PRO6 10CFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""244"">
<Display>NEPT/SCHLUM T10 2 PRO6 10CFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""245"">
<Display>SCHLUM CMPD 6LO PRO6 10CF</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>CMPD T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""246"">
<Display>NEPT T10 1 PRO6 1CFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""247"">
<Display>NEPT 1 1/2 ARBV 6D .5CuMtr</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""248"">
<Display>NEPT 2 ARBV 6D .5CuMtr</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""249"">
<Display>NEPT 5/8 PRO6 .5CuMtr</Display>
<Type>E</Type>
<Vendor>SCHLUM/Neptune</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""250"">
<Display>SCHLUM 2CMPD ARBV HI2 6D10CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""251"">
<Display>SCHLUM 2CMPD ARBVLO5/8 6D1CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""252"">
<Display>SCHLUM 3CMPD ARBV HI3 6D10CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>CMPD TT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""253"">
<Display>SCHLUM3CMPD ARBV LO5/8 6D1CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>CMPD T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""254"">
<Display>SCHLUM 4CMPD ARBV HI4 6D10CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>CMPD TT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""255"">
<Display>SCHLUM4CMPD ARBV LO3/4 6D1CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>CMPD T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""256"">
<Display>SCHLUM6CMPD ARBV HI6 6D 100CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>CMPD TT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""257"">
<Display>SCHLUM 6CMPD ARBV LO1 6D1CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>CMPD T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""258"">
<Display>NEPT 3/4 PRO6 .5CuMtr</Display>
<Type>E</Type>
<Vendor>SCHLUM/Neptune</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""259"">
<Display>NEPT 1 PRO6 .5CuMtr</Display>
<Type>E</Type>
<Vendor>SCHLUM/Neptune</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""260"">
<Display>SCHLUM T10 5/8 PRO4 100CFT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""261"">
<Display>NEPT/SCHLUM T10 3/4 PRO4 100CFT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""262"">
<Display>NEPT/SCHLUM T10 1 PRO4 100CFT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""263"">
<Display>NEPT 1 1/2 PRO6 .5CuMtr</Display>
<Type>E</Type>
<Vendor>SCHLUM/Neptune</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""264"">
<Display>NEPT 2 PRO6 .5CuMtr</Display>
<Type>E</Type>
<Vendor>SCHLUM/Neptune</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""265"">
<Display>SENSUS SRII 5/8 ICE 6D .1CuMtr</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>3</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""266"">
<Display>SENSUS SRII 3/4 ICE 6D .1CuMtr</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>3</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""267"">
<Display>SENSUS SRII 1 ICE 6D .1CuMtr</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>3</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""268"">
<Display>SENSUS SRII 1 1/2 ICE 6D .1CuMtr</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>3</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""269"">
<Display>SENSUS SRII 2 ICE 6D .1CuMtr</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>3</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""270"">
<Display>SCHLUM 2CMPD ARBV HI2 6D100GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""271"">
<Display>SCHLUM 2CMPD ARBVLO5/8 6D10GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""272"">
<Display>SCHLUM 3CMPD ARBV HI3 6D100GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""273"">
<Display>SCHLUM3CMPD ARBV LO5/8 6D10GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""274"">
<Display>SCHLUM 4CMPD ARBV HI4 6D100GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""275"">
<Display>SCHLUM4CMPD ARBV LO3/4 6D10GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""276"">
<Display>SCHLUM6CMPD ARBV HI6 6D 1000GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""277"">
<Display>SCHLUM 6CMPD ARBV LO1 6D10GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""278"">
<Display>Elster T3000 1 1/2 Dig. 1CuMtr</Display>
<Type>R</Type>
<Vendor>Elster/Kent</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""279"">
<Display>Elster T3000 2 Digital 1CuMtr</Display>
<Type>R</Type>
<Vendor>Elster/Kent</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""280"">
<Display>SCHLUM CMPD 2HI PRO6 10CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>CMPD TT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""281"">
<Display>SCHLUM CMPD 2LO PRO6 1CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>CMPD T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""282"">
<Display>NEPT/SCHLUM CMPD 3HI PRO6 10CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>CMPD TT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""283"">
<Display>NEPT/SCHLUM CMPD 3LO PRO6 1CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>CMPD T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""284"">
<Display>NEPT/SCHLUM CMPD 4HI PRO6 10CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>CMPD TT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""285"">
<Display>NEPT/SCHLUM CMPD 4LO PRO6 1CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>CMPD T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""286"">
<Display>NEPT/SCHLUM CMPD 6HI PRO6 100CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>CMPD TT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""287"">
<Display>NEPT/SCHLUM CMPD 6LO PRO6 1CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>CMPD T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""288"">
<Display>Elster T3000 3 Digital 1CuMtr</Display>
<Type>R</Type>
<Vendor>Elster/Kent</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""289"">
<Display>Elster T3000 4 Digital 1CuMtr</Display>
<Type>R</Type>
<Vendor>Elster/Kent</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""290"">
<Display>SCHLM 2CMPD Hi2ARBV4D10000Gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>CMPD T10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""291"">
<Display>SCHLM 2CMPD LO5/8ARBV4D1000Gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>CMPD T10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""292"">
<Display>SCHLM 3CMPD HI3ARBV4D10000Gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>CMPD TT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""293"">
<Display>SCHLM 3CMPD LO5/8ARBV4D1000Gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>CMPD T10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""294"">
<Display>SCHLM 4CMPD HI4ARBV4D10000Gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>CMPD TT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""295"">
<Display>SCHLM 4CMPD LO3/4ARBV4D1000Gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>CMPD T10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""296"">
<Display>SCHLM 6CMPD HI6ARBV4D100000Gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>CMPD T10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""297"">
<Display>SCHLM 6CMPD LO1ARBV4D1000Gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>CMPD T10</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""298"">
<Display>Elster T3000 6 Digital 1CuMtr</Display>
<Type>R</Type>
<Vendor>Elster/Kent</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>1</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""299"">
<Display>Elster T3000 8 Digital 1CuMtr</Display>
<Type>R</Type>
<Vendor>Elster/Kent</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>1</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""300"">
<Display>Schlumberger G4 Gas 100 cu.dm</Display>
<Type>R</Type>
<Vendor>SCHLUM</Vendor>
<Model>G-4</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""301"">
<Display>SCHLM CMPD 10HI PRO6 100CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model></Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""302"">
<Display>SCHLM CMPD 10LO PRO6 10CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model></Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""303"">
<Display>Elster T3000 10 Digital 1CuMtr</Display>
<Type>R</Type>
<Vendor>Elster/Kent</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>1</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""304"">
<Display>Elster T3000 12 Digital 1CuMtr</Display>
<Type>R</Type>
<Vendor>Elster/Kent</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>1</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""305"">
<Display>AM AC250 4d PHF2 100cft Sensus</Display>
<Type>E</Type>
<Vendor>American</Vendor>
<Model>AC 250</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>8</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""306"">
<Display>Elster C700 5/8 Digital .01CuMtr</Display>
<Type>R</Type>
<Vendor>Elster/Kent</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>2</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""307"">
<Display>Elster C700 3/4 Digital .01CuMtr</Display>
<Type>R</Type>
<Vendor>Elster/Kent</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>2</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""308"">
<Display>Elster C700 1 Digital .1CuMtr</Display>
<Type>R</Type>
<Vendor>Elster/Kent</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""309"">
<Display>Elster C700 1 1/2 Dig. .1CuMtr</Display>
<Type>R</Type>
<Vendor>Elster/Kent</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""310"">
<Display>AM AC250 4 Digit PHF 2 100 cuft</Display>
<Type>M</Type>
<Vendor>American</Vendor>
<Model>AC 250</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>4</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""311"">
<Display>AM AL250 4 Digit PHF 2 100 cuft</Display>
<Type>M</Type>
<Vendor>American</Vendor>
<Model>AL 250</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>4</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""312"">
<Display>AM AL425 4 Digit PHF 2 100 cuft</Display>
<Type>M</Type>
<Vendor>American</Vendor>
<Model>AL 425</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>4</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""313"">
<Display>AM AL800 5 Digit PHF 5 100cuft</Display>
<Type>R</Type>
<Vendor>American</Vendor>
<Model>AL 800</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>2</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""314"">
<Display>Elster C700 2 Digital .1CuMtr</Display>
<Type>R</Type>
<Vendor>Elster/Kent</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""315"">
<Display>Metron Spect22 3/4 6D 10CuFt</Display>
<Type>E</Type>
<Vendor>Metron</Vendor>
<Model>Spectrum 22</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""316"">
<Display>Metron Spect50 1 6D 100CuFt</Display>
<Type>E</Type>
<Vendor>Metron</Vendor>
<Model>Spectrum 50</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""317"">
<Display>Metron Spect88 1 1/2 6D 100CuFt</Display>
<Type>E</Type>
<Vendor>Metron</Vendor>
<Model>Spectrum 88</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""318"">
<Display>Metron Spect130 2 6D 100CuFt</Display>
<Type>E</Type>
<Vendor>Metron</Vendor>
<Model>Spectrum 130</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""319"">
<Display>Metron Spect175 3 6D 100CuFt</Display>
<Type>E</Type>
<Vendor>Metron</Vendor>
<Model>Spectrum 175</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""320"">
<Display>RC 11M 5 Digit PHF 10 100cuft</Display>
<Type>R</Type>
<Vendor>Roots</Vendor>
<Model>11M</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""321"">
<Display>RC 16M 6 Digit PHF 100 100cuft</Display>
<Type>R</Type>
<Vendor>Roots</Vendor>
<Model>16M</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""322"">
<Display>RC 38MID 6 Digit PHF 100 100cuft</Display>
<Type>R</Type>
<Vendor>Roots</Vendor>
<Model>38MID</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""323"">
<Display>RC 3M 5 Digit PHF 10 100cuft</Display>
<Type>R</Type>
<Vendor>Roots</Vendor>
<Model>3M</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""324"">
<Display>RC 5M 5 Digit PHF 10 100cuft</Display>
<Type>R</Type>
<Vendor>Roots</Vendor>
<Model>5M</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""325"">
<Display>RC 5M-LD 6 Digit PHF 10 100cuft</Display>
<Type>R</Type>
<Vendor>Roots</Vendor>
<Model>5M-LD</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""326"">
<Display>RC 7M 5 Digit PHF 10 100cuft</Display>
<Type>R</Type>
<Vendor>Roots</Vendor>
<Model>7M</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""327"">
<Display>RC 7M-EH 6 Digit PHF 100 100cuft</Display>
<Type>R</Type>
<Vendor>Roots</Vendor>
<Model>7M-EH</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""328"">
<Display>Metron Spect260 4 6D 100CuFt</Display>
<Type>E</Type>
<Vendor>Metron</Vendor>
<Model>Spectrum 260</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""329"">
<Display>Metron Spect440 6 6D 100CuFt</Display>
<Type>E</Type>
<Vendor>Metron</Vendor>
<Model>Spectrum 440</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""330"">
<Display>ABB G2D1 GAS METER 100cuft</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>G2D1</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""331"">
<Display>Gallus 2000 5 Digit 1 cuft</Display>
<Type>R</Type>
<Vendor>Gallus</Vendor>
<Model>2000</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""332"">
<Display>AMCO G4 GAS METER 100cuft</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>G4</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>0</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""335"">
<Display>RW 250 4 Digit PHF 2 100 cuft</Display>
<Type>M</Type>
<Vendor>ROCKWELL</Vendor>
<Model>250</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>4</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""336"">
<Display>RW 1000 5 Digit PHF 10 100cuft</Display>
<Type>R</Type>
<Vendor>ROCKWELL</Vendor>
<Model>1000</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""337"">
<Display>RW R200 4 Digit PHF 2 100 cuft</Display>
<Type>M</Type>
<Vendor>ROCKWELL</Vendor>
<Model>R 200</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>4</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""338"">
<Display>RW R275 4 Digit PHF 2 100 cuft</Display>
<Type>M</Type>
<Vendor>ROCKWELL</Vendor>
<Model>R 275</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>4</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""339"">
<Display>Metron Enduro2000 6 6D 100CuFt</Display>
<Type>E</Type>
<Vendor>Metron</Vendor>
<Model>Enduro 2000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""340"">
<Display>Metron Enduro2800 8 6D 100CuFt</Display>
<Type>E</Type>
<Vendor>Metron</Vendor>
<Model>Enduro 2800</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""341"">
<Display>Marsh MMag 285 6D 100CuFt</Display>
<Type>R</Type>
<Vendor>Marsh-McBirney</Vendor>
<Model>Multi-Mag 285</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""342"">
<Display>Marsh MMag 285 6D 1000Gal</Display>
<Type>R</Type>
<Vendor>Marsh-McBirney</Vendor>
<Model>Multi-Mag 285</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""343"">
<Display>Metron Enduro2000 6 6D 1000G</Display>
<Type>E</Type>
<Vendor>Metron</Vendor>
<Model>Enduro 2000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""344"">
<Display>Metron Enduro2800 8 6D 1000Gal</Display>
<Type>E</Type>
<Vendor>Metron</Vendor>
<Model>Enduro 2800</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""345"">
<Display>SP 1000 5 Digit PHF 10 100cuft</Display>
<Type>R</Type>
<Vendor>Sprague</Vendor>
<Model>1000</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""346"">
<Display>Sensus SRII 1 1/2ECR-4d 10000gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""347"">
<Display>Sensus SRII 2 ECR-4d 10000gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""348"">
<Display>Sensus SRII 1 1/2ECR-6d 100gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""349"">
<Display>Sensus SRII 2 ECR-6d 100gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""350"">
<Display>Gas Universal Generic PHF .5</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>1</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""351"">
<Display>Gas Universal Generic PHF 1</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>0</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""352"">
<Display>Gas Universal Generic PHF 2</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>4</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""353"">
<Display>Gas Universal Generic PHF 2.5</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>3</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""354"">
<Display>Gas Universal Generic PHF 5</Display>
<Type>R</Type>
<Vendor>GENERIC </Vendor>
<Model>GENERIC</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>2</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""355"">
<Display>Gas Universal Generic PHF 10</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""356"">
<Display>Gas Universal Generic PHF 20</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>9</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""357"">
<Display>Gas Universal Generic PHF 25</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>8</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""358"">
<Display>Gas Universal Generic PHF 50</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""359"">
<Display>Gas Universal Generic PHF 100</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""360"">
<Display>ABB C-700 5/8x3/4 Digital 10 Gal</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""361"">
<Display>ABB C-700 3/4 Digital 10 Gal</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""362"">
<Display>ABB C-700 1 Digital 10 Gal</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""363"">
<Display>ABB C700 1 1/2Digital 100Gal</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""364"">
<Display>ABB C700 2Digital 100Gal</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""365"">
<Display>ABB AquaMaster x1 CU.FT.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""366"">
<Display>ABB AquaMaster x10 CU.FT.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""367"">
<Display>ABB AquaMaster x100 CU.FT.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""368"">
<Display>ABB C-700 5/8x3/4 Digital 1cuft</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""369"">
<Display>ABB C-700 3/4x1 Digital 1cuft</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""370"">
<Display>ABB C-700 5/8 Digital 1cuft</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""371"">
<Display>ABB C-700 3/4 Digital 1cuft</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""372"">
<Display>ABB C-700 1 Digital 1cuft</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""373"">
<Display>ABB C-700 1 1/2 Digital 10cuft</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""374"">
<Display>ABB C-700 2 Digital 10cuft</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""375"">
<Display>ABB AquaMaster x1 Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""376"">
<Display>ABB AquaMaster x10 Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""377"">
<Display>ABB AquaMaster x100 Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""378"">
<Display>Metron Spect22 3/4 6D 100Gal</Display>
<Type>E</Type>
<Vendor>Metron</Vendor>
<Model>Spectrum 22</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""379"">
<Display>Metron Spect50 1 6D 1000Gal</Display>
<Type>E</Type>
<Vendor>Metron</Vendor>
<Model>Spectrum 50</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""380"">
<Display>BADGER DualRTR2HI PLS 6D 10cuft</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Dual RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""381"">
<Display>BADGER DualRTR2LO PLS 6D 1cuft</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Dual RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""382"">
<Display>BADGER DualRTR3HI PLS 6D 10CUFT</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Dual RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""383"">
<Display>BADGER DualRTR3LO PLS 6D 1cuft</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Dual RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""384"">
<Display>BADGER DualRTR4HI PLS 6D100cuft</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Dual RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""385"">
<Display>BADGER DualRTR4LO PLS 6D 10cuft</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Dual RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""386"">
<Display>BADGER DualRTR6HI PLS 6D100cuft</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Dual RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""387"">
<Display>BADGER DualRTR6LO PLS 6D 10cuft</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Dual RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""388"">
<Display>BADGER DualRTR8HI PLS 6D100cuft</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Dual RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""389"">
<Display>BADGER DualRTR8LO PLS 6D 10Cuft</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Dual RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""390"">
<Display>Badger ADE M25 5/8 6D 10Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>M-25</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""391"">
<Display>Badger ADE M25 3/4 6D 10Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>M-25</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""392"">
<Display>Badger ADE M35 3/4 6D 10Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>M-35</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""393"">
<Display>Badger ADE M40 1 6D 10Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>M-40</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""394"">
<Display>Badger ADE M70 1 6D 10Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>M-70</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""395"">
<Display>Badger ADE M120 1 1/2 6D 100Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>M-120</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""396"">
<Display>Badger ADE M170 2 6D 100Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>M-170</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""397"">
<Display>Badger ADE M25 5/8 6D 1CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>M-25</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""398"">
<Display>Badger ADE M25 3/4 6D 1CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>M-25</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""399"">
<Display>Badger ADE M35 3/4 6D 1CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>M-35</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""400"">
<Display>Gas Generic PHF .5</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>1</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""401"">
<Display>Gas Generic PHF 1</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>0</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""402"">
<Display>Gas Generic PHF 2</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>4</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""403"">
<Display>Gas Generic PHF 2.5</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>3</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""404"">
<Display>Gas Generic PHF 5</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>2</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""405"">
<Display>Gas Generic PHF 10</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""406"">
<Display>Gas Generic PHF 20</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>9</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""407"">
<Display>Gas Generic PHF 25</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>8</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""408"">
<Display>Gas Generic PHF 50</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""409"">
<Display>Gas Generic PHF 100</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""410"">
<Display>Gas Generic PHF .5</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>1</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""411"">
<Display>Gas Generic PHF 1</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>0</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""412"">
<Display>Gas Generic PHF 2</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>4</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""413"">
<Display>Gas Generic PHF 2.5</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>3</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""414"">
<Display>Gas Generic PHF 5</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>2</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""415"">
<Display>Gas Generic PHF 10</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""416"">
<Display>Gas Generic PHF 20</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>9</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""417"">
<Display>Gas Generic PHF 25</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>8</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""418"">
<Display>Gas Generic PHF 50</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""419"">
<Display>Gas Generic PHF 100</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""420"">
<Display>Precision PMM 5/8x3/4 6d 10 gal</Display>
<Type>R</Type>
<Vendor>PRECISION</Vendor>
<Model>PMM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""421"">
<Display>Precision PMM 3/4 6d 10 gal</Display>
<Type>R</Type>
<Vendor>PRECISION</Vendor>
<Model>PMM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""422"">
<Display>Precision PMM 1 6d 10 gal</Display>
<Type>R</Type>
<Vendor>PRECISION</Vendor>
<Model>PMM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""423"">
<Display>Precision PMM 1 1/2 6d 100gal</Display>
<Type>R</Type>
<Vendor>PRECISION</Vendor>
<Model>PMM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""424"">
<Display>Precision PMM 2 6d 100gal</Display>
<Type>R</Type>
<Vendor>PRECISION</Vendor>
<Model>PMM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""425"">
<Display>Precision PMM 3 6d 100gal</Display>
<Type>R</Type>
<Vendor>PRECISION</Vendor>
<Model>PMM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""426"">
<Display>Precision PMM 4 6d 100gal</Display>
<Type>R</Type>
<Vendor>PRECISION</Vendor>
<Model>PMM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""427"">
<Display>Precision PMM 6 6d 1000gal</Display>
<Type>R</Type>
<Vendor>PRECISION</Vendor>
<Model>PMM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""428"">
<Display>Metron Spect88 1 1/2 6D 1000Gal</Display>
<Type>E</Type>
<Vendor>Metron</Vendor>
<Model>Spectrum 88</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""429"">
<Display>Metron Spect130 2 6D 1000Gal</Display>
<Type>E</Type>
<Vendor>Metron</Vendor>
<Model>Spectrum 130</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""430"">
<Display>RCU 11M 5 Digit PHF 10 100cuft</Display>
<Type>R</Type>
<Vendor>Roots</Vendor>
<Model>11M</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""431"">
<Display>RCU 16M 6 Digit PHF 100 100cuft</Display>
<Type>R</Type>
<Vendor>Roots</Vendor>
<Model>16M</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""432"">
<Display>RCU 38MID 6 Digit PHF 100 100cuft</Display>
<Type>R</Type>
<Vendor>Roots</Vendor>
<Model>38MID</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""433"">
<Display>RCU 3M 5 Digit PHF 10 100cuft</Display>
<Type>R</Type>
<Vendor>Roots</Vendor>
<Model>3M</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""434"">
<Display>RCU 5M 5 Digit PHF 10 100cuft</Display>
<Type>R</Type>
<Vendor>Roots</Vendor>
<Model>5M</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""435"">
<Display>RCU 5M-LD 6 Digit PHF 10 100cuft</Display>
<Type>R</Type>
<Vendor>Roots</Vendor>
<Model>5M-LD</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""436"">
<Display>RCU 7M 5 Digit PHF 10 100cuft</Display>
<Type>R</Type>
<Vendor>Roots</Vendor>
<Model>7M</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""437"">
<Display>RCU 7M-EH 6 Digit PHF 100 100cft</Display>
<Type>R</Type>
<Vendor>Roots</Vendor>
<Model>7M-EH</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""438"">
<Display>Metron Spect175 3 6D 1000Gal</Display>
<Type>E</Type>
<Vendor>Metron</Vendor>
<Model>Spectrum 175</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""439"">
<Display>Metron Spect260 4 6D 1000Gal</Display>
<Type>E</Type>
<Vendor>Metron</Vendor>
<Model>Spectrum 260</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""440"">
<Display>Metron Spect440 6 6D 1000Gal</Display>
<Type>E</Type>
<Vendor>Metron</Vendor>
<Model>Spectrum 440</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""441"">
<Display>Badger ADE M40 1 6D 1CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>M-40</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""442"">
<Display>Badger ADE M70 1 6D 1CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>M-70</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""443"">
<Display>Badger ADE M120 1 1/2 6D 10CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>M-120</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""444"">
<Display>Badger ADE M170 2 6D 10CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>M-170</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""445"">
<Display>MLOG Leak Detector</Display>
<Type>E</Type>
<Vendor>Flow Metrix</Vendor>
<Model>MLOG</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""446"">
<Display>SENSUS SRII 5/8 ICE 6D .01CuMtr</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>1</LeadingDummy>
<Scale>3</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""447"">
<Display>SENSUS SRII 3/4 ICE 6D .01CuMtr</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>1</LeadingDummy>
<Scale>3</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""448"">
<Display>SENSUS SRII 1 ICE 6D .01CuMtr</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>1</LeadingDummy>
<Scale>3</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""449"">
<Display>SENSUS SRII 1 1/2 ICE 6D .01CuMtr</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>1</LeadingDummy>
<Scale>3</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""450"">
<Display>ISTAMETER 5/8X3/4 PULSE 10 GAL</Display>
<Type>R</Type>
<Vendor>ISTAMETER</Vendor>
<Model> ISTAMETER</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""451"">
<Display>DOMAQUA 5/8X3/4 pulse 10 Gal</Display>
<Type>R</Type>
<Vendor>DOMAQUA</Vendor>
<Model>DOMAQUA</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""452"">
<Display>SENSUS SRII 2 ICE 6D .01CuMtr</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>1</LeadingDummy>
<Scale>3</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""453"">
<Display>4-6 GENERIC PLSGEN 6D 1000Gals</Display>
<Type>G</Type>
<Vendor>ROCKWELL</Vendor>
<Model>GENERIC</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""454"">
<Display>Badger M70 1.5 RTR  6D 10CuFt/10</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>M-70</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""455"">
<Display>Badger M120 2 RTR 10CuFt/10</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>M-120</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""456"">
<Display>Bermad 900 Mag Srs 6D 1CuMtr/100</Display>
<Type>R</Type>
<Vendor>Bermad</Vendor>
<Model>900 Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>0</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""457"">
<Display>Bermad 900 Mag Srs 3 6D 0.01CuMtr</Display>
<Type>R</Type>
<Vendor>Bermad</Vendor>
<Model>900 Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>2</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""458"">
<Display>NEPT 2 PRO6 5CuMtr</Display>
<Type>E</Type>
<Vendor>SCHLUM/Neptune</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""459"">
<Display>NEPT 2 ARBV 6D 5CuMtr</Display>
<Type>E</Type>
<Vendor>SCHLUM/Neptune</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""460"">
<Display>Diaphragm 4 Digit PHF 1</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>Diaphragm</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>0</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""461"">
<Display>Diaphragm 4 Digit PHF 2</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>Diaphragm </Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>4</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""462"">
<Display>Diaphragm 5 Digit PHF 5</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>Diaphragm </Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>2</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""463"">
<Display>Diaphragm 5 Digit PHF 10</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>Diaphragm </Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""464"">
<Display>Diaphragm 4 Digit PHF 1</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>Diaphragm</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>0</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""465"">
<Display>Diaphragm 4 Digit PHF 2</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>Diaphragm </Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>4</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""466"">
<Display>Diaphragm 5 Digit PHF 5</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>Diaphragm </Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>2</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""467"">
<Display>Diaphragm 5 Digit PHF 10</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>Diaphragm </Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""468"">
<Display>Schlum Cmp 4Hi Pro 4D 1000CuFt</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Cmpnd</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""469"">
<Display>Schlum Cmp 4Lo Pro 4D 100CuFt</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Cmpnd</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""470"">
<Display>Rotary 5 Dig 10 CuFt/pulse</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>Rotary</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""471"">
<Display>Rotary 6 Dig 10 CuFt/pulse</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>Rotary</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""472"">
<Display>Rotary 6 Dig 100 CuFt/pulse</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>Rotary</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""473"">
<Display>Badger ADE TurboSRS 1.5 6D 10CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""474"">
<Display>Badger ADE TurboSRS 2 6D 10CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""475"">
<Display>Instrument 6D 10 CuFt/pulse</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>Instrument</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""476"">
<Display>Instrument 6D 100 CuFt/pulse</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>Instrument</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""477"">
<Display>Instrument 6D 1000 CuFt/pulse</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>Instrument</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""478"">
<Display>Instrument 6D 10000 CuFt/pulse</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>Instrument</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""480"">
<Display>Diaphragm 4 Digit PHF 1</Display>
<Type>K</Type>
<Vendor>GENERIC</Vendor>
<Model>Diaphragm</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>0</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""481"">
<Display>Badger ADE TurboSRS 3 6D 10CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""482"">
<Display>Badger ADE TurboSRS 4 6D 10CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""483"">
<Display>Badger ADE TurboSRS 6 6D 100CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""484"">
<Display>Badger ADE TurboSRS 8 6D 100CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""485"">
<Display>Badger ADE TurbSRS 10 6D 100CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""486"">
<Display>Badger ADE TrbSRS 12 6D 1000CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""487"">
<Display>Badger ADE TrbSRS 16 6D 1000CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""488"">
<Display>Bdger ADE TrbSRS 20 6D 10000CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>4</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""489"">
<Display>Badger ADE TurboSRS 1.5 6D 100Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""490"">
<Display>Badger ADE TurboSRS 2 6D 100Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""491"">
<Display>Sensus Icon Electric Meter 2S Kwh</Display>
<Type>I</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>Icon</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""492"">
<Display>Sensus Icon Electric Meter 12S Kwh</Display>
<Type>I</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>Icon</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""493"">
<Display>Sensus Icon Electric Meter 320 Kwh</Display>
<Type>I</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>Icon</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""494"">
<Display>Badger ADE TurboSRS 3 6D 100Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""495"">
<Display>Badger ADE TurboSRS 4 6D 100Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""496"">
<Display>Badger ADE TurboSRS 6 6D 1000Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""497"">
<Display>Badger ADE TurboSRS 8 6D 1000Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""498"">
<Display>Badger ADE TurbSRS 10 6D 1000Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""499"">
<Display>Badger ADE TrbSRS 12 6D 10000Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>4</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""500"">
<Display>SCHLUM HPT 1-1/2 PRO6 10CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""501"">
<Display>SCHLUM HPT 2 PRO6 10CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""502"">
<Display>SCHLUM HPT 3 PRO6 10CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""503"">
<Display>SCHLUM HPT 4 PRO6 10CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""504"">
<Display>SCHLUM HPT 6 PRO6 100CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""505"">
<Display>SCHLUM HPT 8 PRO6 100CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""506"">
<Display>SCHLUM HPT 10 PRO6 100CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""507"">
<Display>Schlum Cmp 3Hi Pro 4D 1000CuFt</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Cmpnd</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""508"">
<Display>Schlum Cmp 3Lo Pro 4D 100CuFt</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Cmpnd</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""509"">
<Display>Badger ADE TrbSRS 16 6D 10000Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>4</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""510"">
<Display>SCHLUM HPT 1 1/2 PRO6 100 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""511"">
<Display>SCHLUM HPT 2 PRO6 100 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""512"">
<Display>SCHLUM HPT 3 PRO6 100 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""513"">
<Display>SCHLUM HPT 4 PRO6 100 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""514"">
<Display>SCHLUM HPT 6 PRO6 1000 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""515"">
<Display>SCHLUM HPT 8 PRO6 1000 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""516"">
<Display>SCHLUM HPT 10 PRO6 1000 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""517"">
<Display>SCHLUM HPT 12 PRO6 1000GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""518"">
<Display>Badger ADE TrbSRS 20 6D 10000Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>4</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""519"">
<Display>Badger ADE CmpdSRS 2 Hi 6D 10CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""520"">
<Display>SCHLUM HPT 1 1/2 ARBV 6D 10CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""521"">
<Display>SCHLUM HPT 2 ARBV 6D 10CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""522"">
<Display>SCHLUM HPT 3 ARBV 6D 10CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""523"">
<Display>SCHLUM HPT 4 ARBV 6D 10CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""524"">
<Display>SCHLUM HPT 6 ARBV 6D 100CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""525"">
<Display>SCHLUM HPT 8 ARBV 6D 100CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""526"">
<Display>SCHLUM HPT 10 ARBV 6D 100CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>HPT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""527"">
<Display>Badger ADE CmpdSRS 2 Lo 6D 1CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""528"">
<Display>Badger ADE CmpdSRS 3 Hi 6D 10CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""529"">
<Display>Badger ADE CmpdSRS 3 Lo 6D 1CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""530"">
<Display>SCHLM HPT 1 1/2 ARBV4D 10000gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""531"">
<Display>SCHLM HPT 2 ARBV 4D 10000gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""532"">
<Display>SCHLM HPT 3 ARBV 4D 10000gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""533"">
<Display>SCHLM HPT 4 ARBV 4D 10000gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""534"">
<Display>SCHLM HPT 6 ARBV 4D 100000gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""535"">
<Display>SCHLM HPT 8 ARBV 4D 100000gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""536"">
<Display>SCHLM HPT 10 ARBV 4D 100000gal</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>HPT</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""537"">
<Display>Badger ADE CmpdSRS 4 Hi 6D 10CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""538"">
<Display>Badger ADE CmpdSRS 4 Lo 6D 1CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""539"">
<Display>Badger ADE CmpdSRS 6 Hi 6D 10CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""540"">
<Display>HERSEY MVR30 3/4 PLS 6D 10gal</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MVR30</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""541"">
<Display>HERSEY MVR50 1 PULSE 6D 10gal</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MVR50</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""542"">
<Display>HERSEY MVR100 1 1/2 PLS6D100gal</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MVR100</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""543"">
<Display>HERSEY MVR160 2 PULSE 6D 100gal</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MVR160</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""544"">
<Display>HERSEY MVR350 3 PULSE 6D 100gal</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MVR350</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""545"">
<Display>HERSEY MVR650 4 PULSE 6D 100gal</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MVR650</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""546"">
<Display>HERSEY MVR1300 6PLS 6D1000gal</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MVR1300</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""547"">
<Display>Badger ADE CmpdSRS 6 Lo 6D 1CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""548"">
<Display>Badger ADE CmpdSRS 2 Hi 6D 100Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""549"">
<Display>Badger ADE CmpdSRS 2 Lo 6D 10Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""550"">
<Display>HERSEY ER1 5/8 WEIG PULS 6D 10GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>ER-1</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""551"">
<Display>HERSEY ER1 3/4 WEIG PULS 6D 10GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>ER-1</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""552"">
<Display>Badger ADE CmpdSRS 3 Hi 6D 100Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""553"">
<Display>Badger ADE CmpdSRS 3 Lo 6D 10Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""554"">
<Display>Badger ADE CmpdSRS 4 Hi 6D 100Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""555"">
<Display>Badger ADE CmpdSRS 4 Lo 6D 10Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""556"">
<Display>Badger ADE CmpdSRS 6 Hi 6D 100Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""557"">
<Display>Badger ADE CmpdSRS 6 Lo 6D 10Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""558"">
<Display>Badger ADE FireSM 3 6D 10 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Fire Service Meter</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""559"">
<Display>HERSEY MVR430 5/8 PLS 6D 1 CU FT</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MVR430</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""560"">
<Display>HERSEY MVR30 3/4 PLS 6D 1 CU FT</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MVR30</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""561"">
<Display>HERSEY MVR50 1 PULSE 6D 1 CU FT</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MVR50</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""562"">
<Display>HERSEY MVR100 1 1/2 PLS 6D10CUFT</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MVR100</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""563"">
<Display>HERSEY MVR160 2 PLS 6D 10 CU FT</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MVR160</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""564"">
<Display>HERSEY MVR350 3 PLS 6D 10 CU FT</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MVR350</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""565"">
<Display>HERSEY MVR650 4 PLS 6D 10 CU FT</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MVR650</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""566"">
<Display>HERSEY MVR1300 6 PLS 6D100CUft</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MVR1300</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""567"">
<Display>Badger ADE FireSM 4 6D 10 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Fire Service Meter</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""568"">
<Display>Badger ADE FireSM 6 6D 100CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Fire Service Meter</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""569"">
<Display>Badger ADE FireSM 8 6D 100CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Fire Service Meter</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""570"">
<Display>HERSEY MCTII CMPD 2HI 6D 100GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCTII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""571"">
<Display>HERSEY MCTII CMPD 2LO 6D 10GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCTII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""572"">
<Display>HERSEY MCTII CMPD 3HI 6D 100GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCTII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""573"">
<Display>HERSEY MCTII CMPD 3LO 6D 10GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCTII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""574"">
<Display>HERSEY MCTII CMPD 4HI 6D 1000GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCTII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""575"">
<Display>HERSEY MCTII CMPD 4LO 6D 10GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCTII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""576"">
<Display>HERSEY MCTII CMPD 6HI 6D 1000GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCTII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""577"">
<Display>HERSEY MCTII CMPD 6LO 6D 100GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCTII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""578"">
<Display>Hersey Generic Reed 6D 10Gal/4</Display>
<Type>R</Type>
<Vendor>Hersey</Vendor>
<Model>Generic</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>8</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""579"">
<Display>Hersey Generic Reed 6D 1CuFt/4</Display>
<Type>R</Type>
<Vendor>Hersey</Vendor>
<Model>Generic</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>8</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""580"">
<Display>INVENSYS SRII 5/8 ICE 6D 10GAL</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""581"">
<Display>INVENSYS SRII 3/4 ICE 6D 10GAL</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""582"">
<Display>INVENSYS SRII 1 ICE 6D 10GAL</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""583"">
<Display>INVENSYS SRII 1 1/2 ICE 6D 100G</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""584"">
<Display>INVENSYS SRII 2 ICE 6D 100GAL</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""585"">
<Display>Badger ADE FireSM 10 6D 100CuFt</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Fire Service Meter</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""586"">
<Display>Badger ADE FireSM 3 6D 100Gals</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Fire Service Meter</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""587"">
<Display>Badger ADE FireSM 4 6D 100Gals</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Fire Service Meter</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""588"">
<Display>Badger ADE FireSM 6 6D 1000Gals</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Fire Service Meter</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""589"">
<Display>Badger ADE FireSM 8 6D 1000Gals</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Fire Service Meter</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""590"">
<Display>INVENSYS FS 4HI ECR 6D 100CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>Fire Service Compound</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""591"">
<Display>INVENSYS FS 4LO ECR 6D 10CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>Fire Service Compound</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""592"">
<Display>INVENSYS FS 6HI ECR 6D 100CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>Fire Service Compound</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""593"">
<Display>INVENSYS FS 6LO ECR 6D 10CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>Fire Service Compound</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""594"">
<Display>INVENSYS FS 8HI ECR 6D 100CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>Fire Service Compound</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""595"">
<Display>INVENSYS FS 8LO ECR 6D 10CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>Fire Service Compound</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""596"">
<Display>INVENSYS FS 10HI ECR 6D 100CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>Fire Service Compound</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""597"">
<Display>INVENSYS FS 10LO ECR 6D 10CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>Fire Service Compound</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""598"">
<Display>Badger ADE FireSM 10 6D 1000Gals</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>Fire Service Meter</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""599"">
<Display>ABB T-3000 12 Scndr 1000 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""600"">
<Display>INVENSYS SRII 5/8 ICE 6D 1CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""601"">
<Display>INVENSYS SRII 3/4 ICE 6D 1CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""602"">
<Display>INVENSYS SRII 1 ICE 6D 1CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""603"">
<Display>Sensus Icon Electric Meter 5D 2S Kwh</Display>
<Type>I</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>Icon</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""604"">
<Display>1KWH Meter, PHF10</Display>
<Type>R</Type>
<Vendor>ITRON</Vendor>
<Model>KWH Meter</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>56</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""608"">
<Display>INVENSYS SR 1 1/2HiRes4d100cuft</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SR - Hi Res</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""609"">
<Display>INVENSYS SR 2HiRes4d100cuft</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SR - Hi Res</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""610"">
<Display>INVENSYS FS 4HI ECR 6D 1000gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>Fire Service Compound</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""611"">
<Display>INVENSYS FS 4LO ECR 6D 100gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>Fire Service Compound</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""612"">
<Display>INVENSYS FS 6HI ECR 6D 1000gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>Fire Service Compound</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""613"">
<Display>INVENSYS FS 6LO ECR 6D 100gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>Fire Service Compound</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""614"">
<Display>INVENSYS FS 8HI ECR 6D 1000gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>Fire Service Compound</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""615"">
<Display>INVENSYS FS 8LO ECR 6D 100gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>Fire Service Compound</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""616"">
<Display>INVENSYS FS 10HI ECR 6D 1000gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>Fire Service Compound</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""617"">
<Display>INVENSYS FS 10LO ECR 6D 100gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>Fire Service Compound</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""618"">
<Display>Invensys FS 8Lo HIRes 4d1000gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>Fire Service Compound</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""619"">
<Display>Badger RTRM25 5/8x5/8 6D 10GAL</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>M25 RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""620"">
<Display>Badger RTRM25 5/8x3/4 6D 10GAL</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>M25 RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""621"">
<Display>Badger RTR M25 3/4 PLS 6D 10GAL</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>M25 RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""622"">
<Display>Badger RTR M35 3/4 PLS 6D 10GAL</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>M35 RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""623"">
<Display>Badger RTR M40 1 PLS 6D 10GAL</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>M40 RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""624"">
<Display>Badger RTR M70 1 PLS 6D 10GAL</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>M70 RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""625"">
<Display>Badger RTR M120 1 1/2PL6D100GAL</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>M120 RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""626"">
<Display>Badger RTR M170 2 PLS 6D 100GAL</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>M170 RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""627"">
<Display>Sensus ICE 2 SRH Cmpnd 6D 100Gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""628"">
<Display>Sensus ICE 3 SRH Cmpnd 6D 100Gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""629"">
<Display>Sensus ICE 4 SRH Cmpnd 6D 1000Gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""630"">
<Display>BADGER DualRTR 2HI PLS 6D 100GAL</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Dual RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""631"">
<Display>BADGER DualRTR 2LO PLS 6D 10GAL</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Dual RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""632"">
<Display>BADGER DualRTR 3HI PLS 6D 100GAL</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Dual RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""633"">
<Display>BADGER DualRTR 3LO PLS 6D 10GAL</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Dual RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""634"">
<Display>BADGER DualRTR 4HI PLS 6D1000GAL</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Dual RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""635"">
<Display>BADGER DualRTR 4LO PLS 6D 100GAL</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Dual RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""636"">
<Display>BADGER DualRTR 6HI PLS 6D1000GAL</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Dual RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""637"">
<Display>BADGER DualRTR 6LO PLS 6D 100GAL</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Dual RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""638"">
<Display>BADGER DualRTR 8HI PLS 6D1000GAL</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Dual RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""639"">
<Display>BADGER DualRTR 8LO PLS 6D 100GAL</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Dual RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""640"">
<Display>BADGER DualRTR 2HI PLS 6D 1000GAL</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Dual RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""641"">
<Display>BADGER DualRTR 3HI PLS 6D 1000GAL</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Dual RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""642"">
<Display>BADGER DualRTR 4LO PLS 6D 10GAL</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Dual RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""644"">
<Display>INVENSYS 2 SRH CMPND 6D 10 CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""645"">
<Display>INVENSYS 3 SRH CMPND 6D 10 CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""646"">
<Display>INVENSYS 4 SRH CMPND 6D 100 CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""647"">
<Display>INVENSYS 6 SRH CMPND 6D 100 CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""648"">
<Display>Sensus 2 W-Turbo 6D 100Gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""649"">
<Display>INVENSYS 2 W-Turbo 6D 10CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""650"">
<Display>INVENSYS 3 W-Turbo 6D 10CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""651"">
<Display>INVENSYS 4 W-Turbo 6D 100CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""652"">
<Display>INVENSYS 6 W-Turbo 6D 100CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""653"">
<Display>INVENSYS 8 W-Turbo 6D 100CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""654"">
<Display>INVENSYS 10 W-Turbo 6D 100CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""655"">
<Display>Metron 2 6D 100CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""656"">
<Display>Metron 3 6D 100CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""657"">
<Display>Metron 4 6D 100CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""658"">
<Display>Metron 6 6D 100CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-Turbo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""659"">
<Display>Sensus ICE 6 SRH Cmpnd 6D 1000Gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""660"">
<Display>INVENSYS SRII5/8x3/4 ECR6 1CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""661"">
<Display>Invensys SRII 3/4 ECR-6 1CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""662"">
<Display>INVENSYS SRII 1 ECR-6 1CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""663"">
<Display>INVENSYS SRII 1 1/2 ECR-6 10CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""664"">
<Display>INVENSYS SRII 2 ECR-6 10CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""665"">
<Display>Sensus ICE 8 SRH Cmpnd 6D 1000Gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""666"">
<Display>Sensus ICE 3 SRH Cmpd 6D 1CuMtr</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>2</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""667"">
<Display>Sensus ICE 4 SRH Cmpd 6D 1CuMtr</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>2</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""668"">
<Display>Sensus ICE 6 SRH Cmpd 6D 1CuMtr</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>2</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""669"">
<Display>Neptune TT 3 PRO 6D 1CuMtr</Display>
<Type>E</Type>
<Vendor>Schlum/Neptune</Vendor>
<Model>TT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""670"">
<Display>MASTERMETER FAM 5/8 6D 10GAL</Display>
<Type>R</Type>
<Vendor>Master Meter</Vendor>
<Model>FAM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""671"">
<Display>Sensus ICE 2 SRH Cmpnd 6D 10CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""672"">
<Display>Sensus ICE 3 SRH Cmpnd 6D 10CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""673"">
<Display>MASTERMETER FAM 1 1/2 6D100GAL</Display>
<Type>R</Type>
<Vendor>Master Meter</Vendor>
<Model>FAM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""674"">
<Display>Sensus ICE 4 SRH Cmpnd 6D 100CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""675"">
<Display>Sensus ICE 6 SRH Cmpnd 6D 100CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""676"">
<Display>Sensus ICE 8 SRH Cmpnd 6D 100CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""677"">
<Display>Neptune TT 4 PRO 6D 1CuMtr</Display>
<Type>E</Type>
<Vendor>Schlum/Neptune</Vendor>
<Model>TT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""678"">
<Display>Neptune TT 6 PRO 6D 1CuMtr</Display>
<Type>E</Type>
<Vendor>Schlum/Neptune</Vendor>
<Model>TT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""679"">
<Display>AMCO C-700 5/8x1/2 Scndr 1 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-700</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""680"">
<Display>HERSEY MCT CMPD 2HI 6D 100GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""681"">
<Display>HERSEY MCT CMPD 2LO 6D 10GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""682"">
<Display>HERSEY MCT CMPD 3HI 6D 100GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""683"">
<Display>HERSEY MCT CMPD 3LO 6D 10GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""684"">
<Display>HERSEY MCT CMPD 4HI 6D 1000GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""685"">
<Display>HERSEY MCT CMPD 4LO 6D 10GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""686"">
<Display>HERSEY MCT CMPD 6HI 6D 1000GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""687"">
<Display>HERSEY MCT CMPD 6LO 6D 100GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""688"">
<Display>HERSEY MCT CMPD 8HI 6D 1000GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""689"">
<Display>HERSEY MCT CMPD 8LO 6D 100GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCT</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""690"">
<Display>HERSEY MCT TURB 3/4 6D 10GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCT Turbine</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""691"">
<Display>HERSEY MCT TURB 1 6D 10GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCT Turbine</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""692"">
<Display>HERSEY MCT TURB 1 1/2 6D 100GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCT Turbine</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""693"">
<Display>HERSEY MCT TURB 2 6D 100GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCT Turbine</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""694"">
<Display>HERSEY MCT TURB 3 6D 100GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCT Turbine</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""695"">
<Display>HERSEY MCT TURB 4 6D 100GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCT Turbine</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""696"">
<Display>HERSEY MCT TURB 6 6D 1000GAL</Display>
<Type>W</Type>
<Vendor>Hersey</Vendor>
<Model>MCT Turbine</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>7</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>0</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""697"">
<Display>Norgas RT500 Module</Display>
<Type>R</Type>
<Vendor>Norgas</Vendor>
<Model>RT500</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>56</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""698"">
<Display>Sensus ICE 3 WTurbo 6D 100Gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""699"">
<Display>Sensus ICE 4 WTurbo 6D 1000Gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""700"">
<Display>GE Electric 1 Pulse = 3.84 KW</Display>
<Type>R</Type>
<Vendor>GE</Vendor>
<Model>GE</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""701"">
<Display>Badger Mag 6-8 Pls 6D 100GAL</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Magnetoflo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""702"">
<Display>Industrial Gas 6Dig 1000 CU.FT.</Display>
<Type>R</Type>
<Vendor>Gas Meter</Vendor>
<Model>Gas Meter</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""703"">
<Display>Yokagawa uXL Steam 1 K-lbs</Display>
<Type>R</Type>
<Vendor>Yokagawa</Vendor>
<Model>uXL Steam Meter</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""704"">
<Display>Hexagram Pulse Adapter</Display>
<Type>P</Type>
<Vendor>Hexagram</Vendor>
<Model>Pulse Adapter</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""705"">
<Display>Hexagram Temp Adapter</Display>
<Type>T</Type>
<Vendor>Hexagram</Vendor>
<Model>Temp Adapter</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>2</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""706"">
<Display>Sensus ICE 6 WTurbo 6D 1000Gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""707"">
<Display>Sensus ICE 8 WTurbo 6D 1000Gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""708"">
<Display>Sensus ICE 10 WTurbo 6D 1000Gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""709"">
<Display>Sensus ICE 16 WTurbo 6D 10000Gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""710"">
<Display>Badger TURBO RTR 2 PLS 6D 10CUFT</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""711"">
<Display>Badger TURBO RTR 3 PLS 6D 10CUFT</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Turbo RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""712"">
<Display>Badger TURBO RTR 4 PLS 6D 10CUFT</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""713"">
<Display>Badger TURBO RTR 6 PLS 6D 10CUFT</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""714"">
<Display>Badger TURBO RTR 8 PLS 6D 10CUFT</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""715"">
<Display>Badger TURBO RTR 10PLS6D 10CUFT</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""716"">
<Display>Badger TURBO RTR 12PLS6D100CUFT</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""719"">
<Display>Badger TURBO RTR 2 PLS 6D 1000Gal</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""720"">
<Display>Badger TURBO RTR 2 PLS 6D 100Gal</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""721"">
<Display>Badger TURBO RTR 3 PLS 6D 100Gal</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""722"">
<Display>Badger TURBO RTR 4 PLS 6D 100Gal</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""723"">
<Display>Badger TURBO RTR 6 PLS 6D 100Gal</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""724"">
<Display>Badger TURBO RTR 8 PLS 6D 100Gal</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""725"">
<Display>Badger TURBO RTR 10PLS6D 100Gal</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""726"">
<Display>Badger TURBO RTR 12PLS6D1000Gal</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""727"">
<Display>Badger TURBO RTR 5/8 PLS 6D 0.1CUFT</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>1</LeadingDummy>
<Scale>1</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""728"">
<Display>Badger T1000 RTR 4 PLS 6D 1CUFT</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo T1000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>1</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""729"">
<Display>Hexagram Pulse Adapter 6P/Min</Display>
<Type>B</Type>
<Vendor>Hexagram</Vendor>
<Model>Pulse Adapter (6P/Min)</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>2</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""730"">
<Display>Hersey Trans 5/8x3/4 6 D 1CuFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>400 Series IIS</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""731"">
<Display>Hersey Trans 1 6Digit 1CuFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>400 Series IIS</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""732"">
<Display>Hersey Trans 1 1/2 6D 10CuFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>500 Series IIS</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""733"">
<Display>Hersey Trans 2 6D 10CuFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>500 Series IIS</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""734"">
<Display>Badger T2000 RTR 6 PLS 6D 10CUFT</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo T2000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>1</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""735"">
<Display>Badger RTR M120 1.5 PLS 6D 1CUFT</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>M120</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>1</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""736"">
<Display>Badger RTR M170 2 PLS 6D 1CUFT</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>M170</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>1</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""737"">
<Display>MASTERMETER FAM 2 6D100GAL</Display>
<Type>R</Type>
<Vendor>Master Meter</Vendor>
<Model>FAM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""738"">
<Display>MasterMeter 1-2 6 Digit 10 Gal</Display>
<Type>R</Type>
<Vendor>Master Meter</Vendor>
<Model>FAM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""740"">
<Display>Invensys SRII 5/8 ECR4 100CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""741"">
<Display>Invensys SRII 3/4 ECR4 100CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""742"">
<Display>Invensys SRII 1 ECR4 100CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""745"">
<Display>Invnsys SRH2HR cmpd ECR4 100cft</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII - High Res</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""746"">
<Display>Invnsys SRH3HR cmpd ECR4 100cft</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII - High Res</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""747"">
<Display>Invnsys SRH4HR cpd ECR4 1000cft</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII - High Res</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""748"">
<Display>Invnsys SRH6HR cpd ECR4 1000cft</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII - High Res</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""749"">
<Display>SENSUS ICE 3/4 WTURBO 6D 10CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""750"">
<Display>Invnsys 1 1/2HR turb ECR4 100cft</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""751"">
<Display>Invnsys 2HR turbo ECR4 100cft</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""752"">
<Display>Invnsys 3HR turbo ECR4 100cft</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""753"">
<Display>SENSUS ICE 1 1/2 WTURBO 6D 10CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""754"">
<Display>SENSUS ICE 2 WTURBO 6D 10CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""755"">
<Display>SENSUS ICE 3 WTURBO 6D 10CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""756"">
<Display>SENSUS ICE 4 WTURBO 6D 100CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""757"">
<Display>SENSUS ICE 1 WTURBO 6D 10CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""758"">
<Display>SENSUS ICE 6 WTURBO 6D 100CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""759"">
<Display>SENSUS ICE 8 WTURBO 6D 100CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""760"">
<Display>Badger Mag 2 PLS 6D 100GAL</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Magnetoflo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""761"">
<Display>Badger Mag 3 PLS 6D 100GAL</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Magnetoflo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""762"">
<Display>Badger Mag 4 PLS 6D 100GAL</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Magnetoflo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""763"">
<Display>Badger Mag 10-12 PLS 6D 1000GAL</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Magnetoflo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""764"">
<Display>Badger Mag 2 PLS 6D 100CUFT</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Magnetoflo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""765"">
<Display>Badger Mag 3 PLS 6D 100CUFT</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Magnetoflo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""766"">
<Display>Badger Mag 4 PLS 6D 100CUFT</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Magnetoflo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""767"">
<Display>Badger Mag 6 PLS 6D 100CUFT</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Magnetoflo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""768"">
<Display>Badger Mag 8 PLS 6D 100CUFT</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Magnetoflo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""769"">
<Display>Badger Mag 10 PLS 6D 100CUFT</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Magnetoflo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""770"">
<Display>NIGC G4 5d phf1</Display>
<Type>R</Type>
<Vendor>NIGC</Vendor>
<Model>G4</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>3</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>0</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""771"">
<Display>NIGC G6 5d phf1</Display>
<Type>R</Type>
<Vendor>NIGC</Vendor>
<Model>G6</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>3</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>0</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""772"">
<Display>NIGC G4 5d phf .1</Display>
<Type>R</Type>
<Vendor>NIGC</Vendor>
<Model>G4</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>3</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>12</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""773"">
<Display>NIGC G6 5d phf .1</Display>
<Type>R</Type>
<Vendor>NIGC</Vendor>
<Model>G6</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>3</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>12</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""774"">
<Display>Sigma Model970 6D 10cuft</Display>
<Type>G</Type>
<Vendor>Sigma</Vendor>
<Model>Model 970 Flow Meter</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>56</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""775"">
<Display>Sigma Model970 6D 100cuft</Display>
<Type>G</Type>
<Vendor>Sigma</Vendor>
<Model>Model 970 Flow Meter</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>56</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""776"">
<Display>Sigma Model970 6D 1000cuft</Display>
<Type>G</Type>
<Vendor>Sigma</Vendor>
<Model>Model 970 Flow Meter</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>56</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""777"">
<Display>Sigma Model970 6D 1cuft</Display>
<Type>G</Type>
<Vendor>Sigma</Vendor>
<Model>Model 970 Flow Meter</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>56</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""780"">
<Display>INVNSYS SR 5/8 Hi Res ECR4 100gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SR - Hi Res</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""781"">
<Display>INVENSYS SR 1 Hi Res ECR4 100gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SR - Hi Res</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""782"">
<Display>INVENSYS SR 1 1/2HiRes4d1000gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SR - Hi Res</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""783"">
<Display>INVENSYS SR 2 Hi Res ECR4 1000gal</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SR - Hi Res</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""784"">
<Display>Invnsys SRH2 cmpd ECR4 10000GL</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""785"">
<Display>Invnsys SRH3 cmpd ECR4 10000GL</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""786"">
<Display>Invnsys SRH4 cmpd ECR4 100000G</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""787"">
<Display>Invnsys SRH6 cmpd ECR4 100000G</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>2</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""790"">
<Display>Invnsys SRH2HR cmpd ECR4 1000G</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND Hi Res</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""791"">
<Display>Invnsys SRH3HR cmpd ECR4 1000G</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND Hi Res</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""792"">
<Display>Invnsys SRH4HR cmpd ECR4 10000G</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND Hi Res</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""793"">
<Display>Invnsys SRH6HR cmpd ECR4 10000G</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND Hi Res</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""794"">
<Display>SCHLUM FIRE 4HI ARBV 6D 100GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus-FM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""795"">
<Display>SCHLUM FIRE 4LO ARBV 6D 10GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus-FM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""796"">
<Display>SCHLUM FIRE 6HI ARBV 6D 1000GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus-FM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""797"">
<Display>SCHLUM FIRE 6LO ARBV 6D 100GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus-FM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""798"">
<Display>SCHLUM FIRE 8HI ARBV 6D 1000GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus-FM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""799"">
<Display>SCHLUM FIRE 8LO ARBV 6D 100GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus-FM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""800"">
<Display>Neptune 5/8x3/4 PRO 6D 10 GAL</Display>
<Type>E</Type>
<Vendor>Neptune</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""801"">
<Display>Neptune 3/4 PRO 6D 10 GAL</Display>
<Type>E</Type>
<Vendor>Neptune</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""802"">
<Display>Neptune 1 PRO 6D 10 GAL</Display>
<Type>E</Type>
<Vendor>Neptune</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""803"">
<Display>Neptune 1 1/2 PRO 6D 100 GAL</Display>
<Type>E</Type>
<Vendor>Neptune</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""804"">
<Display>Neptune 2 PRO 6D 100 GAL</Display>
<Type>E</Type>
<Vendor>Neptune</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""806"">
<Display>Badger T160 1 1/2 PLS 6D 1000GAL</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>T160</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""807"">
<Display>Badger T200 2 PLS 6D 1000GAL</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>T200</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""808"">
<Display>Badger T450 3 PLS 6D 1000GAL</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>T450</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""809"">
<Display>Badger T1000 4 PLS 6D 1000GAL</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>T1000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""810"">
<Display>Badger T2000 6 PLS 6D 1000GAL</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>T2000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""811"">
<Display>Badger T5500 10 PLS 6D 1000GAL</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>T5500</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""812"">
<Display>Badger T3500 8 PLS 6D 1000GAL</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>T3500</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""813"">
<Display>Badger T6200 12 PLS 6D 10000GAL</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo Series T6200</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>4</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""814"">
<Display>Badger T6600 16 PLS 6D 10000GAL</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo Series T6600</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>4</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""815"">
<Display>SCHLUM 3 FIRE PRO HI3 6D 100 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>Protectus</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""816"">
<Display>SCHLUM 3 FIRE PRO LO5/8 6D 10 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM</Vendor>
<Model>Protectus</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""817"">
<Display>SCHLUM FIRE 4HI PRO6 100 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""818"">
<Display>SCHLUM FIRE 4 LO 1 PRO6 10 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""819"">
<Display>SCHLUM FIRE 6HI PRO6 1000 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""820"">
<Display>SCHLUM FIRE 6 LO 1.5 PRO6 100 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""821"">
<Display>SCHLUM FIRE 8HI PRO6 1000 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""822"">
<Display>SCHLUM FIRE 8 LO 2 PRO6 100 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""823"">
<Display>SCHLUM FIRE 10HI PRO6 1000 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""824"">
<Display>SCHLUM FIRE 10 LO 2 PRO6 100 GAL</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""826"">
<Display>Badger T10000 20 PLS 6D 10000Gal</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo Series T10000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>4</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""827"">
<Display>Badger T6200 12 PLS 6D 1000CuFt</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo Series T6200</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""828"">
<Display>Badger T6600 16 PLS 6D 1000CuFt</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo Series T6600</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""829"">
<Display>Badger T10000 20 PLS 6D 1000CuFt</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo Series T10000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""830"">
<Display>Badger T160 1 1/2 PLS 6D 100cuft</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>T160</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""831"">
<Display>Badger T200 2 PLS 6D 100cuft/10</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>T200</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""832"">
<Display>Badger T450 3 PLS 6D 100cuft/10</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>T450</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""833"">
<Display>Badger T1000 4 PLS 6D 100cuft/10</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>T1000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""834"">
<Display>Badger T2000 6 PLS 6D 100cuft/10</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>T2000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""835"">
<Display>Badger T3500 8 PLS 6D 100cuft</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>T3500</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""836"">
<Display>Badger T5500 10 PLS 6D 100cuft</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>T5500</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""837"">
<Display>Nept T10 5/8 Plsgen 6D 10 CuFt</Display>
<Type>G</Type>
<Vendor>Neptune</Vendor>
<Model>T-10</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""838"">
<Display>Nept T10 3/4 Plsgen 6D 10 CuFt</Display>
<Type>G</Type>
<Vendor>Neptune</Vendor>
<Model>T-10</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""839"">
<Display>Nept T10 1 Plsgen 6D 100 CuFt</Display>
<Type>G</Type>
<Vendor>Neptune</Vendor>
<Model>T-10</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""840"">
<Display>Nept T10 1.5 Pgen 6D 100 CuFt</Display>
<Type>G</Type>
<Vendor>Neptune</Vendor>
<Model>T-10</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""841"">
<Display>Nept T10 2 Plsgen 6D 100 CuFt</Display>
<Type>G</Type>
<Vendor>Neptune</Vendor>
<Model>T-10</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""842"">
<Display>Badger RTR M70 1 1/2PL6D100GAL</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>M-70</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""843"">
<Display>Badger RTR M120 2 PLS 6D 100GAL</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>M-120</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""844"">
<Display>Sensus 3 SRH HiResCMPD 6D 1000G</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""845"">
<Display>Invnsys 4HR turbo ECR4 1000cft</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""846"">
<Display>Invnsys 6HR turbo ECR4 1000cft</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""847"">
<Display>Invnsys 8HR turbo ECR4 1000cft</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""848"">
<Display>Invnsys 10HR turbo ECR4 1000cft</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>W-TURBO</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>0</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""849"">
<Display>Badger Mag 2 PLS 6D 1CUFT</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Magnetoflo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""850"">
<Display>Badger Mag 3 PLS 6D 1CUFT</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Magnetoflo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""851"">
<Display>Badger Mag 4 PLS 6D 1CUFT</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Magnetoflo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""852"">
<Display>Badger Mag 6 PLS 6D 1CUFT</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Magnetoflo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""853"">
<Display>Badger Mag 8 PLS 6D 1CUFT</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Magnetoflo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""854"">
<Display>Badger Mag 10 PLS 6D 1CUFT</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Magnetoflo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""855"">
<Display>Badger CpdSRS 170 RTR 2 Hi 100CFT</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series-170</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""856"">
<Display>Badger CpdSRS RTR 2 Lo 1CFt</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""857"">
<Display>Badger CpdSRS 400 RTR 3 Hi 100CFt</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series-400</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""858"">
<Display>Badger CpdSRS RTR 3 Lo 1CFt</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""859"">
<Display>Badger CpdSRS 800 RTR 4 Hi 100CFt</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series-800</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""860"">
<Display>Badger CpdSRS RTR 4 Lo 1CFt</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""861"">
<Display>Badger CpdSRS 1500 RTR 6 Hi 100CF</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series-1500</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""862"">
<Display>Badger CpdSRS RTR 6 Lo 6D 1CFt</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""863"">
<Display>Badger Cmb 3500 RTR 8 Hi 100CuFt</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Combo-3500</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""864"">
<Display>Badger Cmb M120 RTR 8 Lo 10CFt</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Combo M-120</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""865"">
<Display>Badger CpdSRS 170 RTR 2 Hi 1000G</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series-170</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""866"">
<Display>Badger CpdSRS RTR 2 Lo 10G</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""867"">
<Display>Badger CpdSRS 400 RTR 3 Hi 1000G</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series-400</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""868"">
<Display>Badger CpdSRS RTR 3 Lo 10G</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""869"">
<Display>Badger CpdSRS 800 RTR 4 Hi 1000G</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series-800</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""870"">
<Display>Badger CpdSRS RTR 4 Lo 10G</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""871"">
<Display>Badger CpdSRS 1500 RTR 6 Hi 1000G</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series-1500</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""872"">
<Display>Badger CpdSRS RTR 6 Lo 6D 10G</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Cmpnd Series</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""873"">
<Display>Badger Combo 3500 RTR 8 Hi 1000G</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Combo-3500</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""874"">
<Display>Badger Combo M120 RTR 8 Lo 100G</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>Combo M-120</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""875"">
<Display>MasterMeter 5/8 Dialog 6D 1CuFt</Display>
<Type>R</Type>
<Vendor>Master Meter</Vendor>
<Model>Dialog</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""876"">
<Display>MasterMeter 3/4 Dialog 6D 1CuFt</Display>
<Type>R</Type>
<Vendor>Master Meter</Vendor>
<Model>Dialog</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""877"">
<Display>MasterMeter 1 Dialog 6D 1CuFt</Display>
<Type>R</Type>
<Vendor>Master Meter</Vendor>
<Model>Dialog</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""878"">
<Display>MasterMeter 1 1/2 Dialog 6D 10CuFt</Display>
<Type>R</Type>
<Vendor>Master Meter</Vendor>
<Model>Dialog</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""879"">
<Display>MasterMeter 2 Dialog 6D 10CuFt</Display>
<Type>R</Type>
<Vendor>Master Meter</Vendor>
<Model>Dialog</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""880"">
<Display>Hersey MFM II Trans 3 6D 10 CuFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MFM II</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""881"">
<Display>Hersey MFM II Trans 4 6D 10 CuFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MFM II</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""882"">
<Display>Hersey MFM II Trans 6 6D 100 CuFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MFM II</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""883"">
<Display>Hersey MFM II Trans 8 6D 100 CuFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MFM II</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""884"">
<Display>Hersey MFM II Trans 10 6D 100 CuFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MFM II</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""885"">
<Display>Hersey MFM II Trans 12 6D 100 CuFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MFM II</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""886"">
<Display>Hersey MFM II Trans 3 6D 100 Gal</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MFM II</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""887"">
<Display>Hersey MFM II Trans 4 6D 100 Gal</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MFM II</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""888"">
<Display>Hersey MFM II Trans 6 6D 1000 Gal</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MFM II</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""889"">
<Display>Hersey MFM II Trans 8 6D 1000 Gal</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MFM II</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""890"">
<Display>Hersey MFM II Trans 10 6D 1000 Gal</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MFM II</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""891"">
<Display>Hersey MFM II Trans 12 6D 1000 Gal</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MFM II</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""892"">
<Display>Hersey Trans 3/4 6D 1 CuFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>400 Series IIS</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""893"">
<Display>Hersey Trans 3/4 6D 10 Gal</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>400 Series IIS</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""894"">
<Display>Hersey MCTII 2-Hi Trans 6D 100G</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MCT II Cmpnd.</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""895"">
<Display>Hersey MCTII 2-Lo Trans 6D 10G</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MCT II Cmpnd.</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""896"">
<Display>Hersey MCTII 3-Hi Trans 6D 100G</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MCT II Cmpnd.</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""897"">
<Display>Hersey MCTII 3-Lo Trans 6D 10G</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MCT II Cmpnd.</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""898"">
<Display>Hersey MCTII 4-Hi Trans 6D 1000G</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MCT II Cmpnd.</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""899"">
<Display>Hersey MCTII 4-Lo Trans 6D 10G</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MCT II Cmpnd.</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""900"">
<Display>Hersey MCTII 6-Hi Trans 6D 1000G</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MCT II Cmpnd.</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""901"">
<Display>Hersey MCTII 6-Lo Trans 6D 100G</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MCT II Cmpnd.</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""902"">
<Display>Hersey MCTII 8-Hi Trans 6D 1000G</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MCT II Cmpnd.</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""903"">
<Display>Hersey MCTII 8-Lo Trans 6D 100G</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MCT II Cmpnd.</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""904"">
<Display>Hersey MCTII 2-Hi Trans 6D 10CFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MCT II Cmpnd.</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""905"">
<Display>Hersey MCTII 2-Lo Trans 6D 1CFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MCT II Cmpnd.</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""906"">
<Display>Hersey MCTII 3-Hi Trans 6D 10CFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MCT II Cmpnd.</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""907"">
<Display>Hersey MCTII 3-Lo Trans 6D 1CFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MCT II Cmpnd.</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""908"">
<Display>Hersey MCTII 4-Hi Trans 6D 100CFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MCT II Cmpnd.</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""909"">
<Display>Hersey MCTII 4-Lo Trans 6D 1CFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MCT II Cmpnd.</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""910"">
<Display>Hersey MCTII 6-Hi Trans 6D 100CFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MCT II Cmpnd.</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""911"">
<Display>Hersey MCTII 6-Lo Trans 6D 10CFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MCT II Cmpnd.</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""912"">
<Display>Hersey MCTII 8-Hi Trans 6D 100CFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MCT II Cmpnd.</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""913"">
<Display>Hersey MCTII 8-Lo Trans 6D 10CFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MCT II Cmpnd.</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""914"">
<Display>Hersey MVR 3/4 Turbine 6D 10G</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MVR Turbine</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""915"">
<Display>Hersey MVR 1 Turbine 6D 10G</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MVR Turbine</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""916"">
<Display>Hersey MVR 1.5 Turbine 6D 100G</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MVR Turbine</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""917"">
<Display>Hersey MVR 2 Turbine 6D 100G</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MVR Turbine</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""918"">
<Display>Hersey MVR 3 Turbine 6D 100G</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MVR Turbine</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""919"">
<Display>Hersey MVR 4 Turbine 6D 100G</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MVR Turbine</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""920"">
<Display>Hersey MVR 6 Turbine 6D 1000G</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MVR Turbine</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""921"">
<Display>Hersey MVR 3/4 Turbine 6D 1CFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MVR Turbine</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""922"">
<Display>Hersey MVR 1 Turbine 6D 1CFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MVR Turbine</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""923"">
<Display>Hersey MVR 1.5 Turbine 6D 10CFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MVR Turbine</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""924"">
<Display>Hersey MVR 2 Turbine 6D 10CFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MVR Turbine</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""925"">
<Display>Hersey MVR 3 Turbine 6D 10CFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MVR Turbine</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""926"">
<Display>Hersey MVR 4 Turbine 6D 10CFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MVR Turbine</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""927"">
<Display>Hersey MVR 6 Turbine 6D 100CFt</Display>
<Type>E</Type>
<Vendor>Hersey</Vendor>
<Model>MVR Turbine</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""928"">
<Display>Neptune Tricon HPT 6 Pls 6D 1CuMtr</Display>
<Type>R</Type>
<Vendor>Neptune</Vendor>
<Model>Tricon</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""929"">
<Display>Neptune Tricon TT 6 Pls 6D 1CuMtr</Display>
<Type>R</Type>
<Vendor>Neptune</Vendor>
<Model>Tricon</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""930"">
<Display>NEPT/SCHLUM Fire 4 Hi PRO6 10Cu.Ft.</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus-FM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""931"">
<Display>NEPT/SCHLUM Fire 4 Lo 1 PRO6 1Cu.Ft.</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus-FM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""932"">
<Display>NEPT/SCHLUM Fire 6 Hi PRO6 100Cu.Ft.</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus-FM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""933"">
<Display>NEPT/SCHLUM Fire 6 Lo 1.5 PRO6 10CuFt</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus-FM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""934"">
<Display>NEPT/SCHLUM Fire 8 Hi PRO6 100Cu.Ft.</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus-FM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""935"">
<Display>NEPT/SCHLUM Fire 8 Lo 2 PRO6 10Cu.Ft.</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus-FM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""936"">
<Display>NEPT/SCHLUM Fire 10 Hi PRO6 100Cu.Ft.</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus-FM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""937"">
<Display>NEPT/SCHLUM Fire 10 Lo 2 PRO6 10Cu.Ft.</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus-FM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""938"">
<Display>SCHLUM FIRE 6HI ARBV 6D 100CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus-FM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""939"">
<Display>SCHLUM FIRE 6LO ARBV 6D 10CUFT</Display>
<Type>E</Type>
<Vendor>SCHLUM/NEPTUNE</Vendor>
<Model>Protectus-FM</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>1</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""940"">
<Display>ABB C3000 2Hi Digital 10CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""941"">
<Display>ABB C3000 2Lo Digital 1CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""942"">
<Display>ABB C3000 3Hi Digital 100CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""943"">
<Display>ABB C3000 3Lo Digital 1CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""944"">
<Display>ABB C3000 4Hi Digital 100CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""945"">
<Display>ABB C3000 4Lo Digital 1CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""946"">
<Display>ABB C3000 6Hi Digital 1000CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""947"">
<Display>ABB C3000 6Lo Digital 1CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""948"">
<Display>ABB C3000 8Hi Digital 1000CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""949"">
<Display>ABB C3000 8Lo Digital 10CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""950"">
<Display>Metron 4 Encoder 6D 1000 Gal</Display>
<Type>E</Type>
<Vendor>Metron</Vendor>
<Model>Single Jet Turbine</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""953"">
<Display>ABB T-3000 6 Scndr 100 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""954"">
<Display>ABB T-3000 8 Scndr 100 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""955"">
<Display>ABB T-3000 10 Scndr 100 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""956"">
<Display>ABB T3000 1 1/2 Digital 10cuft</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""957"">
<Display>ABB T3000 2 Digital 10cuft</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""958"">
<Display>ABB T3000 3 Digital 100cuft</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""959"">
<Display>ABB T3000 4 Digital 100cuft</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""960"">
<Display>ABB T3000 6 Digital 1000cuft</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""961"">
<Display>ABB T3000 8 Digital 1000cuft</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""962"">
<Display>ABB T3000 10 Digital 1000cuft</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""963"">
<Display>ABB T3000 12 Digital 1000cuft</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""964"">
<Display>Magnetic Flow Meter 6D 100CuFt</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>Magnetic</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""965"">
<Display>AMCO AquaMaster 5/8 6D 1CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""966"">
<Display>AMCO AquaMaster 3/4 6D 1CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""967"">
<Display>AMCO AquaMaster 1 6D 1CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""968"">
<Display>AMCO AquaMaster 1 1/2 6D 10CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""969"">
<Display>AMCO AquaMaster 2 6D 10CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""970"">
<Display>AMCO AquaMaster 2 1/2 6D 100CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""971"">
<Display>AMCO AquaMaster 3 6D 100CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""972"">
<Display>AMCO AquaMaster 4 6D 100CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""973"">
<Display>AMCO AquaMaster 6 6D 100CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""974"">
<Display>AMCO AquaMaster 8 6D 100CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""975"">
<Display>AMCO AquaMaster 10 6D 100CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""976"">
<Display>AMCO AquaMaster 12 6D 100CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""977"">
<Display>AMCO AquaMaster 14 6D 100CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""978"">
<Display>AMCO AquaMaster 16 6D 100CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""979"">
<Display>AMCO AquaMaster 18 6D 100CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""980"">
<Display>AMCO AquaMaster 20 6D 100CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""981"">
<Display>AMCO AquaMaster 24 6D 100CF</Display>
<Type>R</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""982"">
<Display>5/8 Generic PlsGen 6D 100Gal</Display>
<Type>G</Type>
<Vendor>Generic</Vendor>
<Model>Generic</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""983"">
<Display>5/8 Generic PlsGen 6D 1000Gal</Display>
<Type>G</Type>
<Vendor>Generic</Vendor>
<Model>Generic</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""984"">
<Display>3/4 Generic PlsGen 6D 100Gal</Display>
<Type>G</Type>
<Vendor>Generic</Vendor>
<Model>Generic</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""985"">
<Display>3/4 Generic PlsGen 6D 1000Gal</Display>
<Type>G</Type>
<Vendor>Generic</Vendor>
<Model>Generic</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""986"">
<Display>1 Generic PlsGen 6D 100Gal</Display>
<Type>G</Type>
<Vendor>Generic</Vendor>
<Model>Generic</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""987"">
<Display>1 Generic PlsGen 6D 1000Gal</Display>
<Type>G</Type>
<Vendor>Generic</Vendor>
<Model>Generic</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""988"">
<Display>1 1/2 Generic PlsGen 6D 100Gal</Display>
<Type>G</Type>
<Vendor>Generic</Vendor>
<Model>Generic</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""989"">
<Display>1 1/2 Generic PlsGen 6D 1000Gal</Display>
<Type>G</Type>
<Vendor>Generic</Vendor>
<Model>Generic</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""990"">
<Display>2 Generic PlsGen 6D 100 Gal</Display>
<Type>G</Type>
<Vendor>Generic</Vendor>
<Model>Generic</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""991"">
<Display>2 Generic PlsGen 6D 1000Gal</Display>
<Type>G</Type>
<Vendor>Generic</Vendor>
<Model>Generic</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""992"">
<Display>3 Generic PlsGen 6D 100Gal</Display>
<Type>G</Type>
<Vendor>Generic</Vendor>
<Model>Generic</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""993"">
<Display>3 Generic PlsGen 6D 1000Gal</Display>
<Type>G</Type>
<Vendor>Generic</Vendor>
<Model>Generic</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""994"">
<Display>4 Generic PlsGen 6D 100Gal</Display>
<Type>G</Type>
<Vendor>Generic</Vendor>
<Model>Generic</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""995"">
<Display>4 Generic PlsGen 6D 1000Gal</Display>
<Type>G</Type>
<Vendor>Generic</Vendor>
<Model>Generic</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""996"">
<Display>6 Generic PlsGen 6D 100Gal</Display>
<Type>G</Type>
<Vendor>Generic</Vendor>
<Model>Generic</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""997"">
<Display>6 Generic PlsGen 6D 1000Gal</Display>
<Type>G</Type>
<Vendor>Generic</Vendor>
<Model>Generic</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""998"">
<Display>Elster AquaMaster 2 6D 0.01CuMtr</Display>
<Type>R</Type>
<Vendor>Elster</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>2</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""999"">
<Display>Elster AquaMaster 4 6D 0.1CuMtr</Display>
<Type>R</Type>
<Vendor>Elster</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1000"">
<Display>Elster AquaMaster 6 6D 1CuMtr</Display>
<Type>R</Type>
<Vendor>Elster</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1001"">
<Display>Elster AquaMaster 8 6D 1CuMtr</Display>
<Type>R</Type>
<Vendor>Elster</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1002"">
<Display>Elster AquaMaster 10 6D 1CuMtr</Display>
<Type>R</Type>
<Vendor>Elster</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1003"">
<Display>Elster AquaMaster 3-4 6D 0.01CuMtr</Display>
<Type>R</Type>
<Vendor>Elster</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>2</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1004"">
<Display>Elster AquaMaster 6-8 6D 0.1CuMtr</Display>
<Type>R</Type>
<Vendor>Elster</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1005"">
<Display>Sensus ICE 10 SRH Cmpnd 6D 1000CuFt</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRH-CMPND</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>3</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1006"">
<Display>Elster AquaMaster 3 6D 1CuMtr</Display>
<Type>R</Type>
<Vendor>Elster</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1007"">
<Display>Elster AquaMaster 4 6D 1CuMtr</Display>
<Type>R</Type>
<Vendor>Elster</Vendor>
<Model>AquaMaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1008"">
<Display>Gas Generic 5D PHF 10</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1009"">
<Display>Gas Generic 5D PHF 20</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>9</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1010"">
<Display>Gas Generic 5D PHF 100</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1011"">
<Display>Gas Generic 6D PHF 10</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1012"">
<Display>Gas Generic 6D PHF 100</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1013"">
<Display>Gas Generic 5D PHF 5</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>2</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1051"">
<Display>Landis Gyr AXS4 8/9S 1.8Kh</Display>
<Type IsTablesType=""1"">75</Type>
<Vendor>Landis and Gyr</Vendor>
<Model>S4</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
</Meter>
<Meter ID=""1052"">
<Display>Landis Gyr Focus 2S 7.2Kh</Display>
<Type IsTablesType=""1"">74</Type>
<Vendor>Landis and Gyr</Vendor>
<Model>Focus</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
</Meter>
<Meter ID=""1053"">
<Display>Landis Gyr Focus 3S 0.6Kh</Display>
<Type IsTablesType=""1"">74</Type>
<Vendor>Landis and Gyr</Vendor>
<Model>Focus</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
</Meter>
<Meter ID=""1054"">
<Display>Landis Gyr Focus 4S 0.6Kh</Display>
<Type IsTablesType=""1"">74</Type>
<Vendor>Landis and Gyr</Vendor>
<Model>Focus</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
</Meter>
<Meter ID=""1055"">
<Display>Landis Gyr Focus 12S 14.4Kh</Display>
<Type IsTablesType=""1"">74</Type>
<Vendor>Landis and Gyr</Vendor>
<Model>Focus</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
</Meter>
<Meter ID=""1056"">
<Display>Landis Gyr AXS4 12S 2.16Kh</Display>
<Type IsTablesType=""1"">75</Type>
<Vendor>Landis and Gyr</Vendor>
<Model>S4</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
</Meter>
<Meter ID=""1057"">
<Display>Landis Gyr AXS4 15S/16S 1.8Kh</Display>
<Type IsTablesType=""1"">75</Type>
<Vendor>Landis and Gyr</Vendor>
<Model>S4</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
</Meter>
<Meter ID=""1058"">
<Display>Landis Gyr Focus 1S 1.8Kh</Display>
<Type IsTablesType=""1"">74</Type>
<Vendor>Landis and Gyr</Vendor>
<Model>Focus</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
</Meter>
<Meter ID=""1059"">
<Display>Landis Gyr Focus 2SE 12Kh</Display>
<Type IsTablesType=""1"">74</Type>
<Vendor>Landis and Gyr</Vendor>
<Model>Focus</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
</Meter>
<Meter ID=""1060"">
<Display>Landis Gyr Focus 3S 0.3Kh</Display>
<Type IsTablesType=""1"">74</Type>
<Vendor>Landis and Gyr</Vendor>
<Model>Focus</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
</Meter>
<Meter ID=""1061"">
<Display>Landis Gyr Focus 25S 14.4Kh</Display>
<Type IsTablesType=""1"">74</Type>
<Vendor>Landis and Gyr</Vendor>
<Model>Focus</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
</Meter>
<Meter ID=""1062"">
<Display>Landis Gyr AXS4 5S 1.2Kh</Display>
<Type IsTablesType=""1"">75</Type>
<Vendor>Landis and Gyr</Vendor>
<Model>S4</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
</Meter>
<Meter ID=""1063"">
<Display>Landis Gyr AXS4 6S 1.8Kh</Display>
<Type IsTablesType=""1"">75</Type>
<Vendor>Landis and Gyr</Vendor>
<Model>S4</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
</Meter>
<Meter ID=""1064"">
<Display>Landis Gyr AXS4 12S 14.4Kh</Display>
<Type IsTablesType=""1"">75</Type>
<Vendor>Landis and Gyr</Vendor>
<Model>S4</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
</Meter>
<Meter ID=""1065"">
<Display>Landis Gyr AXS4 14/15/16S 21.6Kh</Display>
<Type IsTablesType=""1"">75</Type>
<Vendor>Landis and Gyr</Vendor>
<Model>S4</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
</Meter>
<Meter ID=""1066"">
<Display>Landis Gyr AXS4 36S 1.8Kh</Display>
<Type IsTablesType=""1"">75</Type>
<Vendor>Landis and Gyr</Vendor>
<Model>S4</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
</Meter>
<Meter ID=""1067"">
<Display>Landis Gyr Focus AXR-SD 2S 7.2Kh</Display>
<Type IsTablesType=""1"">74</Type>
<Vendor>Landis and Gyr</Vendor>
<Model>Focus</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
</Meter>
<Meter ID=""1070"">
<Display>NEPT T10 5/8 E-Coder 0.001CuMtr</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>4</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1071"">
<Display>NEPT T10 5/8x3/4 E-Coder 0.001CuMtr</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>4</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1072"">
<Display>NEPT T10 3/4 E-Coder 0.001CuMtr</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>4</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1073"">
<Display>NEPT T10 1 E-Coder 0.001CuMtr</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>4</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1074"">
<Display>NEPT T10 1-1/2 E-Coder 0.001CuMtr</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>4</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1075"">
<Display>NEPT T10 2 E-Coder 0.01CuMtr</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>3</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1076"">
<Display>NEPT TURB 3 E-Coder 0.01CuMtr</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>TURBINE</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>3</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1077"">
<Display>NEPT TURB 4 E-Coder 0.01CuMtr</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>TURBINE</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>3</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1078"">
<Display>NEPT TURB 6 E-Coder 0.1CuMtr</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>TURBINE</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1079"">
<Display>NEPT TURB 8 E-Coder 0.1CuMtr</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>TURBINE</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1080"">
<Display>NEPT TURB 10 E-Coder 0.1CuMtr</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>TURBINE</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1081"">
<Display>NEPT T-10 5/8 PRO 6D 0.1CuMtr</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1082"">
<Display>NEPT T-10 5/8x3/4 PRO 6D 0.1CuMtr</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1083"">
<Display>NEPT T-10 3/4 PRO 6D 0.1CuMtr</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1084"">
<Display>NEPT T-10 1 PRO 6D 0.1CuMtr</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1085"">
<Display>NEPT T-10 1-1/2 PRO 6D 0.1CuMtr</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1086"">
<Display>NEPT T-10 2 PRO 6D 1CuMtr</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1087"">
<Display>NEPT TURB 3 PRO 6D 1CuMtr</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>TURBINE</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1088"">
<Display>NEPT TURB 4 PRO 6D 1CuMtr</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>TURBINE</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1089"">
<Display>NEPT Turb 6 PRO 6D 10CuMtr</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>TURBINE</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1090"">
<Display>NEPT Turb 8 PRO 6D 10CuMtr</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>TURBINE</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1091"">
<Display>NEPT Turb 10 PRO 6D 10CuMtr</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>TURBINE</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1092"">
<Display>NEPT T10 5/8 E-Coder 0.1Gals</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>2</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1093"">
<Display>NEPT T10 5/8x3/4 E-Coder 0.1Gals</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>2</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1094"">
<Display>NEPT T10 3/4 E-Coder 0.1Gals</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>2</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1095"">
<Display>NEPT T10 1 E-Coder 0.1Gals</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>2</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1096"">
<Display>NEPT T10 1-1/2 E-Coder 1Gals</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1097"">
<Display>NEPT T10 2 E-Coder 1Gals</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1098"">
<Display>NEPT T10 3 E-Coder 1Gals</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1099"">
<Display>NEPT T10 4 E-Coder 1Gals</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1100"">
<Display>NEPT T10 6 E-Coder 10Gals</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1101"">
<Display>NEPT T10 8 E-Coder 10Gals</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1102"">
<Display>NEPT T10 10 E-Coder 10Gals</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1110"">
<Display>NEPT T10 5/8 E-Coder 0.01CuFt</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>3</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1111"">
<Display>NEPT T10 5/8x3/4 E-Coder 0.01CuFt</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>3</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1112"">
<Display>NEPT T10 3/4 E-Coder 0.01CuFt</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>3</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1113"">
<Display>NEPT T10 1 E-Coder 0.01CuFt</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>3</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1114"">
<Display>NEPT T10 1-1/2 E-Coder 0.1CuFt</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>2</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1115"">
<Display>NEPT T10 2 E-Coder 0.1CuFt</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>2</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1116"">
<Display>NEPT T10 3 E-Coder 0.1CuFt</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>2</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1117"">
<Display>NEPT T10 4 E-Coder 0.1CuFt</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>2</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1118"">
<Display>NEPT T10 6 E-Coder 1CuFt</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1119"">
<Display>NEPT T10 8 E-Coder 1CuFt</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1120"">
<Display>NEPT T10 10 E-Coder 1CuFt</Display>
<Type>E</Type>
<Vendor>NEPTUNE</Vendor>
<Model>T-10</Model>
<LiveDigits>8</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<EncoderType>2</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1130"">
<Display>SeaMetrics Digital 6D 100Gal</Display>
<Type>R</Type>
<Vendor>SeaMetrics</Vendor>
<Model>Digital</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1131"">
<Display>SeaMetrics Digital 6D 1000Gal</Display>
<Type>R</Type>
<Vendor>SeaMetrics</Vendor>
<Model>Digital</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1132"">
<Display>Badger TURBO RTR 1-1/2 6D1000Gal</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1133"">
<Display>Badger TURBO RTR 3 6D1000Gal</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1134"">
<Display>Badger TURBO RTR 4 6D1000Gal</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1135"">
<Display>Badger TURBO RTR 6 6D1000Gal</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1136"">
<Display>Badger TURBO RTR 8 6D1000Gal</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Turbo RTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1140"">
<Display>ONICON SYS-30 1000BTU</Display>
<Type>R</Type>
<Vendor>ONICON</Vendor>
<Model>Energy Meter</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1141"">
<Display>PGE 3D Int PHF 2 </Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>3</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>4</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1142"">
<Display>PGE 3D Int PHF 10</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>3</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1143"">
<Display>PGE 4D Int PHF 1</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>0</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1144"">
<Display>PGE 4D Int PHF 2</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>4</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1145"">
<Display>PGE 4D Int PHF 5</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>2</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1146"">
<Display>PGE 4D Int PHF 10</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1147"">
<Display>PGE 5D Int PHF 5</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>2</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1148"">
<Display>PGE 5D Int PHF 10</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1149"">
<Display>PGE 5D Int PHF 20</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>9</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1150"">
<Display>PGE 5D Int PHF 100</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1151"">
<Display>PGE 6D Int PHF 10</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1152"">
<Display>PGE 6D Int PHF 100</Display>
<Type>M</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>0</ExternalTamper>
<InternalTamper>4</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1153"">
<Display>PGE 3D Ext PHF 2 </Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>3</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>4</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1154"">
<Display>PGE 3D Ext PHF 10</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>3</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1155"">
<Display>PGE 4D Ext PHF 1</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>0</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1156"">
<Display>PGE 4D Ext PHF 2</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>4</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1157"">
<Display>PGE 4D Ext PHF 5</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>2</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1158"">
<Display>PGE 4D Ext PHF 10</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>4</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1159"">
<Display>PGE 5D Ext PHF 5</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>2</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1160"">
<Display>PGE 5D Ext PHF 10</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1161"">
<Display>PGE 5D Ext PHF 20</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>9</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1162"">
<Display>PGE 5D Ext PHF 100</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1163"">
<Display>PGE 6D Ext PHF 10</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1164"">
<Display>PGE 6D Ext PHF 100</Display>
<Type>R</Type>
<Vendor>GENERIC</Vendor>
<Model>GENERIC</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>1</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1165"">
<Display>ABB C3000 6-HiFlo Scndr 1000CuFt</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1166"">
<Display>Badger M70 1.25 Pgen 6D 10 CuFt</Display>
<Type>G</Type>
<Vendor>BADGER</Vendor>
<Model>M-70</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>1</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>4</WdtPrescalerFollowingEdge>
<MinimumPulseLength>16</MinimumPulseLength>
<EdgePolarity>128</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1167"">
<Display>Amco MagMstr 4 PLS 6D 100CuFt</Display>
<Type>R</Type>
<Vendor>AMCO</Vendor>
<Model>MagMstr</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1168"">
<Display>Amco MagMstr 6 PLS 6D 100CuFt</Display>
<Type>R</Type>
<Vendor>AMCO</Vendor>
<Model>MagMstr</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1169"">
<Display>Sensus TTR 1.5 PLS 6D 100CuFt</Display>
<Type>R</Type>
<Vendor>Sensus\Invensys</Vendor>
<Model>TTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1170"">
<Display>Sensus TTR 1.5 PPG 6D 100CuFt</Display>
<Type>R</Type>
<Vendor>Sensus\Invensys</Vendor>
<Model>TTR</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1171"">
<Display>Badger Mag 12 PLS 6D 100CUFT</Display>
<Type>R</Type>
<Vendor>BADGER</Vendor>
<Model>Magnetoflo</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1172"">
<Display>ABB C3000 3-HiFlo Scndr 100Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1173"">
<Display>ABB C3000 4-HiFlo Scndr 100Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1174"">
<Display>ABB C3000 6-HiFlo Scndr 100 Gal</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>C-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1175"">
<Display>ABB T-3000 3 Scancoder 100 Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1176"">
<Display>ABB T-3000 4 Scancoder 100 Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1177"">
<Display>ABB T-3000 6 Scancoder 100 Gal.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>T-3000</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1178"">
<Display>Badger ADE M55 1 6D 10Gal</Display>
<Type>E</Type>
<Vendor>Badger</Vendor>
<Model>M-55</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1179"">
<Display>SeaMetrics Digital 5D 100Gal</Display>
<Type>R</Type>
<Vendor>SeaMetrics</Vendor>
<Model>Digital</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1180"">
<Display>SeaMetrics Digital 6D 100CuFt</Display>
<Type>R</Type>
<Vendor>SeaMetrics</Vendor>
<Model>Digital</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1181"">
<Display>SeaMetrics Digital 6D 10CuFt</Display>
<Type>R</Type>
<Vendor>SeaMetrics</Vendor>
<Model>Digital</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1182"">
<Display>ABB 3 Invision 10 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>Invision</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1183"">
<Display>ABB 4 Invision 10 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>Invision</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1184"">
<Display>ABB 6 Invision 100 CuFt</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>Invision</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1185"">
<Display>ABB 8 Invision 100 CuFt</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>Invision</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1186"">
<Display>ABB 10 Invision 100 CuFt</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>Invision</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1187"">
<Display>ABB 3 Invision Comp 10 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>Invision</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1188"">
<Display>ABB 4 Invision Comp 10 Cu.Ft.</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>Invision</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1189"">
<Display>ABB 6 Invision Comp 100 CuFt</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>Invision</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1190"">
<Display>ABB 8 Invision Comp 100 CuFt</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>Invision</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1191"">
<Display>ABB 10 Invision Comp 100 CuFt</Display>
<Type>E</Type>
<Vendor>AMCO/ABB</Vendor>
<Model>Invision</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1192"">
<Display>Aquamaster Encoder 1 CuMtr</Display>
<Type>E</Type>
<Vendor>Elster</Vendor>
<Model>Aquamaster</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1193"">
<Display>ONICON BTU 5D</Display>
<Type>R</Type>
<Vendor>ONICON</Vendor>
<Model>Energy Meter</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1194"">
<Display>Amco T3000 2 Digital 10 Gal</Display>
<Type>R</Type>
<Vendor>AMCO</Vendor>
<Model>Pulse</Model>
<LiveDigits>5</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>2</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1195"">
<Display>Amco T3000 6 Digital 100 Gal</Display>
<Type>R</Type>
<Vendor>AMCO</Vendor>
<Model>Pulse</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1196"">
<Display>Amco C700 Lo Digital 1 Gal</Display>
<Type>R</Type>
<Vendor>AMCO</Vendor>
<Model>Pulse</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1197"">
<Display>Amco C3000 Hi Digital 100 Gal</Display>
<Type>R</Type>
<Vendor>AMCO</Vendor>
<Model>Pulse</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>3</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>6</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1198"">
<Display>Badger HRT M25 6Digit 0.1Cu. Mtr.</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>M25</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1199"">
<Display>Badger HRT M40 6Digit 0.1Cu. Mtr.</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>M40</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1200"">
<Display>Badger HRT M70 6Digit 0.1Cu. Mtr.</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>M70</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1201"">
<Display>Badger HRT M120 6Digit 1Cu.Mtr.</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>M120</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1202"">
<Display>Badger RTR M25 6Digit 0.1Cu. Mtr.</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>M25</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1203"">
<Display>Badger RTR M40 6Digit 0.1Cu. Mtr.</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>M40</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1204"">
<Display>Badger RTR M70 6Digit 0.1Cu. Mtr.</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>M70</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1205"">
<Display>Badger RTR M120 6Digit 1Cu.Mtr.</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>M120</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1206"">
<Display>Badger HRT M170 6Digit 10Cu.Mtr.</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>M170</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1207"">
<Display>Badger RTR M170 6Digit 10Cu.Mtr.</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>M170</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1208"">
<Display>Badger Rad-O-Matic M170 6Digit 10Cu.Mtr.</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>M170</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1209"">
<Display>Badger Read-O-Matic M25 6Digit 0.1Cu.Mtr.</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>M25</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1210"">
<Display>Badger Read-O-Matic M40 6Digit 0.1Cu.Mtr.</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>M40</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>1</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1211"">
<Display>Badger Read-O-Matic M120 6Digit1Cu.Mtr.</Display>
<Type>R</Type>
<Vendor>Badger</Vendor>
<Model>M120</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1212"">
<Display>INVENSYS SRII 1.5 ICE 6D 10CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1213"">
<Display>INVENSYS SRII 2 ICE 6D 10CUFT</Display>
<Type>E</Type>
<Vendor>Sensus/Invensys</Vendor>
<Model>SRII</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>8</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>0</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
<Meter ID=""1214"">
<Display>Smart Meter 6D 3/4 1 Gal</Display>
<Type>R</Type>
<Vendor>Trent</Vendor>
<Model>Smart Meter</Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>0</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<Prescaler>0</Prescaler>
<ImmediateAlarmTransmit>0</ImmediateAlarmTransmit>
<DcuUrgentAlarm>0</DcuUrgentAlarm>
<ExternalTamper>8</ExternalTamper>
<InternalTamper>0</InternalTamper>
<ProvingHandFactor>5</ProvingHandFactor>
<WdtPrescalerFollowingEdge>1</WdtPrescalerFollowingEdge>
<MinimumPulseLength>8</MinimumPulseLength>
<EdgePolarity>0</EdgePolarity>
<ReadingType>0</ReadingType>
</Meter>
<Meter ID=""1215"">
<Display>Flostar 6D 3 inch 10CuFt</Display>
<Type>E</Type>
<Vendor>Actaris</Vendor>
<Model>Flostar </Model>
<LiveDigits>6</LiveDigits>
<DummyDigits>0</DummyDigits>
<PaintedDigits>1</PaintedDigits>
<LeadingDummy>0</LeadingDummy>
<Scale>0</Scale>
<EncoderType>4</EncoderType>
<EvenParity>0</EvenParity>
<Reading6Digit>2</Reading6Digit>
<AsynchData>4</AsynchData>
<ResetCmd>0</ResetCmd>
<ReadingType>4</ReadingType>
</Meter>
</MeterTypes>";


                return value;
            }
        }

}
