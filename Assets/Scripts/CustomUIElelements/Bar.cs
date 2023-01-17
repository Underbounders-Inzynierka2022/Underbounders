using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BarsElements
{
    
    /// <summary>
    /// Status bar ui element
    /// </summary>
    public class Bar : VisualElement, INotifyValueChanged<int>
    {
        public int value
        {
            get
            {
                return m_value;
            }

            set
            {
                if (EqualityComparer<int>.Default.Equals(m_value, value))
                    return;
                if (this.panel != null)
                {
                    using (ChangeEvent<int> pooled = ChangeEvent<int>.GetPooled(this.m_value, value))
                    {
                        pooled.target = this;
                        this.SetValueWithoutNotify(value);
                        this.SendEvent((EventBase)pooled);
                    }
                }
                else
                {
                    SetValueWithoutNotify(value);
                }
            }
        }

        /// <summary>
        /// Name of spirte sheet
        /// </summary>
        public string spriteBaseName { set; get; }

        /// <summary>
        /// Name of sprite in sprite sheet
        /// </summary>
        public string spriteName { set; get; }

        /// <summary>
        /// Curent value of status bar
        /// </summary>
        private int m_value = 0;

        /// <summary>
        /// VisualElement displaying the proper sprite
        /// </summary>
        private VisualElement hbParent;

        /// <summary>
        /// Sets value without special notification
        /// </summary>
        /// <param name="newValue">
        /// New value to set
        /// </param>
        public void SetValueWithoutNotify(int newValue)
        {
            m_value = newValue;
        }
        /// <summary>
        /// Element as Uxml factory
        /// </summary>
        public new class UxmlFactory: UxmlFactory<Bar, UxmlTraits>
        {

        }

        /// <summary>
        /// Exclusive element traits factory for uxml editor and events
        /// </summary>
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription m_docName = new UxmlStringAttributeDescription() { name = "doc-name", defaultValue = "" };
            UxmlStringAttributeDescription m_spriteBaseName = new UxmlStringAttributeDescription() { name = "sprite-base-name", defaultValue = "" };
            UxmlStringAttributeDescription m_spriteName = new UxmlStringAttributeDescription() { name = "sprite-name", defaultValue = "" };
            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as Bar;

                ate.spriteBaseName = m_spriteBaseName.GetValueFromBag(bag, cc);
                ate.spriteName = m_spriteName.GetValueFromBag(bag, cc);
                ate.Clear();


                string docName = m_docName.GetValueFromBag(bag, cc);
                VisualTreeAsset vt = Resources.Load<VisualTreeAsset>(docName);
                if(vt != null)
                {
                    VisualElement bar = vt.Instantiate();
                    ate.hbParent = bar.Q<VisualElement>("Bar");
                    ate.Add(bar);

                    ate.RegisterValueChangedCallback(ate.UpdateBar);
                    ate.FillBar();
                }
                
            }
        }
        /// <summary>
        /// Loads new sprite as status bar from image
        /// </summary>
        /// <param name="imageName">
        /// Name of the image with sprites
        /// </param>
        /// <param name="spriteNames">
        /// Name of the sprite to update
        /// </param>
        /// <returns>
        /// Sprite to change with
        /// </returns>
        private Sprite Load(string imageName, string spriteNames)
        {
            Sprite[] all = Resources.LoadAll<Sprite>(imageName);

            foreach (var s in all)
            {
                if (s.name == spriteNames)
                {
                    return s;
                }
            }
            return null;
        }
        /// <summary>
        /// Status bar update 
        /// </summary>
        /// <param name="evt">
        /// Value updating event
        /// </param>
        private void UpdateBar(ChangeEvent<int> evt)
        {
            FillBar();
        }
        /// <summary>
        /// Fill the bar
        /// </summary>
        private void FillBar()
        {
            Sprite sp = Load(spriteBaseName, spriteName + m_value);
            if (sp != null)
                hbParent.style.backgroundImage = new StyleBackground(sp);
        }
    }
}

