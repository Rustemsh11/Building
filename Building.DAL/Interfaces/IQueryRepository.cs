using Building.Domain.Entity;

namespace Building.DAL.Interfaces
{

    public interface IQueryRepository : IBaseRepository<Query>
    {
        IEnumerable<Query> GetAll();
        IEnumerable<Query> GetDelivered(int id);
        IEnumerable<Query> GetDeliveredBySnab(int id);
        IEnumerable<Query> GetNoAgreement();
        IEnumerable<Query> GetMPZ(int id);



    }
}
