using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kibitka.box
{
    public class Client
    {
        public static readonly IntPtr StaticClientConnection = new IntPtr(0x00C79CE0);

        public const int ObjectManagerOffset = 0x2ED0;
        public const int FirstObjectOffset = 0xAC;
        public const int LocalGuidOffset = 0xC0;
        public const int NextObjectOffset = 0x3C;

        public const int HasConnectedOffset = 0x534;

        public static readonly IntPtr MouseOverGuid = new IntPtr(0x00BD0798);

        public static readonly IntPtr StaticLocalPlayerGUID = new IntPtr(0x00BD07A8);
        public static readonly IntPtr StaticLocalTargetGUID = new IntPtr(0x00BD07B0);
        public static readonly IntPtr StaticLastTargetGUID = new IntPtr(0x00BD07B0);

        public static readonly IntPtr StaticCurrentZoneID = new IntPtr(0x00BCEFF0);
        public static readonly IntPtr StaticInWorld = new IntPtr(0x00BD0792);

        public class Other
        {
            public static readonly IntPtr StaticKilledMobsPointer = new IntPtr(0x0CCD4D34);
            public const int KilledMobsOffset1 = 0x22C;
            public const int KilledMobsOffset2 = 0x2E0;

            public static readonly IntPtr StaticLoginString = new IntPtr(0x00C79620);
            public static readonly IntPtr StaticServerNameString = new IntPtr(0x00C79B9E);
        }
    }

    public class Name
    {
        public static readonly IntPtr NameStore = new IntPtr(0x00C5D938 + 0x8);
        public const int NameMask = 0x24;
        public const int NameBase = 0x1C;
        public const int NameString = 0x20;

        public const int MobName = 0x964;
        public const int MobNameEx = 0x05C;
    }

    public class Object
    {
        public const int Type = 0x14;

        public const int Rot = 0x7A8;
        public const int Guid = 0x30;
        public const int UnitFields = 0x8;

        public const int ID = 0xC;
        public const int Pos_X = 0xE8;
        public const int Pos_Y = 0xEC;
        public const int Pos_Z = 0xF0;
    }

    public class Unit
    {
        public const int UnitFields = 0x8;

        public const int Pos_X = 0x798;
        public const int Pos_Y = 0x79C;
        public const int Pos_Z = 0x7A0;

        public const int Level = 0x36 * 4;
        public const int Health = 0x18 * 4;
        public const int Energy = 0x19 * 4;
        public const int MaxHealth = 0x20 * 4;
        public const int SummonedBy = 0xE * 4;
        public const int MaxEnergy = 0x21 * 4;

        public const int NpcID = 0x0C;

        public const int Race = 92;
        public const int Class = 93;
        public const int Gender = 94;

        public const int CurrentXP = 0x9E8;
        public const int MaxXP = 0x9EC;
    }

    public class Quest
    {
        public static readonly IntPtr[] StaticQuests = new IntPtr[25]
        {
            new IntPtr(0x00C23680 + 0),    // Quest1
            new IntPtr(0x00C23680 + 12),   // Quest2
            new IntPtr(0x00C23680 + 24),   // Quest3
            new IntPtr(0x00C23680 + 36),   // Quest4
            new IntPtr(0x00C23680 + 48),   // Quest5
            new IntPtr(0x00C23680 + 60),   // Quest6
            new IntPtr(0x00C23680 + 72),   // Quest7
            new IntPtr(0x00C23680 + 84),   // Quest8
            new IntPtr(0x00C23680 + 96),   // Quest9
            new IntPtr(0x00C23680 + 108),  // Quest10
            new IntPtr(0x00C23680 + 120),  // Quest11
            new IntPtr(0x00C23680 + 132),  // Quest12
            new IntPtr(0x00C23680 + 144),  // Quest13
            new IntPtr(0x00C23680 + 156),  // Quest14
            new IntPtr(0x00C23680 + 168),  // Quest15
            new IntPtr(0x00C23680 + 180),  // Quest16
            new IntPtr(0x00C23680 + 192),  // Quest17
            new IntPtr(0x00C23680 + 204),  // Quest18
            new IntPtr(0x00C23680 + 216),  // Quest19
            new IntPtr(0x00C23680 + 228),  // Quest20
            new IntPtr(0x00C23680 + 240),  // Quest21
            new IntPtr(0x00C23680 + 252),  // Quest22
            new IntPtr(0x00C23680 + 264),  // Quest23
            new IntPtr(0x00C23680 + 276),  // Quest24
            new IntPtr(0x00C23680 + 288)   // Quest25
        };
    }
}