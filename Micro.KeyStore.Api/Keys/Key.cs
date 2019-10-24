using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro.KeyStore.Api.Keys
{
    public class Key
    {
        public string Id { set; get; }
        public string Body { set; get; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(250)]
        public string Sha { set; get; } // needs index

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string ShortSha { set; get; } // needs index

        public DateTime CreatedAt = DateTime.Now;
    }
}
