-- Listar todos advogados
select 
	p.nome as 'Nome Do Advogado'
,	p.cpf as 'CPF Advogado'
,	p.oab as 'Numero De Identificação'
from Pessoa p WHERE p.oab IS NOT NULL

-- Listar todos clientes
select 
	p.nome as 'Nome Dos Cliente'
,	p.cpf as 'CPF Cliente'
from Pessoa p where p.oab IS NULL

-- Listar todos processos
select * from Processo p

-- Listar todos arquivos
select * from Documento d

-- Listar todas distribuições
select * from Distribuir d

-- Listar advogados com processo

select p.nome, pro.tema
from Pessoa p 
JOIN Processo pro ON p.id = pro.id 

-- Listar processos de um advogado especifico
select p.nome, pro.tema
from Pessoa p 
JOIN Processo pro ON p.id = pro.id where p.id = 2
