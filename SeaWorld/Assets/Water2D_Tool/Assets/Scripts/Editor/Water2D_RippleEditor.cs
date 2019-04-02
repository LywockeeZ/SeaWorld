using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Water2DTool {
    [CustomEditor(typeof(Water2D_Ripple))]
    public class Water2D_RippleEditor : Editor {
        public override void OnInspectorGUI()
        {
            Undo.RecordObject(target, "Modified Inspector");

            Water2D_Ripple water2D_Ripple = (Water2D_Ripple)target;

            CustomInspector(water2D_Ripple);
        }

        private void CustomInspector(Water2D_Ripple water2D_Ripple)
        {
            Water2D_Tool water2D = water2D_Ripple.GetComponent<Water2D_Tool>();
            Water2D_Simulation water2DSim = water2D_Ripple.GetComponent<Water2D_Simulation>();

            BoldFontStyle(() =>
            {
                water2D_Ripple.showWaterRipple = EditorGUILayout.Foldout(water2D_Ripple.showWaterRipple, "Water Properties");
            });

            if (water2D_Ripple.showWaterRipple)
            {
                InspectorBox(10, () =>
                {
                    EditorGUI.BeginChangeCheck();
                    water2D_Ripple.rippleSimulationUpdate = (Water2D_RippleSimulationUpdate)EditorGUILayout.EnumPopup(new GUIContent("Run Simulation In"), water2D_Ripple.rippleSimulationUpdate);
                    if (water2D_Ripple.rippleSimulationUpdate == Water2D_RippleSimulationUpdate.FixedUpdateMethod)
                        EditorGUILayout.HelpBox("For smooth looking wave movement it is recommended to use Fixed Update Method with physics based character controllers.", MessageType.Info);

                    if (water2D_Ripple.rippleSimulationUpdate == Water2D_RippleSimulationUpdate.UpdateMethod)
                        EditorGUILayout.HelpBox("For smooth looking wave movement it is recommended to use Update Method with raycast based character controllers.", MessageType.Info);

                    if (EditorGUI.EndChangeCheck() && Application.isPlaying)
                    {
                        water2D_Ripple.rainTimeCounter = 0;
                        water2D_Ripple.heightMapTimeCounter = 0;
                    }

                    if (water2D_Ripple.rippleSimulationUpdate == Water2D_RippleSimulationUpdate.UpdateMethod)
                    {
                        EditorGUI.BeginChangeCheck();
                        water2D_Ripple.rippleWaterFPSCap = EditorGUILayout.Slider(new GUIContent("Simulation Speed", "Determines how many times per second should the height map be processed through the shader that simulates ripple propagation."), water2D_Ripple.rippleWaterFPSCap, 1, 120);

                        if (EditorGUI.EndChangeCheck())
                        {
                            water2D_Ripple.heightUpdateMapTimeStep = 1f / water2D_Ripple.rippleWaterFPSCap;
                            water2D_Ripple.heightMapTimeCounter = 0;
                            water2D_Ripple.interactionsTimeCounter = 0;
                        }
                    }

                    EditorGUI.BeginChangeCheck();
                    water2D_Ripple.waterDamping = EditorGUILayout.Slider(new GUIContent("Damping", "Damping parameter for the water propagation simulation."), water2D_Ripple.waterDamping, 0, 1);
                    if (EditorGUI.EndChangeCheck() && Application.isPlaying)
                    {
                        water2D_Ripple.UpdateRippleShaderParameters();
                    }

                    EditorGUI.BeginChangeCheck();
                    water2D_Ripple.rtPixelsToUnits = EditorGUILayout.IntField(new GUIContent("RT Pixels To Units", "The number of Render Texture pixels that should fit in 1 unit of Unity space."), water2D_Ripple.rtPixelsToUnits);
                    if (EditorGUI.EndChangeCheck())
                    {
                        water2D.RecreateWaterMesh();
                    }

                    water2D_Ripple.bicubicResampling = EditorGUILayout.Toggle(new GUIContent("Bicubic Resampling", "Applies a smoothing effect to the heightmap."
                        + " Eliminates pixelation artifacts. This makes the generated normal map look smoother."), water2D_Ripple.bicubicResampling);

                    EditorGUI.BeginChangeCheck();
                    water2D_Ripple.drawBothSides = EditorGUILayout.Toggle(new GUIContent("Draw Both Sides", "Enabling this will make both sides of the top mesh to be drawn when the camera is below the water line."), water2D_Ripple.drawBothSides);
                    if (EditorGUI.EndChangeCheck() && Application.isPlaying)
                    {
                        water2D_Ripple.SetTopMeshCulling();
                    }

                    EditorGUI.BeginChangeCheck();
                    water2D_Ripple.waveHeightScale = EditorGUILayout.FloatField(new GUIContent("Wave Height Scale", "The scale of the ripples created by objects or rain."), water2D_Ripple.waveHeightScale);
                    if (EditorGUI.EndChangeCheck())
                    {
                        if (Application.isPlaying)
                        {
                            water2D_Ripple.SetWaveHeightScale();
                        }else
                        {
                            water2D.GetComponent<Renderer>().sharedMaterial.SetFloat("_WaveHeightScale", water2D_Ripple.waveHeightScale);
                            water2D.topMeshGameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_WaveHeightScale", water2D_Ripple.waveHeightScale);
                        }
                    }

                    EditorGUI.BeginChangeCheck();
                    water2D_Ripple.obstructionType = (Water2D_ObstructionType)EditorGUILayout.EnumPopup(new GUIContent("Obstruction Type", "List of obstruction methods."), water2D_Ripple.obstructionType);
                    if (EditorGUI.EndChangeCheck() && Application.isPlaying)
                    {
                        water2D_Ripple.UpdateRippleShaderParameters();
                    }

                    if (water2D_Ripple.obstructionType == Water2D_ObstructionType.DynamicObstruction && !water2D.use3DCollider)
                        EditorGUILayout.HelpBox("Dynamic Obstruction only works with objects that have a Box Collider or a Sphere Collider", MessageType.Warning);
                    if (water2D_Ripple.obstructionType == Water2D_ObstructionType.DynamicObstruction)
                        water2D_Ripple.obstructionLayers = (LayerMask)EditorGUILayout.MaskField("Obstruction Layers", water2D_Ripple.obstructionLayers, InternalEditorUtility.layers);

                    if (water2D_Ripple.obstructionType == Water2D_ObstructionType.TextureObstruction)
                        water2D_Ripple.obstructionTexture = EditorGUILayout.ObjectField(new GUIContent("Obstruction Texture"), water2D_Ripple.obstructionTexture, typeof(Texture2D), true) as Texture2D;
                });
            }

            BoldFontStyle(() =>
            {
                water2D_Ripple.showDynamicObjects = EditorGUILayout.Foldout(water2D_Ripple.showDynamicObjects, "Dynamic Objects");
            });

            if (water2D_Ripple.showDynamicObjects)
            {
                InspectorBox(10, () =>
                {
                    if (water2D.use3DCollider)
                        water2D_Ripple.fixedRadius = EditorGUILayout.Toggle(new GUIContent("Fixed Radius", "When enabled, the radius of the ripple created by dynamic objects will be the same for all objects. "
                            + "When disabled the radius of the ripple depends on the collider of that object."), water2D_Ripple.fixedRadius);
                    if (!water2D.use3DCollider && !water2D_Ripple.fixedRadius)
                        water2D_Ripple.fixedRadius = true;

                    if (water2D_Ripple.fixedRadius)
                        water2D_Ripple.objectRippleRadius = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Radius", "The radius of the ripple created by a dynamic object."), water2D_Ripple.objectRippleRadius), 0.000001f, 100f);
                    if (!water2D_Ripple.fixedRadius)
                        water2D_Ripple.objectRadiusScale = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Radius Scale", "Scales up or down the ripple radius. "
                            + "The radius of the ripple created by dynamic objects depends on the size of that objects collider."), water2D_Ripple.objectRadiusScale), 0.000001f, 1000f);
                    water2D_Ripple.objectRippleStrength = EditorGUILayout.FloatField(new GUIContent("Strength", "The strength of the ripple created by dynamic objects."), water2D_Ripple.objectRippleStrength);
                    water2D_Ripple.strengthScale = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Y Axis Strength Scale", "Used to scale up or down the strength of the ripples " +
                        "created by dynamic objects when the Abs value of the velocity on the Y axis is greater than the Abs value of the velocity on the X axis."), water2D_Ripple.strengthScale), 0.000001f, 1000f);
                    water2D_Ripple.objectVelocityFilter = EditorGUILayout.FloatField(new GUIContent("Velocity Filter", "If the Abs value of a dynamic object velocity on the X and Y axis is smaller than the value of this field, no ripples will be generated by that object."), water2D_Ripple.objectVelocityFilter);
                    water2D_Ripple.objectXAxisRippleOffset = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("X Axis Relative Offset", "Offsets the ripple position on the X axis based on the width of the colliders bounding box. A value of 0 means that the ripple will be generated at the center of the collider. " +
                        " A value of 0.5f means that the ripple will be positioned at the left or right edge of the colliders bounding box."), water2D_Ripple.objectXAxisRippleOffset), 0, 100f);

                });
            }

            EditorGUI.indentLevel = 0;

            BoldFontStyle(() =>
            {
                water2D_Ripple.showPlayer = EditorGUILayout.Foldout(water2D_Ripple.showPlayer, "Player");
            });

            if (water2D_Ripple.showPlayer)
            {
                InspectorBox(10, () =>
                {
                    water2D_Ripple.playerRippleRadius = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Radius", "The radius of the ripple created by the player."), water2D_Ripple.playerRippleRadius), 0.0000001f, 1000f);
                    water2D_Ripple.playerRippleStrength = EditorGUILayout.FloatField(new GUIContent("Strength", "The strength of the ripple created by the player."), water2D_Ripple.playerRippleStrength);
                    if (water2DSim.characterControllerType == Water2D_CharacterControllerType.PhysicsBased)
                        water2D_Ripple.playerVelocityFilter = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Velocity Filter", "If the Abs value of the player velocity on the X and Y axis is smaller than the value of this field, no ripples will be generated by the player."), water2D_Ripple.playerVelocityFilter), 0.0000001f, 100f);
                    water2D_Ripple.playerRippleXOffset = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("X Axis Position Offset", "Offsets the position of the ripple created by the player on the X axis."), water2D_Ripple.playerRippleXOffset), 0, 1000f);
                });
            }

            EditorGUI.indentLevel = 0;

            BoldFontStyle(() =>
            {
                water2D_Ripple.showRain = EditorGUILayout.Foldout(water2D_Ripple.showRain, "Rain");
            });

            if (water2D_Ripple.showRain)
            {
                InspectorBox(10, () =>
                {
                    water2D_Ripple.rainDrops = EditorGUILayout.Toggle(new GUIContent("Enable Rain", "Enables the simulation of rain."), water2D_Ripple.rainDrops);

                    if (water2D_Ripple.rainDrops)
                    {
                        water2D_Ripple.rainDropRadius = EditorGUILayout.FloatField(new GUIContent("Drop Radius", "The radius of a rain drop in Unity space."), water2D_Ripple.rainDropRadius);
                        water2D_Ripple.rainDropStrength = EditorGUILayout.FloatField(new GUIContent("Drop Strength", "The strength of a rain drop."), water2D_Ripple.rainDropStrength);
                        water2D_Ripple.rainDropFrequency = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Drop Frequency", "The number of water drops that should fall in a second."), water2D_Ripple.rainDropFrequency), 0.0001f, 1000f);
                    }
                });
            }

            BoldFontStyle(() =>
            {
                water2D_Ripple.showRippleSourcesList = EditorGUILayout.Foldout(water2D_Ripple.showRippleSourcesList, "Ripple Sources");
            });

            if (water2D_Ripple.showRippleSourcesList)
            {
                InspectorBox(10, () =>
                {
                    water2D_Ripple.rippleSources = EditorGUILayout.Toggle(new GUIContent("Enable Ripple Sources"), water2D_Ripple.rippleSources);

                    if (water2D_Ripple.rippleSources)
                    {
                        water2D_Ripple.rippleSourcesSize = Mathf.Clamp(EditorGUILayout.IntField(new GUIContent("Size"), water2D_Ripple.rippleSourcesSize), 1, 200);
                        if (water2D_Ripple.rippleSourcesList == null)
                            water2D_Ripple.rippleSourcesList.Add(null);

                        if (water2D_Ripple.rippleSourcesSize != water2D_Ripple.rippleSourcesList.Count)
                        {
                            if (water2D_Ripple.rippleSourcesSize > water2D_Ripple.rippleSourcesList.Count)
                            {
                                while (water2D_Ripple.rippleSourcesSize > water2D_Ripple.rippleSourcesList.Count)
                                {
                                    water2D_Ripple.rippleSourcesList.Add(null);
                                }
                            }
                            else
                            {
                                while (water2D_Ripple.rippleSourcesSize < water2D_Ripple.rippleSourcesList.Count)
                                {
                                    int last = water2D_Ripple.rippleSourcesList.Count - 1;
                                    water2D_Ripple.rippleSourcesList.RemoveAt(last);
                                }
                            }
                        }

                        for (int i = 0; i < water2D_Ripple.rippleSourcesList.Count; i++)
                        {
                            water2D_Ripple.rippleSourcesList[i] = EditorGUILayout.ObjectField(new GUIContent("Element " + i), water2D_Ripple.rippleSourcesList[i], typeof(RippleSource), true) as RippleSource;
                        }
                    }
                });
            }

            if (water2D.use3DCollider)
            {
                BoldFontStyle(() =>
                {
                    water2D_Ripple.showMouse = EditorGUILayout.Foldout(water2D_Ripple.showMouse, "Mouse Interaction");
                });

                if (water2D_Ripple.showMouse)
                {
                    InspectorBox(10, () =>
                    {
                        water2D_Ripple.mouseInteraction = EditorGUILayout.Toggle(new GUIContent("Enable Mouse Interaction", "Enables the ability to interact with the water using the mouse arrow."), water2D_Ripple.mouseInteraction);

                        if (water2D_Ripple.mouseInteraction)
                        {
                            water2D_Ripple.mouseRadius = EditorGUILayout.FloatField(new GUIContent("Mouse Radius", "The radius of the ripple created by the mouse arrow, in Unity space."), water2D_Ripple.mouseRadius);
                            water2D_Ripple.mouseStregth = EditorGUILayout.FloatField(new GUIContent("Mouse Strength", "The strength of the ripple created by the mouse arrow."), water2D_Ripple.mouseStregth);
                        }
                    });
                }
            }

            BoldFontStyle(() =>
            {
                water2D_Ripple.showAmbiantWaves = EditorGUILayout.Foldout(water2D_Ripple.showAmbiantWaves, "Ambient Waves");
            });

            if (water2D_Ripple.showAmbiantWaves)
            {
                InspectorBox(10, () =>
                {

                    EditorGUI.BeginChangeCheck();
                    water2D_Ripple.ambientWaves = EditorGUILayout.Toggle(new GUIContent("Enable Ambient Waves", "Enable this to simulate waves created by the wind."), water2D_Ripple.ambientWaves);

                    if (water2D_Ripple.ambientWaves)
                    {
                        water2D_Ripple.amplitudeZAxisFade = EditorGUILayout.Toggle(new GUIContent("Z Axis Amplitude Fade", "When enabled the amplitude of the sine waves will fade along the Z axis."), water2D_Ripple.amplitudeZAxisFade);

                        if (water2D_Ripple.amplitudeZAxisFade)
                        {
                            water2D_Ripple.amplitudeFadeStart = EditorGUILayout.Slider(new GUIContent("Fade Start", "The start point of the amplitude fade. Can also be used to set the direction of the amplitude fade. If the start point value is greater than the end point, the amplitude will fade from back to front."), water2D_Ripple.amplitudeFadeStart, 0, 1.0f);
                            water2D_Ripple.amplitudeFadeEnd = EditorGUILayout.Slider(new GUIContent("Fade End", "The end point of the amplitude fade. Can also be used to set the direction of the amplitude fade. If the end point value is greater than the start point, the amplitude will fade towards the back of the water."), water2D_Ripple.amplitudeFadeEnd, 0, 1.0f);
                        }

                        EditorGUILayout.LabelField(new GUIContent("Wave 1"), EditorStyles.boldLabel);

                        water2D_Ripple.amplitude1 = EditorGUILayout.FloatField(new GUIContent("Amplitude", "Sine wave amplitude. The bigger the value the higher the wave top will be."), water2D_Ripple.amplitude1);
                        water2D_Ripple.waveLength1 = EditorGUILayout.FloatField(new GUIContent("Wave Length", "The distance between 2 consecutive points of a sine wave."), water2D_Ripple.waveLength1);
                        water2D_Ripple.phaseOffset1 = EditorGUILayout.FloatField(new GUIContent("Phase Offset", "Sine wave phase offset. The bigger the value of the phase offset the faster the waves move to the left (right)."), water2D_Ripple.phaseOffset1);

                        EditorGUILayout.LabelField(new GUIContent("Wave 2"), EditorStyles.boldLabel);

                        water2D_Ripple.amplitude2 = EditorGUILayout.FloatField(new GUIContent("Amplitude", "Sine wave amplitude. The bigger the value the higher the wave top will be."), water2D_Ripple.amplitude2);
                        water2D_Ripple.waveLength2 = EditorGUILayout.FloatField(new GUIContent("Wave Length", "The distance between 2 consecutive points of a sine wave."), water2D_Ripple.waveLength2);
                        water2D_Ripple.phaseOffset2 = EditorGUILayout.FloatField(new GUIContent("Phase Offset", "Sine wave phase offset. The bigger the value of the phase offset the faster the waves move to the left (right)."), water2D_Ripple.phaseOffset2);

                        EditorGUILayout.LabelField(new GUIContent("Wave 3"), EditorStyles.boldLabel);

                        water2D_Ripple.amplitude3 = EditorGUILayout.FloatField(new GUIContent("Amplitude", "Sine wave amplitude. The bigger the value the higher the wave top will be."), water2D_Ripple.amplitude3);
                        water2D_Ripple.waveLength3 = EditorGUILayout.FloatField(new GUIContent("Wave Length", "The distance between 2 consecutive points of a sine wave."), water2D_Ripple.waveLength3);
                        water2D_Ripple.phaseOffset3 = EditorGUILayout.FloatField(new GUIContent("Phase Offset", "Sine wave phase offset. The bigger the value of the phase offset the faster the waves move to the left (right)."), water2D_Ripple.phaseOffset3);
                    }
                    if (EditorGUI.EndChangeCheck() && Application.isPlaying)
                    {
                        water2D_Ripple.SetAmbientWavesShaderParameters();
                    }
                });
            }
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
