using kibitka.box;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kibitka.box
{
 
    public class Process_Stuff
    {
        Memory Memory = new Memory();
        public IntPtr ClientConnection { get; set; }
        public IntPtr ObjectManager { get; set; }
        public IntPtr FirstObject { get; set; }
        public bool InWorld()
        {
            try
            {
                return Memory.ReadInt32(Client.StaticInWorld) == 1;
            }
            catch
            {
                return false;
            }
        }


        public bool HasConnected()
        {
            try
            {
                return Memory.ReadByte(ClientConnection + box.Client.HasConnectedOffset) == 5;
            }
            catch
            {
                return false;
            }
        }

    }
}
