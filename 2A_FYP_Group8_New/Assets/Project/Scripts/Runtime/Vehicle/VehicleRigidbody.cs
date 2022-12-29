using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VehicleRigidbody : MonoBehaviour
{
    [SerializeField]
    private Transform m_CenterOfMass;
    public Transform CenterOfMass => m_CenterOfMass;

    [SerializeField]
    private Transform m_XROrigin;
    public Transform XROrigin => m_XROrigin;

    private float m_Acceration;
    public float Acceration => m_Acceration;

    private Rigidbody m_Rigidbody;
    private int m_FixedUpdateCounter;
    private const int k_FilterTimeSpanFrames = 10;

    private float m_LastVelocity = 0;
    private readonly float[] m_Accelerations = new float[k_FilterTimeSpanFrames];

    void Awake() => m_Rigidbody = GetComponent<Rigidbody>();

    void Start()
    {
        if (m_CenterOfMass != null)
            m_Rigidbody.centerOfMass = transform.InverseTransformPoint(m_CenterOfMass.position);

        m_FixedUpdateCounter = 0;
    }

    void FixedUpdate()
    {
        m_FixedUpdateCounter = ++m_FixedUpdateCounter % k_FilterTimeSpanFrames;

        float velocity = transform.InverseTransformVector(m_Rigidbody.velocity).z;
        float momentaryAcceleration = (velocity - m_LastVelocity) / Time.fixedDeltaTime;
        m_Accelerations[m_FixedUpdateCounter] = momentaryAcceleration;
        m_Acceration = GetFilteredMean(m_Accelerations, m_Accelerations.Length);
        m_LastVelocity = velocity;
    }

    private float GetFilteredMean(float[] array, int arrayLength)
    {
        float minVal = float.MaxValue;
        float maxVal = float.MinValue;
        float sum = 0;

        for (int i = 0; i < arrayLength; i++)
        {
            if (array[i] < minVal)
                minVal = array[i];
            if (array[i] > maxVal)
                maxVal = array[i];
            sum += array[i];
        }

        return (sum - minVal - maxVal) / (k_FilterTimeSpanFrames - 2f);
    }
}
