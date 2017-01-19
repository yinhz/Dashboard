using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard
{
    /// <summary>
    /// Dashboard Data
    /// </summary>
    public interface IData : IData<string>
    {
    }

    /// <summary>
    /// Dashboard Data
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IData<TKey>
    {
        /// <summary>
        /// Key
        /// </summary>
        TKey Id { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// Dashboard will get data by this command, the result will be serialize by json.net)
        /// </summary>
        string Command { get; set; }
    }
}
