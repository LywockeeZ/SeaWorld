using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Water2DTool
{
    [CustomEditor(typeof(Water2D_Simulation))]
    public class Water2D_SimulationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Undo.RecordObject(target, "Modified Inspector");

            Water2D_Simulation water2D_Sim = (Water2D_Simulation)target;

            CustomInspector(water2D_Sim);
        }

        private void CustomInspector(Water2D_Simulation water2D_Sim)
        {
            Water2D_Ripple rippleScript = water2D_Sim.GetComponent<Water2D_Ripple>();

            if (!rippleScript)
            {
                BoldFontStyle(() =>
                {
                    water2D_Sim.showSpringProperties = EditorGUILayout.Foldout(water2D_Sim.showSpringProperties, "Spring");
                });
            }

            if (water2D_Sim.showSpringProperties && !rippleScript)
            {
                InspectorBox(10, () =>
                {
                    water2D_Sim.springSimulation = EditorGUILayout.Toggle(new GUIContent("Spring Simulation", "Enables the simulation of springs. This what makes the surface of the water react to objects."), water2D_Sim.springSimulation);

                    if (water2D_Sim.springSimulation)
                    {
                        water2D_Sim.springConstant = EditorGUILayout.FloatField(new GUIContent("Spring Constant", "This value controls the stiffness of the springs. "
                            + "A low spring constant will make the springs loose. This means a force will cause large waves that oscillate slowly. A high spring "
                            + "constant will increase the tension in the spring. Forces will create small waves that oscillate quickly."), water2D_Sim.springConstant);

                        water2D_Sim.damping = EditorGUILayout.FloatField(new GUIContent("Damping", "The damping slows down the oscillation of the springs. "
                            + " A high dampening value will make the water look thick like molasses, while a low value will allow the waves to oscillate for a long time."), water2D_Sim.damping);

                        water2D_Sim.spread = EditorGUILayout.FloatField(new GUIContent("Spread", "Controls how fast the waves spread."), water2D_Sim.spread);

                        water2D_Sim.collisionVelocityScale = EditorGUILayout.FloatField(new GUIContent("Collision Velocity", "Limits the velocity "
                            + " a spring will receive from a falling object."), water2D_Sim.collisionVelocityScale);

                        water2D_Sim.waveSpeed = EditorGUILayout.FloatField(new GUIContent("Wave Speed", "Another variable to control the spread speed of the waves."), water2D_Sim.waveSpeed);

                        Water2D_Tool water2D_Tool = water2D_Sim.GetComponent<Water2D_Tool>();
                        if (water2D_Tool.use3DCollider)
                            water2D_Sim.overlapSphereRadius = EditorGUILayout.FloatField(new GUIContent("Overlap Sphere Radius", "The radius of a sphere that will be used to check "
                                + " if there is a 3D collider near a surface vertex."), water2D_Sim.overlapSphereRadius);

                        water2D_Sim.interactionTime = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Spring Simulation Time", "How many seconds after an object interacted with the water surface and generated a ripple, should the spring simulation stop updating."), water2D_Sim.interactionTime), 0.00001f, 100);
                    }
                });
            }

            if (water2D_Sim.waterType == Water2D_Type.Dynamic)
            {
                BoldFontStyle(() =>
                {
                    water2D_Sim.showfloatingBuoyantForce = EditorGUILayout.Foldout(water2D_Sim.showfloatingBuoyantForce, "Buoyancy");
                });

                if (water2D_Sim.showfloatingBuoyantForce)
                {
                    InspectorBox(10, () =>
                    {
                        water2D_Sim.buoyantForceMode = (Water2D_BuoyantForceMode)EditorGUILayout.EnumPopup(new GUIContent("Buoyant Force", "List of methods to simulate the buoyant force. "
                            + "This is what makes the objects float in the water"), water2D_Sim.buoyantForceMode);

                        if (water2D_Sim.buoyantForceMode != Water2D_BuoyantForceMode.None)
                        {
                            if (water2D_Sim.buoyantForceMode == Water2D_BuoyantForceMode.Linear)
                            {
                                water2D_Sim.floatHeight = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Float Height", "Determines how much force should be applied to an object submerged "
                                    + "in the water. A value of 3 means that 3 m under the water the force applied to an object will be 2 times greater than the force applied at the "
                                    + "surface of the water."), water2D_Sim.floatHeight), 0.0001f, 100);

                                water2D_Sim.bounceDamping = EditorGUILayout.FloatField(new GUIContent("Bounce Damping", "Slows down the vertical oscillation of the object."), water2D_Sim.bounceDamping);

                                water2D_Sim.liniarBFDragCoefficient = EditorGUILayout.FloatField(new GUIContent("Drag Coefficient", "Determines how much drag force should be applied to an object."), water2D_Sim.liniarBFDragCoefficient);

                                water2D_Sim.liniarBFAbgularDragCoefficient = EditorGUILayout.FloatField(new GUIContent("Angular Drag Coefficient", "Slow down the angular rotation of the object."), water2D_Sim.liniarBFAbgularDragCoefficient);

                                water2D_Sim.forceScale = EditorGUILayout.FloatField(new GUIContent("Force Scale", "A value of 1 will make an object with the mass of "
                                    + " 1kg float at the surface of the water and an object with the mass of 2kg float 3m below the water surface if Float Height "
                                    + "is set to 3m."), water2D_Sim.forceScale);

                                water2D_Sim.forcePositionOffset = EditorGUILayout.Vector3Field(new GUIContent("Force Position Offset", "By default the force will "
                                    + " be applied at the center of the object. Use this to offset the position where the force will be applied to an object."), water2D_Sim.forcePositionOffset);
                            }
                            else
                            {
                                water2D_Sim.clippingMethod = (Water2D_ClippingMethod)EditorGUILayout.EnumPopup(new GUIContent("Polygon Clipping", "Determines which clipping method will be used to calculate the shape of the polygon that is below the water. "
                                    + "The simple clipping is the cheapest option in terms of performance because the clipping polygon is always a horizontal line."
                                    + "The complex option is best to use when you want the objects to better react to water waves."), water2D_Sim.clippingMethod);

                                if (water2D_Sim.clippingMethod == Water2D_ClippingMethod.Complex)
                                {
                                    water2D_Sim.showClippingPlolygon = EditorGUILayout.Toggle(new GUIContent("Show Clipping Polygon", "When enabled will show in the Scene View the shape of the clipping polygon."), water2D_Sim.showClippingPlolygon);

                                    water2D_Sim.meshSegmentsPerWaterLineSegment = Mathf.Clamp(EditorGUILayout.IntField(new GUIContent("Water Line Segments", "The number of vertical mesh segments that should fit in a water line segment."), water2D_Sim.meshSegmentsPerWaterLineSegment), 1, 1000);
                                }

                                water2D_Sim.polygonCorners = Mathf.Clamp(EditorGUILayout.IntField(new GUIContent("Polygon Vertices", "When an object with a circleCollider2D "
                                    + "is detected an imaginary regular polygon collider is created based on its radius and position. "
                                    + "Use this to set the number of vertices the regular polygon collider should have."), water2D_Sim.polygonCorners), 4, 100);

                                water2D_Sim.dragCoefficient = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Drag Coefficient", "Determines how much drag force should be applied to an object."), water2D_Sim.dragCoefficient), 0, 500);
                                water2D_Sim.liftCoefficient = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Lift Coefficient", "Determines how much lift force should be applied to an object."), water2D_Sim.liftCoefficient), 0, 500);

                                water2D_Sim.waterDensity = EditorGUILayout.FloatField(new GUIContent("Water Density", "Sets the water density. In a water with low "
                                    + " density the objects will submerge faster and come to the surface slower. If the water density is great the objects will "
                                    + "stay more at the surface of the water and will submerge slower."), water2D_Sim.waterDensity);

                                water2D_Sim.showPolygon = EditorGUILayout.Toggle(new GUIContent("Show Polygon Shape", "When enabled will show in the Scene View the shape of the polygon that is below the waterline."), water2D_Sim.showPolygon);
                                water2D_Sim.showForces = EditorGUILayout.Toggle(new GUIContent("Show Forces", "When enabled will show in the Scene View the velocity direction, drag direction, "
                                    + "lift direction and the normal of a leading edge."), water2D_Sim.showForces);
                            }
                        }
                    });
                }
            }

            BoldFontStyle(() =>
            {
                water2D_Sim.showFlow = EditorGUILayout.Foldout(water2D_Sim.showFlow, "Flow");
            });

            if (water2D_Sim.showFlow)
            {
                InspectorBox(10, () =>
                {
                    water2D_Sim.waterFlow = EditorGUILayout.Toggle(new GUIContent("Water Flow", "When enabled, the water flow will affect the objects in the water."), water2D_Sim.waterFlow);
                    if (water2D_Sim.waterFlow)
                    {
                        water2D_Sim.useAngles = EditorGUILayout.Toggle(new GUIContent("Use Angles", "Enable this if you want to control the direction of the water flow using custom angle values."), water2D_Sim.useAngles);

                        if (!water2D_Sim.useAngles)
                            water2D_Sim.flowDirection = (Water2D_FlowDirection)EditorGUILayout.EnumPopup(new GUIContent("Flow Direction", "The direction of the water flow."), water2D_Sim.flowDirection);

                        if (water2D_Sim.useAngles)
                            water2D_Sim.flowAngle = EditorGUILayout.FloatField(new GUIContent("Flow Angle", "The angle of the water flow. " + "When set to 0 degrees the objects will be pushed to the left, "
                                + "when set to 90 degrees the objects will be pushed down, when set to 180 degrees the objects will "
                                + "be pushed to the right, when set to 270 degrees the objects will be pushed up."), water2D_Sim.flowAngle);

                        water2D_Sim.waterFlowForce = EditorGUILayout.FloatField(new GUIContent("Flow Force", "The force of the water flow."), water2D_Sim.waterFlowForce);
                    }
                });
            }

            BoldFontStyle(() =>
            {
                water2D_Sim.showAnimation = EditorGUILayout.Foldout(water2D_Sim.showAnimation, "Animation");
            });

            if (water2D_Sim.showAnimation)
            {
                InspectorBox(10, () =>
                {
                    water2D_Sim.animationMethod = (Water2D_AnimationMethod)EditorGUILayout.EnumPopup(new GUIContent("Animation Method", "Determines the animation method for the handles position."), water2D_Sim.animationMethod);

                    if (water2D_Sim.animationMethod != Water2D_AnimationMethod.None)
                    {
                        if (!rippleScript)
                            water2D_Sim.animateWaterArea = EditorGUILayout.Toggle(new GUIContent("Animate Water Area", "Enable this "
                                + "if you want to animate the increase or decrease of the total water area."), water2D_Sim.animateWaterArea);

                        if (!water2D_Sim.animateWaterArea)
                        {
                            water2D_Sim.topEdge = EditorGUILayout.ObjectField(new GUIContent("Top Edge", "Place here an animated object "
                                + "you want the water line (the top of the water) to follow."), water2D_Sim.topEdge, typeof(Transform), true) as Transform;
                        }

                        if (!rippleScript)
                        {
                            water2D_Sim.bottomEdge = EditorGUILayout.ObjectField(new GUIContent("Bottom Edge", "Place here an animated object "
                                + "you want the bottom edge of the water to follow."), water2D_Sim.bottomEdge, typeof(Transform), true) as Transform;

                            water2D_Sim.leftEdge = EditorGUILayout.ObjectField(new GUIContent("Left Edge", "Place here an animated object you "
                                + " want the left edge of the water to follow."), water2D_Sim.leftEdge, typeof(Transform), true) as Transform;

                            water2D_Sim.rightEdge = EditorGUILayout.ObjectField(new GUIContent("Right Edge", "Place here an animated object "
                                + "you want the right edge of the water to follow."), water2D_Sim.rightEdge, typeof(Transform), true) as Transform;
                        }

                        if (water2D_Sim.animationMethod == Water2D_AnimationMethod.Snap)
                        {
                            if (!water2D_Sim.animateWaterArea)
                                water2D_Sim.topEdgeYOffset = EditorGUILayout.FloatField(new GUIContent("Top Edge Y Offset", "The offset on the Y axis from the position of a referenced object."), water2D_Sim.topEdgeYOffset);
                            if (!rippleScript)
                            {
                                water2D_Sim.bottomEdgeYOffset = EditorGUILayout.FloatField(new GUIContent("Bottom Edge Y Offset", "The offset on the Y axis from the position of a referenced object."), water2D_Sim.bottomEdgeYOffset);
                                water2D_Sim.leftEdgeXOffset = EditorGUILayout.FloatField(new GUIContent("Left Edge X Offset", "The offset on the X axis from the position of a referenced object."), water2D_Sim.leftEdgeXOffset);
                                water2D_Sim.rightEdgeXOffset = EditorGUILayout.FloatField(new GUIContent("Right Edge X Offset", "The offset on the X axis from the position of a referenced object."), water2D_Sim.rightEdgeXOffset);
                            }
                        }

                        if (water2D_Sim.animateWaterArea && water2D_Sim.topEdge != null)
                            water2D_Sim.topEdge = null;
                    }
                });
            }

            if (!rippleScript)
            {
                BoldFontStyle(() =>
                {
                    water2D_Sim.showSurfaceWaves = EditorGUILayout.Foldout(water2D_Sim.showSurfaceWaves, "Surface Waves");
                });
            }

            if (water2D_Sim.showSurfaceWaves && !rippleScript)
            {
                InspectorBox(10, () =>
                {
                    water2D_Sim.surfaceWaves = (Water2D_SurfaceWaves)EditorGUILayout.EnumPopup(new GUIContent("Surface Waves", "List of methods to generate surface waves. Random"
                        + " method generates small random splashes. Sine wave method overlaps a number of sine waves to  get a final wave that changes the velocity of the surface vertices."), water2D_Sim.surfaceWaves);

                    if (water2D_Sim.surfaceWaves != Water2D_SurfaceWaves.None)
                    {
                        if (water2D_Sim.surfaceWaves == Water2D_SurfaceWaves.SineWaves)
                        {
                            water2D_Sim.sineWavesType = (Water2D_SineWaves)EditorGUILayout.EnumPopup(new GUIContent("Sine Waves", "The type of sine waves to use. If you want to animate the amplitude and stretch of the sine wave use the Single Sine Wave option."), water2D_Sim.sineWavesType);

                            if (water2D_Sim.sineWavesType == Water2D_SineWaves.MultipleSineWaves)
                                water2D_Sim.randomValues = EditorGUILayout.Toggle(new GUIContent("Random Values", "When enabled, the amplitude, stretch and phase offset will be random values for each sine wave."), water2D_Sim.randomValues);

                            if (water2D_Sim.sineWavesType == Water2D_SineWaves.MultipleSineWaves)
                            {
                                water2D_Sim.sineWaves = Mathf.Clamp(EditorGUILayout.IntField(new GUIContent("Sine Waves Number", "The number of individual sine waves."), water2D_Sim.sineWaves), 1, 100);

                                if (water2D_Sim.randomValues)
                                {
                                    water2D_Sim.maxAmplitude = EditorGUILayout.FloatField(new GUIContent("Max Amplitude", "The constant is used to generate a random amplitude value between a Max and a Min. Controls the height of the sine wave."), water2D_Sim.maxAmplitude);
                                    water2D_Sim.minAmplitude = EditorGUILayout.FloatField(new GUIContent("Min Amplitude", "The constant is used to generate a random amplitude value between a Max and a Min. Controls the height of the sine wave."), water2D_Sim.minAmplitude);

                                    water2D_Sim.maxStretch = EditorGUILayout.FloatField(new GUIContent("Max Stretch", "The constant is used to generate a random sine wave stretch value between a Max and a Min. "
                                        + "The bigger the value of the stretch the more compact the waves are."), water2D_Sim.maxStretch);
                                    water2D_Sim.minStretch = EditorGUILayout.FloatField(new GUIContent("Min Stretch", "The constant is used to generate a random sine wave stretch value between a Max and a Min. "
                                        + "The bigger the value of the stretch the more compact the waves are."), water2D_Sim.minStretch);

                                    water2D_Sim.maxPhaseOffset = EditorGUILayout.FloatField(new GUIContent("Max Phase Offset", "The constant is used to generate a random phase offset value between a Max and a Min. "
                                        + "The bigger the value of the phase offset the faster the waves move to the left (right)."), water2D_Sim.maxPhaseOffset);
                                    water2D_Sim.minPhaseOffset = EditorGUILayout.FloatField(new GUIContent("Min Phase Offset", "The constant is used to generate a random phase offset value between a Max and a Min. "
                                        + "The bigger the value of the phase offset the faster the waves move to the left (right)."), water2D_Sim.minPhaseOffset);
                                }
                                else
                                {
                                    if (water2D_Sim.sineAmplitudes.Count != water2D_Sim.sineWaves)
                                    {
                                        water2D_Sim.sineAmplitudes.Clear();
                                        water2D_Sim.sineStretches.Clear();
                                        water2D_Sim.phaseOffset.Clear();

                                        for (int i = 0; i < water2D_Sim.sineWaves; i++)
                                        {
                                            water2D_Sim.sineAmplitudes.Add(Random.Range(water2D_Sim.minAmplitude, water2D_Sim.maxAmplitude));
                                            water2D_Sim.sineStretches.Add(Random.Range(water2D_Sim.minStretch, water2D_Sim.maxStretch));
                                            water2D_Sim.phaseOffset.Add(Random.Range(water2D_Sim.minPhaseOffset, water2D_Sim.maxPhaseOffset));
                                        }
                                    }
                                    else
                                    {
                                        int n = 0;
                                        for (int i = 0; i < water2D_Sim.sineWaves; i++)
                                        {
                                            n = i + 1;
                                            EditorGUILayout.LabelField(new GUIContent("Sine Wave " + n), EditorStyles.boldLabel);

                                            water2D_Sim.sineAmplitudes[i] = EditorGUILayout.FloatField(new GUIContent("Amplitude", "The amplitude of the wave. This value controls the height of the sine wave."), water2D_Sim.sineAmplitudes[i]);
                                            water2D_Sim.sineStretches[i] = EditorGUILayout.FloatField(new GUIContent("Stretch", "The bigger the value of the stretch the more compact the waves are."), water2D_Sim.sineStretches[i]);
                                            water2D_Sim.phaseOffset[i] = EditorGUILayout.FloatField(new GUIContent("Phase Offset", "The bigger the value of the phase offset the faster the waves move to the left (right). "
                                                + " A negative value will make the sine wave move to the right."), water2D_Sim.phaseOffset[i]);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                water2D_Sim.waveAmplitude = EditorGUILayout.FloatField(new GUIContent("Wave Amplitude", "The amplitude of the wave. This value controls the height of the sine wave."), water2D_Sim.waveAmplitude);
                                water2D_Sim.waveStretch = EditorGUILayout.FloatField(new GUIContent("Wave Stretch", "The bigger the value of the stretch the more compact the waves are."), water2D_Sim.waveStretch);
                                water2D_Sim.wavePhaseOffset = EditorGUILayout.FloatField(new GUIContent("Wave Phase Offset", "The bigger the value of the phase offset the faster the waves move to the left (right)."
                                    + " A negative value will make the sine wave move to the right."), water2D_Sim.wavePhaseOffset);
                            }

                            water2D_Sim.sineWaveVelocityScale = EditorGUILayout.FloatField(new GUIContent("Sine Wave Velocity Scale", "Will scale down (up) the velocity that is applied to a vertex from a sine wave."), water2D_Sim.sineWaveVelocityScale);
                        }

                        if (water2D_Sim.surfaceWaves == Water2D_SurfaceWaves.RandomSplashes)
                        {
                            water2D_Sim.timeStep = EditorGUILayout.FloatField(new GUIContent("Wave Time Step", "The time between splashes."), water2D_Sim.timeStep);
                            water2D_Sim.maxVelocity = EditorGUILayout.FloatField(new GUIContent("Max Velocity", "The constant is used to generate a random velocity between a Max and a Min."), water2D_Sim.maxVelocity);
                            water2D_Sim.minVelocity = EditorGUILayout.FloatField(new GUIContent("Min Velocity", "The constant is used to generate a random velocity between a Max and a Min."), water2D_Sim.minVelocity);
                            water2D_Sim.neighborVertVelocityScale = EditorGUILayout.FloatField(new GUIContent("Neighbor Vertex Velocity Scale", "Will scale down (up) the velocity that "
                                + "is applied to the neighbor vertices when RandomWave method is called."), water2D_Sim.neighborVertVelocityScale);
                        }
                    }
                });
            }

            BoldFontStyle(() =>
            {
                water2D_Sim.showPlayerSettings = EditorGUILayout.Foldout(water2D_Sim.showPlayerSettings, "Player");
            });

            if (water2D_Sim.showPlayerSettings)
            {
                InspectorBox(10, () =>
                {
                    water2D_Sim.characterControllerType = (Water2D_CharacterControllerType)EditorGUILayout.EnumPopup(new GUIContent("Character Controller", "If you are using a physics based character controller select Physics Based option, otherwise select Raycast Based."), water2D_Sim.characterControllerType);

                    if (water2D_Sim.characterControllerType == Water2D_CharacterControllerType.PhysicsBased)
                    {
                        water2D_Sim.playerBoundingBoxSize = EditorGUILayout.Vector2Field(new GUIContent("Player BBox Size", "The size for the players bounding box. In most cases the player character will have more than one collider. "
                                + "So to simplify the things, Water2D uses this variable to set the size for an imaginary bounding box that will be used when applying buoyant force. "), water2D_Sim.playerBoundingBoxSize);

                        water2D_Sim.playerBoundingBoxCenter = EditorGUILayout.Vector2Field(new GUIContent("Player BBox Center", "By default the center of the bounding box will be "
                            + "the transform.position of the object. Use this variable to offset the players bounding box center."), water2D_Sim.playerBoundingBoxCenter);

                        water2D_Sim.playerBuoyantForceScale = EditorGUILayout.FloatField(new GUIContent("Player Buoyant Force Scale", "Depending on what character controller you are using, you may have a big character "
                            + "that must have a small mass. As a result the Player will not submerge in the water because of its low mass that results in low density. To resolve this problem use this variable to scale "
                            + "down the buoyant force applied to the Player."), water2D_Sim.playerBuoyantForceScale);

                        water2D_Sim.playerOnExitPSAndSound = EditorGUILayout.Toggle(new GUIContent("On Exit PS and Sound", "When enabled, a particles system will be instantiated and a sound effect will be played when the player exits the water."), water2D_Sim.playerOnExitPSAndSound);
                        water2D_Sim.playerOnExitRipple = EditorGUILayout.Toggle(new GUIContent("On Exit Ripple", "When enabled, a ripple will be generated when the player exits the water."), water2D_Sim.playerOnExitRipple);

                        if (!rippleScript && water2D_Sim.playerOnExitRipple)
                            water2D_Sim.playerOnExitVelocity = EditorGUILayout.FloatField(new GUIContent("On Exit Velocity", "The velocity that should be applied to a surface vertex when the player exits the water."), water2D_Sim.playerOnExitVelocity);
                    }
                    else
                    {
                        if (!rippleScript)
                        {
                            water2D_Sim.rippleWidth = EditorGUILayout.FloatField(new GUIContent("Ripple Width", "The initial width of the water wave."), water2D_Sim.rippleWidth);
                            water2D_Sim.playerOnEnterVelocity = EditorGUILayout.FloatField(new GUIContent("On Enter Velocity", "The velocity that will be applied to the surface vertices to generate a wave."), water2D_Sim.playerOnEnterVelocity);

                            if (water2D_Sim.playerOnExitRipple)
                                water2D_Sim.playerOnExitVelocity = EditorGUILayout.FloatField(new GUIContent("On Exit Velocity", "The velocity that should be applied to a surface vertex when the player exits the water."), water2D_Sim.playerOnExitVelocity);
                        }

                        water2D_Sim.playerOnExitRipple = EditorGUILayout.Toggle(new GUIContent("On Exit Ripple", "When enabled, a ripple will be generated when the player exits the water."), water2D_Sim.playerOnExitRipple);
                        water2D_Sim.playerOnExitPSAndSound = EditorGUILayout.Toggle(new GUIContent("On Exit PS and Sound", "When enabled, a particles system will be instantiated and a sound effect will be played when the player exits the water."), water2D_Sim.playerOnExitPSAndSound);
                    }
                });
            }

            BoldFontStyle(() =>
            {
                water2D_Sim.showMiscellaneous = EditorGUILayout.Foldout(water2D_Sim.showMiscellaneous, "Miscellaneous");
            });

            if (water2D_Sim.showMiscellaneous)
            {
                InspectorBox(10, () =>
                {
                    water2D_Sim.waterType = (Water2D_Type)EditorGUILayout.EnumPopup(new GUIContent("Water Type", "A list of water types. "
                        + "A dynamic water can be animated and reacts to objects. A decorative water can be animated, but will not react "
                        + " to objects and will not influence their position."), water2D_Sim.waterType);

                    water2D_Sim.collisionLayers = (LayerMask)EditorGUILayout.MaskField("Collider Mask", water2D_Sim.collisionLayers, InternalEditorUtility.layers);

                    if (!rippleScript && water2D_Sim.springSimulation)
                    {
                        water2D_Sim.collisionDetectionMode = (Water2D_CollisionDetectionMode)EditorGUILayout.EnumPopup(new GUIContent("Collider Detection", "When Raycast Based option is selected, for every dynamic object that is near the surface of the water a ray cast " 
                            + "if performed for every vertex that is inside the bounding box of that objects collider. This is done to find if a particular vertex is inside the collider and not only his bounding box. If the vertex is inside the collider, his velocity is " 
                            + "changed based on the velocity of the dynamic object. If Bounds Based option is selected, no raycast is perfomed and instead the vertex position relative to the objects bounding box is used to determine if a vertex velocity should be changed by an object."), water2D_Sim.collisionDetectionMode);

                        if (water2D_Sim.collisionDetectionMode == Water2D_CollisionDetectionMode.RaycastBased)
                            water2D_Sim.raycastDistance = Mathf.Clamp(EditorGUILayout.FloatField(new GUIContent("Raycast Distance", "How far to the left and right from a vertex world position should we look for a collider. "), water2D_Sim.raycastDistance), 0, 100);
                    }

                    if (water2D_Sim.waterType == Water2D_Type.Decorative)
                    {
                        Water2D_Tool water2D_Tool = water2D_Sim.GetComponent<Water2D_Tool>();
                        if (water2D_Tool.createCollider)
                        {
                            water2D_Tool.createCollider = false;
                            water2D_Tool.RecreateWaterMesh();
                        }
                    }
                    else
                    {
                        Water2D_Tool water2D_Tool = water2D_Sim.GetComponent<Water2D_Tool>();
                        if (!water2D_Tool.createCollider)
                        {
                            water2D_Tool.createCollider = true;
                            water2D_Tool.RecreateWaterMesh();
                        }
                    }

                    if (water2D_Sim.waterType == Water2D_Type.Dynamic)
                    {
                        if (!rippleScript)
                        {
                            water2D_Sim.velocityFilter = EditorGUILayout.FloatField(new GUIContent("Velocity Filter", "An object with a velocity on the Y axis "
                                + " greater than the value of Velocity Filter will not create splashes."), water2D_Sim.velocityFilter);

                            water2D_Sim.interactionRegion = EditorGUILayout.FloatField(new GUIContent("Interaction Region", "The bottom region of a colliders bounding box "
                                + "that can affect the velocity of a vertex. This value is used to limit the ability of the objects with big bounding boxes to affect the "
                                + "velocity of the surface vertices. A value of 1 means that only the first 1m of the bottom of the bounding box will affect the velocity "
                                + "of the surface vertices. "), water2D_Sim.interactionRegion);

                                water2D_Sim.waterDisplacement = EditorGUILayout.Toggle(new GUIContent("Water Displacement", "Floating objects will influence the final water area."), water2D_Sim.waterDisplacement);
                        }
                    }

                    if (!rippleScript)
                        water2D_Sim.constantWaterArea = EditorGUILayout.Toggle(new GUIContent("Constant Water Area", "If the width of the water changes, the height will " +
                            " change too, to keep a constant water Area."), water2D_Sim.constantWaterArea);

                    if (water2D_Sim.waterType == Water2D_Type.Dynamic)
                    {
                        water2D_Sim.particleS = EditorGUILayout.ObjectField(new GUIContent("PS Prefab", "A particle system prefab used to simulate the water splash effect."), water2D_Sim.particleS, typeof(GameObject), true) as GameObject;
                        water2D_Sim.particleSystemPosOffset = EditorGUILayout.Vector3Field(new GUIContent("PS Position Offset", "Offsets the position where the particle systems are created on the Z axis."), water2D_Sim.particleSystemPosOffset);

                        water2D_Sim.particleSystemSorting = EditorGUILayout.Toggle(new GUIContent("PS Sorting", "Enable this toggle if you want to set the sorting layer and order in layer of the particle system when it is instantiated."), water2D_Sim.particleSystemSorting);

                        if (water2D_Sim.particleSystemSorting)
                        {
                            water2D_Sim.particleSystemSortingLayerName = EditorGUILayout.TextField(new GUIContent("PS Sorting Layer Name", "Insert here the sorting layer name for the particle system."), water2D_Sim.particleSystemSortingLayerName);
                            water2D_Sim.particleSystemOrderInLayer = EditorGUILayout.IntField(new GUIContent("PS Order In Layer", "Insert here the order in layer for the particle system."), water2D_Sim.particleSystemOrderInLayer);
                        }

                        water2D_Sim.splashSound = EditorGUILayout.ObjectField(new GUIContent("Sound Effect", "A sound effect generated when an object hits the water surface."), water2D_Sim.splashSound, typeof(AudioClip), true) as AudioClip;
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
