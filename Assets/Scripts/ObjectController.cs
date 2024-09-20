using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public float moveSpeed;
    private float destroyLocation; 
    
    // Start is called before the first frame update
    void Start()
    {
        //Sets x position where object is destroyed (out of view).
        destroyLocation = -transform.position.x-1;
    }
    
    void FixedUpdate()
    {
        //Moves object at constant rate.
        transform.Translate(Vector2.left * moveSpeed *Time.deltaTime);
        
        //Checks if object is out of view, and destroys when true.
        if (transform.position.x < destroyLocation)
        {
            Destroy(gameObject);
        }
        
    }
}
