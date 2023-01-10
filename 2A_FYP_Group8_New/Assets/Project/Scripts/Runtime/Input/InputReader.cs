using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InputReader : ScriptableObject, GameInput.IDriverActions, GameInput.IPassengerActions
{
    // Driver
    public event UnityAction<float> DriverAccerateEvent = delegate { };
    public event UnityAction<float> DriverBrakeEvent = delegate { };
    public event UnityAction<float> DriverSteeringEvent = delegate { };

    // Passenger
    public event UnityAction<Vector2> PassengerLookEvent = delegate { };
    public event UnityAction<bool> PassengerReadyWeaponEvent = delegate { };
    public event UnityAction<bool> PassengerFireWeaponEvent = delegate { };

    // Device
    private GameInput m_GameInput;
    public GameInput GameInput
    {
        get => m_GameInput;
    }

    private void OnEnable()
    {
        if (m_GameInput == null)
        {
            m_GameInput = new GameInput();
            m_GameInput.Driver.SetCallbacks(this);
            m_GameInput.Passenger.SetCallbacks(this);
        }
    }

    private void OnDisable() => DisableAllInput();

    public void EnableInput(ActionMapType type)
    {
        DisableAllInput();
        switch (type)
        {
            case ActionMapType.Driver:
                m_GameInput.Driver.Enable();
                break;
            case ActionMapType.Passenger:
                m_GameInput.Passenger.Enable();
                break;
        }
    }

    public void DisableAllInput()
    {
        if (m_GameInput == null)
            return;

        m_GameInput.Driver.Disable();
        m_GameInput.Passenger.Disable();
    }

    private void GenericAction(InputAction.CallbackContext context, UnityAction action)
    {
        if (context.phase == InputActionPhase.Performed)
            action.Invoke();
    }

    private void DigitalAction(InputAction.CallbackContext context, UnityAction<float> action, float value)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                action.Invoke(value);
                break;
            case InputActionPhase.Canceled:
                action.Invoke(0f);
                break;
        }
    }

    private void DigitalAction(InputAction.CallbackContext context, UnityAction<bool> action)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                action.Invoke(true);
                break;
            case InputActionPhase.Canceled:
                action.Invoke(false);
                break;
        }
    }

    // Driver - Digital
    public void OnAccelerate(InputAction.CallbackContext context) => DigitalAction(context, DriverAccerateEvent, 1f);
    public void OnBrake(InputAction.CallbackContext context) => DigitalAction(context, DriverBrakeEvent, 1f);
    public void OnSteerLeft(InputAction.CallbackContext context) => DigitalAction(context, DriverSteeringEvent, -1f);
    public void OnSteerRight(InputAction.CallbackContext context) => DigitalAction(context, DriverSteeringEvent, 1f);

    // Driver - Analog
    public void OnAccelerateAnalog(InputAction.CallbackContext context) => DriverAccerateEvent.Invoke(context.ReadValue<float>());
    public void OnBrakeAnalog(InputAction.CallbackContext context) => DriverBrakeEvent.Invoke(context.ReadValue<float>());
    public void OnSteeringAnalog(InputAction.CallbackContext context) => DriverSteeringEvent.Invoke(context.ReadValue<float>());

    // Passenger
    public void OnLook(InputAction.CallbackContext context) => PassengerLookEvent.Invoke(context.ReadValue<Vector2>());
    public void OnReadyWeapon(InputAction.CallbackContext context) => DigitalAction(context, PassengerReadyWeaponEvent);
    public void OnFireWeapon(InputAction.CallbackContext context) => DigitalAction(context, PassengerFireWeaponEvent);

    public enum ActionMapType
    {
        Driver,
        Passenger
    }
}
