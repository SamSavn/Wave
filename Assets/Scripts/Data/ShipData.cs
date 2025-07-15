using System;
using System.Collections.Generic;

namespace Wave.Data
{
    [Serializable]
    public class ShipData
    {
        public int index;
        public int version = 0;
        public HashSet<int> unlockedVersions = new();
    }
}