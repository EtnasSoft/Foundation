using UnityEngine;

namespace EtnasSoft.Foundation.Unity {
    public static class ColorRgba32UnityAdapter {
        public static Color32 ToUnity(
            this ColorRgba32 c
        ) {
            return new Color32(c.R, c.G, c.B, c.A);
        }

        public static ColorRgba32 ToDomain(
            this Color32 c
        ) {
            return new ColorRgba32(c.r, c.g, c.b, c.a);
        }
    }
}
