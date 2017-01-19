using System;
using System.Xml;

namespace yhz.Dashboard.Common
{
    public static class XmlNodeEx
    {
        public static bool IsNullOrEmptyNode(this XmlNode xn)
        {
            return 
                xn == null 
                || string.IsNullOrEmpty(xn.InnerText)
                || !xn.HasChildNodes;
        }

        public static bool IsNullOrEmptyCNode(this XmlNode xn, string xpath)
        {
            if (IsNullOrEmptyNode(xn))
                return true;

            if (string.IsNullOrEmpty(xpath))
                throw new ArgumentNullException("xpath");

            return 
                !xn.HasChildNodes 
                || xn.SelectSingleNode(xpath) == null 
                || string.IsNullOrEmpty(xn.SelectSingleNode(xpath).InnerText);
        }
    }
}
