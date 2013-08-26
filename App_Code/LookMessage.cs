using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using ShopSenseDemo;

/// <summary>
/// Summary description for LookMessage
/// </summary>
[DataContract(Name = "LookMessage")]
[KnownType(typeof(LookMessage))]
public class LookMessage
{
    [DataMember]
    public string ErrorMessage { set; get; }

    [DataMember]
    public Look Look { set; get; }

    [DataMember]
    public string LookDescription { set; get; }
}