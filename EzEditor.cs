#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EzEditor
{
    public enum GuiFieldType { String, Vector3, Int }

    public class EzGUI
    {
        // Instance of HorizontalBlock:
        private static readonly gui.HorizontalBlock HorBlock = new gui.HorizontalBlock();

        // Then a 'Horizontal' method:
        public gui.HorizontalBlock Horizontal()
        {
            return HorBlock.Begin();
        }
    }

    public class gui : Editor
    {
        public static Texture2D DeleteButton = Resources.Load("deletebutton", typeof(Texture2D)) as Texture2D;
        public static Texture2D AddButton = Resources.Load("addbutton", typeof(Texture2D)) as Texture2D;
        public static Texture2D LoadValuesButton = Resources.Load("LoadValues", typeof(Texture2D)) as Texture2D;
        public static Texture2D MoveUpButton = Resources.Load("MoveUp", typeof(Texture2D)) as Texture2D;
        public static Texture2D MoveDownButton = Resources.Load("MoveDown", typeof(Texture2D)) as Texture2D;
        public static Texture2D Undobutton = Resources.Load("Undo", typeof(Texture2D)) as Texture2D;

        public static void HBlockBegin() { EditorGUILayout.BeginHorizontal(); }
        public static void HBlockEnd() { EditorGUILayout.EndHorizontal(); }
        public static void VBlockBegin() { EditorGUILayout.BeginVertical(); }
        public static void VBlockEnd() { EditorGUILayout.EndVertical(); }

        #region Horizontal Block: using (gui.Horizontal()) {...}
        private static readonly HorizontalBlock Hblock = new HorizontalBlock();

        public class HorizontalBlock : IDisposable
        {
            public HorizontalBlock Begin() { GUILayout.BeginHorizontal(); return this; }
            public void Dispose() { GUILayout.EndHorizontal(); }
        }

        public static HorizontalBlock Horizontal()
        {
            return Hblock.Begin();
        }
        #endregion

        #region Vertical Block: using (gui.Vertical()) {...}
        private static readonly VerticalBlock Vblock = new VerticalBlock();

        public class VerticalBlock : IDisposable
        {
            public VerticalBlock Begin() { GUILayout.BeginVertical(); return this; }
            public void Dispose() { GUILayout.EndVertical(); }
        }

        public static VerticalBlock Vertical()
        {
            return Vblock.Begin();
        }
        #endregion

        public static void Separator()
        {
            EditorGUILayout.Separator();
        }

        public static void Separator(int pixels)
        {
            GUILayout.Label("", EditorStyles.label, GUILayout.Height(pixels));
        }

        public static GUILayoutOption MinWidth(float minWidth)
        {
            return GUILayout.MinWidth(minWidth);
        }

        public static GUILayoutOption MaxWidth(float maxWidth)
        {
            return GUILayout.MaxWidth(maxWidth);
        }

        public static void EzCol(Action body)
        {
            EditorGUILayout.BeginVertical();
            body();
            EditorGUILayout.EndVertical();
        }

        public static void EzRow(Action body)
        {
            HBlockBegin();
            body();
            HBlockEnd();
        }

        public static void EzRow(float indent, Action body)
        {
            HBlockBegin();
            EzSpacer(indent);
            body();
            HBlockEnd();
        }

        public static void EzSpacer(float pixels)
        {
            GUILayout.Label("", EditorStyles.label, MaxWidth(pixels), MinWidth(pixels));
        }

        public static void EzLabel(string label, params GUILayoutOption[] options)
        {
            GUILayout.Label(label, options);
        }

        public static void EzLabel(Texture2D image, params GUILayoutOption[] options)
        {
            
            GUILayout.Label(image, options);
        }

        public static void EzHeader(string label, int fontSize = 20, params GUILayoutOption[] options)
        {
            var previousSize = GUI.skin.label.fontSize;
            var previousStyle = GUI.skin.label.fontStyle;
            GUI.skin.label.fontSize = fontSize;
            GUI.skin.label.fontStyle = FontStyle.Bold;
            GUILayout.Label(label, options);
            GUI.skin.label.fontSize = previousSize;
            GUI.skin.label.fontStyle = previousStyle;
        }

        public static Color EzColorField(string label, Color color, float offset = 0f, params GUILayoutOption[] options)
        {
            AutosetFieldSize(label, offset);
            return EditorGUILayout.ColorField(label, color, options);
        }

        private static void AutosetFieldSize(string label, float offset)
        {
            var textDimensions = GUI.skin.label.CalcSize(new GUIContent(label));
            LookLikeControls(textDimensions.x + offset);
        }

        public static int EzIntField(string label, int val, float offset = 0f,
                                        params GUILayoutOption[] options)
        {
            AutosetFieldSize(label, offset);
            return EditorGUILayout.IntField(label, val, options);
        }

        public static string EzTextField(string label, string text, float offset = 0f,
            params GUILayoutOption[] options) 
        {
            AutosetFieldSize(label, offset);
            return EditorGUILayout.TextField(label, text, options);            
        }

        public static float EzFloatField(string label, float val, float offset = 0f,
                                            params GUILayoutOption[] options)
        {
            AutosetFieldSize(label, offset);
            return EditorGUILayout.FloatField(label, val, options);
        }

        public static float EzFloatSlider(string label, float value, float minValue, float maxValue,
                                            float offset = 0f, params GUILayoutOption[] options)
        {
            using (Horizontal())
            {
                EzLabel(label);
                return EditorGUILayout.Slider(value, minValue, maxValue, options);
            }
        }

        public static bool EzToggleButton (bool toggleVariable, Texture2D textureOn, Texture2D textureOff, string tooltip = "", params GUILayoutOption[] options)
        {
            bool buttonClicked;
            var buttonImg = toggleVariable ? textureOn : textureOff;
            var optionsList = options.ToList();
            optionsList.Add(GUILayout.ExpandWidth(false));
            optionsList.Add(GUILayout.Height(16)); 
            options = optionsList.ToArray();
            if (tooltip != "")
            {
                var guiContent = new GUIContent(buttonImg, tooltip);
                buttonClicked = GUILayout.Button(guiContent, options);
            }
            else
                buttonClicked = GUILayout.Button(buttonImg, options);

            if (!buttonClicked) return toggleVariable;

            return !toggleVariable;
        }

        public static bool EzToggleButton (string label, bool toggleVariable, params GUILayoutOption[] options)
        {
            return GUILayout.Toggle(toggleVariable, label, "Button", options);
        }

        public static void LookLikeControls(float size1, float size2)
        {
            EditorGUIUtility.labelWidth = size1;
            EditorGUIUtility.fieldWidth = size2;
        }

        public static void LookLikeControls(float size)
        {
            EditorGUIUtility.labelWidth = size;
        }

        public static void LookLikeControls()
        {
            EditorGUIUtility.labelWidth = 0f;
            EditorGUIUtility.fieldWidth = 0f;
        }

        public static T EzObjectField<T>(string label, T obj, float offset = 0f, params GUILayoutOption[] options) where T : Object
        {
            AutosetFieldSize(label, offset);
            return EditorGUILayout.ObjectField(label, obj, typeof(T), true, options) as T;
        }

        public static Vector3 EzV3Field(string label, Vector3 v3, float offset = 0f, params GUILayoutOption[] options)
        {
            AutosetFieldSize(label, offset);
            return EditorGUILayout.Vector3Field(label, v3, options);
        }

        // Eg.: _lineMesh.WipeMode = (LineOrientation)gui.EzEnumPopup("Fill Mode", _lineMesh.WipeMode);
        public static Enum EzEnumPopup(string label, Enum enumToShow, float offset = 0f, params GUILayoutOption[] options)
        {
            AutosetFieldSize(label, offset);
            return EditorGUILayout.EnumPopup(label, enumToShow, options);
        }

        public static int EzPopup(string label, int selectedIdx, string[] strings, float offset = 0f, params GUILayoutOption[] options)
        {
            AutosetFieldSize(label, offset);
            return EditorGUILayout.Popup(label, selectedIdx, strings, options);
        }


        public static GameObject EzGameObjectField(string label, GameObject gO, float offset, params GUILayoutOption[] options)
        {
            AutosetFieldSize(label, offset);
            return EditorGUILayout.ObjectField(label, gO, typeof(GameObject), true, options) as GameObject;
        }

        public static bool EzFoldout(string label, bool variable, float offset = 0f)
        {
            AutosetFieldSize(label, offset);
            return EditorGUILayout.Foldout(variable, label);
        }

        // For 8x8 textured buttons, 20x15 is the optimal size
        public static bool EzButton(Texture2D texture) {
            return GUILayout.Button(texture, GUILayout.Width(20f), GUILayout.Height(15f));            
        }

        public static bool EzButton(Texture2D texture, GUILayoutOption[] options)
        {
            return GUILayout.Button(texture, options);
        }

        public static bool EzButton(string label, params GUILayoutOption[] options)
        {
            return GUILayout.Button(label, options);
        }

        public static bool EzGrayoutButton(string label, bool shouldGrayout, params GUILayoutOption[] options)
        {
            if (!shouldGrayout)
            {
                return GUILayout.Button(label);
            }
            var grayOutButtonStyle = new GUIStyle(EditorStyles.miniButton);
            var myStyleColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            grayOutButtonStyle.fontSize = 11;
            grayOutButtonStyle.alignment = TextAnchor.UpperCenter;
            grayOutButtonStyle.normal.textColor = myStyleColor;
            grayOutButtonStyle.onNormal.textColor = myStyleColor;
            grayOutButtonStyle.hover.textColor = myStyleColor;
            grayOutButtonStyle.onHover.textColor = myStyleColor;
            grayOutButtonStyle.focused.textColor = myStyleColor;
            grayOutButtonStyle.onFocused.textColor = myStyleColor;
            grayOutButtonStyle.active.textColor = myStyleColor;
            grayOutButtonStyle.onActive.textColor = myStyleColor;
            grayOutButtonStyle.fixedHeight = 18f;
            grayOutButtonStyle.margin = new RectOffset(4, 4, 3, 3);

            GUILayout.Button(label, grayOutButtonStyle, options);
            return false;
        }

        public static bool EzToggle(string label, bool variable)
        {
            return GUILayout.Toggle(variable, label);
        }

        public static int EzToolbar(int variable, string[] texts, params GUILayoutOption[] options)
        {
            return GUILayout.Toolbar(variable, texts, options);
        }

        public static int EzToolbar(int variable, Texture[] images, params GUILayoutOption[] options)
        {
            return GUILayout.Toolbar(variable, images, options);
        }

        public static List<T> EzList<T>(string title, List<T> list, ref bool foldoutFlag) where T : Object
        {
            using (Vertical())
            {
                LookLikeControls(110f, 50f);
                var fullTitle = title + " (" + list.Count + "):";
                foldoutFlag = EzFoldout(fullTitle, foldoutFlag);
                if (!foldoutFlag) return list;

                T newVar = null;
                using (Horizontal())
                {
                    EzSpacer(5f);
                    using (Vertical())
                    {
                        LookLikeControls(110f, 100f);
                        for (var i = 0; i < list.Count; i++)
                        {
                            using (Horizontal())
                            {
                                list[i] = EzObjectField("", list[i], 5f);
                                if (!EzButton(DeleteButton, null))
                                    continue;
                                
                                list.RemoveAt(i);
                                break;
                            }
                        }
                        using (Horizontal())
                        {
                            LookLikeControls(15f, 10f);
                            newVar = EzObjectField("+", newVar, 10f);
                        }
                    }
                }

                if (newVar == null)
                    return list;
                
                if (list.Contains(newVar))
                {
                    Separator();
                    return list;
                }
                list.Add(newVar);
                Separator();
                return list;
            }
        }

        public static Vector3[] EzV3Array(string title, Vector3[] array, ref Vector3 newVar, ref bool foldoutFlag)
        {
            using (Vertical())
            {
                LookLikeControls(110f, 50f);
                var fullTitle = title + " (" + array.Length + "):";
                foldoutFlag = EzFoldout(fullTitle, foldoutFlag);
                if (!foldoutFlag) return array;

                using (Horizontal())
                {
                    EzSpacer(5f);
                    using (Vertical())
                    {
                        for (var i = 0; i < array.Length; i++)
                        {
                            using (Horizontal())
                            {
                                array[i] = EzV3Field("", array[i], 0f, GUILayout.Height(22f));
                                if (!EzButton(DeleteButton, null))
                                    continue;
                                
                                array = array.RemoveAt(i);
                                break;
                            }
                        }
                        Separator();
                        using (Horizontal())
                        {
                            LookLikeControls(15f, 10f);
                            if (EzButton(AddButton, null))
                                array = array.Add(newVar);
                            newVar = EzV3Field("", newVar, 10f);
                        }
                    }
                }
                return array;
            }
        }

        public static T[] EzObjectArray<T>(string title, T[] array, ref T newVar, ref bool foldoutFlag) where T : Object
        {
            using (Vertical())
            {
                LookLikeControls(110f, 50f);
                var fullTitle = title + " (" + array.Length + "):";
                foldoutFlag = EzFoldout(fullTitle, foldoutFlag);
                if (!foldoutFlag)
                    return array;

                using (Horizontal())
                {
                    EzSpacer(5f);
                    using (Vertical())
                    {
                        for (var i = 0; i < array.Length; i++)
                        {
                            using (Horizontal())
                            {
                                array[i] = EzObjectField("", array[i]);
                                if (EzButton(DeleteButton, null))
                                {
                                    array = array.RemoveAt(i);
                                    //break;
                                }
                            }
                        }
                        Separator();
                        using (Horizontal())
                        {
                            LookLikeControls(15f, 10f);
                            EzLabel("+", GUILayout.MaxWidth(12f));
                            newVar = EzObjectField("", newVar, 10f);
                        }
                        if (newVar == null)
                            return array;
                        
                        array = array.Add(newVar);
                        newVar = null;
                    }
                }
                return array;
            }
        }

        
        public static void LoseFocus() {
            GUIUtility.keyboardControl = 0;
        }

        public static void VerticalSpacer(float pixels)
        {
            EzLabel("", GUILayout.Height(pixels));
        }

        public static void HorizontalBar(float padding = 0f) {
            VerticalSpacer(padding);
            GUILayout.Label("", EditorStyles.textArea, GUILayout.Height(3f));
            VerticalSpacer(padding);
        }

        public static void EzHelpBox(string message, MessageType type, bool wide)
        {
            EditorGUILayout.HelpBox(message, type, wide);
        }
    }

} //namespace EzEditor
#endif // UNITY_EDITOR