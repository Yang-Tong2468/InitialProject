﻿/// ---------------------------------------------
/// Ultimate Inventory System
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.UltimateInventorySystem.Editor.Utility
{
    using Opsive.Shared.Utility;
    using UnityEditor;

    /// <summary>
    /// Editor script which will define or remove the Ultimate Invnetory System compiler symbols so the components are aware of the asset import status.
    /// </summary>
    [InitializeOnLoad]
    public class DefineCompilerSymbols
    {
        private static string s_TextMeshProSymbol = "TEXTMESH_PRO_PRESENT";

        /// <summary>
        /// If the specified classes exist then the compiler symbol should be defined, otherwise the symbol should be removed.
        /// </summary>
        static DefineCompilerSymbols()
        {
            // The TMP_Text component will exist when the TextMesh Pro asset is imported.
            var textMeshProExists = TypeUtility.GetType("TMPro.TMP_Text") != null;
#if TEXTMESH_PRO_PRESENT
            if (!textMeshProExists) {
                RemoveSymbol(s_TextMeshProSymbol);
            }
#else
            if (textMeshProExists) {
                AddSymbol(s_TextMeshProSymbol);
            }
#endif
        }

        /// <summary>
        /// Adds the specified symbol to the compiler definitions.
        /// </summary>
        /// <param name="symbol">The symbol to add.</param>
        private static void AddSymbol(string symbol)
        {
#if UNITY_6000_0_OR_NEWER
            var buildGroup = UnityEditor.Build.NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var symbols = PlayerSettings.GetScriptingDefineSymbols(buildGroup);
#else
            var buildGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildGroup);
#endif
            
            if (symbols.Contains(symbol)) {
                return;
            }
            symbols += (";" + symbol);
            
#if UNITY_6000_0_OR_NEWER
            PlayerSettings.SetScriptingDefineSymbols(buildGroup, symbols);
#else
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildGroup, symbols);
#endif
            
        }

        /// <summary>
        /// Remove the specified symbol from the compiler definitions.
        /// </summary>
        /// <param name="symbol">The symbol to remove.</param>
        private static void RemoveSymbol(string symbol)
        {
#if UNITY_6000_0_OR_NEWER
            var buildGroup = UnityEditor.Build.NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var symbols = PlayerSettings.GetScriptingDefineSymbols(buildGroup);
#else
            var buildGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildGroup);
#endif
            
           
            if (!symbols.Contains(symbol)) {
                return;
            }
            if (symbols.Contains(";" + symbol)) {
                symbols = symbols.Replace(";" + symbol, "");
            } else {
                symbols = symbols.Replace(symbol, "");
            }
            
#if UNITY_6000_0_OR_NEWER
            PlayerSettings.SetScriptingDefineSymbols(buildGroup, symbols);
#else
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildGroup, symbols);
#endif

            
        }
    }
}