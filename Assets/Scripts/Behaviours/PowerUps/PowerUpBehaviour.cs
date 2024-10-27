using System;
using System.Linq;
using JetBrains.Annotations;
using Managers;
using Model;
using UnityEngine;
using UnityEngine.UIElements;

namespace Behaviours.PowerUps
{
    public abstract class PowerUpBehaviour : MonoBehaviour
    {
        protected PowerUp PowerUp { get; set; }
        protected abstract VisualElement UiElement { get; set; }


        public static event Action PowerUpRemoved;
        public static event Action<PowerUp, VisualElement> PowerUpAdded;



        public abstract void ResetDuration(); 
        

        protected void Start() => PowerUpAdded?.Invoke(PowerUp, UiElement);

        
        
        protected void RemovePowerUp()
        {
            PowerUpRemoved?.Invoke();
            Destroy(this);
        }
        
    }
}