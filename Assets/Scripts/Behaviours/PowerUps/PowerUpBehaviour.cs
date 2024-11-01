using System;
using Model;
using UnityEngine;
using UnityEngine.UIElements;

namespace Behaviours.PowerUps
{
    public abstract class PowerUpBehaviour : MonoBehaviour
    {
        protected PowerUp PowerUpData { get; set; }
        protected abstract VisualElement UiElement { get; set; }


        public static event Action PowerUpRemoved;
        public static event Action<PowerUp, VisualElement> PowerUpAdded;



        public abstract void ResetDuration(); 
        

        protected void Start() => PowerUpAdded?.Invoke(PowerUpData, UiElement);

        
        
        protected void RemovePowerUp()
        {
            PowerUpRemoved?.Invoke(); 
            Destroy(this);
        }
        
    }
}