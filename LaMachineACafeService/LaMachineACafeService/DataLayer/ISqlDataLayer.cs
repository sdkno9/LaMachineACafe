using LaMachineACafe.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaMachineACafe.Common
{
    public interface ISqlDataLayer
    {
        List<T> ExecuteQuery<T>(SqlQueryBuilder query) where T : class, new();

        void Insert<T>(T objectToInsert) where T : class, new();

        void Update<T>(T objetToUpdate) where T : class, new();
    }
}