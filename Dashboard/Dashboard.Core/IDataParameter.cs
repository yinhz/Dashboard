using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard
{
    public interface IDataParameter : IDataParameter<string>
    { }
    public interface IDataParameter<TKey> : IParameter<TKey>
    {
        string Value { get; set; }
    }
}
