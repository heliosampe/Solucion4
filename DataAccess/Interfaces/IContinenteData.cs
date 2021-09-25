using Models;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Interfaces
{
    public interface IContinenteData
    {
        Result InsertContinente(Continente item);

        Result UpdateContinente(int id, Continente item);

        List<Continente> GetContinentes(int page, int limit);

        Continente GetContinente(int id);

        Result DeleteContinente(int id);
    }
}
