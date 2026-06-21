# 🤖 AnuncieCompre - Chatbot de Pedidos via WhatsApp

![Status](https://img.shields.io/badge/status-em%20desenvolvimento-yellow)

Chatbot backend para coleta estruturada de pedidos via WhatsApp, integrado com Twilio.  
O sistema guia o usuário por um fluxo de conversa, validando dados e gerando pedidos automaticamente, utilizando uma arquitetura baseada em estados (**Conversation Flow**) e princípios de **Domain-Driven Design (DDD)**.

O projeto utiliza processamento assíncrono através de **Redis Streams**, permitindo desacoplamento entre os componentes do sistema utilizando o padrão **Producer/Consumer**.

---

## 🚀 Funcionalidades

- Atendimento automatizado via WhatsApp (Twilio)
- Fluxo de conversa baseado em estados (**Conversation Flow**)
- Validação de entrada do usuário (produto, quantidade, etc)
- Criação automática de pedidos
- Controle de estado da conversa por usuário
- Armazenamento temporário dos dados da conversa utilizando Redis
- Comunicação assíncrona através de Redis Streams
- Processamento de eventos utilizando padrão Producer/Consumer
- Uso de Domain Events para desacoplamento

---

## 🧱 Tecnologias

- Backend: .NET
- Banco de dados: PostgreSQL
- ORM: Entity Framework Core
- Cache e armazenamento temporário: Redis
- Mensageria/Event Streaming: Redis Streams
- Integração: Twilio (WhatsApp API)

---

## 🧠 Arquitetura e Conceitos

Este projeto foi desenvolvido com foco em boas práticas de arquitetura e modelagem de domínio:

- Domain-Driven Design (DDD)
- Clean Architecture
- State Machine para controle do fluxo da conversa
- Domain Events para comunicação entre partes do sistema
- Value Objects para validação e consistência dos dados
- Strategy Pattern para validação das mensagens do usuário
- Redis para gerenciamento de estado temporário da conversa
- Redis Streams para processamento assíncrono baseado em eventos
- Producer/Consumer Pattern para desacoplamento entre publicação e processamento

---

## 🔄 Como funciona

1. O usuário envia uma mensagem via WhatsApp
2. O Twilio encaminha a mensagem para o backend através de um Webhook
3. O sistema identifica o estado atual da conversa armazenado no Redis
4. A mensagem recebida é validada de acordo com o estado atual
5. O próximo passo do fluxo é determinado
6. Os dados temporários da conversa são atualizados no Redis
7. Eventos são publicados no Redis Stream
8. Consumers processam esses eventos de forma assíncrona
9. O pedido é persistido no banco de dados
10. O sistema responde automaticamente ao usuário

---

## ⚙️ Como rodar o projeto

### 1. Clonar o repositório

```bash
git clone https://github.com/GustavoFMarcial/AnuncieCompre.git
```
### 2. Configurar o banco de dados
PostgreSQL e Redis rodando localmente
```bash
### 3. Configurar secrets
dotnet user-secrets set "ConnectionStrings:AnuncieCompreContext" "SUA_CONNECTION_STRING"

### 4. Rodar o projeto
dotnet run```