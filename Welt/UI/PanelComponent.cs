#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.UI
{
    public class PanelComponent : UIComponent, ICollection<UIComponent>
    {
        public PanelComponent(string name, int width, int height, GraphicsDevice device, params UIComponent[] components)
            : this(name, width, height, null, device, components)
        {
        }

        public PanelComponent(string name, int width, int height, UIComponent parent, GraphicsDevice device,
            params UIComponent[] components) : base(name, width, height, parent, device)
        {
            AddRange(components);
        }

        public IEnumerator<UIComponent> GetEnumerator() => Components.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(UIComponent item)
        {
            AddComponent(item);
        }

        public void AddRange(UIComponent[] items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void Clear()
        {
            Components.Clear();
        }

        public bool Contains(UIComponent item)
        {
            return Components.ContainsValue(item);
        }

        public void CopyTo(UIComponent[] array, int arrayIndex)
        {
            Components.Values.CopyTo(array, arrayIndex);
        }

        public bool Remove(UIComponent item)
        {
            if (!Components.ContainsKey(item.Name)) return false;
            RemoveComponent(item.Name);
            return true;
        }

        public int Count => Components.Count;
        public bool IsReadOnly => false;
    }
}