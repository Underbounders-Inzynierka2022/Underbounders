using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BarsElements
{
    

    public class Bar : VisualElement, INotifyValueChanged<int>
    { 

        public string spriteBaseName { set; get; }

        public string spriteName { set; get; }

        private int m_value = 0;
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
                if(this.panel != null)
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

        public VisualElement hbParent;

        public new class UxmlFactory: UxmlFactory<Bar, UxmlTraits>
        {

        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            //UxmlIntAttributeDescription m_count = new UxmlIntAttributeDescription() { name = "count", defaultValue =5};
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

                //ate.count = m_count.GetValueFromBag(bag, cc);
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

        public void SetValueWithoutNotify(int newValue)
        {
            m_value = newValue;
        }

        private void FillBar()
        {
            Sprite sp = Load(spriteBaseName, spriteName + m_value);
            if (sp != null)
                hbParent.style.backgroundImage = new StyleBackground(sp);
        }

        private void UpdateBar(ChangeEvent<int> evt)
        {
            FillBar();
        }
    }
}

