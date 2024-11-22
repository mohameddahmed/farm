using UnityEngine;

public class PickUpPlantedSeed : MonoBehaviour
{
    public Transform handPosition; // Position where the seed will be held when picked up
    public Transform dropPosition; // Position where the seed will be dropped
    public GameObject newPrefab; // The prefab that will replace the seed when dropped

    private bool isPickedUp = false; // Track if the planted seed is picked up
    private GameObject currentPlantedSeed; // The currently held planted seed
    private Rigidbody seedRigidbody; // Reference to the seed's Rigidbody

    void Update()
    {
        // Handle picking up and dropping the planted seed with right-click
        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            if (isPickedUp)
            {
                DropPlantedSeed(); // Drop the planted seed if it's already picked up
            }
            else
            {
                AttemptPickup(); // Try to pick up a planted seed if it's not picked up yet
            }
        }
    }

    void AttemptPickup()
    {
        // Find the nearest planted seed with the tag "plantedSeed"
        GameObject[] plantedSeeds = GameObject.FindGameObjectsWithTag("plantedSeed");
        GameObject nearestSeed = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject seed in plantedSeeds)
        {
            float distanceToSeed = Vector3.Distance(transform.position, seed.transform.position);
            if (distanceToSeed < closestDistance && distanceToSeed <= 2f) // Check if within range
            {
                closestDistance = distanceToSeed;
                nearestSeed = seed;
            }
        }

        if (nearestSeed != null)
        {
            currentPlantedSeed = nearestSeed;
            seedRigidbody = currentPlantedSeed.GetComponent<Rigidbody>();
            TogglePickup();
        }
        else
        {
            Debug.Log("No planted seed nearby to pick up.");
        }
    }

    public bool IsPickedUp()
    {
        return isPickedUp;
    }

    void TogglePickup()
    {
        isPickedUp = !isPickedUp;

        if (isPickedUp && currentPlantedSeed != null)
        {
            // Set Rigidbody to kinematic to avoid concave collider issues
            if (seedRigidbody != null)
            {
                seedRigidbody.isKinematic = true;
            }

            // Parent the seed to the hand position
            currentPlantedSeed.transform.SetParent(handPosition);
            currentPlantedSeed.transform.localPosition = Vector3.zero; // Center it on the hand position
            currentPlantedSeed.transform.localRotation = Quaternion.identity; // Align rotation with the hand
        }
    }

    void DropPlantedSeed()
    {
        if (currentPlantedSeed != null)
        {
            isPickedUp = false;

            // Unparent the seed
            currentPlantedSeed.transform.SetParent(null);

            // Drop the planted seed at the predefined dropPosition
            if (dropPosition != null)
            {
                currentPlantedSeed.transform.position = dropPosition.position;
                currentPlantedSeed.transform.rotation = dropPosition.rotation;
            }
            else
            {
                // If dropPosition is not assigned, reset it to the player's position
                currentPlantedSeed.transform.position = transform.position;
                currentPlantedSeed.transform.rotation = Quaternion.identity;
            }

            // Replace with the new prefab
            if (newPrefab != null)
            {
                Instantiate(newPrefab, currentPlantedSeed.transform.position, currentPlantedSeed.transform.rotation);
            }

            // Reset Rigidbody to non-kinematic after dropping
            if (seedRigidbody != null)
            {
                seedRigidbody.isKinematic = false;
            }

            // Destroy the original planted seed object
            Destroy(currentPlantedSeed);

            // Clear the reference to the dropped seed
            currentPlantedSeed = null;
            seedRigidbody = null;
        }
    }
}
