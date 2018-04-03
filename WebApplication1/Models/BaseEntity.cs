using LiteDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public abstract class BaseEntity
    {

        [BsonId(true)]
        public int Id { get; set; }

    }
}