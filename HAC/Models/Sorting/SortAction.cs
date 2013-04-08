using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SortAction
/// </summary>
public enum SortAction
{
    NameASC=0,
    NameDESC=1,
    DateTimeASC=2,
    DateTimeDESC = 3,
    CustomOrderProperty = 5, //use the folder property "Order"
    Inherit=6

}

