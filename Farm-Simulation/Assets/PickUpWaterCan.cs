using UnityEngine;

public class PickUpWaterCan : MonoBehaviour
{
    public Transform WaterCan; // Assign the watering can object in the Inspector
    public Transform handPosition; // Position where the can will be held when picked up
    public Transform dropPosition; // Position where the can will be dropped

    private bool isPickedUp = false; // Track if the watering can is picked up
    private Pouring pouringScript; // Reference to the Pouring script

    void Start()
    {
        // Get the Pouring script from the WaterCan GameObject
        pouringScript = WaterCan.GetComponent<Pouring>();
        // Disable the Pouring script initially
        if (pouringScript != null)
        {
            pouringScript.enabled = false;
        }
    }

    void Update()
    {
        // Handle picking up and dropping the watering can with right-click
        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            if (isPickedUp)
            {
                DropWaterCan(); // Drop the watering can if it is already picked up
            }
            else
            {
                AttemptPickup(); // Pick up the watering can if it's not picked up yet
            }
        }

        // If the watering can is picked up, update its position to the handPosition
        if (isPickedUp)
        {
            WaterCan.position = handPosition.position;
            WaterCan.rotation = handPosition.rotation; // Optional: align rotation with hand
        }
    }

    void AttemptPickup()
    {
        // Check if the player is close enough to the watering can
        float distanceToCan = Vector3.Distance(transform.position, WaterCan.position);

        if (distanceToCan <= 2f) // Default pick up range (you can adjust this as needed)
        {
            TogglePickup();
        }
        else
        {
            Debug.Log("You are too far away to pick up the watering can.");
        }
    }

    public bool IsPickedUp()
    {
        return isPickedUp;
    }

    void TogglePickup()
    {
        isPickedUp = !isPickedUp;

        if (isPickedUp)
        {
            // Enable the Pouring script when picked up
            if (pouringScript != null)
            {
                pouringScript.enabled = true;
            }
        }
        else
        {
            // Disable the Pouring script when dropped
            if (pouringScript != null)
            {
                pouringScript.enabled = false;
            }
        }
    }

    void DropWaterCan()
    {
        isPickedUp = false;

        // Disable the Pouring script when dropped
        if (pouringScript != null)
        {
            pouringScript.enabled = false;
        }

        // Drop the watering can at the predefined dropPosition
        if (dropPosition != null)
        {
            WaterCan.position = dropPosition.position; // Set to drop position
            WaterCan.rotation = dropPosition.rotation; // Optionally, align with drop position rotation
        }
        else
        {
            // If dropPosition is not assigned, reset it to its original position
            WaterCan.position = transform.position; // You can adjust this if needed
            WaterCan.rotation = Quaternion.identity; // Reset rotation (optional)
        }
    }
}
