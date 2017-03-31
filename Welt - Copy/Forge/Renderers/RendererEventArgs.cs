using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.Forge.Renderers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class RendererEventArgs<TItem, TVertex> : EventArgs where TVertex : struct, IVertexType 
    {
        /// <summary>
        /// 
        /// </summary>
        public TItem Item { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Mesh<TVertex> Result { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPriority { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="result"></param>
        /// <param name="isPriority"></param>
        public RendererEventArgs(TItem item, Mesh<TVertex> result, bool isPriority)
        {
            Item = item;
            Result = result;
            IsPriority = isPriority;
        }
    }
}
