using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiSa.Web08.Core.Exceptions
{
    public class MISAValidateException:Exception
    {
        IDictionary MISAData = new Dictionary<string, object>();
        public MISAValidateException(object data)
        {
            this.MISAData.Add("ex", data);
        }
        public override string Message
        {
            get {

                return Properties.Resource.ValidateErrMsg;
            }
        }

        public override IDictionary Data
        {
            get
            {
                return MISAData;
            }
        }
    }
}
