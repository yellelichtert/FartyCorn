using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

namespace UIElements
{
    
    public class FuelBar : VisualElement
    {
        
        private readonly float _totalFuel;
        private float _currentFuelLevel = 1;
        
        
        public FuelBar(float totalFuel)
        {
            _totalFuel = totalFuel;
            generateVisualContent += GenerateVisualContent;
            
            style.position = Position.Absolute;
            style.visibility = Visibility.Visible;
            style.flexGrow = 0;
            style.alignSelf = Align.Center;

            style.top = Length.Percent(5);
            style.width = Length.Percent(80);
            style.height = Length.Percent(1);
            
        }

        private void GenerateVisualContent(MeshGenerationContext context)
        {
            float width = contentRect.width;
            float height = contentRect.height;
            var painter = context.painter2D;
            
            painter.lineWidth = 4f;
            painter.fillColor = Color.gray;
            
            //Draz grey bar
            painter.BeginPath();
            painter.MoveTo(new Vector2(0, 0));
            
            painter.LineTo(new Vector2(width, 0));
            painter.LineTo(new Vector2(width, height));
            painter.LineTo(new Vector2(0, height));
            painter.ClosePath();
            painter.Fill();
            painter.Stroke();
            
            painter.fillColor = Color.magenta;
           
            //Draw red bar based on value
            painter.BeginPath();
            painter.MoveTo(new Vector2(0, 0));
            painter.LineTo(new Vector2(width / _currentFuelLevel, 0));
            painter.LineTo(new Vector2(width / _currentFuelLevel, height));
            painter.LineTo(new Vector2(0, height));
            painter.ClosePath();
            painter.Fill();
            painter.Stroke();
            
            

        }
        
        public float CurrentFuel
        {
            set
            {
                _currentFuelLevel = (_totalFuel/value);
                MarkDirtyRepaint();
            }
        }
        
       
        
        
    }
}