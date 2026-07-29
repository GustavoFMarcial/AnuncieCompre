# 🤖 AnuncieCompre - Chatbot de Pedidos via WhatsApp

![Status](https://img.shields.io/badge/status-em%20desenvolvimento-yellow)

Chatbot backend para coleta estruturada de pedidos via WhatsApp, integrado com Twilio.

O sistema guia o usuário por um fluxo de conversa estruturado, validando as informações fornecidas e gerando pedidos automaticamente. A arquitetura é baseada em **Conversation Flow**, seguindo princípios de **Domain-Driven Design (DDD)** e **Clean Architecture**, priorizando organização, desacoplamento e facilidade de manutenção.

---

## 🚀 Funcionalidades

- Atendimento automatizado via WhatsApp (Twilio)
- Fluxo de conversa baseado em estados (Conversation Flow)
- Validação de entrada do usuário
- Criação automática de pedidos
- Controle do estado da conversa por usuário
- Persistência das conversas e pedidos no PostgreSQL
- Uso de Domain Events para desacoplamento entre regras de negócio
- Arquitetura modular e extensível para criação de novos fluxos

---

## 🧱 Tecnologias

- Backend: .NET
- Banco de dados: PostgreSQL
- ORM: Entity Framework Core
- Integração: Twilio (WhatsApp API)

---

## 🧠 Arquitetura e Conceitos

Este projeto foi desenvolvido com foco em boas práticas de arquitetura e modelagem de domínio:

- Domain-Driven Design (DDD)
- Clean Architecture
- Conversation Flow (State Machine)
- Domain Events
- Value Objects
- Strategy Pattern para validação das mensagens
- Repository Pattern
- Dependency Injection

---

## 🔄 Como funciona

1. O usuário envia uma mensagem pelo WhatsApp.
2. O Twilio encaminha a mensagem para o backend através de um Webhook.
3. O sistema identifica a conversa do usuário.
4. O estado atual da conversa determina qual etapa do fluxo será executada.
5. A mensagem é validada de acordo com as regras daquela etapa.
6. Os dados da conversa são atualizados.
7. Quando o fluxo é concluído, o pedido é persistido no banco de dados.
8. O sistema responde automaticamente ao usuário.

---

## ⚙️ Como rodar o projeto

### 1. Clonar o repositório

```bash
git clone https://github.com/GustavoFMarcial/AnuncieCompre.git
```

### 2. Configurar o banco de dados

Tenha uma instância do PostgreSQL em execução.

### 3. Configurar os User Secrets

```bash
dotnet user-secrets set "ConnectionStrings:AnuncieCompreContext" "SUA_CONNECTION_STRING"
dotnet user-secrets set "Twilio:AccountSid" "SEU_ACCOUNT_SID"
dotnet user-secrets set "Twilio:AuthToken" "SEU_AUTH_TOKEN"
```

### 4. Aplicar as migrations

```bash
dotnet ef database update
```

### 5. Executar o projeto

```bash
dotnet run
```