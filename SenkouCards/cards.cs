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
    
    public partial class cards
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cards()
        {
            this.cardsAudios = new HashSet<cardsAudios>();
            this.cardsImages = new HashSet<cardsImages>();
            this.responses = new HashSet<responses>();
        }
    
        public int id { get; set; }
        public string front { get; set; }
        public string back { get; set; }
        public int points { get; set; }
        public int deckId { get; set; }
    
        public virtual decks decks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cardsAudios> cardsAudios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cardsImages> cardsImages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<responses> responses { get; set; }
    }
}
