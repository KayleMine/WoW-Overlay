using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kibitka.box
{
    public class WowObject
    {
        public ulong Guid = 0;
        public ulong SummonedBy = 0;
        public float XPos = 0;
        public float YPos = 0;
        public float ZPos = 0;
        public float Rotation = 0;
        public IntPtr BaseAddress = IntPtr.Zero;
        public IntPtr UnitFieldsAddress = IntPtr.Zero;
        public short Type = 0;
        public String Name = "";

        public uint CurrentHealth = 0;
        public uint MaxHealth = 0;
        public uint CurrentEnergy = 0; // mana, rage or energy
        public uint MaxEnergy = 0;
        public uint Level = 0;
    }
}
