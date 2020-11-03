using fitcard_estabelecimentos.domain.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fitcard_estabelecimentos.data.Repository
{
    public interface IEstabelecimentoRepository
    {
        public Task<IEnumerable<Estabelecimento>> BuscaTodos();
        public Task<Estabelecimento> BuscaUm(Guid id);
        public Task<Guid> Insere(Estabelecimento estabelecimento);
        public Task<int> Edita(Estabelecimento estabelecimento);
        public Task<int> Remove(Guid id);
    }
}