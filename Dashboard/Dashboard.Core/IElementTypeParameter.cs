using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard
{
    public interface IElementTypeParameter : IElementTypeParameter<string>
    { }
    public interface IElementTypeParameter<TKey> : IParameter<TKey>
    {
        string RenderScript { get; set; }
    }
}
