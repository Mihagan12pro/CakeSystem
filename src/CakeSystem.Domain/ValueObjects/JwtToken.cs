using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace CakeSystem.Domain.ValueObjects
{
    public class JwtToken
    {
        public string Audience { get; set; }

        public string Issuer { get; set; }

        public string IssuerSigningKey { get; set; }

        public int ExpiresMinutes { get; set; }
    }
}
