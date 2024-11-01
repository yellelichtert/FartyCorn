using System.Linq;
using System.Threading.Tasks;
using Managers;
using Model;
using UnityEngine;

namespace Behaviours.Modifiers
{
    public class RotateCamera : IModifier
    {
        private readonly Modifier _data;
        private Camera _camera;

        public RotateCamera()
        {
            _data = CollectableManager.Modifiers.First(m => m.Name == GetType().Name);
            
            _camera = Camera.main;
        }
        
        public async void Apply()
        {
            Rotate();

            await Task.Delay((int)(_data.Duration * 1000));
            
            Rotate();
        }
        
        private void Rotate() => _camera.transform.Rotate(new Vector3(180, 0, 0));
    }
}