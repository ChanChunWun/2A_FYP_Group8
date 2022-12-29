using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InputReader : ScriptableObject, GameInput.IDriverActions
{
    // Driver
    public event UnityAction<float> DriverAccerateEvent = delegate { };
    public event UnityAction<float> DriverBrakeEvent = delegate { };
    public event UnityAction<float> DriverSteeringEvent = delegate { };

    private GameInput m_GameInput;

    private void OnEnable()
    {
        if (m_GameInput == null)
        {
            m_GameInput = new GameInput();
            m_GameInput.Driver.SetCallbacks(this);
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
        }
    }

    public void DisableAllInput()
    {
        if (m_GameInput == null)
            return;

        m_GameInput.Driver.Disable();
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

    // Driver - Digital
    public void OnAccelerate(InputAction.CallbackContext context) => DigitalAction(context, DriverAccerateEvent, 1f);
    public void OnBrake(InputAction.CallbackContext context) => DigitalAction(context, DriverBrakeEvent, 1f);
    public void OnSteerLeft(InputAction.CallbackContext context) => DigitalAction(context, DriverSteeringEvent, -1f);
    public void OnSteerRight(InputAction.CallbackContext context) => DigitalAction(context, DriverSteeringEvent, 1f);

    // Driver - Analog
    public void OnAccelerateAnalog(InputAction.CallbackContext context) => DriverAccerateEvent.Invoke(context.ReadValue<float>());
    public void OnBrakeAnalog(InputAction.CallbackContext context) => DriverBrakeEvent.Invoke(context.ReadValue<float>());
    public void OnSteeringAnalog(InputAction.CallbackContext context) => DriverSteeringEvent.Invoke(context.ReadValue<float>());

    public enum ActionMapType
    {
        Driver,
        Passenger
    }
}