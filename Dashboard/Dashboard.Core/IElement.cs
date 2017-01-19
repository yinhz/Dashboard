using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard
{
    public interface IElement : IElement<string>
    {
    }

    public interface IElement<TKey> 
    {
        TKey Id { get; }

        string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        string Description { get; set; }
    }
}
