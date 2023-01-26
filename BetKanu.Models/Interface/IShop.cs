using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetKanu.Models.Interface
{
    public interface IShop
    {
        List<Shop> GetAll();
        Shop Getone(int id);
    }
}
