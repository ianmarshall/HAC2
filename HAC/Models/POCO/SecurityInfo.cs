using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class SecurityInfo
{
    public SecurityInfo (){
        UserAccessList = new List<string>();
    }

    public bool PrivateFolder { get; set; }
    public List<string> UserAccessList { get; set; } //in case we set it private
}
