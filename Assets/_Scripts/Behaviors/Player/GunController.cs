using UnityEngine;

public class GunController : MonoBehaviour
{
    
    public float yRotation = 90f;

    [SerializeField]
    private GameObject reticlePrefab;
    private GameObject reticle;

    [HideInInspector]
    public bool targeting;

    private void Start()
    {   
        reticle = Instantiate(reticlePrefab, new Vector3(0,0,0), Quaternion.identity);
        reticle.GetComponent<Renderer>().material.color = Color.red;
        targeting = true;
    }
    void Update()
    {
        
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, 30f));
        reticle.transform.position = mouseWorldPosition;

        // Calculate the direction vector from the gun to the mouse cursor
        Vector3 direction = mouseWorldPosition - transform.position;
        direction.y = 0f;
        direction.Normalize();

        // Calculate the rotation quaternion based on the direction vector
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        targetRotation *= Quaternion.Euler(0f, yRotation - 90f, 90f);
        
        
            // Set the rotation of the gun to the calculated rotation quaternion
            transform.rotation = targetRotation;
          
    }

    /**
    void Update()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, 30f));
        reticle.transform.position = mouseWorldPosition;

        // Calculate the direction vector from the gun to the mouse cursor
        Vector3 direction = mouseWorldPosition - transform.position;
        direction.z = 0f;
        direction.Normalize();

        // Calculate the rotation quaternion based on the direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - yRotation;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = rotation;
    }*/
    /**
    // Update is called once per frame
    void Update()
    {

        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, 30f));
        //Debug.Log(mouseScreenPosition + " -> " + mouseWorldPosition);
        reticle.transform.position = mouseWorldPosition;
        Vector3 direction = mouseWorldPosition - transform.position;
        direction.z = 0f;
        Debug.Log(direction);
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation = Quaternion.Euler(rotation.z, rotation.y, rotation.x);

        this.transform.rotation = rotation;
    }*/
    private void OnDestroy()
    {
        Destroy(reticle);
    }
}
