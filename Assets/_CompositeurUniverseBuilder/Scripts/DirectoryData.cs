///-----------------------------------------------------------------
/// Author : #Arien Bordes#
/// Date : #19.09.2019#
///-----------------------------------------------------------------

using System;
using System.IO;

namespace Com.Docaret.CompositeurUniverseBuilder
{
    public class DirectoryData
    {

        private static DirectoryInfo _currentDirectory;
        private static string _currentUniversePath;
        private static string _currentUniverseName;
        private static string _compositeurFolderPath;
        private static UniverseStruct _universeStruct;

        public static event Action ImportData;

        public static bool openExistingProject;


        #region Getter & Setter
        public static DirectoryInfo CurrentDirectory { get; set; }
        public static string CurrentUniversePath { get; set; }
        public static string CurrentUniverseName { get; set; }
        public static string CompositeurFolderPath { get; set; }
        public static UniverseStruct UniverseStruct { get; set; }
        #endregion
    }
}