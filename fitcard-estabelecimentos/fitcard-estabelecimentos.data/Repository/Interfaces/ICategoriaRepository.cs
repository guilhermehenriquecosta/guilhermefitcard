using fitcard_estabelecimentos.domain.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fitcard_estabelecimentos.data.Repository
{
    public interface ICategoriaRepository
    {
        public Task<IEnumerable<Categoria>> BuscaTodas();
        public Task<Categoria> BuscaUma(Guid id);
        public Task<Guid> Insere(Categoria categoria);
        public Task<int> Edita(Categoria categoria);
        public Task<int> Remove(Guid id);
    }
}