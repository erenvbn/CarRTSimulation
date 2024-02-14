using UnityEngine;

public class SimpleCarController : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 0f; // Initial speed of the car
    [SerializeField] private float initialRotation = 0f; // Initial rotation of the car

    // Auto-implemented properties for target speed and rotation
    public float TargetSpeed { get; set; }
    public float TargetRotation { get; set; }

    // Properties to get the current speed and rotation of the car
    public float CurrentSpeed { get; private set; }
    public float CurrentRotation { get; private set; }

    // Properties for speed and rotation change rates
    public float SpeedChangeRate { get; set; } = 20f; // Default value is 20f
    public float RotationChangeRate { get; set; } = 20f; // Default value is 20f

    // Start is called before the first frame update
    void Start()
    {
        CurrentSpeed = initialSpeed;
        CurrentRotation = initialRotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Update speed
        CurrentSpeed = Mathf.MoveTowards(CurrentSpeed, TargetSpeed, SpeedChangeRate * Time.deltaTime);

        // Update rotation
        CurrentRotation = Mathf.MoveTowards(CurrentRotation, TargetRotation, RotationChangeRate * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, CurrentRotation, 0f);

        // Calculate movement direction based on current rotation
        Vector3 movementDirection = transform.forward * CurrentSpeed * Time.deltaTime;

        // Move the car forward based on current speed
        transform.position += movementDirection;
    }
}
