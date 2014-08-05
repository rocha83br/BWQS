using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Linq.Dynamic.BitWise;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace System.Linq.Dynamic.BitWise.Service
{
    public class BWQS : IBWQS
    {
        #region Declarations

        static string initilizeMsg = "Initialize BWQS first.";
        static BitWiseServiceBridge internalEngine = null;

        #endregion

        #region Public Methods

        public void Initialize(string packedAssemblyBuffer, string className, string packedDataSource)
        {
            internalEngine = new BitWiseServiceBridge();
            internalEngine.Initialize(packedAssemblyBuffer, className, packedDataSource);
        }

        public string Query(string bwqExpr, string serialType)
        {
            if (internalEngine == null)
                throw new TypeInitializationException(initilizeMsg, null);

            return internalEngine.Query(bwqExpr, "True", serialType);
        }

        public string Where(string bwqExpr, string serialType)
        {
            if (internalEngine == null)
                throw new TypeInitializationException(initilizeMsg, null);

            return internalEngine.Where(bwqExpr, "True", serialType);
        }

        public string OrderBy(string bwqExpr, string serialType)
        {
            if (internalEngine == null)
                throw new TypeInitializationException(initilizeMsg, null);

            return internalEngine.OrderBy(bwqExpr, "True", serialType);
        }

        public string OrderByDescending(string bwqExpr, string serialType)
        {
            if (internalEngine == null)
                throw new TypeInitializationException(initilizeMsg, null);

            return internalEngine.OrderByDescending(bwqExpr, "True", serialType);
        }

        #endregion        
    }
}
