using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard
{
    public interface IParameter : IParameter<string>
    { }

    public interface IParameter<TKey>
    {
        TKey Id { get; set; }
        string Name { get; set; }
        string Code { get; set; }
        string Description { get; set; }
    }
}
