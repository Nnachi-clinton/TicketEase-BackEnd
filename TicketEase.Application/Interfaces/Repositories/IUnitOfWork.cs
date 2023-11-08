using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketEase.Domain.Entities;

namespace TicketEase.Application.Interfaces.Repositories
{
	public interface IUnitOfWork : IDisposable
	{
		IBoardRepository BoardRepository { get; }
		ICommentRepository CommentRepository { get; }
		IManagerRepository ManagerRepository { get; }
		IPaymentRepository PaymentRepository { get; }
		IProjectRepository ProjectRepository { get; }	
		ITicketRepository TicketRepository { get; }
		IUserRepository UserRepository { get; }


       
        int SaveChanges();
	}
}
