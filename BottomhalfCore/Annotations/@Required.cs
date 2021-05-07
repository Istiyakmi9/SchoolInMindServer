using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottomhalfCore.Annotations
{
    /// <summary>
    /// Make field mandatory by calling IBottomhalf ValidateModalService class
    /// Use: _oValidateModalService.ValidateModalFieldsService(typeof(Class), Your-Object)
    /// </summary>
    public class @Required : Attribute
    {
        /// <summary>
        /// Default: If no value found it will inject default value of current type.
        /// Based on property type.
        /// </summary>
        public @Required(Boolean Default) { }
        public @Required() { }
    }
}
