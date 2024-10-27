using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectController : MonoBehaviour
{
    public float moveSpeed;
    [FormerlySerializedAs("_destroyLocation")] public float destroyLocation; 
    
    private void Awake()
    {
        destroyLocation = -transform.position.x-1; 
    }
    
    private void FixedUpdate()
    {
        transform.Translate(Vector2.left * (moveSpeed *Time.deltaTime));
        
        if (transform.position.x < destroyLocation)
        {
            Destroy(gameObject);
        }
    }
}
