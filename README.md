# 🤖 AnuncieCompre — CRM de Atendimento via WhatsApp

![Status](https://img.shields.io/badge/status-em%20desenvolvimento-yellow)

O **AnuncieCompre** é um CRM para atendimento e automação de conversas via WhatsApp, integrado à API da **Twilio**.

O projeto começou como um sistema de **e-commerce reverso**, no qual usuários conversavam com um chatbot para criar pedidos de compra e fornecedores eram posteriormente envolvidos no processo.

Durante o desenvolvimento, surgiu uma limitação importante: o fluxo de conversa era **hard coded**, fazendo com que cada novo cliente ou alteração no atendimento exigisse mudanças diretamente no código.

Para resolver esse problema, o projeto está sendo transformado em um **CRM configurável**, no qual a própria empresa poderá definir como o chatbot deve se comportar através de fluxos, mensagens, validações e transições persistidos no banco de dados.

---

## 🚀 O que o sistema faz

O CRM possui atualmente duas partes principais:

### 💬 Atendimento

Um painel permite que um operador acompanhe e converse com usuários que chegaram ao atendimento automatizado através do WhatsApp.

### 🔀 Conversation Flows

O sistema permite criar fluxos de conversa compostos por **Conversation Nodes**, onde cada node representa uma etapa do atendimento.

Um node pode definir, por exemplo:

* Mensagem enviada ao usuário
* Tipo de validação da resposta
* Value Object Validator
* Opções disponíveis
* Transições para outros nodes
* Se o node representa o final do fluxo

Os nodes podem ser conectados entre si, permitindo construir diferentes fluxos de atendimento sem precisar implementar cada conversa diretamente no código.

---

## 🧠 Evolução do projeto

A primeira versão do AnuncieCompre possuía um fluxo de conversa específico para o processo de compra:

```text
Usuário
   ↓
Chatbot
   ↓
Coleta de informações
   ↓
Pedido de compra
   ↓
Notificação de fornecedores
   ↓
Processo de venda
```

Esse modelo funcionava para o caso inicial, mas não era adequado para transformar o sistema em uma solução reutilizável.

A arquitetura está sendo evoluída para:

```text
                    ┌─────────────────┐
                    │  CRM / Admin    │
                    └────────┬────────┘
                             │
                             ▼
                    ┌─────────────────┐
                    │ Conversation    │
                    │ Flow            │
                    └────────┬────────┘
                             │
                ┌────────────┼────────────┐
                ▼            ▼            ▼
             Node 1       Node 2       Node 3
                │            │            │
                └────────────┴────────────┘
                             │
                             ▼
                         WhatsApp
```

Dessa forma, o comportamento do chatbot deixa de depender exclusivamente de código e passa a ser definido pelos dados configurados no CRM.

---

## 🧱 Tecnologias

### Backend

* **.NET / ASP.NET Core**
* **Entity Framework Core**
* **PostgreSQL**

### Frontend

* **React**

### Integrações

* **Twilio**

---

## 🏗️ Arquitetura e conceitos

O projeto utiliza conceitos de arquitetura e modelagem de domínio com foco em desacoplamento e extensibilidade.

Entre os principais conceitos utilizados:

* **Domain-Driven Design (DDD)**
* **Clean Architecture**
* **Conversation Flow / State Machine**
* **Value Objects**
* **Domain Events**
* **Strategy Pattern**
* **Repository Pattern**
* **Dependency Injection**

A estrutura dos Conversation Flows permite que regras como mensagens, validações e transições sejam configuradas e persistidas, reduzindo a necessidade de alterar o código da aplicação para cada novo fluxo de atendimento.

---

## 🔄 Funcionamento

O objetivo do fluxo de atendimento é permitir que uma conversa seja processada de acordo com a configuração armazenada no sistema.

De forma simplificada:

```text
Usuário envia mensagem
        ↓
Twilio recebe a mensagem
        ↓
Aplicação identifica a conversa
        ↓
Identifica o Flow e Node atual
        ↓
Executa a validação configurada
        ↓
Processa a transição
        ↓
Avança para o próximo Node
        ↓
Envia a resposta ao usuário
```

A API responsável pelo gerenciamento dos **Conversation Flows** já está implementada e os **Flows e Nodes já são persistidos no PostgreSQL**.

A próxima etapa é implementar a API responsável pelo processamento da conversa com o usuário.

---

## 📌 Estado atual

O projeto está em desenvolvimento e atualmente possui:

* [x] Integração inicial com WhatsApp através da Twilio
* [x] Estrutura de Conversation Flow
* [x] Conversation Nodes
* [x] Transições entre Nodes
* [x] Configuração de mensagens
* [x] Configuração de validações
* [x] Persistência de Flows
* [x] Persistência de Nodes
* [x] API para gerenciamento de Conversation Flows
* [x] Painel inicial do CRM para atendimento
* [ ] API de processamento das conversas
* [ ] Autenticação
* [ ] Autorização
* [ ] Multi-tenancy
* [ ] Integração assíncrona entre Twilio e aplicação

---

## 🗺️ Próximos passos

A evolução planejada do projeto inclui:

### 1. API de conversas

Implementar a lógica responsável por processar as mensagens recebidas, identificar o estado da conversa, executar as validações e realizar as transições entre nodes.

### 2. Autenticação e autorização

Adicionar controle de acesso ao CRM e às funcionalidades administrativas.

### 3. Multi-tenancy

Permitir que diferentes empresas utilizem a mesma aplicação mantendo seus dados e configurações isolados.

### 4. Testes

Aumentar a cobertura de testes unitários, principalmente sobre as regras de domínio e processamento dos Conversation Flows.

### 5. Processamento assíncrono

Adicionar uma fila entre a Twilio e a aplicação para desacoplar o recebimento das mensagens do processamento.

A ideia é evoluir de:

```text
Twilio → API → Processamento → Resposta
```

para:

```text
Twilio
   ↓
API
   ↓
Queue
   ↓
Worker / Backend
   ↓
Processamento
   ↓
Twilio
```

Isso permitirá que o processamento das conversas seja mais assíncrono e resiliente.

---

## ⚙️ Como rodar o projeto

### 1. Clonar o repositório

```bash
git clone https://github.com/GustavoFMarcial/AnuncieCompre.git

cd AnuncieCompre
```

### 2. Configurar o PostgreSQL

Tenha uma instância do PostgreSQL em execução.

Configure a connection string utilizando **User Secrets**:

```bash
dotnet user-secrets set "ConnectionStrings:AnuncieCompreContext" "SUA_CONNECTION_STRING"
```

Configure também as credenciais da Twilio:

```bash
dotnet user-secrets set "Twilio:AccountSid" "SEU_ACCOUNT_SID"

dotnet user-secrets set "Twilio:AuthToken" "SEU_AUTH_TOKEN"
```

### 3. Aplicar as migrations

```bash
dotnet ef database update
```

### 4. Executar o backend

```bash
dotnet run
```

### 5. Executar o frontend

Na pasta do frontend:

```bash
npm install

npm run dev
```

---

## 🔧 Práticas de Desenvolvimento

O projeto também está sendo desenvolvido seguindo práticas de **Software Development Life Cycle (SDLC)**, buscando simular um ambiente de desenvolvimento profissional.

### 📋 Planejamento

* Utilização do **Jira** para gerenciamento do projeto
* Backlog para organização e priorização das funcionalidades
* Planejamento das tarefas que serão desenvolvidas
* Utilização de **Sprints** para organizar e acompanhar o desenvolvimento
* Acompanhamento das tarefas até sua conclusão

### 🌿 Git Flow

O desenvolvimento utiliza um fluxo organizado de branches, incluindo:

* `main` — versão principal/estável
* `develop` — branch de desenvolvimento
* Branches específicas para novas funcionalidades e alterações

As alterações são desenvolvidas em branches próprias e posteriormente integradas às branches principais através de merge.

### 🔄 CI/CD

O projeto possui **Continuous Integration (CI)** configurado para automatizar verificações durante o desenvolvimento, incluindo a execução dos testes.

Como próxima etapa, será implementado **Continuous Delivery/Deployment (CD)** para automatizar o processo de entrega da aplicação.

---

## 📚 Objetivo do projeto

Além de ser um projeto de portfólio, o AnuncieCompre está sendo utilizado para aplicar na prática conceitos de desenvolvimento de software, arquitetura e engenharia de sistemas:

* Desenvolvimento de APIs com .NET
* React
* Domain-Driven Design (DDD)
* Clean Architecture
* Modelagem de domínio
* Entity Framework Core
* PostgreSQL
* Integração com APIs externas
* Processamento de mensagens
* Sistemas orientados a eventos
* Arquitetura assíncrona
* Multi-tenancy
* Testes automatizados
* Git e gerenciamento de branches
* SDLC
* Planejamento e gerenciamento de tarefas com Jira
* CI/CD
