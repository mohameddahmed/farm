using UnityEngine;

public class Pouring : MonoBehaviour
{
    public Transform WaterCan; // Assign the watering can object in the Inspector
    public float rotationAngle = 90f; // Angle to rotate when pouring
    public float rotationSpeed = 30f; // Speed of rotation
    public GameObject plantPrefab; // Small plant prefab
    public GameObject mediumPlantPrefab; // Medium plant prefab (e.g., Plant_Tomato_Medium)
    public GameObject finalPlantPrefab; // Final plant prefab (e.g., fully grown tomato plant)
    public float wateringRange = 1.5f; // Range to detect seeds
    public float growthTime = 5f; // Time for each growth stage
    public Scoring scoring;

    void Update()
    {
        RotateCan();

        if (Input.GetMouseButton(0)) // Check if left mouse button is held
        {
            WaterSeeds();
        }
    }

    void RotateCan()
    {
        // Check if the left mouse button is held down
        bool isPouring = Input.GetMouseButton(0);
        float targetAngle = isPouring ? rotationAngle : 0;
        Quaternion targetRotation = Quaternion.Euler(targetAngle, 0, 0);

        WaterCan.localRotation = Quaternion.Lerp(WaterCan.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    void WaterSeeds()
    {
        GameObject[] seeds = GameObject.FindGameObjectsWithTag("Seed");

        foreach (GameObject seed in seeds)
        {
            if (Vector3.Distance(transform.position, seed.transform.position) < wateringRange)
            {
                // Replace the seed with a small plant and start its growth cycle
                GameObject smallPlant = Instantiate(plantPrefab, seed.transform.position, Quaternion.identity);
                Destroy(seed);

                // Start the plant growth process
                StartCoroutine(GrowPlant(smallPlant));
                scoring.AddScore(1);
                break;
            }
        }
    }

    // Coroutine to handle the plant's growth stages
    private System.Collections.IEnumerator GrowPlant(GameObject plant)
    {
        // Wait for the initial growth time, then grow to medium stage
        yield return new WaitForSeconds(growthTime);

        // Transition to the medium plant
        GameObject mediumPlant = Instantiate(mediumPlantPrefab, plant.transform.position, Quaternion.identity);
        Destroy(plant);

        // Wait again for the next growth stage, then grow to the final stage
        yield return new WaitForSeconds(growthTime);

        // Transition to the final plant stage
        GameObject finalPlant = Instantiate(finalPlantPrefab, mediumPlant.transform.position, Quaternion.identity);
        
        // Set the tag of the final plant to "InteractablePlant"
        finalPlant.tag = "InteractablePlant"; 

        // Destroy the medium plant after the final plant has been placed
        Destroy(mediumPlant);
    }
}
