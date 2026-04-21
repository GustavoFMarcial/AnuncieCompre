# 🤖 AnuncieCompre - Chatbot de Pedidos via WhatsApp

![Status](https://img.shields.io/badge/status-em%20desenvolvimento-yellow)

🚧 **Status: Em desenvolvimento**

Este projeto ainda não está finalizado e novas funcionalidades estão sendo implementadas.
Algumas partes podem mudar ou não estar completas.

Chatbot backend para coleta estruturada de pedidos via WhatsApp, integrado com Twilio.  
O sistema guia o usuário por um fluxo de conversa, validando dados e gerando pedidos automaticamente, utilizando uma arquitetura baseada em estados (Conversation Flow) e princípios de Domain-Driven Design (DDD).

---

## 🚀 Funcionalidades

- Atendimento automatizado via WhatsApp (Twilio)
- Fluxo de conversa baseado em estados (Conversation Flow)
- Validação de entrada do usuário (produto, quantidade, etc)
- Criação de pedidos automatizada
- Uso de Domain Events para desacoplamento
- Controle de estado da conversa por usuário

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
- State Machine para controle do fluxo da conversa
- Domain Events para comunicação entre partes do sistema
- Value Objects para validação e consistência dos dados
- Strategy Pattern para validação das mensagens do usuário

---

## 🔄 Como funciona

1. O usuário envia uma mensagem via WhatsApp
2. O Twilio encaminha a mensagem para o backend (Webhook)
3. O sistema identifica o estado atual da conversa
4. A entrada do usuário é validada
5. O próximo passo do fluxo é determinado
6. O sistema responde automaticamente ao usuário

---

## ⚙️ Como rodar o projeto

1. Clonar o repositório
git clone https://github.com/GustavoFMarcial/AnuncieCompre.git

2. Configurar o banco de dados
PostgreSQL rodando localmente

3. Configurar secrets
dotnet user-secrets set "ConnectionStrings:AnuncieCompreContext" "SUA_CONNECTION_STRING"

4. Rodar o projeto
dotnet run