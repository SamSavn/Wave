using Wave.Services;
using Wave.Settings;

namespace Wave.Data
{
    public struct ColorVersionsSetData
    {
        public ShipInfo shipInfo;
        public ShipsService shipsService;
        public int shipIndex;
        public int selectedVersion;
        public int equippedVersion;
    }
}