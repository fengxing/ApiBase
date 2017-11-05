using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// request response entity
/// </summary>
namespace ApiBase.Entity
{

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class doc
    {

        private docAssembly assemblyField;

        private docMember[] membersField;

        /// <remarks/>
        public docAssembly assembly
        {
            get
            {
                return this.assemblyField;
            }
            set
            {
                this.assemblyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("member", IsNullable = false)]
        public docMember[] members
        {
            get
            {
                return this.membersField;
            }
            set
            {
                this.membersField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class docAssembly
    {

        private string nameField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class docMember
    {

        private docMemberSummary summaryField;

        private string[] valueField;

        private object nullField;

        private docMemberParam[] paramField;

        private object returnsField;

        private string nameField;

        /// <remarks/>
        public docMemberSummary summary
        {
            get
            {
                return this.summaryField;
            }
            set
            {
                this.summaryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value")]
        public string[] value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        public object @null
        {
            get
            {
                return this.nullField;
            }
            set
            {
                this.nullField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("param")]
        public docMemberParam[] param
        {
            get
            {
                return this.paramField;
            }
            set
            {
                this.paramField = value;
            }
        }

        /// <remarks/>
        public object returns
        {
            get
            {
                return this.returnsField;
            }
            set
            {
                this.returnsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class docMemberSummary
    {

        private object nullField;

        private string[] valueField;

        private string[] textField;

        /// <remarks/>
        public object @null
        {
            get
            {
                return this.nullField;
            }
            set
            {
                this.nullField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value")]
        public string[] value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class docMemberParam
    {

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }


}
