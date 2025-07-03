using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Models
{
    public interface IEntity<EId>
    {
        EId GetId();
    }
}