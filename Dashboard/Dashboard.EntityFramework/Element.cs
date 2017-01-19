using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.EntityFramework
{
    public class Element<TKey> : IElement<TKey>
    {
        public int ColumnIndex { get; set; }

        public int ColumnSpan { get; set; }

        public string CssTemplate { get; set; }

        public IData Data { get; set; }

        public string Descript { get; set; }

        public string ElementId { get; set; }

        public string ElementName { get; set; }

        public IElementType ElementType { get; set; }

        public string HtmlTemplate { get; set; }

        public TKey Id
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string JavaScriptTemplate
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string LocElementId
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string RenderHtml
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int RowIndex
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int RowSpan
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int ZIndex
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
