using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public float moveSpeed;
    private float destroyLocation; 
    
    // Start is called before the first frame update
    public virtual void Start()
    {
        //Sets x position where object is destroyed (out of view).
        destroyLocation = -transform.position.x-1;
    }
    
    public virtual void FixedUpdate()
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
