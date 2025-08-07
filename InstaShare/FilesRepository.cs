using ContactAdministrationSystem.Infrastructure;
using ContactSystem.Application.Entities;
using ContactSystem.Application.Repositories.Interfaces;
using ContactSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class FilesRepository : EntityRepository<OfficeEntity, Guid>, IOfficesRepository
{
	private readonly GraniteDataContext _context;

	public FilesRepository(GraniteDataContext context) : base(context)
	{
		_context = context;
	}
}
