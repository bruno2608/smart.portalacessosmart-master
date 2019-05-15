using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class WebReturn<T>
{
    public long Code { get; set; }
    public string Message { get; set; }
    public T ObjectReturn { get; set; }

}