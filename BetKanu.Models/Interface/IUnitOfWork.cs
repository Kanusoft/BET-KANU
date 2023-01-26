using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetKanu.Models.Interface
{
    public interface IUnitOfWork
    {
        public IBKBundle bKBundle { get;}
        public IProduct product { get;}
        public IManger manger { get;}
        public IShop Shop { get;}
    }
}
