using kibitka.box;
using kibitka.box.DB;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using WinPoint = System.Drawing.Point;
using Object = kibitka.box.Object;
using System.Windows.Forms;

namespace kibitka
{
    public class Scanner
    {
        private readonly Memory _memory;
        private readonly Process_Stuff _stct;
        private readonly WoWCamera _camera;
        private readonly DB _db;
        private readonly PlayerObject _myPlayer;
        private readonly Dictionary<ulong, PlayerData> _playerCache = new Dictionary<ulong, PlayerData>();
        private Form1 _form;
        private bool IsOreBoxChecked;
        private bool IsHerbBoxChecked;
        private bool IsContainerBoxChecked;
        private bool IsRareBoxChecked;

        public Scanner(Memory memory, Process_Stuff stct, WoWCamera camera, DB db, PlayerObject myPlayer, Form1 form)
        {
            _memory = memory;
            _stct = stct;
            _camera = camera;
            _db = db;
            _myPlayer = myPlayer;
            _form = form;
        }

        public List<PlayerData> Update()
        {
            var visiblePlayers = new List<PlayerData>();

            try
            {
                DeactivateCachedPlayers();
                ReadMyPlayer();

                Vector3 cameraPos = _camera.GetCameraPosition();
                Vector3 cameraForward = _camera.GetCameraForward();
                cameraForward.Normalize();
                 IsOreBoxChecked = _form.IsOreBoxChecked;
                 IsHerbBoxChecked = _form.IsHerbBoxChecked;
                 IsContainerBoxChecked = _form.IsContainerBoxChecked;
                 IsRareBoxChecked = _form.IsRareBoxChecked;

                IntPtr baseAddress = _memory.ReadPointer(_stct.ObjectManager + Client.FirstObjectOffset);
                while (baseAddress != IntPtr.Zero)
                {
                    ulong objectGuid = _memory.ReadUInt64(baseAddress + Object.Guid);
                    short type = (short)_memory.ReadUInt32(baseAddress + Object.Type);
                    IntPtr unitFields = _memory.ReadPointer(baseAddress + Unit.UnitFields);

                    float xPos = _memory.ReadFloat(baseAddress + Object.Pos_X);
                    float yPos = _memory.ReadFloat(baseAddress + Object.Pos_Y);
                    float zPos = _memory.ReadFloat(baseAddress + Object.Pos_Z);

                    int objectId = _memory.ReadInt32(unitFields + Object.ID);
                    string NpcName = null;
                    if (type == 3)
                    {    
                         unitFields = _memory.ReadPointer(baseAddress + Unit.UnitFields);
                         xPos = _memory.ReadFloat(baseAddress + Unit.Pos_X);
                         yPos = _memory.ReadFloat(baseAddress + Unit.Pos_Y);
                         zPos = _memory.ReadFloat(baseAddress + Unit.Pos_Z);
                         objectId = _memory.ReadInt32(unitFields + Unit.NpcID);
                         IntPtr a = (IntPtr)_memory.ReadInt32(baseAddress + Name.MobName);
                         IntPtr b = (IntPtr)_memory.ReadInt32(a + Name.MobNameEx);
                         NpcName = _memory.ReadUTF8String(b, 0x128);
                    } 

                    if (IsInterestingObject(type, objectId))
                    {
                        System.Numerics.Vector2 screenPos = _camera.GetObjectScreenPosition(xPos, yPos, zPos);
                        var screenPoint = new WinPoint((int)screenPos.X, (int)screenPos.Y);

                        bool isOnScreen = IsOnScreen(screenPoint);
                        var position3D = new Vector3(xPos, yPos, zPos);

                        var playerData = GetOrCreatePlayerData(objectGuid, objectId, position3D, screenPoint, NpcName);
                        playerData.Distance = GetDistanceTo(position3D);
                        playerData.IsOffScreen = !isOnScreen;
                        if (!isOnScreen)
                        {
                            playerData.DirectionAngle = CalculateDirectionAngle(cameraForward, cameraPos, position3D);
                        }

                        playerData.IsActive = true;
                        visiblePlayers.Add(playerData);
                    }

                    baseAddress = _memory.ReadPointer(baseAddress + Client.NextObjectOffset);
                }

                RemoveInactivePlayers();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Scanner error: {ex}");
            }

            return visiblePlayers;
        }

