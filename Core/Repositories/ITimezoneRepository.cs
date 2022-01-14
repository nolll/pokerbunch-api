using System.Collections.Generic;
using Core.Entities;

namespace Core.Repositories;

public interface ITimezoneRepository
{
    IList<Timezone> List();
}