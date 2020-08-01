namespace KianCommons.UI {
    using ColossalFramework.UI;
    using System;
    using UnityEngine;

    public class UICheckBoxExt : UICheckBox {
        public override void Awake() {
            base.Awake();
            name = nameof(UICheckBoxExt);
            height = 30f;
            clipChildren = true;

            UISprite sprite = AddUIComponent<UISprite>();
            sprite.spriteName = "ToggleBase";
            sprite.size = new Vector2(19f, 19f);
            sprite.relativePosition = new Vector2(0, (height - sprite.height) / 2);

            checkedBoxObject = sprite.AddUIComponent<UISprite>();
            ((UISprite)checkedBoxObject).spriteName = "ToggleBaseFocused";
            checkedBoxObject.size = sprite.size;
            checkedBoxObject.relativePosition = Vector3.zero;

            label = AddUIComponent<UILabel>();
            label.text = GetType().Name;
            label.textScale = 0.9f;
            label.relativePosition = new Vector2(sprite.width + 5f, (height - label.height) / 2 + 1);

            eventCheckChanged += (component, value) => OnCheckChanged();
        }

        public virtual string Label {
            get => label.text;
            set {
                label.text = value;
                Invalidate();
            }
        }

        public virtual string Tooltip {
            get => tooltip;
            set {
                tooltip = value;
                RefreshTooltip();
            }
        }

        public override void Start() {
            base.Start();
            width = parent.width;
        }
    }
}


