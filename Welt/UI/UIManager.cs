#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.UI
{
    public class UIManager : UIComponent
    {
        private readonly List<UIComponent> _levels;

        public UIManager(Game game) : base("__m", game.Window.ClientBounds.Width, game.Window.ClientBounds.Height, null, game.GraphicsDevice)
        {
            _levels = new List<UIComponent>();
        }

        /// <summary>
        ///     Adds a UIElement to the last non-null layer.
        /// </summary>
        /// <param name="component"></param>
        public new void AddComponent(UIComponent component)
        {
            for (var i = 0; i < _levels.Count; i++)
            {
                if (_levels[i] == null) AddComponent(component, i - 1);
            }
            // there's either no null layers or we reached the last item
            AddComponent(component, _levels.Count - 1);
        }

        /// <summary>
        ///     Adds a UIElement to the specified layer.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="level"></param>
        public void AddComponent(UIComponent component, int level)
        {
            _levels[level].AddComponent(component);
        }
    }
}