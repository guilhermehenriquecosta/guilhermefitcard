using Dapper;
using fitcard_estabelecimentos.domain.Domain;
using Microsoft.Extensions.Configuration;
using Slapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace fitcard_estabelecimentos.data.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly string _connectionString;

        public CategoriaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("EstabelecimentosBase");
        }

        public async Task<IEnumerable<Categoria>> BuscaTodas()
        {
            using var conexao = new SqlConnection(_connectionString);
            var sql = "SELECT " +
                        "c.nome, c.dataCriacao, c.dataEdicao, c.excluido, c.id, "+
                        "e.id 'Estabelecimentos_id', e.idCategoria 'Estabelecimentos_idCategoria', e.razaoSocial 'Estabelecimentos_razaoSocial', e.nomeFantasia 'Estabelecimentos_nomeFantasia', e.cnpj 'Estabelecimentos_cnpj', e.email 'Estabelecimentos_email', e.endereco 'Estabelecimentos_endereco', e.cidade 'Estabelecimentos_cidade', e.estado 'Estabelecimentos_estado', e.telefone 'Estabelecimentos_telefone', e.dataCadastro 'Estabelecimentos_dataCadastro', e.status 'Estabelecimentos_status', e.agencia 'Estabelecimentos_agencia', e.conta 'Estabelecimentos_conta', e.dataCriacao 'Estabelecimentos_dataCriacao', e.dataEdicao 'Estabelecimentos_dataEdicao', e.excluido 'Estabelecimentos_excluido' " +
                        "FROM dbo.Categoria c " +
                        "LEFT JOIN dbo.Estabelecimento e on e.idCategoria = c.id AND e.excluido = 0 " +
                        "WHERE c.excluido = 0";

            var categorias = await conexao.QueryAsync<dynamic>(sql);

            AutoMapper.Configuration.AddIdentifier(typeof(Categoria), "id");
            AutoMapper.Configuration.AddIdentifier(typeof(Estabelecimento), "id");

            List<Categoria> categoriasMap = (AutoMapper.MapDynamic<Categoria>(categorias) as IEnumerable<Categoria>).ToList();

            return categoriasMap;
        }

        public async Task<Categoria> BuscaUma(Guid id)
        {
            using var conexao = new SqlConnection(_connectionString);
            var sql = "SELECT " +
                        "c.nome, c.dataCriacao, c.dataEdicao, c.excluido, c.id, " +
                        "e.id 'Estabelecimentos_id', e.idCategoria 'Estabelecimentos_idCategoria', e.razaoSocial 'Estabelecimentos_razaoSocial', e.nomeFantasia 'Estabelecimentos_nomeFantasia', e.cnpj 'Estabelecimentos_cnpj', e.email 'Estabelecimentos_email', e.endereco 'Estabelecimentos_endereco', e.cidade 'Estabelecimentos_cidade', e.estado 'Estabelecimentos_estado', e.telefone 'Estabelecimentos_telefone', e.dataCadastro 'Estabelecimentos_dataCadastro', e.status 'Estabelecimentos_status', e.agencia 'Estabelecimentos_agencia', e.conta 'Estabelecimentos_conta', e.dataCriacao 'Estabelecimentos_dataCriacao', e.dataEdicao 'Estabelecimentos_dataEdicao', e.excluido 'Estabelecimentos_excluido' " +
                        "FROM dbo.Categoria c " +
                        "LEFT JOIN dbo.Estabelecimento e on e.idCategoria = c.id AND e.excluido = 0 " +
                        "WHERE c.id = @Id AND c.excluido = 0";
            var categoria = await conexao.QueryFirstOrDefaultAsync<dynamic>(sql, new { Id = id });

            AutoMapper.Configuration.AddIdentifier(typeof(Categoria), "id");
            AutoMapper.Configuration.AddIdentifier(typeof(Estabelecimento), "id");

            Categoria categoriaMap = (AutoMapper.MapDynamic<Categoria>(categoria) as Categoria);

            return categoriaMap;
        }

        public async Task<Guid> Insere(Categoria categoria)
        {
            using var conexao = new SqlConnection(_connectionString);
            var sql = "INSERT INTO dbo.Categoria " +
                "(nome, dataCriacao, dataEdicao, excluido) " +
                "OUTPUT inserted.id " +
                "VALUES " +
                "(@nome, GETDATE(), GETDATE(), 0) ";
            var idInserido = await conexao.QuerySingleAsync<Guid>(sql, new
            {
                nome = categoria.Nome
            });

            return idInserido;
        }

        public async Task<int> Edita(Categoria categoria)
        {
            using var conexao = new SqlConnection(_connectionString);
            var sql = "UPDATE dbo.Categoria SET " +
                " nome = @nome " +
                ", dataEdicao = GETDATE() " +
                "WHERE " +
                "Id = @Id " +
                "AND excluido = 0";
            var qtdLinhasAfetadas = await conexao.ExecuteAsync(sql, new
            {
                nome = categoria.Nome,
                id = categoria.Id
            });

            return qtdLinhasAfetadas;
        }

        public async Task<int> Remove(Guid id)
        {
            using var conexao = new SqlConnection(_connectionString);
            //var sql = "DELETE FROM dbo.Categoria " +
            //    "WHERE Id = @Id ";
            var sql = "UPDATE dbo.Categoria SET excluido = 1 " +
                "WHERE Id = @Id AND excluido = 0";
            var qtdLinhasAfetadas = await conexao.ExecuteAsync(sql, new
            {
                Id = id
            });

            return qtdLinhasAfetadas;
        }
    }
}
