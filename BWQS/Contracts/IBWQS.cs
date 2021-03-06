﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace System.Linq.Dynamic.BitWise.Service
{
    [ServiceContract]
    public interface IBWQS
    {
        [OperationContract]
        void Initialize(string packedAssemblyBuffer, string className, string packedDataSource);

        [OperationContract]
        string Query(string bwqExpr, string serialType);

        [OperationContract]
        string Where(string bwqExpr, string serialType);

        [OperationContract]
        string OrderBy(string bwqExpr, string serialType);

        [OperationContract]
        string OrderByDescending(string bwqExpr, string serialType);

        [OperationContract]
        string GroupBy(string grpExpr, string _byExpr);

        //---

        [OperationContract]
        string Q(string bwqExpr, string serialType);

        [OperationContract]
        string W(string bwqExpr, string serialType);

        [OperationContract]
        string O(string bwqExpr, string serialType);

        [OperationContract]
        string OD(string bwqExpr, string serialType);

        [OperationContract]
        string G(string grpExpr, string _byExpr, string serialType);

        [OperationContract]
        GroupResult GetGroupResult();
    }
}
