//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class like
    {
        public string uid { get; set; }
        public int id_post { get; set; }
        public Nullable<System.DateTime> time { get; set; }
    
        public virtual post post { get; set; }
        public virtual user user { get; set; }
    }
}
