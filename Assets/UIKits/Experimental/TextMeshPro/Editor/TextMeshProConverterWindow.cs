using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
#if UIKIT_TMP
using TMPro;
#endif

namespace VRUiKits.Utils
{
    public class TextMeshProConverterWindow : EditorWindow
    {
        int totalTexts = 0;
        int totalSelectedTexts = 0;
#if UIKIT_TMP
        /*
         * The ability to control the default size of a RectTransform is a feature
         * of Text Mesh Pro.
         */
        bool allowTMPControlRectSize = false;
        TMP_FontAsset fontAsset;
        // If include inactive component in selection
        bool includeInactive = true;
#endif

        [UnityEditor.MenuItem("Window/VR UIKit/TextMeshPro Converter")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<TextMeshProConverterWindow>("TextMeshPro Converter");
        }

        void OnGUI()
        {
            // Helper message
            GUILayout.Label("Please make sure TextMeshPro is in your project", EditorStyles.helpBox);
            // Check Text Total Number
            GUILayout.Label("Check Total Number of Text", EditorStyles.boldLabel);
            if (GUILayout.Button("Check Total Active Texts Number"))
            {
                totalTexts = TextMeshProConverter.CheckTextsTotalNumber();
            }

            GUILayout.Label("Founded " + totalTexts + " Active Text Component in the scene.");

            if (GUILayout.Button("Check Total Texts Number in Selected Object"))
            {
                var selected = Selection.GetFiltered(typeof(Text), SelectionMode.Deep);
                totalSelectedTexts = selected.Length;
            }

            GUILayout.Label("Founded " + totalSelectedTexts + " Text Component in selected object.");

            GUILayout.Label("TextMeshPro Converter", EditorStyles.boldLabel);
            // Helper message
            GUILayout.Label("To enable the TextMeshPro Converter,"
                + " go to Player Settings and add new custom define: UIKIT_TMP", EditorStyles.helpBox);
#if UIKIT_TMP
            // Text to TMP converter
            // Variables
            allowTMPControlRectSize = EditorGUILayout.ToggleLeft("Allow TMP Resize Rect", allowTMPControlRectSize);
            fontAsset = EditorGUILayout.ObjectField("Font", fontAsset, typeof(TMP_FontAsset), false) as TMP_FontAsset;

            if (GUILayout.Button("Convert All Active Text to TMP"))
            {
                TextMeshProConverter.ConvertAllTexts(allowTMPControlRectSize, fontAsset);
            }
            // Helper message
            GUILayout.Label("Inactive Text Component in Hierarchy will be ignored.\n"
                    + "Converting an inactive Text to TextMeshPro might cause"
                    + " unpleasant result, so please make sure the Text active in hierarchy before"
                    + " converting them.", EditorStyles.helpBox);

            if (GUILayout.Button("Convert Selected Text to TMP"))
            {
                var selected  = Selection.GetFiltered(typeof(Text), SelectionMode.Deep);
                TextMeshProConverter.ConvertSelectedTexts(selected, allowTMPControlRectSize, fontAsset);
            }

            // Update TMP Font
            GUILayout.Label("Update TextMeshPro Font", EditorStyles.boldLabel);

            if (GUILayout.Button("Update All Active TMP"))
            {
                TextMeshProConverter.UpdateAllTmp(fontAsset);
            }

            // Helper message
            GUILayout.Label("Even though the font has been updated successfully, "
            + "it might not refresh immediately in the scene.", EditorStyles.helpBox);

            includeInactive = EditorGUILayout.ToggleLeft("Update Inactive Text in Selection", includeInactive);
            if (GUILayout.Button("Update Selected TMP"))
            {
                var selected  = Selection.GetFiltered(typeof(TextMeshProUGUI), SelectionMode.Deep);
                TextMeshProConverter.UpdateSelectedTmp(selected, fontAsset, includeInactive);
            }
#endif
        }
    }
}