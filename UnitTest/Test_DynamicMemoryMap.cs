using Xunit;
using MTUComm.MemoryMap;
using System;
using System.Linq;
using System.Linq.Expressions;

// http://blog.benhall.me.uk/2008/01/introduction-to-xunit
// https://www.devexpress.com/Support/Center/Question/Details/T562649/test-runner-does-not-run-xunit-2-2-unit-tests-in-net-standard-2-0-project
namespace UnitTest.Tests
{
    public class Test_DynamicMemoryMap
    {
        private const string ERROR  = "ERROR: ";
        private const string FOLDER = @"\Aclara_Test_Files\";

        private const string ERROR_MMAP     = ERROR + "Dynamic mapping of memory map from XML fails";
        private const string ERROR_READONLY = ERROR + "Readonly protection not works as expected";

        private bool TestExpression ( Func<dynamic> func )
        {
            try
            {
                func.Invoke ();
            }
            catch ( Exception e )
            {
                return false;
            }
            return true;
        }

        [Theory]
        [InlineData("family_31xx32xx_test1")]
        //[InlineData("family_31xx32xx_test2")]
        //[InlineData("family_31xx32xx_test3")]
        public void Test_GenerateMemoryMapFromXml ( string xmlName )
        {
            string path = Environment.GetFolderPath ( Environment.SpecialFolder.Desktop );

            byte[] memory = new byte[400];

            // Dynamic memory map generation
            dynamic map = null;
            try { map = new MemoryMap ( memory, xmlName, path + FOLDER ); }
            catch ( Exception e ) { }
            Assert.True ( map != null, ERROR_MMAP );

            // Readonly register
            Assert.False ( this.TestExpression ( () => { return map.MtuType == 123; } ), ERROR_READONLY );

            // Custom operations
            //Assert.True (  );



            // TEST: Diferentes opciones campo custom ( metodo y operacion matematica )
            //Console.WriteLine ( "Test operation register: " + base.registers.BatteryVoltage );
            //Console.WriteLine ( "Test custom format: " + base.registers.DailyRead );

            // TEST: Separacion entre Value.get y funGetCustom
            //dynamic mInt = this.GetProperty_Int ( "DailyRead" );
            //Console.WriteLine ( base.registers.DailyRead + " == " + mInt.Value );
            //mInt.Value = 123;
            //Console.WriteLine ( base.registers.DailyRead + " == " + mInt.Value );

            // TEST: Recuperar registros modificados
            //this.SetRegisterModified ( "MtuType"   );
            //this.SetRegisterModified ( "Shipbit"   );
            //this.SetRegisterModified ( "DailyRead" );
            //MemoryRegisterDictionary regs = this.GetModifiedRegisters ();

            // TEST: Recuperar objetos registro
            //dynamic             reg1 = this.GetProperty      ( "MtuType" );
            //MemoryRegister<int> reg2 = this.GetProperty<int> ( "MtuType" );
            //MemoryRegister<int> reg3 = this.GetProperty_Int  ( "MtuType" );
            //Console.WriteLine ( "Registro MtuType: " +
            //    reg1.Value + " " + reg2.Value + " " + reg3.Value );

            // TEST: Trabajar con overloads
            //Console.WriteLine ( "Test metodo overload: "       + base.registers.Overload_Method );
            //Console.WriteLine ( "Test metodo reuse overload: " + base.registers.Overload_Method_Reuse );
            //Console.WriteLine ( "Test metodo array overload: " + base.registers.Overload_Method_Array );
            //Console.WriteLine ( "Test operation overload: "    + base.registers.Overload_Operation );
        }
    }
}
