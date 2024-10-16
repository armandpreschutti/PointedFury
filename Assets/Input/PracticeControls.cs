//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/Input/PracticeControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PracticeControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PracticeControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PracticeControls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""68ae4ed7-0ac1-48ff-9c1f-4585f2e55b09"",
            ""actions"": [
                {
                    ""name"": ""SpawnEnemy"",
                    ""type"": ""Button"",
                    ""id"": ""4b86241e-dee3-4381-90e3-5c56b4b13f6f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ToggleHealthSystems"",
                    ""type"": ""Button"",
                    ""id"": ""f4174a9f-b9bf-4041-89e1-33fe4102d958"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CycleEnemyTypes"",
                    ""type"": ""Button"",
                    ""id"": ""19fc13d4-1676-4b5f-9712-b57fe6639809"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ClearEnemies"",
                    ""type"": ""Button"",
                    ""id"": ""bfb62951-35ac-46a8-b4a1-cdf6d596ad68"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TogglePracticeControls"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9298f464-4563-4955-bff2-898629ec8691"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""137b4d46-fe3f-4ab6-aef7-189e263975ea"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ClearEnemies"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""79e7523a-3582-49ba-8b39-56b267e29be0"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleHealthSystems"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""15101e7c-9fed-450c-8afa-9ccd28bda491"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CycleEnemyTypes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5affa728-12ec-4111-9f86-11f987fc0568"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpawnEnemy"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""395f90bf-7419-4e03-94e4-94e142acd1f0"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TogglePracticeControls"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_SpawnEnemy = m_Player.FindAction("SpawnEnemy", throwIfNotFound: true);
        m_Player_ToggleHealthSystems = m_Player.FindAction("ToggleHealthSystems", throwIfNotFound: true);
        m_Player_CycleEnemyTypes = m_Player.FindAction("CycleEnemyTypes", throwIfNotFound: true);
        m_Player_ClearEnemies = m_Player.FindAction("ClearEnemies", throwIfNotFound: true);
        m_Player_TogglePracticeControls = m_Player.FindAction("TogglePracticeControls", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_SpawnEnemy;
    private readonly InputAction m_Player_ToggleHealthSystems;
    private readonly InputAction m_Player_CycleEnemyTypes;
    private readonly InputAction m_Player_ClearEnemies;
    private readonly InputAction m_Player_TogglePracticeControls;
    public struct PlayerActions
    {
        private @PracticeControls m_Wrapper;
        public PlayerActions(@PracticeControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @SpawnEnemy => m_Wrapper.m_Player_SpawnEnemy;
        public InputAction @ToggleHealthSystems => m_Wrapper.m_Player_ToggleHealthSystems;
        public InputAction @CycleEnemyTypes => m_Wrapper.m_Player_CycleEnemyTypes;
        public InputAction @ClearEnemies => m_Wrapper.m_Player_ClearEnemies;
        public InputAction @TogglePracticeControls => m_Wrapper.m_Player_TogglePracticeControls;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @SpawnEnemy.started += instance.OnSpawnEnemy;
            @SpawnEnemy.performed += instance.OnSpawnEnemy;
            @SpawnEnemy.canceled += instance.OnSpawnEnemy;
            @ToggleHealthSystems.started += instance.OnToggleHealthSystems;
            @ToggleHealthSystems.performed += instance.OnToggleHealthSystems;
            @ToggleHealthSystems.canceled += instance.OnToggleHealthSystems;
            @CycleEnemyTypes.started += instance.OnCycleEnemyTypes;
            @CycleEnemyTypes.performed += instance.OnCycleEnemyTypes;
            @CycleEnemyTypes.canceled += instance.OnCycleEnemyTypes;
            @ClearEnemies.started += instance.OnClearEnemies;
            @ClearEnemies.performed += instance.OnClearEnemies;
            @ClearEnemies.canceled += instance.OnClearEnemies;
            @TogglePracticeControls.started += instance.OnTogglePracticeControls;
            @TogglePracticeControls.performed += instance.OnTogglePracticeControls;
            @TogglePracticeControls.canceled += instance.OnTogglePracticeControls;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @SpawnEnemy.started -= instance.OnSpawnEnemy;
            @SpawnEnemy.performed -= instance.OnSpawnEnemy;
            @SpawnEnemy.canceled -= instance.OnSpawnEnemy;
            @ToggleHealthSystems.started -= instance.OnToggleHealthSystems;
            @ToggleHealthSystems.performed -= instance.OnToggleHealthSystems;
            @ToggleHealthSystems.canceled -= instance.OnToggleHealthSystems;
            @CycleEnemyTypes.started -= instance.OnCycleEnemyTypes;
            @CycleEnemyTypes.performed -= instance.OnCycleEnemyTypes;
            @CycleEnemyTypes.canceled -= instance.OnCycleEnemyTypes;
            @ClearEnemies.started -= instance.OnClearEnemies;
            @ClearEnemies.performed -= instance.OnClearEnemies;
            @ClearEnemies.canceled -= instance.OnClearEnemies;
            @TogglePracticeControls.started -= instance.OnTogglePracticeControls;
            @TogglePracticeControls.performed -= instance.OnTogglePracticeControls;
            @TogglePracticeControls.canceled -= instance.OnTogglePracticeControls;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnSpawnEnemy(InputAction.CallbackContext context);
        void OnToggleHealthSystems(InputAction.CallbackContext context);
        void OnCycleEnemyTypes(InputAction.CallbackContext context);
        void OnClearEnemies(InputAction.CallbackContext context);
        void OnTogglePracticeControls(InputAction.CallbackContext context);
    }
}
