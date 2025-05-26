
# CMS Frontend - Fluxos e Interação com API

## 1. Autenticação do Usuário (Login)
Antes de começar a interagir com qualquer conteúdo ou template, o usuário (redator, editor ou admin) precisa fazer login para obter um token de autorização (JWT).

### Endpoint:
`POST /api/Auth/login`

### Requisição:

```json
{
  "email": "redator@exemplo.com",
  "senha": "senhaSegura"
}
```

### Descrição:
O frontend deve enviar a requisição com as credenciais do usuário para obter o token JWT. Esse token será usado em todas as requisições subsequentes, incluindo criação, edição, submissão, etc.

---

## 2. Criar Template (Admin, Editor e redator)
Um administrador ou editor ou redator pode criar e deve um novo template, que será usado na criação de conteúdos somente é possivel criar um conteudo usando um template de base.

### Endpoint:
`POST /api/Templates`

### Requisição:

```json
{
  "nome": "Template de Ficção",
  "campos": [
    {
      "nome": "Título",
      "tipo": 0,
      "obrigatorio": true
    },
    {
      "nome": "Corpo",
      "tipo": 2,
      "obrigatorio": true
    },
    {
      "nome": "Resumo",
      "tipo": 1,
      "obrigatorio": false
    }
  ]
}
```

### Descrição:
O frontend deve permitir que  insira os dados para criar um template. Cada template pode ter campos obrigatórios e não obrigatórios. Esses templates são usados para a criação de conteúdo.

---

## 3. Criar Conteúdo (Redator)
Agora, o redator pode criar um conteúdo utilizando um template previamente criado. o Redator passa o template que vai usar o Id dai ele deve usar o s campos que sao obrigatorios (true) e pode usar ou não os falsos

### Endpoint:
`POST /api/Conteudos`

### Requisição:

```json
{
  "titulo": "Explorando o Cosmos - A Jornada do Futuro",
  "templateId": "ec031f2e-49b8-4894-bfd6-b1347da986f3",
  "camposPreenchidos": [
    {
      "nome": "Título",
      "valor": "Explorando o Cosmos - A Jornada do Futuro"
    },
    {
      "nome": "Corpo",
      "valor": "Em um futuro distante, a humanidade explora os limites do espaço em busca de novas fronteiras..."
    }
  ]
}
```

### Descrição:
O frontend usa o template já criado para preencher os campos obrigatórios (e opcionais, se o redator desejar) para criar um novo conteúdo.

---

## 4. Submeter Conteúdo para Aprovação (Redator)
Após criar o conteúdo, o redator submete ele para aprovação. Isso muda o status do conteúdo para "Submetido".

### Endpoint:
`POST /api/Conteudos/{id}/submeter`

### Requisição:

```json
{
  "id": "d6a9f380-f5b2-43c8-9eeb-482d3f903a97"
}
```

### Descrição:
O frontend envia a requisição para que o conteúdo seja submetido para revisão do editor. O status do conteúdo será alterado para "Submetido".

---

## 5. Aprovar Conteúdo (Editor)
O editor aprova o conteúdo, passando o status para "Aprovado". onde depois vou aplicar uma rota publica so para exibir esses conteudos.

### Endpoint:
`POST /api/AprovacaoConteudo/{id}/aprovar`

### Requisição:

```json
{
  "id": "d6a9f380-f5b2-43c8-9eeb-482d3f903a97"
}
```

### Descrição:
O frontend envia uma requisição para o editor aprovar o conteúdo submetido. O status do conteúdo será alterado para "Aprovado".

---

## 6. Rejeitar Conteúdo (Editor)
Se o conteúdo não estiver adequado, o editor pode rejeitar o conteúdo. O editor também adiciona um comentário explicando os motivos.

### Endpoint:
`POST /api/AprovacaoConteudo/{id}/rejeitar`

### Requisição:

```json
{
  "id": "d6a9f380-f5b2-43c8-9eeb-482d3f903a97",
  "comentario": "Conteúdo não atende aos requisitos de qualidade."
}
```

### Descrição:
Se o conteúdo não for adequado, o editor pode rejeitar o conteúdo e adicionar um comentário sobre o motivo. O status do conteúdo será alterado para "Rejeitado" e o comentário será salvo.

---

## 7. Devolver Conteúdo para Correção (Editor)
O editor pode devolver o conteúdo ao redator para ajustes, adicionando um comentário com a correção solicitada.

### Endpoint:
`POST /api/AprovacaoConteudo/{id}/devolver`

