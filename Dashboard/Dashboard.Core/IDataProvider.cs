using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard
{
    public interface IDataProvider<TKey>
    {
        TKey Id { get; set; }

        string Name { get; set; }

        object Query(string command);

        object Query(string command, Dictionary<string, string> paras);
    }
}
