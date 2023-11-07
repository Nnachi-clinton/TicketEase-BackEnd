
using TicketEase.Application.Interfaces.Repositories;
using TicketEase.Domain.Entities;
using TicketEase.Persistence.Context;

namespace TicketEase.Persistence.Repositories
{
	public class CommentRepository : GenericRepository<Comment>, ICommentRepository
	{
        public CommentRepository(TicketEaseDbContext ticketEaseDbContext) : base(ticketEaseDbContext) { }
	}
}
