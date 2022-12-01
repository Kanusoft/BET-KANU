using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetKanu.Models.Interface
{
    public interface IManger
    {
        int Add(Product product);
        bool Edit(Product product);
        bool Delete(int id);
    }
}
