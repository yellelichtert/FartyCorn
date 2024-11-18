using UnityEngine;

namespace Controllers
{
    public class ObjectController : MonoBehaviour
    {
    
        public float moveSpeed;
        public float destroyLocation; 
    
    
    
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
}
