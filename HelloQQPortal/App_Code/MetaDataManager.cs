using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using HelloQQPortal.Database;
using System.Data;
using System.Data.Entity;

namespace HelloQQPortal.Manager
{
    public class MetaDataManager
    {
        private helloqqdbEntities dbInfo;

        public MetaDataManager()
        {

        }

        public hqq_meta_location GetMetaLocationInfo(string strLocation)
        {
            hqq_meta_location result;
            using (dbInfo = new helloqqdbEntities())
            {
                result = dbInfo.hqq_meta_location.Where(
                    item => item.name == strLocation
                || item.keyword.Contains(strLocation)).FirstOrDefault();
            }

            return result;
        }
    }
}