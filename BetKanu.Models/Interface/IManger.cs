﻿using BET_KANU.ViewModels;
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
        bool Edit(MangerVM manger);
        MangerVM GetProductInfo(int ProdId);
        List<Product> GetAll(string Select);
        int Add(ProductEpisode episode);
        bool DeleteEP(int id);
        bool EditEP(ProductEpisode episode);
    }
}
