using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public float moveSpeed;
    private float _destroyLocation; 
    
    private void Start()
    {
        _destroyLocation = -transform.position.x-1;
    }
    
    private void FixedUpdate()
    {
        transform.Translate(Vector2.left * (moveSpeed *Time.deltaTime));
        
        if (transform.position.x < _destroyLocation)
        {
            Destroy(gameObject);
        }
    }
}
