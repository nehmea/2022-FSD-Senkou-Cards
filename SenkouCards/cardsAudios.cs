//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SenkouCards
{
    using System;
    using System.Collections.Generic;
    
    public partial class cardsAudios
    {
        public int id { get; set; }
        public int cardId { get; set; }
        public byte[] audio { get; set; }
    
        public virtual cards cards { get; set; }
    }
}