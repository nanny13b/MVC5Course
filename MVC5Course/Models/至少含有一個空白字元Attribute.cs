using System;
using System.ComponentModel.DataAnnotations;

namespace MVC5Course.Models
{
    public class 至少含有一個空白字元Attribute : DataTypeAttribute
    {
        public 至少含有一個空白字元Attribute() : base(DataType.Text)
        {
        }


        public override bool IsValid(object value)
        {
            if (value.ToString().Contains(" "))
                return true;
            else
                return false;
            //return base.IsValid(value);

            ///SampleCode
            //public override bool IsValid(object value)
            ///{
            ///    var str = (string)value;

            ///    return str.Contains(" ");
            ///}
        }
    }


}