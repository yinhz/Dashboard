using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard
{
    public interface IElementType<TElementTypeParameter> : IElementType<string, TElementTypeParameter>
        where TElementTypeParameter : IElementTypeParameter<string>
    {
    }

    public interface IElementType<TKey, TElementTypeParameter>
        where TElementTypeParameter : IElementTypeParameter<TKey>
    {
        TKey Id { get; }
        string Name { get; set; }
        string Description { get; set; }

        string HtmlTemplate { get; set; }
        string CssTemplate { get; set; }
        string JavaScriptTemplate { get; set; }

        ICollection<TElementTypeParameter> ElementTypeParameters { get; set; }
    }
}
