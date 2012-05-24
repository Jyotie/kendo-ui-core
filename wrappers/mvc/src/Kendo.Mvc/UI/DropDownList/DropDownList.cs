namespace Kendo.Mvc.UI
{
    using Extensions;

    using System.Web.Mvc;
    using System.Linq;
    using System.Collections.Generic;
    using System.Web.Routing;

    public class DropDownList : ViewComponentBase
    {
        private bool hasItems = false;
        //private readonly IList<IEffect> defaultEffects = new List<IEffect> { new SlideAnimation() };

        public DropDownList(ViewContext viewContext, IClientSideObjectWriterFactory clientSideObjectWriterFactory, IUrlGenerator urlGenerator)
            : base(viewContext, clientSideObjectWriterFactory)
        {
            UrlGenerator = urlGenerator;

            ClientEvents = new Dictionary<string, object>();
            DropDownHtmlAttributes = new RouteValueDictionary();
            HiddenInputHtmlAttributes = new RouteValueDictionary();
            
            //defaultEffects.Each(el => Effects.Container.Add(el));

            Items = new List<DropDownItem>();
            SelectedIndex = 0;
            Enabled = true;
            Encoded = true;
            Delay = 500;
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        public new string Id
        {
            get
            {
                // Return from htmlattributes if user has specified
                // otherwise build it from name
                return HiddenInputHtmlAttributes.ContainsKey("id") ?
                       HiddenInputHtmlAttributes["id"].ToString() :
                       (!string.IsNullOrEmpty(Name) ? Name.Replace(".", HtmlHelper.IdAttributeDotReplacement) : null);
            }
        }

        public string CascadeTo
        {
            get;
            set;
        }

        public bool Encoded
        {
            get;
            set;
        }

        public IUrlGenerator UrlGenerator 
        {
            get; 
            set; 
        }

        public IDictionary<string, object> ClientEvents
        {
            get;
            private set;
        }

        public IDictionary<string, object> DropDownHtmlAttributes
        {
            get;
            private set;
        }

        public IDictionary<string, object> HiddenInputHtmlAttributes
        {
            get;
            private set;
        }

        public Effects Effects
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the items of the treeview.
        /// </summary>
        public IList<DropDownItem> Items
        {
            get;
            private set;
        }
        
        public int SelectedIndex
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public bool Enabled 
        { 
            get; 
            set; 
        }

        public int Delay
        {
            get;
            set;
        }

        public string Placeholder
        {
            get;
            set;
        }

        public override void WriteInitializationScript(System.IO.TextWriter writer)
        {
            IClientSideObjectWriter objectWriter = ClientSideObjectWriterFactory.Create(Id, "tDropDownList", writer);

            objectWriter.Start();

            objectWriter.Append("delay", Delay, 500);
            objectWriter.Append("placeholder", this.Placeholder);
            objectWriter.Append("cascadeTo", this.CascadeTo);

            if (hasItems)
            {
                objectWriter.AppendCollection("data", Items);
            }
            else
            {
                objectWriter.Append("selectedValue", this.GetValue<string>(Value));
            }

            objectWriter.Append("index", SelectedIndex, 0);

            if (DropDownHtmlAttributes.Any()) 
            {
                objectWriter.Append("dropDownAttr", DropDownHtmlAttributes.ToAttributeString());
            }

            objectWriter.Append("enabled", this.Enabled, true);
            objectWriter.Append("encoded", this.Encoded, true);

            objectWriter.Complete();

            base.WriteInitializationScript(writer);
        }

        protected override void WriteHtml(System.Web.UI.HtmlTextWriter writer)
        {
            hasItems = Items.Any();
            //this.AddPlaceholderItem();
            //if (hasItems)
            //{
            //    this.SyncSelectedIndex();
            //}

            (new DropDownListHtmlBuilder(this)).Build().WriteTo(writer);

            base.WriteHtml(writer);
        }
    }
}
