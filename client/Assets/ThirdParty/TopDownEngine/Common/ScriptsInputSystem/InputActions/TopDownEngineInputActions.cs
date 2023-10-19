// GENERATED AUTOMATICALLY FROM 'Assets/TopDownEngine/Common/ScriptsInputSystem/InputActions/TopDownEngineInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace MoreMountains.TopDownEngine
{
    public class @TopDownEngineInputActions : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @TopDownEngineInputActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""TopDownEngineInputActions"",
    ""maps"": [
        {
            ""name"": ""PlayerControls"",
            ""id"": ""aa0e1094-3ae9-4de6-877e-1fa7a26edd69"",
            ""actions"": [
                {
                    ""name"": ""PrimaryMovement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""265ae792-b274-4758-9c3f-67c2f395c758"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SecondaryMovement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""e4146e01-7578-46fe-b394-b240c2256b2c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""314ace32-fb21-4bab-a569-ebe4e48f2102"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""6b5c4b16-7607-4f0a-ad5b-62efd798065d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""56a37787-c3be-4789-a219-0509391dc113"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""5e6e408c-e79e-4f5c-87b7-8a6d181bd681"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""7016cc32-6ee0-4d53-b08c-02cbe6eb2c0e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SecondaryShoot"",
                    ""type"": ""Button"",
                    ""id"": ""1b70a5ec-5da5-42ce-a7c0-8f54ff84052f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""ae461515-11b0-4cc9-b221-92ed2df4118e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""cf90ff3a-006b-4ae6-9594-cc2189d6f5f0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""c01ffcc5-adff-4aa3-bb8d-e0139087e5ed"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwitchWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""6e3febaf-dba5-4b43-b4f8-3b720e7ad1be"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwitchCharacter"",
                    ""type"": ""Button"",
                    ""id"": ""cde4a43f-3a08-47c8-afdf-2c5dca8d1241"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TimeControl"",
                    ""type"": ""Button"",
                    ""id"": ""c71e92f2-77ca-4de6-98ad-64b961ed706e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraRotation"",
                    ""type"": ""PassThrough"",
                    ""id"": ""239ee3ca-17ea-4fb2-85ef-91f8046dfddb"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""ae9b37c8-3b37-4a3c-92de-a883c1eaec34"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""2e66d043-6153-419e-932e-0bbd8ef8df6b"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PrimaryMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f1c785e7-a3bf-4f5c-9d5a-2302a0b10975"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PrimaryMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""36b9b939-b3ed-40a9-a305-7ed3251f5a9b"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PrimaryMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d7bfdd17-5246-4bde-aa16-bcb3ea0467a1"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PrimaryMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""157de9f0-00a9-4767-9eca-18eb35914def"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PrimaryMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8923c29f-eb06-45a4-9c25-5ed809d8fea2"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f7f8488a-fd5a-4098-8088-d75677e01b98"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""718dcfd4-5383-4a9f-bab4-92688fd5d743"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""385119bf-31ce-4c23-8780-028608d0481c"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d6b36ab1-5730-4ee6-934d-0da3fd9ad3c7"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""44ef3905-a71f-42e8-b234-953799ede87d"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""62dc31f7-26f0-4d3b-b772-f5fa2bcf5714"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c74dc322-5120-478a-af7a-0d716b64cfc1"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""202fe3e6-b8da-4f61-9aa9-faaad6af7ebb"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f112453b-6f03-459b-a23e-70f29feb18bf"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""63bf589f-5ef3-4931-bb6e-63038c725e66"",
                    ""path"": ""<Keyboard>/leftAlt"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SecondaryShoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""70b57a6b-cd02-4751-90a1-9027a8ed50f0"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c93a01d5-38f1-43b3-9711-d66667df202e"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5149bd3a-469e-4f0d-bb3b-1f08823c3acd"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dd9bdaac-271a-4f2a-bdd2-7e6718d1c076"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""17af30da-1784-4cae-a214-838b1bc78a38"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0edbe9a3-9973-478d-b677-437ae7b60275"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4c3ea5cb-6737-4947-ba0f-12fe1143e541"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SwitchWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""31347280-3484-4765-8820-ab0871c9d6ad"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SwitchWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5a170d0f-bc8a-4a8d-a2d3-31939a85f104"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SwitchCharacter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6fd51ad5-2a26-4828-8ad1-28016fbc2c3d"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""TimeControl"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f7d3e6ec-7190-4a72-81ec-d23f276d6b45"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""TimeControl"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2f940a20-9887-4115-a4cc-f2bcfaa3209c"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SecondaryMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""77200925-45c4-4483-b584-d3255b060915"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""CameraRotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // PlayerControls
            m_PlayerControls = asset.FindActionMap("PlayerControls", throwIfNotFound: true);
            m_PlayerControls_PrimaryMovement = m_PlayerControls.FindAction("PrimaryMovement", throwIfNotFound: true);
            m_PlayerControls_SecondaryMovement = m_PlayerControls.FindAction("SecondaryMovement", throwIfNotFound: true);
            m_PlayerControls_Jump = m_PlayerControls.FindAction("Jump", throwIfNotFound: true);
            m_PlayerControls_Run = m_PlayerControls.FindAction("Run", throwIfNotFound: true);
            m_PlayerControls_Dash = m_PlayerControls.FindAction("Dash", throwIfNotFound: true);
            m_PlayerControls_Crouch = m_PlayerControls.FindAction("Crouch", throwIfNotFound: true);
            m_PlayerControls_Shoot = m_PlayerControls.FindAction("Shoot", throwIfNotFound: true);
            m_PlayerControls_SecondaryShoot = m_PlayerControls.FindAction("SecondaryShoot", throwIfNotFound: true);
            m_PlayerControls_Interact = m_PlayerControls.FindAction("Interact", throwIfNotFound: true);
            m_PlayerControls_Reload = m_PlayerControls.FindAction("Reload", throwIfNotFound: true);
            m_PlayerControls_Pause = m_PlayerControls.FindAction("Pause", throwIfNotFound: true);
            m_PlayerControls_SwitchWeapon = m_PlayerControls.FindAction("SwitchWeapon", throwIfNotFound: true);
            m_PlayerControls_SwitchCharacter = m_PlayerControls.FindAction("SwitchCharacter", throwIfNotFound: true);
            m_PlayerControls_TimeControl = m_PlayerControls.FindAction("TimeControl", throwIfNotFound: true);
            m_PlayerControls_CameraRotation = m_PlayerControls.FindAction("CameraRotation", throwIfNotFound: true);
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

        // PlayerControls
        private readonly InputActionMap m_PlayerControls;
        private IPlayerControlsActions m_PlayerControlsActionsCallbackInterface;
        private readonly InputAction m_PlayerControls_PrimaryMovement;
        private readonly InputAction m_PlayerControls_SecondaryMovement;
        private readonly InputAction m_PlayerControls_Jump;
        private readonly InputAction m_PlayerControls_Run;
        private readonly InputAction m_PlayerControls_Dash;
        private readonly InputAction m_PlayerControls_Crouch;
        private readonly InputAction m_PlayerControls_Shoot;
        private readonly InputAction m_PlayerControls_SecondaryShoot;
        private readonly InputAction m_PlayerControls_Interact;
        private readonly InputAction m_PlayerControls_Reload;
        private readonly InputAction m_PlayerControls_Pause;
        private readonly InputAction m_PlayerControls_SwitchWeapon;
        private readonly InputAction m_PlayerControls_SwitchCharacter;
        private readonly InputAction m_PlayerControls_TimeControl;
        private readonly InputAction m_PlayerControls_CameraRotation;
        public struct PlayerControlsActions
        {
            private @TopDownEngineInputActions m_Wrapper;
            public PlayerControlsActions(@TopDownEngineInputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @PrimaryMovement => m_Wrapper.m_PlayerControls_PrimaryMovement;
            public InputAction @SecondaryMovement => m_Wrapper.m_PlayerControls_SecondaryMovement;
            public InputAction @Jump => m_Wrapper.m_PlayerControls_Jump;
            public InputAction @Run => m_Wrapper.m_PlayerControls_Run;
            public InputAction @Dash => m_Wrapper.m_PlayerControls_Dash;
            public InputAction @Crouch => m_Wrapper.m_PlayerControls_Crouch;
            public InputAction @Shoot => m_Wrapper.m_PlayerControls_Shoot;
            public InputAction @SecondaryShoot => m_Wrapper.m_PlayerControls_SecondaryShoot;
            public InputAction @Interact => m_Wrapper.m_PlayerControls_Interact;
            public InputAction @Reload => m_Wrapper.m_PlayerControls_Reload;
            public InputAction @Pause => m_Wrapper.m_PlayerControls_Pause;
            public InputAction @SwitchWeapon => m_Wrapper.m_PlayerControls_SwitchWeapon;
            public InputAction @SwitchCharacter => m_Wrapper.m_PlayerControls_SwitchCharacter;
            public InputAction @TimeControl => m_Wrapper.m_PlayerControls_TimeControl;
            public InputAction @CameraRotation => m_Wrapper.m_PlayerControls_CameraRotation;
            public InputActionMap Get() { return m_Wrapper.m_PlayerControls; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerControlsActions set) { return set.Get(); }
            public void SetCallbacks(IPlayerControlsActions instance)
            {
                if (m_Wrapper.m_PlayerControlsActionsCallbackInterface != null)
                {
                    @PrimaryMovement.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnPrimaryMovement;
                    @PrimaryMovement.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnPrimaryMovement;
                    @PrimaryMovement.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnPrimaryMovement;
                    @SecondaryMovement.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSecondaryMovement;
                    @SecondaryMovement.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSecondaryMovement;
                    @SecondaryMovement.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSecondaryMovement;
                    @Jump.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJump;
                    @Jump.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJump;
                    @Jump.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJump;
                    @Run.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnRun;
                    @Run.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnRun;
                    @Run.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnRun;
                    @Dash.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnDash;
                    @Dash.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnDash;
                    @Dash.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnDash;
                    @Crouch.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnCrouch;
                    @Crouch.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnCrouch;
                    @Crouch.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnCrouch;
                    @Shoot.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnShoot;
                    @Shoot.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnShoot;
                    @Shoot.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnShoot;
                    @SecondaryShoot.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSecondaryShoot;
                    @SecondaryShoot.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSecondaryShoot;
                    @SecondaryShoot.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSecondaryShoot;
                    @Interact.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnInteract;
                    @Interact.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnInteract;
                    @Interact.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnInteract;
                    @Reload.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnReload;
                    @Reload.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnReload;
                    @Reload.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnReload;
                    @Pause.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnPause;
                    @Pause.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnPause;
                    @Pause.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnPause;
                    @SwitchWeapon.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSwitchWeapon;
                    @SwitchWeapon.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSwitchWeapon;
                    @SwitchWeapon.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSwitchWeapon;
                    @SwitchCharacter.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSwitchCharacter;
                    @SwitchCharacter.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSwitchCharacter;
                    @SwitchCharacter.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSwitchCharacter;
                    @TimeControl.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnTimeControl;
                    @TimeControl.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnTimeControl;
                    @TimeControl.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnTimeControl;
                    @CameraRotation.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnCameraRotation;
                    @CameraRotation.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnCameraRotation;
                    @CameraRotation.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnCameraRotation;
                }
                m_Wrapper.m_PlayerControlsActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @PrimaryMovement.started += instance.OnPrimaryMovement;
                    @PrimaryMovement.performed += instance.OnPrimaryMovement;
                    @PrimaryMovement.canceled += instance.OnPrimaryMovement;
                    @SecondaryMovement.started += instance.OnSecondaryMovement;
                    @SecondaryMovement.performed += instance.OnSecondaryMovement;
                    @SecondaryMovement.canceled += instance.OnSecondaryMovement;
                    @Jump.started += instance.OnJump;
                    @Jump.performed += instance.OnJump;
                    @Jump.canceled += instance.OnJump;
                    @Run.started += instance.OnRun;
                    @Run.performed += instance.OnRun;
                    @Run.canceled += instance.OnRun;
                    @Dash.started += instance.OnDash;
                    @Dash.performed += instance.OnDash;
                    @Dash.canceled += instance.OnDash;
                    @Crouch.started += instance.OnCrouch;
                    @Crouch.performed += instance.OnCrouch;
                    @Crouch.canceled += instance.OnCrouch;
                    @Shoot.started += instance.OnShoot;
                    @Shoot.performed += instance.OnShoot;
                    @Shoot.canceled += instance.OnShoot;
                    @SecondaryShoot.started += instance.OnSecondaryShoot;
                    @SecondaryShoot.performed += instance.OnSecondaryShoot;
                    @SecondaryShoot.canceled += instance.OnSecondaryShoot;
                    @Interact.started += instance.OnInteract;
                    @Interact.performed += instance.OnInteract;
                    @Interact.canceled += instance.OnInteract;
                    @Reload.started += instance.OnReload;
                    @Reload.performed += instance.OnReload;
                    @Reload.canceled += instance.OnReload;
                    @Pause.started += instance.OnPause;
                    @Pause.performed += instance.OnPause;
                    @Pause.canceled += instance.OnPause;
                    @SwitchWeapon.started += instance.OnSwitchWeapon;
                    @SwitchWeapon.performed += instance.OnSwitchWeapon;
                    @SwitchWeapon.canceled += instance.OnSwitchWeapon;
                    @SwitchCharacter.started += instance.OnSwitchCharacter;
                    @SwitchCharacter.performed += instance.OnSwitchCharacter;
                    @SwitchCharacter.canceled += instance.OnSwitchCharacter;
                    @TimeControl.started += instance.OnTimeControl;
                    @TimeControl.performed += instance.OnTimeControl;
                    @TimeControl.canceled += instance.OnTimeControl;
                    @CameraRotation.started += instance.OnCameraRotation;
                    @CameraRotation.performed += instance.OnCameraRotation;
                    @CameraRotation.canceled += instance.OnCameraRotation;
                }
            }
        }
        public PlayerControlsActions @PlayerControls => new PlayerControlsActions(this);
        private int m_KeyboardSchemeIndex = -1;
        public InputControlScheme KeyboardScheme
        {
            get
            {
                if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
                return asset.controlSchemes[m_KeyboardSchemeIndex];
            }
        }
        private int m_GamepadSchemeIndex = -1;
        public InputControlScheme GamepadScheme
        {
            get
            {
                if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
                return asset.controlSchemes[m_GamepadSchemeIndex];
            }
        }
        public interface IPlayerControlsActions
        {
            void OnPrimaryMovement(InputAction.CallbackContext context);
            void OnSecondaryMovement(InputAction.CallbackContext context);
            void OnJump(InputAction.CallbackContext context);
            void OnRun(InputAction.CallbackContext context);
            void OnDash(InputAction.CallbackContext context);
            void OnCrouch(InputAction.CallbackContext context);
            void OnShoot(InputAction.CallbackContext context);
            void OnSecondaryShoot(InputAction.CallbackContext context);
            void OnInteract(InputAction.CallbackContext context);
            void OnReload(InputAction.CallbackContext context);
            void OnPause(InputAction.CallbackContext context);
            void OnSwitchWeapon(InputAction.CallbackContext context);
            void OnSwitchCharacter(InputAction.CallbackContext context);
            void OnTimeControl(InputAction.CallbackContext context);
            void OnCameraRotation(InputAction.CallbackContext context);
        }
    }
}
