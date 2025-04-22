using Funcionarios.Domain.Models;
using Funcionarios.Infra.Data.Context;
using Funcionarios.Infra.Data.Interfaces;
using Funcionarios.Infra.Data.Repositories.Parametros;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funcionarios.Infra.Data.Repositories;

public class FuncionarioRepository : AggregateRepository<FuncionarioModel>, IFuncionarioRepository
{
    public FuncionarioRepository(ApiContext context) : base(context) { }

    //public IUnitOfWork UnitOfWork => throw new NotImplementedException();

    //public void Dispose()
    //{
    //    throw new NotImplementedException();
    //}

    public async Task<IEnumerable<FuncionarioModel>> GetAll(FuncionarioQuery args)
    {
        var table = Track(_dbSet, false);
        table = MontaWhere(table, args);


        //return await table.ToListAsync();
        //var table = Track(_dbSet.Include(x => x.historicoAcaoModel), false);
        //table = MontaWhere(table, args);


        return await table.ToListAsync();
    }

    public async Task<FuncionarioModel> GetById(int id, bool track = false)
    {
        return await Track(_dbSet, false).FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public async void Insert(FuncionarioModel funcionariosModel)
    {
        await _dbSet.AddAsync(funcionariosModel);
    }

    public void Update(FuncionarioModel funcionariosModel)
    {
        _dbSet.Update(funcionariosModel);
    }



    private IQueryable<FuncionarioModel> MontaWhere(IQueryable<FuncionarioModel> table, FuncionarioQuery sel)
    {

        if (sel.Id > 0)
            table = table.Where(e => e.Id.Equals(sel.Id));

        if (!string.IsNullOrEmpty(sel.Nome))
            table = table.Where(e => e.Nome.Equals(sel.Nome));

        if (!string.IsNullOrEmpty(sel.Sobrenome))
            table = table.Where(e => e.Sobrenome.Equals(sel.Sobrenome));

        if (!string.IsNullOrEmpty(sel.Departamento))
            table = table.Where(e => e.Departamento.Equals(sel.Departamento));

        if (sel.Ativo != null)
            table = table.Where(e => e.Ativo.Equals(sel.Ativo.Value));

        if (sel.Turno != null)
            table = table.Where(e => e.Turno.Equals(sel.Turno));

        if (sel.DataDeCriacao.HasValue && sel.DataDeCriacao.Value != DateTime.MinValue)
            table = table.Where(e => e.DataDeCriacao.HasValue && e.DataDeCriacao.Value.Date == sel.DataDeCriacao.Value.Date);


        if (sel.DataAlteracao.HasValue && sel.DataAlteracao.Value != DateTime.MinValue)
            table = table.Where(e => e.DataAlteracao.HasValue && e.DataAlteracao.Value.Date == sel.DataAlteracao.Value.Date);


        if (!string.IsNullOrEmpty(sel.Email))
            table = table.Where(e => e.Email.Equals(sel.Email));

        
        return table;
    }
}
