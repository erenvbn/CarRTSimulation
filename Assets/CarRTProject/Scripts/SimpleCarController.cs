using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 0f; // Initial speed of the car
    private float currentSpeed; // Current speed of the car

    [SerializeField] private float initialRotation = 0f; // Initial rotation of the car
    private float currentRotation; // Current rotation of the car

    [SerializeField] private float speedChangeRate = 20f; // Rate at which speed changes
    [SerializeField] private float rotationChangeRate = 20f; // Rate at which rotation changes

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = initialSpeed;
        currentRotation = initialRotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Update speed
        currentSpeed = Mathf.MoveTowards(currentSpeed, TargetSpeed, speedChangeRate * Time.deltaTime);

        // Update rotation
        currentRotation = Mathf.MoveTowards(currentRotation, TargetRotation, rotationChangeRate * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, currentRotation, 0f);

        // Calculate movement direction based on current rotation
        Vector3 movementDirection = transform.forward * currentSpeed * Time.deltaTime;

        // Move the car forward based on current speed
        transform.position += movementDirection;
    }

    // Serialized field for setting target speed
    public float TargetSpeed { get; set; }

    // Serialized field for setting target rotation
    public float TargetRotation { get; set; }
}
