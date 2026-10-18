# AGENTS.md — Backend AnuncieCompre

## 1. Objetivo

Este arquivo define as instruções específicas para agentes de IA que trabalham no diretório `src/`, responsável pelo backend do AnuncieCompre.

Estas regras complementam o `AGENTS.md` da raiz do repositório. Caso existam arquivos `AGENTS.md` em diretórios mais específicos, suas instruções também devem ser consideradas para os arquivos abrangidos.

## 2. Arquitetura

O backend segue Clean Architecture e está organizado nas seguintes camadas:

- **Web:** `src/AnuncireCompre.Web`
- **Application:** `src/AnuncireCompre.Application`
- **Infra:** `src/AnuncireCompre.Infra`
- **Domain:** `src/AnuncireCompre.Domain`

Regras:

- Respeite a arquitetura e a separação de responsabilidades existentes.
- Coloque cada implementação na camada correspondente à sua responsabilidade.
- Preserve as regras de dependência entre as camadas, evitando que o Domain dependa de detalhes de infraestrutura ou apresentação.
- Antes de implementar uma funcionalidade, investigue como as camadas existentes se comunicam e reutilize os padrões já estabelecidos.
- Não mova responsabilidades entre camadas nem altere a arquitetura sem uma necessidade concreta relacionada à tarefa.

## 3. Acesso a dados

O acesso a dados segue o padrão Generic Repository com Specialized Repository na camada Infra.

Regras:

- Reutilize os repositórios e contratos existentes sempre que forem adequados.
- Utilize o repositório genérico para operações comuns já suportadas pelo padrão atual.
- Utilize ou crie repositórios especializados quando houver necessidade de consultas ou operações específicas de uma entidade ou Aggregate.
- Evite duplicar operações já disponíveis no repositório genérico.
- Não acesse o banco de dados diretamente em camadas que não sejam responsáveis pela infraestrutura de persistência.
- Antes de criar um novo repositório ou método, verifique se já existe uma implementação equivalente.

## 4. DTOs e conversões

Utilize DTOs quando forem necessários para definir contratos de entrada e saída, validar dados de requisição ou controlar quais informações são expostas pela API.

Regras:

- Na camada Web, utilize DTOs de entrada quando necessário para validar e representar os dados recebidos pela API.
- Utilize DTOs de saída para expor apenas os dados necessários ao consumidor, evitando revelar informações internas ou sensíveis.
- Quando for necessário converter um DTO da camada Web para um DTO utilizado pelas camadas internas, prefira as extension methods já adotadas pelo projeto.
- Siga as convenções existentes para nomes, localização e implementação dessas extensions.
- Não crie DTOs ou conversões desnecessários quando um contrato existente atender adequadamente à necessidade.
- Não exponha entidades de domínio diretamente pela API quando isso comprometer o contrato ou a separação de responsabilidades do sistema.

## 5. Segurança do webhook da Twilio

O endpoint do webhook da Twilio, implementado no controller correspondente a `TwilioWebhookController.cs`, deve utilizar o filtro `ValidateTwilioRequest` para validar a autenticidade das requisições recebidas.

Regras:

- Preserve o uso do filtro de validação no endpoint protegido.
- Não remova, desabilite ou contorne a validação para resolver problemas de integração.
- Se a validação falhar, investigue a causa considerando a URL pública utilizada pela Twilio, os parâmetros recebidos e a configuração do token de autenticação.
- Não utilize atalhos que aceitem requisições sem validação em produção.
- Respeite o mecanismo de configuração e os procedimentos de segurança já adotados pelo projeto.

## 6. Modelagem de domínio

Siga os padrões de modelagem já existentes na camada Domain.

### 6.1. Value Objects

Ao criar um tipo de domínio que representa um conceito definido por seus valores, sem identidade própria, siga o padrão de Value Objects existente no projeto.

- Preserve as regras de igualdade e imutabilidade adotadas pelos Value Objects existentes.
- Centralize no tipo as validações e invariantes que sejam de sua responsabilidade.
- Evite duplicar validações já garantidas pelo próprio Value Object.

### 6.2. Entidades e Aggregates

Ao criar uma entidade ou um Aggregate, siga os padrões de modelagem existentes na camada Domain.

- Não confunda uma entidade com um Aggregate: um Aggregate define uma fronteira de consistência para as regras e alterações do domínio.
- Identifique a Aggregate Root e respeite as regras de acesso e alteração de seus objetos internos.
- Preserve as invariantes do domínio por meio dos comportamentos e operações apropriados.
- Evite expor setters públicos ou permitir alterações que contornem as regras de negócio, seguindo as convenções existentes.
- Não crie Aggregates ou entidades sem uma necessidade real de domínio.

## 7. Regras gerais de implementação

- Investigue implementações semelhantes antes de criar uma nova solução.
- Preserve as convenções existentes de nomenclatura, tratamento de erros, injeção de dependência e organização do código.
- Evite introduzir novas bibliotecas ou padrões quando os recursos existentes forem suficientes.
- Faça alterações localizadas e compatíveis com o restante do backend.
- Ao finalizar, revise o diff e execute os testes e builds pertinentes à alteração.
- Informe verificações que não puderam ser executadas e limitações que permanecerem.