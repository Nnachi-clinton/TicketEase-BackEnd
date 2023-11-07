
using System.Linq.Expressions;
using TicketEase.Application.Interfaces.Repositories;
using TicketEase.Domain.Entities;
using TicketEase.Persistence.Context;

namespace TicketEase.Persistence.Repositories
{
	public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
	{
		public TicketRepository(TicketEaseDbContext ticketEaseDbContext) : base(ticketEaseDbContext) { }

		public void AddTicket(Ticket ticket)
		{
			Add(ticket);
		}

		public void DeleteTicket(Ticket ticket)
		{
			Delete(ticket);
		}

		public List<Ticket> FindTicket(Expression<Func<Ticket, bool>> condition)
		{
			return Find(condition);
		}

		public Ticket GetTicketById(string id)
		{
			return GetById(id);
		}

		public List<Ticket> GetTickets()
		{
			return GetAll();
		}

		public void UpdateTicket(Ticket ticket)
		{
			Update(ticket);
		}
	}
}
