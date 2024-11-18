using Controllers;
using Model;
using UnityEngine;
using UnityEngine.UIElements;

namespace Behaviours.PowerUps
{
    public abstract class PowerUpBehaviour : MonoBehaviour
    {
        protected PowerUp PowerUpData { get; set; }
        protected abstract VisualElement UiElement { get; set; }

        
        
        public abstract void ResetDuration(); 
        

        
        protected void Start() 
        {
            PlayerController.Instance.ConfigureComponents(PowerUpData.Name);
            UIController.Instance.AddModifierElement(UiElement);
        }

        
        
        protected void RemovePowerUp()
        {
            PlayerController.Instance.ConfigureComponents();
            UiElement?.RemoveFromHierarchy();
            Destroy(this);
        }
        
    }
}