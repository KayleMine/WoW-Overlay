using System.Drawing;

namespace kibitka
{
    public class PlayerData
    {
        public string Name { get; set; }
        public Point ScreenPosition { get; set; }
        public string TexturePath { get; set; }
        public bool IsActive { get; set; }
        public float Distance { get; set; }
        public bool IsOffScreen { get; set; }
        public float DirectionAngle { get; set; }


        public Point OffScreenPosition { get; set; }
        public float OffScreenAngle { get; set; }
    }
}