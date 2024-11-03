using Managers;
using Model;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UIElements;

namespace UIElements
{
    public class UpgradeBar : VisualElement
    {
        private PowerUp _powerUp;
        private readonly Button _upgradeButton;
        
        public UpgradeBar(PowerUp powerUp, VisualElement parent)
        {
            _powerUp = powerUp;
            generateVisualContent += GenerateVisualContent;
            
            style.flexDirection = FlexDirection.Row;
            style.flexGrow = 0;
            style.flexShrink = 0;
            style.visibility = parent.style.visibility;
            style.width = Length.Percent(100);
            style.height = Length.Percent(30);

            _upgradeButton = new Button();
            _upgradeButton.text = "+";
            _upgradeButton.clicked += UpgradeButtonOnClicked;
            _upgradeButton.style.width = Length.Percent(10);
            _upgradeButton.style.left = Length.Percent(85);
            _upgradeButton.style.height = Length.Percent(50);
            _upgradeButton.style.top = Length.Percent(50);
            _upgradeButton.style.fontSize = Length.Percent(100);
            Add(_upgradeButton);
        }

        private int GetCurrentLevel()
            => CollectableManager.GetUpgrade(_powerUp).UpgradeLevel;

        private void UpgradeButtonOnClicked()
        {
            CollectableManager.Upgrade(_powerUp);
            MarkDirtyRepaint();
        }

        private void GenerateVisualContent(MeshGenerationContext context)
        {
            var currentUpgradeCost = CollectableManager.GetUpgradeCost(_powerUp);
            _upgradeButton.SetEnabled(
                currentUpgradeCost <= CollectableManager.CoinsCollected && GetCurrentLevel() < _powerUp.UpgradeLevels
                );
           
            
            context.DrawText(_powerUp.Name, new Vector2(0, 0), 40, Color.black);
            context.DrawText($"Cost: {currentUpgradeCost}", new Vector2(contentRect.width * 0.8f, 0),40, Color.black);
            
            float height = contentRect.height;
            float spacing = (contentRect.width * 0.8f) / _powerUp.UpgradeLevels;
            
            var painter = context.painter2D;
            painter.lineWidth = 4;
            painter.strokeColor = Color.black;
            
            
            for (int i = 0; i < _powerUp.UpgradeLevels; i++)
            {
                painter.fillColor =  i >= GetCurrentLevel() ? Color.gray : Color.green;
                
                painter.BeginPath();
                
                painter.MoveTo(new Vector2(spacing*i, height/2));
                painter.LineTo(new Vector2(spacing*(i+1), height/2));
                painter.LineTo(new Vector2(spacing * (i + 1), height));
                painter.LineTo(new Vector2(spacing * i, height));
                painter.LineTo(new Vector2(spacing * i, height/2));

                painter.ClosePath();
                
                painter.Fill();
                painter.Stroke();
            }
        }
    }
}