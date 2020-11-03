--CRIA TABELA PARA CATEGORIAS
CREATE TABLE [dbo].[Categoria1](
	[id] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[nome] [varchar](100) NOT NULL,
	[dataCriacao] [datetime] NOT NULL,
	[dataEdicao] [datetime] NOT NULL,
	[excluido] [bit] NOT NULL,
 CONSTRAINT [PK_Categoria1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


--CRIA TABELA PARA ESTABELECIMENTOS
CREATE TABLE [dbo].[Estabelecimento2](
	[id] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[idCategoria] [uniqueidentifier] NULL,
	[razaoSocial] [varchar](255) NOT NULL,
	[nomeFantasia] [varchar](255) NULL,
	[cnpj] [varchar](14) NOT NULL,
	[email] [varchar](100) NULL,
	[endereco] [varchar](255) NULL,
	[cidade] [varchar](50) NULL,
	[estado] [char](2) NULL,
	[telefone] [varchar](11) NULL,
	[dataCadastro] [datetime] NULL,
	[status] [int] NULL,
	[agencia] [varchar](4) NULL,
	[conta] [varchar](6) NULL,
	[dataCriacao] [datetime] NOT NULL,
	[dataEdicao] [datetime] NOT NULL,
	[excluido] [bit] NOT NULL,
 CONSTRAINT [PK_Estabelecimento1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Estabelecimento]  WITH CHECK ADD FOREIGN KEY([idCategoria]) REFERENCES [dbo].[Categoria] ([id])
GO




