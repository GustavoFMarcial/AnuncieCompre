# AGENTS.md — AnuncieCompre

## 1. Objetivo

Este arquivo define as regras gerais que agentes de IA devem seguir ao analisar, implementar, corrigir ou refatorar código neste projeto.

Estas instruções valem para todo o repositório. Arquivos `AGENTS.md` em diretórios mais específicos podem complementar estas regras com orientações próprias do backend ou frontend.

## 2. Regras fundamentais

- Antes de alterar qualquer código, investigue a implementação existente e entenda como ela funciona.
- Identifique os arquivos, métodos, componentes e dependências diretamente envolvidos na tarefa.
- Reutilize os padrões, abstrações e soluções já existentes quando forem adequados.
- Não crie novas abstrações, interfaces, serviços, classes ou camadas sem uma necessidade concreta.
- Não altere a arquitetura do projeto por iniciativa própria quando uma mudança localizada resolver o problema.
- Não refatore código fora do escopo da tarefa.
- Não adicione dependências, bibliotecas, serviços externos ou infraestrutura sem necessidade justificada.
- Não altere comportamentos existentes sem que isso seja necessário para cumprir a tarefa.
- Não invente regras de negócio. Quando uma decisão importante não estiver clara, investigue o código e a documentação existentes. Se ainda houver dúvida, pergunte antes de assumir um comportamento.
- Não considere uma implementação concluída apenas porque o código compila. Verifique também se o comportamento atende ao objetivo solicitado.

## 3. Consulta à documentação

- Antes de realizar alterações que exijam compreender o contexto geral do projeto, consulte o `README.md` da raiz do repositório.
- Utilize o README como referência para entender o propósito do sistema, suas funcionalidades, tecnologias e instruções de configuração.
- Quando houver dúvidas sobre o funcionamento de uma funcionalidade, investigue também os arquivos de código e a documentação mais específica relacionada a ela.
- Não presuma que o README está atualizado. Quando suas informações divergirem da implementação atual, investigue o código e os demais registros relevantes antes de decidir como proceder.
- Se o README não fornecer informações suficientes para tomar uma decisão importante, continue investigando os arquivos relevantes antes de implementar uma solução.

## 4. Processo de trabalho

Para cada tarefa:

1. **Investigue:** leia os arquivos relevantes e rastreie o fluxo de execução até compreender o comportamento atual.
2. **Planeje:** identifique a causa do problema e determine a menor alteração capaz de resolvê-lo.
3. **Implemente:** faça mudanças focadas, preservando os padrões existentes.
4. **Valide:** execute os testes, builds e verificações pertinentes às alterações.
5. **Revise:** examine o diff completo para identificar alterações acidentais, código desnecessário e possíveis regressões.
6. **Relate:** apresente um resumo do que foi alterado, os motivos, as verificações executadas e quaisquer limitações restantes.

Para tarefas simples, não produza planejamento excessivo. O processo deve ser proporcional à complexidade e ao risco da alteração.

## 5. Preservação do código existente

- Preserve a organização atual do repositório.
- Respeite as responsabilidades já estabelecidas entre domínio, aplicação, infraestrutura, API e frontend.
- Evite duplicação de lógica e mantenha uma única fonte de verdade para cada regra de negócio, quando aplicável.
- Não mova arquivos ou renomeie elementos sem necessidade para a tarefa.
- Não substitua uma implementação existente por outra abordagem apenas por preferência pessoal.
- Não remova código, validações ou tratamentos de erro sem entender sua finalidade.
- Não altere contratos de API sem avaliar os impactos nos consumidores.
- Não modifique arquivos de configuração, segredos, variáveis de ambiente ou pipelines sem necessidade justificada.
- Não sobrescreva alterações preexistentes do usuário.
- Não reverta mudanças que não foram feitas como parte da tarefa atual.

## 6. Arquitetura e decisões do projeto

- Antes de propor mudanças arquiteturais, verifique como o projeto já organiza suas responsabilidades.
- Preserve as decisões arquiteturais existentes, a menos que a tarefa exija uma mudança ou que exista um problema concreto que justifique discuti-la.
- Prefira soluções simples, explícitas e compatíveis com a estrutura atual.
- Não introduza padrões arquiteturais apenas por serem considerados boas práticas em outros contextos.
- Não adicione cache, filas, mensageria ou serviços externos sem avaliar a necessidade real e o custo de manutenção.
- Mantenha as regras de negócio independentes de detalhes de interface e infraestrutura, respeitando os limites já adotados pelo projeto.

## 7. Banco de dados e contratos

- Avalie os impactos de alterações nas entidades, configurações do Entity Framework Core, consultas, relacionamentos e persistência.
- Não presuma que uma alteração em uma entidade pode ser feita sem avaliar o mapeamento e o banco de dados.
- Crie ou altere migrations quando a mudança de esquema realmente exigir isso, seguindo o procedimento existente no projeto.
- Evite consultas, carregamentos de dados e operações de persistência desnecessários.
- Preserve a compatibilidade entre os dados enviados pelo frontend e os contratos aceitos pelo backend.
- Ao alterar contratos, valide os pontos de uso e os fluxos afetados.

## 8. Qualidade e testes

- Siga as convenções de nomenclatura, formatação e organização já utilizadas no código.
- Evite métodos excessivamente complexos, duplicação e responsabilidades sem relação entre si.
- Não adicione comentários que apenas repitam o código.
- Adicione ou atualize testes quando isso for pertinente e viável para a alteração.
- Execute os testes e builds relacionados à tarefa, conforme as ferramentas disponíveis.
- Nunca afirme que testes ou builds passaram sem realmente executá-los.
- Se não for possível executar alguma verificação, informe isso explicitamente.
- Não corrija problemas preexistentes fora do escopo sem necessidade. Informe-os separadamente quando forem relevantes.

## 9. Uso de ferramentas

- Inspecione os arquivos de configuração e os comandos existentes antes de executar operações que possam alterar o projeto.
- Prefira os scripts e comandos já definidos no repositório.
- Não execute operações destrutivas, apague dados ou descarte alterações do usuário sem autorização explícita.
- Não exponha segredos, tokens, credenciais ou informações sensíveis em logs, respostas ou arquivos versionados.
- Não instale dependências ou altere ferramentas de desenvolvimento sem uma justificativa relacionada à tarefa.

## 10. Critérios de conclusão

Antes de finalizar uma tarefa:

- Confirme que a alteração atende ao objetivo solicitado.
- Revise todos os arquivos modificados.
- Verifique se existem alterações não relacionadas à tarefa.
- Execute as verificações pertinentes, quando possível.
- Informe os principais arquivos alterados e as decisões relevantes.
- Liste testes e builds executados e seus resultados.
- Informe limitações, dúvidas ou riscos que ainda precisem de atenção.

**Princípio principal:** compreender antes de alterar, preservar o que funciona e fazer somente as mudanças necessárias para resolver o problema.