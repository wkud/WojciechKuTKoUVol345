#if UNITY_EDITOR

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace TKOU.SimAI.Editor
{
    /// <summary>
    /// Adds some useful methods for editor workflow improvements.
    /// </summary>
    public static class EditorExtensions
    {
        #region Variables

        private const string MenuAssetPath = "Assets/Editor Extensions/";
        private const int Priority = 2000;

        #endregion Variables

        #region Public methods

        /// <summary>
        /// Loads an object using a GUID.
        /// </summary>
        /// <param name="GUID"></param>
        /// <returns></returns>
        public static T LoadProjectAssetByGUID<T>(string GUID) where T : Object
        {
            string path = AssetDatabase.GUIDToAssetPath(GUID);
            Assert.IsFalse(path == string.Empty, "GUID used is invalid.");
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            Assert.IsNotNull(asset, "Object type is invalid");
            return asset;
        }
        
        /// <summary>
        /// Loads project assets by a type.
        /// </summary>
        /// <returns>An Object array of assets.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> LoadProjectAssetsByType<T>() where T : UnityEngine.Object
        {
            return LoadProjectAssetsByType<T>(null);
        }

        /// <summary>
        /// Loads project assets by a type.
        /// </summary>
        /// <returns>An Object array of assets.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> LoadProjectAssetsByTypeInFolder<T>(string searchFolder) where T : UnityEngine.Object
        {
            return LoadProjectAssetsByTypeInFolders<T>(new string[] { searchFolder });
        }

        /// <summary>
        /// Loads project assets by a type.
        /// </summary>
        /// <returns>An Object array of assets.</returns>
        public static List<T> LoadProjectAssetsByTypeInFolders<T>(string [] searchFolders) where T : UnityEngine.Object
        {
            List<T> assets = new List<T>();
            string[] guids;

            if (searchFolders == null)
            {
                guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
            }
            else
            {
                guids = AssetDatabase.FindAssets($"t:{typeof(T)}", searchFolders);
            }

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }

        /// <summary>
        /// Loads project assets by a type by using a filter.
        /// filter should return TRUE for all objects that should be added, and FALSE otherwise.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> LoadProjectAssetsByType<T>(System.Func<T, bool> filter) where T : UnityEngine.Object
        {
            return LoadProjectAssetsByTypeInFolders<T>(filter, null);
        }

        /// <summary>
        /// Loads project assets by a type.
        /// </summary>
        /// <returns>An Object array of assets.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> LoadProjectAssetsByTypeInFolder<T>(System.Func<T, bool> filter, string searchFolder) where T : UnityEngine.Object
        {
            return LoadProjectAssetsByTypeInFolders<T>(filter, new string [] {searchFolder});
        }

        /// <summary>
        /// Loads project assets by a type.
        /// </summary>
        /// <returns>An Object array of assets.</returns>
        public static List<T> LoadProjectAssetsByTypeInFolders<T>(System.Func<T, bool> filter, string[] searchFolders) where T : UnityEngine.Object
        {
            List<T> assets = new List<T>();
            string[] guids;

            if (searchFolders == null)
            {
                guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
            }
            else
            {
                guids = AssetDatabase.FindAssets($"t:{typeof(T)}", searchFolders);
            }

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    if (filter == null)
                    {
                        assets.Add(asset);
                    }
                    else if(filter(asset))
                    {
                        assets.Add(asset);
                    }
                }
            }
            return assets;
        }

        #endregion Public methods

        #region Private methods

        [MenuItem(MenuAssetPath + "Copy full path %#1", false, Priority + 2)]
        private static void CopyFullAssetPath()
        {
            var guids = Selection.assetGUIDs;

            var assetPath = System.IO.Path.Combine(Application.dataPath,
                AssetDatabase.GUIDToAssetPath(guids[0]).Replace("Assets/", string.Empty));
            EditorGUIUtility.systemCopyBuffer = assetPath;
        }

        [MenuItem(MenuAssetPath + "Copy full path %#1", true, Priority + 2)]
        private static bool CopyFullPathValidation()
        {
            return Selection.assetGUIDs.Length == 1;
        }

        [MenuItem(MenuAssetPath + "Copy asset GUID %#0", false, Priority)]
        private static void CopyAssetGUID()
        {
            EditorGUIUtility.systemCopyBuffer = Selection.assetGUIDs[0];
        }

        [MenuItem(MenuAssetPath + "Copy asset GUID %#0", true, Priority)]
        private static bool CopyAssetGUIDValidation()
        {
            return Selection.assetGUIDs.Length == 1;
        }

        #endregion Private methods
    }
}

#endif
