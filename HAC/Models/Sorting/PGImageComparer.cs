using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Image
/// </summary>
public class PGImageNameASCComparer : IComparer<PGImage>
{
    public int Compare(PGImage x, PGImage y)
    {
        return (x.FileName.CompareTo(y.FileName));
    }
}


public class ImageNameDESCComparer : IComparer<PGImage>
{
    public int Compare(PGImage x, PGImage y)
    {
        return (y.FileName.CompareTo(x.FileName));
    }
}


public class ImageDateASCComparer : IComparer<PGImage>
{
    public int Compare(PGImage x, PGImage y)
    {
        return (x.TimeStamp.CompareTo(y.TimeStamp));
    }
}

public class ImageDateDESCComparer : IComparer<PGImage>
{
    public int Compare(PGImage x, PGImage y)
    {
        return (y.TimeStamp.CompareTo(x.TimeStamp));
    }
}
