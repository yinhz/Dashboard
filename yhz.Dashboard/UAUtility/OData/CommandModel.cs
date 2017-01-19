
using Newtonsoft.Json.Linq;
namespace UAUtility.OData
{
    /// <summary>
    /// command model
    /// </summary>
    public class CommandModel
    {
        public CommandModel(string appName, string cmdName, dynamic paramsObj)
        {
            this.AppName = appName;
            this.CommandName = cmdName;
            this.Params = paramsObj;
        }

        /// <summary>
        /// app name
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// app command name
        /// </summary>
        public string CommandName { get; set; }
        /// <summary>
        /// command params
        /// </summary>
        public dynamic Params { get; set; }
    }
}
