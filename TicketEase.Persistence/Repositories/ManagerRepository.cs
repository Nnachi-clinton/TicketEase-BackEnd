
using TicketEase.Application.Interfaces.Repositories;
using TicketEase.Domain.Entities;
using TicketEase.Persistence.Context;

namespace TicketEase.Persistence.Repositories
{
	public class ManagerRepository : GenericRepository<Manager>, IManagerRepository
	{
		public ManagerRepository(TicketEaseDbContext ticketEaseDbContext) : base(ticketEaseDbContext) { }
	}
}
