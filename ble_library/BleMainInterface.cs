using System;
namespace ble_library
{
    public interface InterfazPadre {  
        void ONE();   
    }  
   
    public interface InterfazHijo: InterfazPadre {  
        void TWO();  
    }  

    public interface InterfazHija: InterfazPadre {  
        void THREE();  
    }  

    public interface InterfazFamilia: InterfazHijo, InterfazPadre, InterfazHija {
        
    }  

    public interface InterfazInvitado 
    {
        void FOUR();
    }


    public class BleMainInterface: InterfazFamilia, InterfazInvitado
    {  
        public void ONE() 
        {  
            Console.WriteLine("This is ONE");  
        }  
        public void TWO() {  
            Console.WriteLine("This is TWO");  
        }  
        public void THREE() {  
            Console.WriteLine("This is THERE");  
        }
        public void FOUR()
        {
            Console.WriteLine("This is FOUR");
        }

    }  


}
