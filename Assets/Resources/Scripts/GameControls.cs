// GENERATED AUTOMATICALLY FROM 'Assets/Resources/Scripts/GameControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @GameControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameControls"",
    ""maps"": [
        {
            ""name"": ""fpsPlayer"",
            ""id"": ""c9d51e3b-f1c9-4375-adea-b5f7bb23c46a"",
            ""actions"": [
                {
                    ""name"": ""movement"",
                    ""type"": ""Value"",
                    ""id"": ""f15bd67a-3ae0-4604-ae00-a52bf629f971"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""turn"",
                    ""type"": ""Value"",
                    ""id"": ""d96bfbfa-c433-4c2c-89cd-3da458ab803d"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""look"",
                    ""type"": ""Value"",
                    ""id"": ""dc386038-960e-4156-aa3e-564d64dc1914"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""47caf493-46a9-4331-adae-b992f22b874c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""321b4113-3c6e-4f90-8867-1bef339de3ba"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e6134323-5448-4c67-b1f5-a2152ee15a62"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e7748a81-4228-41b2-94e8-d3715c2d2791"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""53e6455b-b17a-47c5-a541-7e8f5ff27cbb"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1f70365a-6c3c-40cf-abb4-c37bca39e5b0"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""turn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5c77c9a6-a8f2-41ca-9800-ea5dd6386192"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // fpsPlayer
        m_fpsPlayer = asset.FindActionMap("fpsPlayer", throwIfNotFound: true);
        m_fpsPlayer_movement = m_fpsPlayer.FindAction("movement", throwIfNotFound: true);
        m_fpsPlayer_turn = m_fpsPlayer.FindAction("turn", throwIfNotFound: true);
        m_fpsPlayer_look = m_fpsPlayer.FindAction("look", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // fpsPlayer
    private readonly InputActionMap m_fpsPlayer;
    private IFpsPlayerActions m_FpsPlayerActionsCallbackInterface;
    private readonly InputAction m_fpsPlayer_movement;
    private readonly InputAction m_fpsPlayer_turn;
    private readonly InputAction m_fpsPlayer_look;
    public struct FpsPlayerActions
    {
        private @GameControls m_Wrapper;
        public FpsPlayerActions(@GameControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @movement => m_Wrapper.m_fpsPlayer_movement;
        public InputAction @turn => m_Wrapper.m_fpsPlayer_turn;
        public InputAction @look => m_Wrapper.m_fpsPlayer_look;
        public InputActionMap Get() { return m_Wrapper.m_fpsPlayer; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FpsPlayerActions set) { return set.Get(); }
        public void SetCallbacks(IFpsPlayerActions instance)
        {
            if (m_Wrapper.m_FpsPlayerActionsCallbackInterface != null)
            {
                @movement.started -= m_Wrapper.m_FpsPlayerActionsCallbackInterface.OnMovement;
                @movement.performed -= m_Wrapper.m_FpsPlayerActionsCallbackInterface.OnMovement;
                @movement.canceled -= m_Wrapper.m_FpsPlayerActionsCallbackInterface.OnMovement;
                @turn.started -= m_Wrapper.m_FpsPlayerActionsCallbackInterface.OnTurn;
                @turn.performed -= m_Wrapper.m_FpsPlayerActionsCallbackInterface.OnTurn;
                @turn.canceled -= m_Wrapper.m_FpsPlayerActionsCallbackInterface.OnTurn;
                @look.started -= m_Wrapper.m_FpsPlayerActionsCallbackInterface.OnLook;
                @look.performed -= m_Wrapper.m_FpsPlayerActionsCallbackInterface.OnLook;
                @look.canceled -= m_Wrapper.m_FpsPlayerActionsCallbackInterface.OnLook;
            }
            m_Wrapper.m_FpsPlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @movement.started += instance.OnMovement;
                @movement.performed += instance.OnMovement;
                @movement.canceled += instance.OnMovement;
                @turn.started += instance.OnTurn;
                @turn.performed += instance.OnTurn;
                @turn.canceled += instance.OnTurn;
                @look.started += instance.OnLook;
                @look.performed += instance.OnLook;
                @look.canceled += instance.OnLook;
            }
        }
    }
    public FpsPlayerActions @fpsPlayer => new FpsPlayerActions(this);
    public interface IFpsPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnTurn(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
    }
}
