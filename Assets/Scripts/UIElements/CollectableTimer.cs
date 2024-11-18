using System;
using Model;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

namespace UIElements
{
    
    public class CollectableTimer : VisualElement
    {
        
        private readonly float _totalDuration;
        private readonly VisualElement _progressBar;
        private readonly Color _fillColor;
        
        private float _remainingDuration = 1;
        
        
        
        public CollectableTimer(CollectableData collectable, Color fillColor)
        {
            _totalDuration = collectable.Duration;
            _fillColor = fillColor;
            
            style.display = DisplayStyle.Flex;
            style.flexDirection = FlexDirection.Row;
            style.visibility = Visibility.Visible;
            style.flexGrow = 0;
            style.alignSelf = Align.Center;

            style.top = Length.Percent(5);
            style.width = Length.Percent(95);
            style.height = Length.Percent(4);


            VisualElement collectableSprite = new VisualElement
            {
                style =
                {
                    height = Length.Percent(100),
                    width = Length.Percent(15),
                    backgroundImage = new StyleBackground(Resources.Load<Sprite>(ResourcePaths.CollectableSprites + collectable.Name)),
                    visibility = Visibility.Visible
                }
            };
            Add(collectableSprite);


            _progressBar = new VisualElement();
            _progressBar.generateVisualContent += GenerateVisualContent;
            Add(_progressBar);
        }

        private void GenerateVisualContent(MeshGenerationContext context)
        {
            float defaultWidth = contentRect.width*0.85f;
            float height = contentRect.height;
            var painter = context.painter2D;
            
            painter.lineWidth = 4f;
            
            DrawBar(defaultWidth, Color.gray);
            DrawBar(defaultWidth/_remainingDuration, _fillColor);
           
            
            void DrawBar(float width, Color fill)
            {
                painter.fillColor = fill;
                
                painter.BeginPath();
                painter.MoveTo(new Vector2(0, height*0.25f));
            
                painter.LineTo(new Vector2(width, height*0.25f));
                painter.LineTo(new Vector2(width, height*0.75f));
                painter.LineTo(new Vector2(0, height*0.75f));
                painter.ClosePath();
                painter.Fill();
                painter.Stroke();
            }
        }
        
        public float RemainingDuration
        {
            set
            {
                _remainingDuration = (_totalDuration/value);
                _progressBar.MarkDirtyRepaint();
            }
        }
    }
}