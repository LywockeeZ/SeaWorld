using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace Water2DTool
{
    [CustomEditor(typeof(Water2D_Tool))]
    public class Water2D_ToolEditor : Editor
    {
        private Texture2D texDot;
        private List<int> selectedPoints = new List<int>();
        private static float pathScale = 1f;
        private bool prevChanged = false;

        void OnEnable()
        {
            texDot = GetGizmo("water2D_circle.png");
            selectedPoints.Clear();
            Water2D_Tool water2D_Tool = (Water2D_Tool)target;
            if (water2D_Tool.GetComponent<MeshFilter>().sharedMesh == null)
                water2D_Tool.RecreateWaterMesh();
        }

        void OnSceneGUI()
        {
            Water2D_Tool water2D_Tool = (Water2D_Tool)target;

            if (water2D_Tool.showMesh)
            {
                EditorUtility.SetSelectedRenderState(water2D_Tool.gameObject.GetComponent<Renderer>(), EditorSelectedRenderState.Wireframe);

                if (water2D_Tool.cubeWater)
                    EditorUtility.SetSelectedRenderState(water2D_Tool.topMeshGameObject.gameObject.GetComponent<Renderer>(), EditorSelectedRenderState.Wireframe);
            }
            else
            {
                EditorUtility.SetSelectedRenderState(water2D_Tool.gameObject.GetComponent<Renderer>(), EditorSelectedRenderState.Highlight);

                if (water2D_Tool.cubeWater)
                    EditorUtility.SetSelectedRenderState(water2D_Tool.topMeshGameObject.gameObject.GetComponent<Renderer>(), EditorSelectedRenderState.Highlight);
            }

            GUIStyle iconStyle = new GUIStyle();
            iconStyle.alignment = TextAnchor.MiddleCenter;

            // Setup undoing things
            Undo.RecordObject(target, "Handles Position");

            if (Event.current.type == EventType.Repaint)
            {
                ResetChildTransform(water2D_Tool);

                DrawRectangle(water2D_Tool);
            }

            // Draw and interact with the handles.
            if (water2D_Tool.useHandles)
                UpdateHandles(water2D_Tool, iconStyle);

            if (water2D_Tool.use3DCollider && !water2D_Tool.GetComponent<Water2D_Ripple>() && Event.current.type == EventType.Repaint)
                DrawOverlapSphereGizmo(water2D_Tool);

            // Update everything that relies on the handles, if the GUI changed.
            if (GUI.changed)
            {
                water2D_Tool.RecreateWaterMesh();
                EditorUtility.SetDirty(target);
                prevChanged = true;
            }
            else if (Event.current.type == EventType.Used)
            {
                if (prevChanged == true)
                {
                    water2D_Tool.RecreateWaterMesh();
                }
                prevChanged = false;
            }
        }

        private void ResetChildTransform(Water2D_Tool water2D)
        {
            if (water2D.cubeWater)
            {
                water2D.topMeshGameObject.transform.localPosition = Vector3.zero;
                water2D.topMeshGameObject.transform.localRotation = Quaternion.identity;
                water2D.topMeshGameObject.transform.localScale = Vector3.one;
            }
        }

        public override void OnInspectorGUI()
        {
            Undo.RecordObject(target, "Modified Inspector");
            Water2D_Tool water2D = (Water2D_Tool)target;

            // Render the custom inspector fields.
            CustomInspector(water2D);

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);

                water2D.RecreateWaterMesh();
            }

            if (Event.current.type == EventType.ValidateCommand)
            {
                switch (Event.current.commandName)
                {
                    case "UndoRedoPerformed":
                        water2D.RecreateWaterMesh();
                        break;
                }
            }

            // A button for updating the Transform Position of the water object.
            if (GUILayout.Button("Center Position"))
                water2D.ReCenterPivotPoint();
            if (GUILayout.Button("Rebuild Water Object"))
            {
                water2D.AddNewWaterMeshInstance();

                Debug.LogFormat("Water object rebuild.");
            }

        }

        private void DrawOverlapSphereGizmo(Water2D_Tool water)
        {
            UnityEditor.Handles.color = Color.blue;
            Vector3[] vertices = water.GetComponent<MeshFilter>().sharedMesh.vertices;
            Water2D_Simulation water2D_Sim = water.GetComponent<Water2D_Simulation>();
            int surfaceVertsCount = water.frontMeshVertsCount / 2;

            for (int i = 0; i < surfaceVertsCount; i++)
            {
                Vector3 vertexGlobalPos = water.transform.TransformPoint(vertices[i + surfaceVertsCount]);
                UnityEditor.Handles.DrawWireDisc(new Vector3(vertexGlobalPos.x, vertexGlobalPos.y, vertexGlobalPos.z + water.overlapingSphereZOffset), Vector3.up, water2D_Sim.overlapSphereRadius);
                UnityEditor.Handles.DrawWireDisc(new Vector3(vertexGlobalPos.x, vertexGlobalPos.y, vertexGlobalPos.z + water.overlapingSphereZOffset), Vector3.forward, water2D_Sim.overlapSphereRadius);
            }
        }

        //------------------------------------------------------------------------------

        private void CustomInspector(Water2D_Tool water)
        {
            BoldFontStyle(() =>
            {
                water.showVisuals = EditorGUILayout.Foldout(water.showVisuals, "Mesh Properties");
            });

            if (water.showVisuals)
            {
                InspectorBox(10, () =>
                {
                    water.showMesh = EditorGUILayout.Toggle(new GUIContent("Show Mesh", "Shows or hides the mesh of the water in the scene view"), water.showMesh);

                    

                    if (!water.water2DRippleScript)
                        water.pixelsPerUnit = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Pixels To Units", "The number of pixels in 1 Unity unit."), water.pixelsPerUnit), 1, 768);

                    water.useHandles = EditorGUILayout.Toggle(new GUIContent("Use Handles", "The water size is changed using handles."), water.useHandles);
                    if (water.useHandles)
                    {
                        water.handleScale = EditorGUILayout.Slider(new GUIContent("Handle Scale", "Sets the scale of the water handles."), water.handleScale, 0.1f, 10f);
                        pathScale = water.handleScale;
                    }

                    if (!water.useHandles)
                    {
                        water.waterHeight = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Water Height", "Water height."), water.waterHeight), 0, 5000);
                        water.waterWidth = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Water Width", "Water width."), water.waterWidth), 0, 5000);


                        float pos = water.waterHeight / 2f;
                        water.handlesPosition[0] = new Vector2(0, pos);
                        water.handlesPosition[1] = new Vector2(0, -pos);

                        pos = water.waterWidth / 2f;

                        water.handlesPosition[2] = new Vector2(-pos, 0);
                        water.handlesPosition[3] = new Vector2(pos, 0);

                        if (water.cubeWater)
                        {
                            water.zSize = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Water Z Length", "The size of the water on the Z axis."), water.zSize), 0, 5000);
                            water.handlesPosition[4] = new Vector3(0, water.handlesPosition[0].y, water.zSize);
                        }
                    }

                    if (!water.quadMesh)
                        water.segmentsPerUnit = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Segments To Units", "The number of horizontal segments "
                            + "that should fit in 1 Unity unit."), water.segmentsPerUnit), 0.00001f, 500f);
                    water.quadMesh = EditorGUILayout.Toggle(new GUIContent("Single Quad Mesh", "What Enabled, the front mesh and top mesh will always be a quad and have only 4 vertices each."), water.quadMesh);


                    Type utility = Type.GetType("UnityEditorInternal.InternalEditorUtility, UnityEditor");
                    GUILayout.Space(5);
                    EditorGUILayout.LabelField(new GUIContent("Front Mesh"), EditorStyles.boldLabel);
                    if (utility != null)
                    {
                        PropertyInfo sortingLayerNames = utility.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
                        if (sortingLayerNames != null)
                        {
                            string[] layerNames = sortingLayerNames.GetValue(null, null) as string[];
                            string currName = water.GetComponent<Renderer>().sortingLayerName == "" ? "Default" : water.GetComponent<Renderer>().sortingLayerName;
                            int nameID = EditorGUILayout.Popup("Sorting Layer", Array.IndexOf(layerNames, currName), layerNames);

                            water.GetComponent<Renderer>().sortingLayerName = layerNames[nameID];

                        }
                        else
                        {
                            water.GetComponent<Renderer>().sortingLayerID = EditorGUILayout.IntField("Sorting Layer", water.GetComponent<Renderer>().sortingLayerID);
                        }
                    }
                    else
                    {
                        water.GetComponent<Renderer>().sortingLayerID = EditorGUILayout.IntField("Sorting Layer", water.GetComponent<Renderer>().sortingLayerID);
                    }
                    water.GetComponent<Renderer>().sortingOrder = EditorGUILayout.IntField("Order in Layer", water.GetComponent<Renderer>().sortingOrder);

                    if (water.cubeWater)
                    {
                        GUILayout.Space(5);
                        EditorGUILayout.LabelField(new GUIContent("Top Mesh"), EditorStyles.boldLabel);
                        if (utility != null)
                        {
                            PropertyInfo sortingLayerNames = utility.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
                            if (sortingLayerNames != null)
                            {
                                string[] layerNames = sortingLayerNames.GetValue(null, null) as string[];
                                string currName = water.topMeshGameObject.GetComponent<Renderer>().sortingLayerName == "" ? "Default" : water.topMeshGameObject.GetComponent<Renderer>().sortingLayerName;
                                int nameID = EditorGUILayout.Popup("Sorting Layer", Array.IndexOf(layerNames, currName), layerNames);

                                water.topMeshGameObject.GetComponent<Renderer>().sortingLayerName = layerNames[nameID];

                            }
                            else
                            {
                                water.topMeshGameObject.GetComponent<Renderer>().sortingLayerID = EditorGUILayout.IntField("Sorting Layer", water.topMeshGameObject.GetComponent<Renderer>().sortingLayerID);
                            }
                        }
                        else
                        {
                            water.topMeshGameObject.GetComponent<Renderer>().sortingLayerID = EditorGUILayout.IntField("Sorting Layer", water.topMeshGameObject.GetComponent<Renderer>().sortingLayerID);
                        }
                        water.topMeshGameObject.GetComponent<Renderer>().sortingOrder = EditorGUILayout.IntField("Order in Layer", water.topMeshGameObject.GetComponent<Renderer>().sortingOrder);
                    }
                });
            }


            EditorGUI.indentLevel = 0;
            BoldFontStyle(() =>
            {
                water.showCubeWater = EditorGUILayout.Foldout(water.showCubeWater, "2.5D Water");
            });

            if (water.showCubeWater)
            {
                InspectorBox(10, () =>
                {
                    if (!water.water2DRippleScript)
                        water.cubeWater = EditorGUILayout.Toggle(new GUIContent("Enable 2.5D Water", "Adds a horizontal mesh at the top of the vertical water mesh."), water.cubeWater);

                    if (water.cubeWater && !water.quadMesh)
                    {
                        if (!water.quadMesh)
                            water.squareSegments = EditorGUILayout.Toggle(new GUIContent("TM Square Segments", "When enabled, the top mesh quads will have the same width and length."), water.squareSegments);


                        if (water.squareSegments)
                            water.zSegmentsCap = EditorGUILayout.Toggle(new GUIContent("Z Segments Limit", "Limits the number of segments the top mesh has on the Z axis."), water.zSegmentsCap);


                        if (!water.squareSegments || (water.squareSegments && water.zSegmentsCap))
                            water.zSegmentsSize = Mathf.Clamp(EditorGUILayout.IntField(new GUIContent("Z Segments", "The number of segments, the top water mesh will have."), water.zSegmentsSize), 1, 500);

                    }

                    if (water.cubeWater && water.handlesPosition.Count == 4)
                    {
                        water.handlesPosition.Add(new Vector3(water.handlesPosition[0].x, water.handlesPosition[0].y, water.zSize));
                    }

                    if (!water.cubeWater && water.topMeshGameObject)
                        DestroyImmediate(water.topMeshGameObject.gameObject);

                    if (water.cubeWater)
                        CreateTopMeshObject(water);

                    if (!water.cubeWater && water.handlesPosition.Count > 4)
                        water.handlesPosition.RemoveAt(4);
                });
            }

            Water2D_Simulation water2D_Sim = water.GetComponent<Water2D_Simulation>();

            if (water2D_Sim.waterType == Water2D_Type.Dynamic)
            {
                BoldFontStyle(() =>
                {
                    water.showCollider = EditorGUILayout.Foldout(water.showCollider, "Collider");
                });

                if (water.showCollider && water.createCollider)
                {
                    InspectorBox(10, () =>
                    {
                        if (water.createCollider)
                        {
                            water.colliderYAxisOffset = EditorGUILayout.FloatField(new GUIContent("Collider Top Offset", "Offsets the position of the top edge of the water collider. "
                                + "This is done so that objects that are a little above the waterline are detected."), water.colliderYAxisOffset);

                            if (water.use3DCollider)
                            {
                                water.boxColliderCenterOffset = EditorGUILayout.Vector3Field(new GUIContent("BC Center Offset", "Info."), water.boxColliderCenterOffset);
                                water.boxColliderSizeOffset = EditorGUILayout.Vector3Field(new GUIContent("BC Size Offset", "Info."), water.boxColliderSizeOffset);

                                if (!water.cubeWater)
                                    water.boxColliderZSize = EditorGUILayout.FloatField(new GUIContent("Box Collider Z Size", "The size of the box collider on the Z axis."), water.boxColliderZSize);

                                if (!water.water2DRippleScript)
                                    water.overlapingSphereZOffset = EditorGUILayout.FloatField(new GUIContent("Overlaping Sphere Z Offset", "Info."), water.overlapingSphereZOffset);
                            }
                        }

                    });
                }
            }

            BoldFontStyle(() =>
            {
                water.showInfo = EditorGUILayout.Foldout(water.showInfo, "Mesh Info");
            });

            if (water.showInfo)
            {
                InspectorBox(10, () =>
                {
                    water.showMeshInfo = EditorGUILayout.Toggle("Show Mesh Info", water.showMeshInfo);

                    if (water.showMeshInfo)
                    {
                        if (water.cubeWater)
                        {
                            EditorGUILayout.LabelField(new GUIContent("Top Mesh"), EditorStyles.boldLabel);
                            EditorGUILayout.LabelField(new GUIContent("Mesh Grid"), new GUIContent(water.xVerts.ToString() + " x " + water.zVerts.ToString()));
                            EditorGUILayout.LabelField(new GUIContent("Mesh Vertices", "The number of vertices the top mesh currently has."), new GUIContent((water.zVerts * water.xVerts).ToString()));
                            GUILayout.Space(5);
                            EditorGUILayout.LabelField(new GUIContent("Front Mesh"), EditorStyles.boldLabel);
                            EditorGUILayout.LabelField(new GUIContent("Mesh Grid"), new GUIContent(water.xVerts.ToString() + " x " + 2));
                            EditorGUILayout.LabelField(new GUIContent("Mesh Vertices", "The number of vertices the front mesh currently has."), new GUIContent((water.xVerts * 2).ToString()));

                            if (water.GetComponent<Water2D_Ripple>())
                            {
                                GUILayout.Space(5);
                                EditorGUILayout.LabelField(new GUIContent("Render Texture"), EditorStyles.boldLabel);
                                EditorGUILayout.LabelField(new GUIContent("Resolution", "The resolution of the Render Texture that will be created when the game starts."), new GUIContent(water.renderTextureWidth.ToString() + " x " + water.renderTextureHeight.ToString()));
                            }

                        }
                        else
                        {
                            EditorGUILayout.LabelField(new GUIContent("Front Mesh"), EditorStyles.boldLabel);
                            EditorGUILayout.LabelField(new GUIContent("Mesh Grid"), new GUIContent(water.xVerts.ToString() + " x " + 2));
                            EditorGUILayout.LabelField(new GUIContent("Mesh Vertices", "The number of vertices the front mesh currently has."), new GUIContent((water.xVerts * 2).ToString()));
                        }
                    }
                });
            }
        }

        private void CreateTopMeshObject(Water2D_Tool water)
        {
            if (water.topMeshGameObject == null)
            {
                GameObject obj = new GameObject("new Water2D_TM");
                obj.AddComponent<MeshRenderer>();
                obj.AddComponent<MeshFilter>();
                obj.transform.position = water.transform.position;
                obj.transform.SetParent(water.transform);
                water.topMeshGameObject = obj;
                water.RecreateWaterMesh();
            }
        }

        private void UpdateHandles(Water2D_Tool water2D, GUIStyle iconStyle)
        {
            Quaternion inv = Quaternion.Inverse(water2D.transform.rotation);
            SceneView sv = SceneView.sceneViews[0] as SceneView;

            if (sv.in2DMode)
                Handles.color = new Color(1, 1, 1, 0);
            else
                Handles.color = new Color(1, 1, 1, 1);

            Vector3 global;

            for (int i = 0; i < water2D.handlesPosition.Count; i++)
            {
                Vector3 pos = water2D.transform.position + Vector3.Scale(new Vector3(water2D.handlesPosition[i].x, water2D.handlesPosition[i].y, water2D.handlesPosition[i].z), water2D.transform.localScale);

                if (IsVisible(pos))
                {
                    SetScale(pos, texDot, ref iconStyle);
                    if (sv.in2DMode)
                        Handles.Label(pos, new GUIContent(texDot), iconStyle);
                }

                if (i < 2)
                    global = Handles.Slider(pos, Vector3.up, HandleScale(pos), Handles.CubeHandleCap, 0);
                else
                {
                    if (i == 4)
                        global = Handles.Slider(pos, Vector3.forward, HandleScale(pos), Handles.CubeHandleCap, 0);
                    else
                        global = Handles.Slider(pos, Vector3.right, HandleScale(pos), Handles.CubeHandleCap, 0);

                }

                if (global != pos)
                {
                    selectedPoints.Clear();
                    selectedPoints.Add(i);

                    Vector3 local = inv * (global - water2D.transform.position);

                    Vector3 relative = new Vector3(local.x / water2D.transform.localScale.x, local.y / water2D.transform.localScale.y, local.z / water2D.transform.localScale.z) - water2D.handlesPosition[i];

                    water2D.handlesPosition[selectedPoints[0]] += relative;

                    if (i == 0 || i == 1)
                    {
                        float distance = Mathf.Abs(water2D.handlesPosition[0].y - water2D.handlesPosition[1].y);

                        water2D.waterHeight = distance;
                        water2D.handlesPosition[2] = new Vector2(water2D.handlesPosition[2].x, water2D.handlesPosition[0].y - distance / 2);
                        water2D.handlesPosition[3] = new Vector2(water2D.handlesPosition[3].x, water2D.handlesPosition[0].y - distance / 2);

                        if (water2D.cubeWater)
                            water2D.handlesPosition[4] = new Vector3(water2D.handlesPosition[0].x, water2D.handlesPosition[0].y, water2D.handlesPosition[4].z);

                    }

                    if (i == 2 || i == 3)
                    {
                        float distance = Mathf.Abs(water2D.handlesPosition[2].x - water2D.handlesPosition[3].x);

                        water2D.waterWidth = distance;
                        water2D.handlesPosition[0] = new Vector2(water2D.handlesPosition[2].x + distance / 2, water2D.handlesPosition[0].y);
                        water2D.handlesPosition[1] = new Vector2(water2D.handlesPosition[2].x + distance / 2, water2D.handlesPosition[1].y);


                        if (water2D.cubeWater)
                            water2D.handlesPosition[4] = new Vector3(water2D.handlesPosition[0].x, water2D.handlesPosition[0].y, water2D.handlesPosition[4].z);
                    }

                    if (i == 4)
                    {
                        float distance = Mathf.Abs(water2D.handlesPosition[0].z - water2D.handlesPosition[4].z);
                        water2D.zSize = distance;
                    }
                }
            }

            water2D.curentWaterArea = Mathf.Abs(water2D.handlesPosition[2].x - water2D.handlesPosition[3].x) * Mathf.Abs(water2D.handlesPosition[0].y - water2D.handlesPosition[1].y);
        }


        private void DrawRectangle(Water2D_Tool water2D)
        {
            Handles.color = Color.white;

            for (int i = 2; i < 4; i++)
            {
                Vector3 pos = water2D.transform.position + water2D.transform.rotation * Vector3.Scale(new Vector3(water2D.handlesPosition[i].x, water2D.handlesPosition[0].y, 0), water2D.transform.localScale);
                Vector3 pos2 = water2D.transform.position + water2D.transform.rotation * Vector3.Scale(new Vector3(water2D.handlesPosition[i].x, water2D.handlesPosition[1].y, 0), water2D.transform.localScale);
                Handles.DrawLine(pos, pos2);
            }

            for (int i = 0; i < 2; i++)
            {
                Vector3 pos = water2D.transform.position + water2D.transform.rotation * Vector3.Scale(new Vector3(water2D.handlesPosition[2].x, water2D.handlesPosition[i].y, 0), water2D.transform.localScale);
                Vector3 pos2 = water2D.transform.position + water2D.transform.rotation * Vector3.Scale(new Vector3(water2D.handlesPosition[3].x, water2D.handlesPosition[i].y, 0), water2D.transform.localScale);
                Handles.DrawLine(pos, pos2);
            }
        }

        public static float GetCameraDist(Vector3 aPt)
        {
            return Vector3.Distance(SceneView.lastActiveSceneView.camera.transform.position, aPt);
        }

        public bool IsVisible(Vector3 aPos)
        {
            Transform t = SceneView.lastActiveSceneView.camera.transform;
            if (Vector3.Dot(t.forward, aPos - t.position) > 0)
                return true;
            return false;
        }

        public void SetScale(Vector3 aPos, Texture aIcon, ref GUIStyle aStyle, float aScaleOverride = 1)
        {
            float max = (Screen.width + Screen.height) / 2;
            float dist = SceneView.lastActiveSceneView.camera.orthographic ? SceneView.lastActiveSceneView.camera.orthographicSize / 0.5f : GetCameraDist(aPos);

            float div = (dist / (max / 160));
            float mul = pathScale * aScaleOverride;

            aStyle.fixedWidth = (aIcon.width / div) * mul;
            aStyle.fixedHeight = (aIcon.height / div) * mul;
        }

        public static float HandleScale(Vector3 aPos)
        {
            float dist = SceneView.lastActiveSceneView.camera.orthographic ? SceneView.lastActiveSceneView.camera.orthographicSize / 0.45f : GetCameraDist(aPos);
            return Mathf.Min(0.4f * pathScale, (dist / 5.0f) * 0.4f * pathScale);
        }

        public void InspectorBox(int aBorder, System.Action inside, int aWidthOverride = 0, int aHeightOverride = 0)
        {
            Rect r = EditorGUILayout.BeginHorizontal(GUILayout.Width(aWidthOverride));
            if (aWidthOverride != 0)
            {
                r.width = aWidthOverride;
            }
            GUI.Box(r, GUIContent.none);
            GUILayout.Space(aBorder);
            if (aHeightOverride != 0)
                EditorGUILayout.BeginVertical(GUILayout.Height(aHeightOverride));
            else
                EditorGUILayout.BeginVertical();
            GUILayout.Space(aBorder);
            inside();
            GUILayout.Space(aBorder);
            EditorGUILayout.EndVertical();
            GUILayout.Space(aBorder);
            EditorGUILayout.EndHorizontal();
        }

        private static Texture2D GetGizmo(string aFileName)
        {
            Texture2D tex = AssetDatabase.LoadAssetAtPath("Assets/Water2D_Tool/Assets/Gizmos/" + aFileName, typeof(Texture2D)) as Texture2D;
            if (tex == null)
            {
                tex = EditorGUIUtility.whiteTexture;
                Debug.Log("Couldn't load Gizmo texture. This happened most likely because you moved Water2D_Tool "
                    + "from the Assets folder to a subfolder. Click on this message to edit the path to the texture" + aFileName);
            }
            return tex;
        }

        public void BoldFontStyle(System.Action inside)
        {
            GUIStyle style = EditorStyles.foldout;
            FontStyle previousStyle = style.fontStyle;
            style.fontStyle = FontStyle.Bold;
            inside();
            style.fontStyle = previousStyle;
        }
    }
}
