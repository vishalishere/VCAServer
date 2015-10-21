using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VCACommon
{


    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class vca
    {

        private vcaVca_hdr vca_hdrField;

        private vcaObject[] objectsField;

        private vcaEvent[] eventsField;

        private vcaCount[] countsField;

        private decimal schema_versionField;

        /// <remarks/>
        public vcaVca_hdr vca_hdr
        {
            get
            {
                return this.vca_hdrField;
            }
            set
            {
                this.vca_hdrField = value;
            }
        }

        [XmlArrayItem("object")]
        public vcaObject[] objects
        {
            get
            {
                return this.objectsField;
            }
            set
            {
                this.objectsField = value;
            }
        }

        [XmlArrayItem("event")]
        public vcaEvent[] events
        {
            get
            {
                return this.eventsField;
            }
            set
            {
                this.eventsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("count", IsNullable = false)]
        public vcaCount[] counts
        {
            get
            {
                return this.countsField;
            }
            set
            {
                this.countsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal schema_version
        {
            get
            {
                return this.schema_versionField;
            }
            set
            {
                this.schema_versionField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class vcaVca_hdr
    {

        private uint frame_idField;

        private byte vca_statusField;

        private byte trk_modeField;

        /// <remarks/>
        public uint frame_id
        {
            get
            {
                return this.frame_idField;
            }
            set
            {
                this.frame_idField = value;
            }
        }

        /// <remarks/>
        public byte vca_status
        {
            get
            {
                return this.vca_statusField;
            }
            set
            {
                this.vca_statusField = value;
            }
        }

        /// <remarks/>
        public byte trk_mode
        {
            get
            {
                return this.trk_modeField;
            }
            set
            {
                this.trk_modeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class vcaObject
    {
        private int idField;

        private int csField;

        private int caField;

        private int clsField;

        private string cls_nameField;

        private vcaObjectsObjectBB bbField;

        private vcaObjectsObjectPT[] trailField;

        /// <remarks/>
        public int id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public int cs
        {
            get
            {
                return this.csField;
            }
            set
            {
                this.csField = value;
            }
        }

        /// <remarks/>
        public int ca
        {
            get
            {
                return this.caField;
            }
            set
            {
                this.caField = value;
            }
        }

        /// <remarks/>
        public int cls
        {
            get
            {
                return this.clsField;
            }
            set
            {
                this.clsField = value;
            }
        }

        /// <remarks/>
        public string cls_name
        {
            get
            {
                return this.cls_nameField;
            }
            set
            {
                this.cls_nameField = value;
            }
        }

        /// <remarks/>
        public vcaObjectsObjectBB bb
        {
            get
            {
                return this.bbField;
            }
            set
            {
                this.bbField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("pt", IsNullable = false)]
        public vcaObjectsObjectPT[] trail
        {
            get
            {
                return this.trailField;
            }
            set
            {
                this.trailField = value;
            }
        }

    }


    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class vcaObjectsObjectBB
    {

        private ushort xField;

        private ushort yField;

        private ushort wField;

        private ushort hField;

        /// <remarks/>
        public ushort x
        {
            get
            {
                return this.xField;
            }
            set
            {
                this.xField = value;
            }
        }

        /// <remarks/>
        public ushort y
        {
            get
            {
                return this.yField;
            }
            set
            {
                this.yField = value;
            }
        }

        /// <remarks/>
        public ushort w
        {
            get
            {
                return this.wField;
            }
            set
            {
                this.wField = value;
            }
        }

        /// <remarks/>
        public ushort h
        {
            get
            {
                return this.hField;
            }
            set
            {
                this.hField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class vcaObjectsObjectPT
    {

        private ushort xField;

        private ushort yField;

        /// <remarks/>
        public ushort x
        {
            get
            {
                return this.xField;
            }
            set
            {
                this.xField = value;
            }
        }

        /// <remarks/>
        public ushort y
        {
            get
            {
                return this.yField;
            }
            set
            {
                this.yField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class vcaEvent
    {
        private int idField;

        private string typeField;

        private int rule_idField;

        private string rule_nameField;

        private string rule_typeField;

        private int zone_idField;

        private string zone_nameField;

        private int obj_idField;

        private string obj_cls_nameField;

        private int statusField;

        private vcaEventsEventStart_time start_timeField;

        private vcaEventsEventEnd_time end_timeField;

        private vcaEventsEventBB bbField;

        /// <remarks/>
        public int id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public int rule_id
        {
            get
            {
                return this.rule_idField;
            }
            set
            {
                this.rule_idField = value;
            }
        }

        /// <remarks/>
        public string rule_name
        {
            get
            {
                return this.rule_nameField;
            }
            set
            {
                this.rule_nameField = value;
            }
        }

        /// <remarks/>
        public string rule_type
        {
            get
            {
                return this.rule_typeField;
            }
            set
            {
                this.rule_typeField = value;
            }
        }

        /// <remarks/>
        public int zone_id
        {
            get
            {
                return this.zone_idField;
            }
            set
            {
                this.zone_idField = value;
            }
        }

        /// <remarks/>
        public string zone_name
        {
            get
            {
                return this.zone_nameField;
            }
            set
            {
                this.zone_nameField = value;
            }
        }

        /// <remarks/>
        public int obj_id
        {
            get
            {
                return this.obj_idField;
            }
            set
            {
                this.obj_idField = value;
            }
        }

        /// <remarks/>
        public string obj_cls_name
        {
            get
            {
                return this.obj_cls_nameField;
            }
            set
            {
                this.obj_cls_nameField = value;
            }
        }

        /// <remarks/>
        public int status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public vcaEventsEventStart_time start_time
        {
            get
            {
                return this.start_timeField;
            }
            set
            {
                this.start_timeField = value;
            }
        }

        /// <remarks/>
        public vcaEventsEventEnd_time end_time
        {
            get
            {
                return this.end_timeField;
            }
            set
            {
                this.end_timeField = value;
            }
        }

        /// <remarks/>
        public vcaEventsEventBB bb
        {
            get
            {
                return this.bbField;
            }
            set
            {
                this.bbField = value;
            }
        }

    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class vcaEventsEventStart_time
    {

        private int sField;

        private int msField;

        /// <remarks/>
        public int s
        {
            get
            {
                return this.sField;
            }
            set
            {
                this.sField = value;
            }
        }

        /// <remarks/>
        public int ms
        {
            get
            {
                return this.msField;
            }
            set
            {
                this.msField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class vcaEventsEventEnd_time
    {

        private uint sField;

        private ushort msField;

        /// <remarks/>
        public uint s
        {
            get
            {
                return this.sField;
            }
            set
            {
                this.sField = value;
            }
        }

        /// <remarks/>
        public ushort ms
        {
            get
            {
                return this.msField;
            }
            set
            {
                this.msField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class vcaEventsEventBB
    {

        private ushort xField;

        private ushort yField;

        private ushort wField;

        private ushort hField;

        /// <remarks/>
        public ushort x
        {
            get
            {
                return this.xField;
            }
            set
            {
                this.xField = value;
            }
        }

        /// <remarks/>
        public ushort y
        {
            get
            {
                return this.yField;
            }
            set
            {
                this.yField = value;
            }
        }

        /// <remarks/>
        public ushort w
        {
            get
            {
                return this.wField;
            }
            set
            {
                this.wField = value;
            }
        }

        /// <remarks/>
        public ushort h
        {
            get
            {
                return this.hField;
            }
            set
            {
                this.hField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class vcaCount
    {

        private int idField;

        private string nameField;

        private int valField;

        /// <remarks/>
        public int id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

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

        /// <remarks/>
        public int val
        {
            get
            {
                return this.valField;
            }
            set
            {
                this.valField = value;
            }
        }
    }




}
