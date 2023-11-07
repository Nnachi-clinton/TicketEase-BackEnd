
using TicketEase.Domain.Entities;

namespace TicketEase.Application.Interfaces.Repositories
{
	public interface IBoardRepository : IGenericRepository<Board>
	{
		List<Board> GetBoards();
	}
}
