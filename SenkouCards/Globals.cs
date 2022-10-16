using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenkouCards
{
    public class Globals
    {
        //Singleton DB Context
        private static SenkoucardsConfig _SenkouDbContextInternal;

        public static SenkoucardsConfig SenkouDbAuto
        {
            get
            {
                try
                {
                    if (_SenkouDbContextInternal == null)
                    {
                        _SenkouDbContextInternal = new SenkoucardsConfig();
                        return _SenkouDbContextInternal;
                    }
                }
                catch(SqlException ex)
                {
                    Debug.WriteLine("Database conneciton error" + ex.Message);
                }
                return _SenkouDbContextInternal;
            }
        }

    }
}
