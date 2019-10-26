using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro.KeyStore.Api.Keys.Models
{
    public class Key
    {
        public string Id { set; get; }
        public string Body { set; get; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(250)]
        public string Sha { set; get; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string ShortSha { set; get; }

        public DateTime CreatedAt { set; get; } = DateTime.Now;
    }
}
