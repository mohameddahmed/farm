using UnityEngine;

public class TomatoPlantInteraction : MonoBehaviour
{
    public GameObject tomatoBunchPrefab; // Reference to the Tomato_Bunch prefab
    public string interactableTag = "InteractablePlant"; // Tag for interactable plants
    public string collectableTag = "TomatoBunch"; // Tag for collectable tomato bunches

    public PickUpWaterCan waterCanHandler; // Reference to the PickUpWaterCan script

    void Update()
    {
        // Check if the watering can is picked up and disable interaction if so
        if (waterCanHandler != null && waterCanHandler.IsPickedUp())
        {
            return; // Skip further processing if the watering can is held
        }

        // Check for left-click input
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast from the camera to where the mouse is pointing
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // If the raycast hits an object with the interactable tag (e.g., Plant)
                if (hit.collider.CompareTag(interactableTag))
                {
                    ReplaceWithTomatoBunch(hit.collider.gameObject);
                }
                // If the raycast hits an object with the collectable tag (e.g., Tomato Bunch)
                else if (hit.collider.CompareTag(collectableTag))
                {
                    CollectTomatoBunch(hit.collider.gameObject);
                }
            }
        }
    }

    void ReplaceWithTomatoBunch(GameObject clickedObject)
    {
        // Get the current position and rotation of the clicked object
        Vector3 position = clickedObject.transform.position;
        Quaternion rotation = clickedObject.transform.rotation;

        // Destroy the current plant object
        Destroy(clickedObject);

        // Instantiate the Tomato_Bunch prefab at the same position and rotation
        GameObject tomatoBunch = Instantiate(tomatoBunchPrefab, position, rotation);

        // Check if the tag is set correctly; if not, apply it again
        if (tomatoBunch.tag != collectableTag)
        {
            tomatoBunch.tag = collectableTag;
            Debug.Log("Tag applied to Tomato Bunch: " + tomatoBunch.tag);
        }
    }

    void CollectTomatoBunch(GameObject tomatoBunch)
    {
        // Handle collecting the tomato bunch (e.g., adding to inventory)
        // For now, we will just destroy it to simulate collection
        Destroy(tomatoBunch);

        // Optionally, you can add more logic to keep track of the collected tomatoes
        Debug.Log("Tomato Bunch Collected!");
    }
}
