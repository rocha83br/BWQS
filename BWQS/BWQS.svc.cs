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

        public void Initialize(byte[] assemblyBuffer, string className, string serialData)
        {
            internalEngine = new BitWiseServiceBridge();
            internalEngine.Initialize(assemblyBuffer, className, serialData);
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

            var result = internalEngine.Where(bwqExpr, "True", serialType);

            return result;
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

        public string GroupBy(string grpExpr, string _byExpr)
        {
            if (internalEngine == null)
                throw new TypeInitializationException(initilizeMsg, null);

            return internalEngine.GroupBy(_byExpr, grpExpr, "True", "json");
        }

        public GroupResult GetGroupResult()
        {
            return new GroupResult();
        }

        #endregion        

        #region Public Methods Aliases

        public string Q(string bwqExpr, string serialType)
        {
            return Query(bwqExpr, serialType);
        }

        public string W(string bwqExpr, string serialType)
        {
            return Where(bwqExpr, serialType);
        }

        public string O(string bwqExpr, string serialType)
        {
            return OrderBy(bwqExpr, serialType);
        }

        public string OD(string bwqExpr, string serialType)
        {
            return OrderByDescending(bwqExpr, serialType);
        }

        public string G(string grpExpr, string _byExpr, string serialType)
        {
            return GroupBy(grpExpr, _byExpr);
        }

        #endregion
    }
}
