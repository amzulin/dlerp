using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Invoicing.Common
{

    public static class JsonHelper
    {
        // Methods
        public static T FromJson<T>(string strJson) where T : class
        {
            try
            {
                if (!strJson.IsNullOrEmpty<string>())
                {
                    return JsonConvert.DeserializeObject<T>(strJson);
                }
            }
            catch
            { }
            return default(T);
        }

        public static string ToJson(object t)
        {
            JsonSerializerSettings g__initLocal0 = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include
            };
            //return JsonConvert.SerializeObject(t, Formatting.Indented, g__initLocal0);
            return JsonConvert.SerializeObject(t);
        }

        public static string ToJson(object t, bool HasNullIgnore)
        {
            if (HasNullIgnore)
            {
                JsonSerializerSettings g__initLocal1 = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                return JsonConvert.SerializeObject(t, Formatting.Indented, g__initLocal1);
            }
            return ToJson(t);
        }
    }


}