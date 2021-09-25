using Models;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Interfaces
{
   public interface IPaisData
    {
        Result InsertPais(Pais item);

        Result UpdatePais(int id, Pais item);

        List<Pais> GetPaises(int page, int limit);

        Pais GetPais(int id);

        Result DeletePais(int id);
    }
}
