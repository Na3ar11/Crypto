using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Modal
{
    public interface IApiSevice
    {
        Task<IEnumerable<Coin>> GetPage(int pageNumber);

        Task<IEnumerable<Coin>> SearchCoin(string name);
    }
}