        private void DeactivateCachedPlayers()
        {
            foreach (var player in _playerCache.Values)
            {
                player.IsActive = false;
            }
        }
        private bool IsInterestingObject(short type, int objectId)
        {
            if (type != (int)Defines.ObjectType.GAMEOBJ
                && type != (int)Defines.ObjectType.CONTAINER
                && type != (int)Defines.ObjectType.UNIT)
                return false;

            if (objectId <= 0 || _db.HasInBlackList(objectId))
                return false;

            // Проверяем состояние чекбоксов перед добавлением объекта
            if (IsOreBoxChecked && _db.HasOre((uint)objectId)) return true;
            if (IsHerbBoxChecked && _db.HasHerb((uint)objectId)) return true;
            if (IsContainerBoxChecked && _db.HasContainer((uint)objectId)) return true;
            if (IsRareBoxChecked && _db.HasRareNpc((uint)objectId)) return true;

            return false;
        }

        private bool IsOnScreen(WinPoint screenPoint)
        {
            return (screenPoint.X > 0 && screenPoint.X < _camera.scX &&
                    screenPoint.Y > 0 && screenPoint.Y < _camera.scY);
        }

        private PlayerData GetOrCreatePlayerData(ulong guid, int objectId, Vector3 position3D, WinPoint screenPoint, string NpcName)
        {
            if (!_playerCache.TryGetValue(guid, out var playerData))
            {
                var resourceInfo = new kibitka.box.DB.Name_And_TextureName();
                string texturePath = null;
                string name = "Unknown";
                // OreBox HerbBox ContainerBox RareBox

                if (_db.GetOre((uint)objectId, ref resourceInfo))
                {
                    name = resourceInfo.Name;
                    texturePath = $"Mine/{resourceInfo.TextureName}.tga";
                }
                else if (_db.GetHerb((uint)objectId, ref resourceInfo))
                {
                    name = resourceInfo.Name;
                    texturePath = $"Herb/{resourceInfo.TextureName}.tga";
                }
                else if (_db.GetContainer((uint)objectId, ref resourceInfo))
                {
                    name = resourceInfo.Name;
                    texturePath = $"Other/{resourceInfo.TextureName}.tga";
                }                
                else if (_db.GetRareNpc((uint)objectId, ref resourceInfo))
                {
                    name = NpcName ?? resourceInfo.Name;
                    texturePath = $"Other/{resourceInfo.TextureName}.tga";
                }

                playerData = new PlayerData
                {
                    Name = name,
                    TexturePath = texturePath,
                    ScreenPosition = screenPoint
                };
                _playerCache[guid] = playerData;
            }
            else
            {
                playerData.ScreenPosition = screenPoint;
            }

            return playerData;
        }

        private void ReadMyPlayer()
        {
            _myPlayer.BaseAddress = GetObjectBaseByGuid(_myPlayer.Guid);
            if (_myPlayer.BaseAddress == IntPtr.Zero) return;

            _myPlayer.XPos = _memory.ReadFloat(_myPlayer.BaseAddress + Unit.Pos_X);
            _myPlayer.YPos = _memory.ReadFloat(_myPlayer.BaseAddress + Unit.Pos_Y);
            _myPlayer.ZPos = _memory.ReadFloat(_myPlayer.BaseAddress + Unit.Pos_Z);
        }

        private IntPtr GetObjectBaseByGuid(ulong guid)
        {
            IntPtr current = _memory.ReadPointer(_stct.ObjectManager + Client.FirstObjectOffset);
            while (current != IntPtr.Zero)
            {
                if (_memory.ReadUInt64(current + Object.Guid) == guid)
                    return current;
                current = _memory.ReadPointer(current + Client.NextObjectOffset);
            }
            return IntPtr.Zero;
        }

        private float GetDistanceTo(Vector3 obj)
        {
            var myPos = new Vector3(_myPlayer.XPos, _myPlayer.YPos, _myPlayer.ZPos);
            return (obj - myPos).Length();
        }

        private float CalculateDirectionAngle(Vector3 cameraForward, Vector3 cameraPos, Vector3 objPos)
        {
            Vector3 dirToObj = objPos - cameraPos;
            dirToObj.Normalize();

            Vector3 horizontalDir = new Vector3(dirToObj.X, dirToObj.Y, 0);
            horizontalDir.Normalize();

            float dot = Vector3.Dot(cameraForward, horizontalDir);
            float angle = (float)Math.Acos(MathExt.Clamp(dot, -1f, 1f));

            Vector3 cross = Vector3.Cross(cameraForward, horizontalDir);
            return cross.Z < 0 ? -angle : angle;
        }

        private void RemoveInactivePlayers()
        {
            var inactive = _playerCache.Where(p => !p.Value.IsActive).ToList();
            foreach (var item in inactive)
                _playerCache.Remove(item.Key);
        }
    }

    public static class MathExt
    {
        public static float Clamp(float value, float min, float max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }
    }
}
