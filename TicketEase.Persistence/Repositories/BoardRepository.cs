using TicketEase.Application.Interfaces.Repositories;
using TicketEase.Domain.Entities;
using TicketEase.Persistence.Context;

namespace TicketEase.Persistence.Repositories
{
	public class BoardRepository : GenericRepository<Board>, IBoardRepository
	{
        private readonly TicketEaseDbContext _ticketEaseDbContext;
		public BoardRepository(TicketEaseDbContext ticketEaseDbContext): base(ticketEaseDbContext)
        {
            _ticketEaseDbContext = ticketEaseDbContext;
        }

		public List<Board> GetBoards()
		{
			throw new NotImplementedException();
		}
	}
}
