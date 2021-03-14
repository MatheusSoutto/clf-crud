using System;
using System.Collections.Generic;

#nullable disable

namespace ClfApi.Models
{
    public partial class Clf
    {
        public Guid Id { get; set; }
        public string IpAddress { get; set; }
        public string RfcIdentity { get; set; }
        public string UserId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Request { get; set; }
        public int StatusCode { get; set; }
        public int? ResponseSize { get; set; }
        public string Client { get; set; }
        public string UserAgent { get; set; }
    }
}
