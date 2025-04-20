using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Modal
{
    public interface IWindowService
    {
        public bool? ShowDialog<T> (T window);
    }
}
