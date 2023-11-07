
using TicketEase.Application.Interfaces.Repositories;
using TicketEase.Domain.Entities;
using TicketEase.Persistence.Context;

namespace TicketEase.Persistence.Repositories
{
	public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
	{
		public PaymentRepository(TicketEaseDbContext ticketEaseDbContext) : base(ticketEaseDbContext) { }
	}
}
