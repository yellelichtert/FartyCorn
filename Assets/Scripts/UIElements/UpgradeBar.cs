using UnityEngine;
using UnityEngine.UIElements;

namespace UIElements
{
    public class UpgradeBar : VisualElement
    {
        private readonly int _upgradeLevels;
        private readonly string _upgradeName;
        private readonly float _upgradeCost;
        
        private int _currentUpgradeLevel;

        public UpgradeBar(string upgradeName, int upgradeLevels, float upgradeCost)
        {
            generateVisualContent += GenerateVisualContent;
            
            _upgradeName = upgradeName;
            _upgradeLevels = upgradeLevels;
            _upgradeCost = upgradeCost;
        }


        private void GenerateVisualContent(MeshGenerationContext context)
        {
            float width = contentRect.width;
            float height = contentRect.height;
            
            context.DrawText(_upgradeName, new Vector2(contentRect.xMin, contentRect.yMin), 30, Color.white);
            
            var painter = context.painter2D;
            
            painter.lineWidth = 4;
            painter.fillColor = Color.black;
            
            //Draw full bar
            painter.MoveTo(new Vector2(0,0));
            painter.LineTo(new Vector2(1,0));
            painter.LineTo(new Vector2(1,1));
            painter.LineTo(new Vector2(0,1));
            
        }
    }
}