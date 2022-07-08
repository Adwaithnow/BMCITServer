using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BMCIT.Models;
using Newtonsoft.Json;

namespace BMCIT.Services
{
    public class JsonOp
    {
        public static bool WriteData(dynamic data, string path)
        {
            try
            {
                string op = JsonConvert.SerializeObject(data, Formatting.Indented);
                System.IO.File.WriteAllText(path, op);
                return true;
            }
            catch (System.Exception)
            {
                return false;
                throw;
            }
        }

    }
}