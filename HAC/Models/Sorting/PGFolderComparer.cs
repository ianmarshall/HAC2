using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class PGFolderComparer : IComparer<PGFolder> {

    public static PGFolderComparer GetComparerBySortAction(SortAction sortAction, PGFolderComparer inheritComparer){
     
        PGFolderComparer comparer = new PGFolderComparer();
        if (sortAction == SortAction.NameASC)
        {
            comparer = new PGFolderNameASCComparer();
        }
        else if (sortAction == SortAction.NameDESC)
        {
            comparer = new PGFolderNameDESCComparer();
        }
        else if (sortAction == SortAction.DateTimeASC)
        {
            comparer= new PGFolderDateASCComparer();
        }
        else if (sortAction == SortAction.DateTimeDESC)
        {
            comparer= new PGFolderDateDESCComparer();
        }
        else if (sortAction == SortAction.CustomOrderProperty)
        {
            comparer = new PGFolderOrderComparer();
        }
        else if (sortAction == SortAction.Inherit)
        {
            comparer = inheritComparer;
        }
        else
        {
            throw new Exception("Comparer not found");
        }        

        return comparer;
    }


    public virtual int Compare(PGFolder x, PGFolder y)
    {
        return (x.Name.CompareTo(y.Name));
    }
}

public class PGFolderOrderComparer : PGFolderComparer
{
    public override int Compare(PGFolder x, PGFolder y)
    {
        return (x.Order.CompareTo(y.Order));
    }
}

public class PGFolderNameASCComparer : PGFolderComparer
{
    public override int Compare(PGFolder x, PGFolder y)
    {
        return (x.Name.CompareTo(y.Name));
    }
}


public class PGFolderNameDESCComparer : PGFolderComparer
{
    public override int Compare(PGFolder x, PGFolder y)
    {
        return (y.Name.CompareTo(x.Name));
    }
}


public class PGFolderDateASCComparer : PGFolderComparer
{
    public override int Compare(PGFolder x, PGFolder y)
    {
        return (x.TimeStamp.CompareTo(y.TimeStamp));
    }
}



public class PGFolderDateDESCComparer : PGFolderComparer
{
    public override int Compare(PGFolder x, PGFolder y)
    {
        return (y.TimeStamp.CompareTo(x.TimeStamp));
    }
}

