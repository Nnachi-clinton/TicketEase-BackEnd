
using System.Linq.Expressions;
using TicketEase.Domain.Entities;

namespace TicketEase.Application.Interfaces.Repositories
{
	public interface ITicketRepository : IGenericRepository<Ticket>
	{
		List<Ticket> GetTickets();
		void AddTicket(Ticket ticket);
		void DeleteTicket(Ticket ticket);
		public List<Ticket> FindTicket(Expression<Func<Ticket, bool>> condition);
		Ticket GetTicketById(string id);
		void UpdateTicket(Ticket ticket);
		Task<List<Ticket>> GetTicketByUserId(Expression<Func<Ticket, bool>> condition);
		Task<List<Ticket>> GetTicketByProjectId(Expression<Func<Ticket, bool>> condition);
	}
}
