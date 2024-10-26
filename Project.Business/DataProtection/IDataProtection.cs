using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Business.DataProtection
{
    public interface IDataProtection
    {
        // Gönderilen metini şifreleyece.
        string Protect(string text);
        string UnProtect(string protectedText);
    }
}
