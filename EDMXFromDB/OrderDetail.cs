//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EDMXFromDB
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderDetail
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    
        public virtual Order Order { get; set; }
    }
}
