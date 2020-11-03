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
    public class EstabelecimentoRepository : IEstabelecimentoRepository
    {
        private readonly string _connectionString;

        public EstabelecimentoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("EstabelecimentosBase");
        }

        public async Task<IEnumerable<Estabelecimento>> BuscaTodos()
        {
            using var conexao = new SqlConnection(_connectionString);
            
            var sql = "SELECT " +
                        "e.id, e.idCategoria, e.razaoSocial, e.nomeFantasia, e.cnpj, e.email, e.endereco, e.cidade, e.estado, e.telefone, e.dataCadastro, e.status, e.agencia, e.conta, e.dataCriacao, e.dataEdicao, e.excluido, " +
                        "c.id 'Categoria_id', c.nome 'Categoria_nome', c.dataCriacao 'Categoria_dataCriacao', c.dataEdicao 'Categoria_dataEdicao', c.excluido 'Categoria_excluido' " +                        
                        "FROM dbo.Estabelecimento e " +
                        "LEFT JOIN dbo.Categoria c on c.id = e.idCategoria AND c.excluido = 0 " +
                        "WHERE e.excluido = 0" + 
                        "ORDER BY 3";
            var estabelecimentos = await conexao.QueryAsync<dynamic>(sql);

            AutoMapper.Configuration.AddIdentifier(typeof(Estabelecimento), "id");
            AutoMapper.Configuration.AddIdentifier(typeof(Categoria), "id");

            List<Estabelecimento> estabelecimentosMap = (AutoMapper.MapDynamic<Estabelecimento>(estabelecimentos) as IEnumerable<Estabelecimento>).ToList();

            return estabelecimentosMap;
        }

        public async Task<Estabelecimento> BuscaUm(Guid id)
        {
            using var conexao = new SqlConnection(_connectionString);
            var sql = "SELECT " +
                        "e.id, e.idCategoria, e.razaoSocial, e.nomeFantasia, e.cnpj, e.email, e.endereco, e.cidade, e.estado, e.telefone, e.dataCadastro, e.status, e.agencia, e.conta, e.dataCriacao, e.dataEdicao, e.excluido, " +
                        "c.id 'Categoria_id', c.nome 'Categoria_nome', c.dataCriacao 'Categoria_dataCriacao', c.dataEdicao 'Categoria_dataEdicao', c.excluido 'Categoria_excluido' " +
                        "FROM dbo.Estabelecimento e " +
                        "LEFT JOIN dbo.Categoria c on c.id = e.idCategoria AND c.excluido = 0 " +
                        "WHERE e.id = @Id AND e.excluido = 0" +
                        "ORDER BY 3";
            var estabelecimento = await conexao.QueryFirstOrDefaultAsync<dynamic>(sql, new { Id = id });

            AutoMapper.Configuration.AddIdentifier(typeof(Estabelecimento), "id");
            AutoMapper.Configuration.AddIdentifier(typeof(Categoria), "id");
            
            Estabelecimento estabelecimentoMap = (AutoMapper.MapDynamic<Estabelecimento>(estabelecimento) as Estabelecimento);

            return estabelecimentoMap;
        }

        public async Task<Guid> Insere(Estabelecimento estabelecimento)
        {
            using var conexao = new SqlConnection(_connectionString);
            
            var sql = "INSERT INTO dbo.Estabelecimento " +
                "(idCategoria, razaoSocial, cnpj, nomeFantasia, email, endereco, cidade, estado, telefone, dataCadastro, status, agencia, conta, dataCriacao, dataEdicao, excluido) " +
                "OUTPUT inserted.id " +
                "VALUES " +
                "(@idCategoria, @razaoSocial, @cnpj, @nomeFantasia, @email, @endereco, @cidade, @estado, @telefone, @dataCadastro, @status, @agencia, @conta, GETDATE(), GETDATE(), 0) ";

            var idInserido = await conexao.QuerySingleAsync<Guid>(sql, new { 
                idCategoria = estabelecimento.IdCategoria.Equals(Guid.Empty) ? null : estabelecimento.IdCategoria.ToString(),
                razaoSocial = estabelecimento.RazaoSocial, 
                cnpj = estabelecimento.CNPJ,
                nomeFantasia = estabelecimento.NomeFantasia,
                email = estabelecimento.Email,
                endereco = estabelecimento.Endereco,
                cidade = estabelecimento.Cidade,
                estado = estabelecimento.Estado,
                telefone = estabelecimento.Telefone,
                dataCadastro = estabelecimento.DataCadastro,
                status = estabelecimento.Status,
                agencia = estabelecimento.Agencia,
                conta = estabelecimento.Conta
            });

            return idInserido;
        }

        public async Task<int> Edita(Estabelecimento estabelecimento)
        {
            using var conexao = new SqlConnection(_connectionString);
            var sql = "UPDATE dbo.Estabelecimento SET " +
                " idCategoria = @idCategoria " +
                ", razaoSocial = @razaoSocial " + 
                ", cnpj = @cnpj " +
                ", nomeFantasia = @nomeFantasia " +
                ", email = @email " +
                ", endereco = @endereco " + 
                ", cidade = @cidade " +
                ", estado = @estado " +
                ", telefone = @telefone " +
                ", dataCadastro = @dataCadastro " +
                ", status = @status " +
                ", agencia = @agencia " +
                ", conta = @conta " +
                ", dataEdicao = GETDATE() " +
                "WHERE " +
                "id = @Id " +
                "AND excluido = 0";

            var qtdeLinhasAfetadas = await conexao.ExecuteAsync(sql, new
            {
                idCategoria = estabelecimento.IdCategoria.Equals(Guid.Empty) ? null : estabelecimento.IdCategoria.ToString(),
                razaoSocial = estabelecimento.RazaoSocial,
                cnpj = estabelecimento.CNPJ,
                nomeFantasia = estabelecimento.NomeFantasia,
                email = estabelecimento.Email,
                endereco = estabelecimento.Endereco,
                cidade = estabelecimento.Cidade,
                estado = estabelecimento.Estado,
                telefone = estabelecimento.Telefone,
                dataCadastro = estabelecimento.DataCadastro,
                status = estabelecimento.Status,
                agencia = estabelecimento.Agencia,
                conta = estabelecimento.Conta,
                id = estabelecimento.Id
            });

            return qtdeLinhasAfetadas;
        }

        public async Task<int> Remove(Guid id)
        {
            using var conexao = new SqlConnection(_connectionString);
            //var sql = "DELETE FROM dbo.Estabelecimento " +
            //    "WHERE Id = @Id ";
            var sql = "UPDATE dbo.Estabelecimento SET excluido = 1 " +
                "WHERE Id = @Id AND excluido = 0";
            var qtdeLinhasAfetadas = await conexao.ExecuteAsync(sql, new
            {
                Id = id
            });

            return qtdeLinhasAfetadas;
        }
    }
}
