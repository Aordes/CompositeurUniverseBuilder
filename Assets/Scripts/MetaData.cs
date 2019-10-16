///-----------------------------------------------------------------
/// Author : Adrien Bordes
/// Date : 15/10/2019 18:09
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.Docaret.UniverseBuilder
{
	public class MetaData {

        public int desiredWidthValue;
        public bool desiredWidth;

        public bool showOnStart;
        public bool videoLoop;
        public bool videoAutoplay = true;
        public bool videoMute;

        public const string DESIRED_WIDTH = "desiredWidth =";
        public const string SHOW_ON_START = "showOnStart =";
        public const string VDEO_LOOP = "videoLoop =";
        public const string VDEO_AUTOPLAY = "videoAutoplay =";
        public const string VDEO_MUTE = "videoMute =";
	}
}