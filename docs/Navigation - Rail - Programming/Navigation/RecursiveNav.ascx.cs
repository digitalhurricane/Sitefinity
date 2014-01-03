﻿using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;
using Microsoft.VisualBasic;

namespace SitefinityWebApp.Custom.Navigation
{

    public partial class RecursiveNav : HierarchicalDataBoundControl
    {

        //  Template Declarations
        private ITemplate _HeaderTemplate;

        private ITemplate _FooterTemplate;

        private ITemplate _ItemTemplate;

        private ITemplate _ItemHeaderTemplate;

        private ITemplate _ItemFooterTemplate;

        [PersistenceMode(PersistenceMode.InnerProperty),
         TemplateContainer(typeof(MyDataItem))]
        public ITemplate HeaderTemplate
        {
            get
            {
                return _HeaderTemplate;
            }
            set
            {
                _HeaderTemplate = value;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty),
         TemplateContainer(typeof(MyDataItem))]
        public ITemplate FooterTemplate
        {
            get
            {
                return _FooterTemplate;
            }
            set
            {
                _FooterTemplate = value;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty),
         TemplateContainer(typeof(MyDataItem))]
        public ITemplate ItemTemplate
        {
            get
            {
                return _ItemTemplate;
            }
            set
            {
                _ItemTemplate = value;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty),
         TemplateContainer(typeof(MyDataItem))]
        public ITemplate ItemHeaderTemplate
        {
            get
            {
                return _ItemHeaderTemplate;
            }
            set
            {
                _ItemHeaderTemplate = value;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty),
         TemplateContainer(typeof(MyDataItem))]
        public ITemplate ItemFooterTemplate
        {
            get
            {
                return _ItemFooterTemplate;
            }
            set
            {
                _ItemFooterTemplate = value;
            }
        }

        protected override void CreateChildControls()
        {
            RecursiveCreateChildControls(GetData("").Select());
        }

        private void RecursiveCreateChildControls(IHierarchicalEnumerable dataItems)
        {
            bool firstItem = true;
            foreach (object e in dataItems)
            {
                //  Render the header only if we have child items
                if (firstItem)
                {
                    MyDataItem header = new MyDataItem();
                    HeaderTemplate.InstantiateIn(header);
                    Controls.Add(header);
                    firstItem = false;
                }
                IHierarchyData data = dataItems.GetHierarchyData(e);

                //  Find the data value based on the TextField
                string title = (string)DataBinder.GetPropertyValue(data, "Title");
                string url = (string)DataBinder.GetPropertyValue(data, "Url");

                //  Create an item header
                MyDataItem itemHeader = new MyDataItem();
                itemHeader.Url = url; 
                ItemHeaderTemplate.InstantiateIn(itemHeader);
                Controls.Add(itemHeader);
                itemHeader.DataBind();
                
                //  Create the data item
                MyDataItem item = new MyDataItem();
                item.Title = title;
                item.Url = url;
                ItemTemplate.InstantiateIn(item);
                Controls.Add(item);
                item.DataBind();

                //  Create any child items
                RecursiveCreateChildControls(data.GetChildren());

                //  Create the item footer
                MyDataItem itemFooter = new MyDataItem();
                ItemFooterTemplate.InstantiateIn(itemFooter);
                Controls.Add(itemFooter);
                itemFooter.DataBind();
            }

            //  If we had a header, then render out the footer
            if ((firstItem == false))
            {
                MyDataItem footer = new MyDataItem();
                FooterTemplate.InstantiateIn(footer);
                Controls.Add(footer);
            }
        }

        public override void RenderBeginTag(System.Web.UI.HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
            writer.AddAttribute("class", "active");
            writer.RenderBeginTag(HtmlTextWriterTag.Li);
        }
    }

    //  In most applications the DataItem and the container should be separated into
    //  two classes, in this case, I've collapsed them into itself since I only have
    //  one data property.  
    public class MyDataItem : Control, INamingContainer, IDataItemContainer
    {

        //  The item data
        private string _title;
        private string _url;


        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
            }
        }

        public string Url
        {
            get
            {
                return _url;
            }

            set
            {
                _url = value.Replace("~","");
            }
        }

        public object DataItem
        {
            get
            {
                return this;
            }
        }

        public int DataItemIndex
        {
            get
            {
                return 0;
            }
        }

        public int DisplayIndex
        {
            get
            {
                return 0;
            }
        }
    }
}