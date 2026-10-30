# AGENTS.md — Frontend AnuncieCompre

## 1. Objetivo

Este arquivo define as instruções específicas para agentes de IA que trabalham no diretório `frontend/`, responsável pelo frontend do AnuncieCompre.

Estas regras complementam o `AGENTS.md` da raiz do repositório. Caso existam arquivos `AGENTS.md` em diretórios mais específicos, suas instruções também devem ser consideradas para os arquivos abrangidos.

## 2. Stack e comandos

- React 19 + TypeScript, empacotados pelo Vite com React Compiler habilitado.
- Tailwind CSS v4; componentes de UI no padrão shadcn/ui em `shared/components/ui`.
- Estado de servidor com TanStack React Query v5; chamadas HTTP com axios.
- Roteamento com react-router-dom v7, declarado em `src/router.tsx`.
- Editor de fluxos com @xyflow/react + dagre, restrito à feature `conversationflows`.
- Ícones via lucide-react.

Comandos (a partir de `frontend/`):

- `npm run dev` — servidor de desenvolvimento.
- `npm run build` — verificação de tipos (`tsc -b`) seguida do build do Vite.
- `npm run lint` — ESLint.
- `npm run preview` — serve o build de produção localmente.

## 3. Arquitetura

O frontend usa feature-based architecture: o código é organizado por funcionalidade de domínio (fatias verticais), apoiado em uma camada horizontal de código compartilhado.

frontend/
├── src/        → casca da aplicação: bootstrap, providers e roteamento
├── features/   → fatias verticais por funcionalidade de domínio
│   ├── conversationflows/   → editor de fluxos de conversa
│   ├── conversations/       → painel de atendimento
│   ├── customers/           → cadastro de clientes
│   └── dashboard/           → página inicial
└── shared/    → código transversal, reutilizável e sem conhecimento de domínio

### 3.1. src/ — casca da aplicação
- main.tsx: bootstrap (React root + QueryClientProvider).
- App.tsx: apenas expõe o RouterProvider.
- router.tsx: único ponto onde rotas são conectadas às pages das features, todas aninhadas em DashboardLayout.
- Sem lógica de negócio em src/. Uma nova tela significa: nova page na feature correspondente + registro da rota em router.tsx.

### 3.2. features/<nome>/ — fatia por funcionalidade
Cada feature se organiza internamente por responsabilidade. Crie apenas as subpastas que a feature precisar:
Pasta	Responsabilidade
pages/	componentes nível de rota; compõem hooks e componentes; guardam estado de UI local
components/	apresentação; recebem dados via props; não chamam services
hooks/	estado de servidor com React Query (queries, mutations, query keys, invalidações)
services/	única fronteira com a API (axios + tradução DTO ↔ domínio)
types/	tipos de domínio e DTOs da feature
utils/	helpers puros específicos da feature

### 3.3. shared/ — camada transversal
- components/ui/: primitivos no padrão shadcn/ui, com barrel export em index.ts.
- components/navigation/: Header e Sidebar.
- components/PageHeader.tsx: cabeçalho genérico de página (título, descrição, ação).
- layouts/: DashboardLayout (Sidebar + Header + <Outlet />).
- lib/api.ts: instância axios (baseURL via VITE_API_URL, timeout, interceptor de erro).
- lib/queryClient.ts: configuração global do React Query.
- utils/: helpers genéricos (ex.: cn()).

Critério de fronteira: código que fala a linguagem do negócio (Conversation, Flow, validação de node) fica na feature; código genérico e ignorante do domínio (botão, cliente HTTP, layout) fica em shared/.

## 4. Regras de dependência
Fluxo permitido, sempre de cima para baixo:
pages → components + hooks
hooks → services
services → shared/lib/api + types
- Components não importam services. Pages também não — o acesso a dados passa pelos hooks.
- Services são a única camada que fala com a API. Ninguém usa axios direto; sempre via shared/lib/api a partir de um service.
- features/ pode importar de shared/; shared/ nunca importa de features/.
- Evite imports cruzados entre features. Existe hoje uma exceção (ConversationThread, em conversations, usa EditCustomerDialog de customers): mantenha-a, mas não crie novas. Se um componente for necessário em duas features, mova-o para shared/ (ex.: PageHeader).

## 5. Dados, DTOs e cache
- types/ mantém dois contratos: tipos de domínio (unions em minúsculas, createdAt: Date) e DTOs (forma crua do backend, sufixo DTO, datas em string). Não misture os dois.
- A tradução DTO → domínio acontece apenas nos services (ex.: mapConversation em conversations.service.ts), incluindo conversão de enums do backend. O resto da feature nunca manipula o formato cru da API.
- Query keys seguem o padrão existente: coleção em [recurso] (com parâmetros de filtro quando houver, ex.: ["conversations", status]) e item único em [recurso, id].
- Mutations invalidam as queries afetadas em onSuccess/onSettled, seguindo os hooks existentes. Optimistic updates seguem o padrão de useSendMessage (atualização otimista + rollback em onError).

## 6. Convenções
- Exports nomeados de funções componentes (PascalCase); sem default exports.
- Arquivos: components e pages em PascalCase (FlowCard.tsx); hooks como useXxx.ts; services e utils em kebab-case (conversations.service.ts, date-creator.ts); types em kebab-case singular (conversation.ts).
- Services: objeto exportado com métodos (conversationsService.list()).
- Estilização: classes Tailwind; combinação de classes via cn() de shared/utils/cn.ts. Reuse os primitivos de shared/components/ui antes de criar componentes novos.
- Imports: caminhos relativos (o projeto não usa aliases @/).

## 7. Variáveis de ambiente
- A URL da API vem de VITE_API_URL (fallback http://localhost:5235), lida apenas em shared/lib/api.ts.
- Não modifique .env, components.json, tsconfig*.json ou vite.config.ts sem necessidade justificada pela tarefa.

## 8. Verificação
- Antes de concluir, execute npm run build (inclui tsc -b) e, quando pertinente, npm run lint.
- O tsconfig.app.json inclui apenas src/; arquivos fora dele só entram no typecheck se forem importados (direta ou transitivamente) a partir de src/. Garanta que todo arquivo novo seja alcançável pelo grafo de imports.
- Não afirme que build ou lint passaram sem executá-los.

## 9. Práticas a evitar
- Criar pastas globais de components/, hooks/ ou types/ fora das features — a organização é por fatia.
- Chamar a API fora de um service, ou usar fetch paralelamente ao axios.
- Duplicar tipos, helpers ou componentes entre features e shared/ (hoje já há duas cópias de date-creator.ts; não crie novas duplicações).
- Introduzir gerenciadores de estado globais (Redux, Zustand) ou novas bibliotecas sem necessidade; React Query cobre o estado de servidor.
- Adicionar bibliotecas de UI que não o padrão shadcn/ui já existente.
- Criar abstrações, contexts ou camadas novas sem necessidade concreta.