using UnityEngine;

public class VehicleSensors : VehicleComponent
{
    [SerializeField]
    private Rigidbody m_Rigidbody;
    [SerializeField]
    private VehicleRigidbody m_VehicleRigidbody;

    private float m_Velocity;
    private const float k_Smoothness = 0.5f;

    void FixedUpdate()
    {
        // Velocity
        var longitudinalVelocity = m_Rigidbody.transform.InverseTransformVector(m_Rigidbody.velocity).z;
        m_Velocity = (1f - k_Smoothness) * Mathf.Round(longitudinalVelocity * 1000f) / 1000f + k_Smoothness * m_Velocity;

        // Update Data
        Vehicle.Velocity.SetValue(m_Velocity);
    }
}
