using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard
{
    public interface ITerminalParameter : ITerminalParameter<string>
    { }

    public interface ITerminalParameter<TKey> : IParameter<TKey>
    {
        string Value { get; set; }
    }
}
