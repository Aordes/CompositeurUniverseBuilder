///-----------------------------------------------------------------
/// Author : #Arien Bordes#
/// Date : #19.09.2019#
///-----------------------------------------------------------------

using System.IO;
using UnityEngine;

namespace Com.Docaret.UniverseBuilder
{
	public class DirectoryData {

        private static DirectoryInfo _currentDirectory;
        private static string _currentUniversePath;
        private static string _currentUniverseName;
        private static string _compositeurFolderPath;

        #region Getter & Setter
        public static DirectoryInfo CurrentDirectory { get; set; }
        public static string CurrentUniversePath { get; set; }
        public static string CurrentUniverseName { get; set; }
        public static string CompositeurFolderPath { get; set; }
        #endregion
	}
}