///-----------------------------------------------------------------
/// Author : Adrien Bordes
/// Date : 15/10/2019 18:09
///-----------------------------------------------------------------


namespace Com.Docaret.CompositeurUniverseBuilder
{
    public class MetaData
    {

        public int desiredWidthValue = 75;
        public bool desiredWidth;

        public bool showOnStart;
        public bool videoLoop;
        public bool videoAutoplay = true;
        public bool videoMute;

        public bool[] metaList;

        public const string DESIRED_WIDTH = "desiredWidth =";
        public const string SHOW_ON_START = "table.showOnStart =";
        public const string VDEO_LOOP = "video.loop =";
        public const string VDEO_AUTOPLAY = "video.autoplay =";
        public const string VDEO_MUTE = "video.mute =";
    }
}