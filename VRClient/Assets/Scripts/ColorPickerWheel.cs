// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using HoloToolkit.Unity.InputModule;
using UnityEngine;

#if UNITY_WSA && UNITY_2017_2_OR_NEWER
using UnityEngine.XR.WSA.Input;
#endif

namespace HoloToolkit.Unity.ControllerExamples
{
    public class ColorPickerWheel : AttachToController
    {
        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                if (value)
                {
                    lastTimeVisible = Time.unscaledTime;
                }
            }
        }
        
        [Header("ColorPickerWheel Elements")]
        [SerializeField]
        private bool visible = false;
        [SerializeField]
        private Transform UITransform;
        [SerializeField]
        private float inputScale = 1.1f;
        [SerializeField]
        private float timeout = 2f;

        private Vector2 UIPosition;
        private Vector3 UIRoration;
        private float lastTimeVisible;
        private bool visibleLastFrame = false;

        private void Update()
        {

            if (visible != visibleLastFrame)
            {
                // Based on visible property, it triggers Show and Hide animation triggers in the color picker's animator component
                if (visible)
                {
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            visibleLastFrame = visible;

            if (!visible)
            {
                return;
            }

            // Transform the touchpad's input x, y position information to ColorPickerWheel's local position x, z
            Vector3 localPosition = new Vector3(UIPosition.x * inputScale, 0.15f, UIPosition.y * inputScale);
            if (localPosition.magnitude > 1)
            {
                localPosition = localPosition.normalized;
            }
            UITransform.localPosition = localPosition + new Vector3 (0, 0.025f, -0.01f);
            UITransform.Rotate(UIRoration);

            
        }

        protected override void OnAttachToController()
        {
#if UNITY_WSA && UNITY_2017_2_OR_NEWER
            // Subscribe to input now that we're parented under the controller
            InteractionManager.InteractionSourceUpdated += InteractionSourceUpdated;
#endif
        }

        protected override void OnDetachFromController()
        {
            Visible = false;

#if UNITY_WSA && UNITY_2017_2_OR_NEWER
            // Unsubscribe from input now that we've detached from the controller
            InteractionManager.InteractionSourceUpdated -= InteractionSourceUpdated;
#endif
        }

        

#if UNITY_WSA && UNITY_2017_2_OR_NEWER
        private void InteractionSourceUpdated(InteractionSourceUpdatedEventArgs obj)
        {
            // Check if it is a touchpadTouched event and from the left controller
            if (obj.state.source.handedness == Handedness && obj.state.touchpadTouched)
            {
                // If both are true, Visible is set to true and the touchpad position is assigned to UIPosition. 
                Visible = true;
                UIPosition = obj.state.touchpadPosition;
            }
        }
#endif
    }
}