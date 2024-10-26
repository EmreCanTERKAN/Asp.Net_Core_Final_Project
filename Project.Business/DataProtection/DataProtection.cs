using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Business.DataProtection
{
    public class DataProtection : IDataProtection
    {
        private readonly IDataProtector _protector;
        public DataProtection(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("Security-V1");
        }
        public string Protect(string text)
        {
            return _protector.Protect(text);
        }

        public string UnProtect(string protectedText)
        {
            return _protector.Unprotect(protectedText);
        }
    }
}
