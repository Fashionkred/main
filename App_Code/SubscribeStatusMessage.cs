using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

/// <summary>
/// Summary description for SubscribeStatusMessage
/// </summary>
[DataContract]
public class SubscribeStatusMessage
{
	
    [DataMember]
    public string ErrorMessage { get; set; }
    
    [DataMember]
    public string RedirectUrl { set; get; }

}