### Requisição:

```json
{
  "id": "d6a9f380-f5b2-43c8-9eeb-482d3f903a97",
  "comentario": "Por favor, ajuste o título para torná-lo mais atraente."
}
```

### Descrição:
Caso o editor não aprove o conteúdo, mas ache que ele precisa de correção, o editor pode devolver o conteúdo para o redator com um comentário detalhado sobre o que precisa ser alterado e o conteudo volta para o estado rascunho para depois poder ser submetido novamente.

---

## 8. Editar Conteúdo (Redator)
O redator edita o conteúdo com base no feedback recebido após o conteúdo ser devolvido para correção. Esse endpoint so permite editar o conteudo ali ja feito nao adcionar novas coisas ainda como novos campos etc.

### Endpoint:
`PUT /api/Conteudos/{id}`

### Requisição:

```json
{
  "id": "d6a9f380-f5b2-43c8-9eeb-482d3f903a97",
  "titulo": "Explorando o Cosmos - Versão Ajustada",
  "templateId": "ec031f2e-49b8-4894-bfd6-b1347da986f3",
  "status": "Rascunho",
  "camposPreenchidos": [
    {
      "nome": "Título",
      "valor": "Explorando o Cosmos - Versão Ajustada"
    },
    {
      "nome": "Corpo",
      "valor": "Em um futuro distante, a humanidade explora os limites do espaço em busca de novas fronteiras, agora com mais detalhes."
    }
  ],
  "comentario": "Ajustes no título feitos conforme solicitado."
}
```

### Descrição:
O redator faz os ajustes solicitados pelo editor, edita o conteúdo e o envia novamente para o editor submetendo.

---

## 9. Clonar Conteúdo
O redator ou editor pode clonar um conteúdo para reutilizá-lo em outro momento ou para criar uma nova versão. 

### Endpoint:
`POST /api/Conteudos/{id}/clone`

### Requisição:

```json
{
  "id": "d6a9f380-f5b2-43c8-9eeb-482d3f903a97"
}
```

### Descrição:
O frontend pode enviar uma requisição para clonar o conteúdo existente. Isso cria uma nova versão do conteúdo com as mesmas informações, mas sem status e campos preenchidos.

# Descrição das Rotas e Regras de Acesso

## 1. Usuários

### Criação de Usuários
- Apenas Admin e Editor podem criar novos usuários.
- O Redator não tem permissão para criar usuários.

### Acesso aos Templates
- Todos os usuários autenticados (Admin, Editor e Redator) têm acesso a todos os templates existentes.

### Edição de Templates
- Apenas o usuário que criou o template ou um Admin pode editar um template.

### Exclusão de Templates
- Somente o criador do template ou um Admin pode excluí-lo.

---

## 2. Templates

### Criação de Template
- Pode ser feito por Admin ou Editor.
- Ao criar um template, o sistema permite adicionar campos obrigatórios e não obrigatórios.

### Clonagem de Template
- Funciona criando um novo template com o nome original seguido de "Cópia".
- O novo template mantém os mesmos campos e estrutura.

### Exclusão de Template
- Apenas o criador ou um Admin pode excluir um template.

---

## 3. Conteúdos

### Criação de Conteúdo
- Um Redator pode criar um conteúdo baseado em qualquer template disponível.
- O conteúdo deve obrigatoriamente preencher todos os campos obrigatórios do template.
- O status inicial será "Rascunho".

### Acesso aos Conteúdos
- Os Redatores podem visualizar apenas os conteúdos que criaram.
- Os Editores e Admins podem visualizar todos os conteúdos criados, independentemente de quem os criou.

### Edição de Conteúdo
- Só é possível editar os conteúdos que o Redator ou Editor criou.
- O Admin também pode editar qualquer conteúdo.
- A edição é restrita aos campos que já foram preenchidos no conteúdo original (não permite adicionar novos campos).

### Exclusão de Conteúdo
- Somente o Redator que criou o conteúdo ou um Admin pode excluí-lo.

### Clone de Conteúdo
- A funcionalidade de clonagem ainda está sendo ajustada e será implementada posteriormente.

---

## 4. Fluxo de Aprovação

### Submissão para Aprovação
- O conteúdo criado por um Redator pode ser submetido para aprovação de um Editor.
- O status do conteúdo será alterado para "Submetido".

### Aprovação de Conteúdo
- O Editor pode aprovar o conteúdo.
- O status será alterado para "Aprovado".

