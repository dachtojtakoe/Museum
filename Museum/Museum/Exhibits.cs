//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Museum
{
    using System;
    using System.Collections.Generic;
    
    public partial class Exhibits
    {
        public int id { get; set; }
        public string name { get; set; }
        public int type_id { get; set; }
        public int hall_id { get; set; }
        public int author_id { get; set; }
        public string description { get; set; }
        public Nullable<bool> deleted { get; set; }
    
        public virtual Authors Authors { get; set; }
        public virtual ExhibitTypes ExhibitTypes { get; set; }
        public virtual Halls Halls { get; set; }
    }
}
