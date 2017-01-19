using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Dashboard.EntityFramework
{

    public class Dashboard<TElement> : Dashboard<string, TElement> where TElement : IElement<string>
    { }

    public class Dashboard<TKey, TElement> : IDashboard<TKey, TElement> where TElement : IElement<TKey>
    {
        public virtual string BackgroundImagePath { get; set; }

        public virtual int ColumnNums { get; set; }

        public virtual DateTime CreateTime { get; set; }

        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual ICollection<TElement> Element { get; set; }

        public virtual JObject QueryParas { get; set; }

        public virtual int RowNums { get; set; }
    }
}