### Rejeição de Conteúdo
- O Editor pode rejeitar o conteúdo e fornecer um comentário explicando o motivo da rejeição.
- O status do conteúdo será alterado para "Rejeitado".

### Devolução para Correção
- O Editor pode devolver o conteúdo ao Redator com um comentário explicativo.
- O status será alterado para "Devolvido".

---

## 5. Acesso de Usuários

### Controle de Acesso Baseado em Papéis
A aplicação usa o padrão Strategy para controlar as permissões dos usuários com base no papel atribuído (Admin, Editor, Redator).

- **Admin**: Tem permissões totais para criar, editar, excluir e visualizar qualquer conteúdo e template.
- **Editor**: Pode editar, aprovar, rejeitar ou devolver conteúdo, mas só pode editar conteúdo criado por outros Editor ou Redator.
- **Redator**: Pode criar e editar apenas seus próprios conteúdos.

---

## Considerações Finais

- A parte de **Notificação (Observer)** ainda precisa ser implementada, mas o objetivo é notificar os Editores e Redatores sobre as ações realizadas no conteúdo (como aprovação ou rejeição).
- O processo de **Clone de Conteúdo** ainda está em desenvolvimento e será revisado posteriormente.
"""
---

## Conclusão

Este fluxo de trabalho permite que o sistema CMS funcione de maneira eficiente, com uma clara divisão de responsabilidades entre os papéis de administrador, editor e redator. O frontend deve interagir com esses endpoints para garantir que cada etapa do processo, desde a criação de templates até a aprovação ou rejeição de conteúdo, seja gerenciada adequadamente. A implementação de autenticação com JWT garante a segurança e a continuidade das interações entre o usuário e a aplicação.

### Instalação

# CMS Project - Backend (.NET 8)

Este repositório contém a API backend do CMS, desenvolvida em C# utilizando .NET 8 e PostgreSQL.

O projeto é composto por múltiplos módulos:
- `CMSProject` → **Projeto principal** (executável).
- `CMS.Application` → Camada de aplicação.
- `CMS.Infrastructure` → Infraestrutura (banco, serviços).
- `CMS.Domain` → Domínio (entidades, regras de negócio).

## Como Rodar a Aplicação

### 1. Modo Manual (Desenvolvimento Local)

**Pré-requisitos:**  
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- [PostgreSQL](https://www.postgresql.org/download/)  
- [VS Code](https://code.visualstudio.com/) ou qualquer IDE compatível  

#### Passos:
1. Clone o repositório:
    ```bash
    git clone https://github.com/seu-usuario/seu-repo.git
    cd seu-repo
    ```
2. Configure a `ConnectionStrings:DefaultConnection` no `appsettings.json` do `CMSProject` com seus dados locais de PostgreSQL:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Host=localhost;Port=5432;Database=cms_db;Username=cms_user;Password=123456"
    }
    ```
3. No terminal, navegue até o projeto principal (`CMSProject`):
    ```bash
    cd CMSProject
    ```
4. Execute:
    ```bash
    dotnet run
    ```
5. A API estará disponível em:
    ➡️ `http://localhost:5031` (ou a porta configurada no `launchSettings.json`)

### 2. Modo Automático com Docker

**Pré-requisitos:**  
- Docker  
- Docker Compose  

#### Passos:
1. Clone o repositório:
    ```bash
    git clone https://github.com/seu-usuario/seu-repo.git
    cd seu-repo
    ```
2. Execute:
    ```bash
    docker-compose up --build -d
    ```
3. A API estará disponível em:
    ➡️ `http://localhost:8080`

O banco de dados PostgreSQL também estará rodando automaticamente.

### Configurações Importantes:
A API usa variáveis de ambiente para configuração no Docker:
- `ConnectionStrings__DefaultConnection`
- `Jwt` (chaves, issuer, audience, etc.)

Já estão definidas no `docker-compose.yml`. No ambiente local, configure no `appsettings.json`.

### Estrutura do Projeto
```plaintext
├── CMSProject/            # Projeto principal (executável)
├── CMS.Application/      # Camada de aplicação
├── CMS.Infrastructure/   # Infraestrutura (banco, serviços)
├── CMS.Domain/           # Entidades e domínio
├── docker-compose.yml    # Arquivo para subir o ambiente com Docker
├── CMSProject/Dockerfile # Dockerfile da aplicação
└── .dockerignore         # Arquivos ignorados no build Docker
Como Consumir a API
Após rodar, acesse a documentação Swagger:

➡️ http://localhost:8080/swagger

ou ➡️ http://localhost:5031/swagger (no modo manual)
