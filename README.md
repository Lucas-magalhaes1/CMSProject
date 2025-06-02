# CMSProject - Sistema de Gerenciamento de Conteúdo

Este projeto é um sistema de gerenciamento de conteúdo (CMS) desenvolvido para facilitar a criação, edição, revisão e publicação de notícias, artigos, eventos e outros tipos de conteúdo digital.

Com ele, diferentes tipos de usuários (como redatores, editores e administradores) podem colaborar no processo de produção, garantindo que o conteúdo seja revisado, aprovado e publicado de forma organizada e segura.

## Funcionalidades principais

- **Criação de templates de conteúdo:** Define modelos com campos específicos (como título, corpo, data, autor) que facilitam a criação de conteúdos padronizados.

- **Gerenciamento de conteúdos:** Criação, edição, clonagem e exclusão de conteúdos baseados em templates.

- **Fluxo de aprovação:** Conteúdos podem ser submetidos para revisão, aprovados, rejeitados ou devolvidos para ajustes com comentários para o autor.

- **Controle de acesso:** Usuários com diferentes papéis possuem permissões específicas para criar, editar, aprovar e gerenciar conteúdos.

- **Notificações:** O sistema envia avisos para os usuários responsáveis quando ocorrem ações importantes, como a aprovação de um conteúdo.

- **Acesso público:** Usuários não autenticados podem acessar conteúdos aprovados, funcionando como um portal de notícias.

---



# Mapeamento dos Padrões, Princípios e Arquitetura no Projeto CMS

Este documento apresenta o mapeamento dos principais padrões de projeto, princípios SOLID, arquitetura limpa e conceitos de programação orientada a objetos (POO) aplicados no projeto CMS (Content Management System).

---

## Padrões de Projeto

| Padrão de Projeto          | Localização / Arquivos                              | Explicação / Como Está Implementado                                 |
|---------------------------|----------------------------------------------------|--------------------------------------------------------------------|
| **Factory Method**         | `PermissaoFactory` (em `CMS.Application.Services`) | Método `CriarPermissao` que cria instâncias de `IPermissaoUsuario` conforme papel (Admin, Editor, Redator). Centraliza criação de objetos complexos. <br>Possivelmente usado na criação de `Usuario` e `Conteudo` via construtores ou fábricas específicas. | Permite encapsular a criação de entidades, facilitando expansão e manutenção.                           |
| **Observer**               | `NotificationPublisher` (em `CMS.Domain.Events`)    | Mantém lista de observers e notifica todos quando um evento ocorre (ex: conteúdo publicado).           |
|                           | `INotificationObserver` (interface)                 | Define contrato para observadores responderem a eventos.                                               |
|                           | `ConteudoPublicadoObserver` (em `CMS.Infrastructure.Notifications`) | Implementa a ação de enviar notificação (ex: persistir no banco) quando evento de conteúdo publicado é disparado. |
| **Chain of Responsibility**| Pasta `CMS.Domain.Chain` e arquivos `AprovarConteudoHandler`, `SubmeterConteudoHandler`, `RejeitarConteudoHandler`, `DevolverConteudoHandler` | Handlers encadeados para processar o fluxo de aprovação, cada um lidando com uma ação específica e podendo passar para o próximo. |
| **Proxy**                  | Implementação indireta via `IPermissaoUsuario` e verificações no Use Cases (ex: `CriarConteudoUseCase`) e Controllers (`ConteudosController`) | O padrão proxy está no controle de acesso, onde a interface de permissão verifica se o usuário pode executar determinada ação, funcionando como um "proxy" antes de permitir o acesso. |
| **Prototype**              | Método `Clone()` na entidade `Conteudo` (em `CMS.Domain.Entities.Conteudo`) | Permite clonar conteúdos com seus campos, para evitar recriação manual e facilitar cópias.             |
|                           | Também aplicável para Templates (não enviado código, mas citado no projeto) | Redatores podem clonar templates para criar novos conteúdos a partir deles.                            |

---

## Arquitetura Limpa

| Item                           | Onde Está Aplicado (Arquivos/Pastas/Classes)                                  | Explicação / Comentário                                                                                           |
|-------------------------------|-----------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------|
| **Separação em camadas**       | Pastas `CMS.Domain`, `CMS.Application`, `CMS.Infrastructure`, `CMSProject`  | Domínio contém entidades e regras puras; Application com casos de uso; Infrastructure com repositórios e serviços técnicos; API com controllers e endpoints. Responsabilidades bem separadas. |
| **Comunicação entre camadas**  | Controllers (`ConteudosController`), Use Cases (`CriarConteudoUseCase`), Repositórios (`ConteudoRepository`) | Controller delega lógica para Use Cases, que usam entidades do domínio e persistem via repositórios — fluxo típico da Clean Architecture. |

---

## Princípios SOLID

| Princípio                  | Onde Está Aplicado (Arquivos/Pastas/Classes)                  | Explicação / Comentário                                                                                           |
|----------------------------|--------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------|
| **SRP** (Responsabilidade única)     | Entidades `Conteudo`, `Usuario`, Use Cases, Controllers      | Cada classe tem uma responsabilidade única: Entidades modelam domínio, Use Cases orquestram lógica, Controllers lidam com API.                             |
| **OCP** (Aberto/Fechado)              | Interfaces (ex: `IConteudoRepository`, `IPermissaoUsuario`), Handlers Chain | Permite extensão adicionando handlers e implementações sem modificar código existente (ex: adicionar nova permissão ou handler).                           |
| **LSP** (Substituição de Liskov)      | Implementações de interfaces (ex: `IPermissaoUsuario` com AdminPermissao etc) | Objetos derivados (permissões específicas) podem substituir base sem alterar comportamento esperado.                                                  |
| **ISP** (Segregação de interfaces)    | Interfaces específicas para repositórios, permissões, notificações          | Evita interfaces gordas; cada cliente depende só do que usa (ex: `IPermissaoUsuario` só métodos relacionados à permissões).                                   |
| **DIP** (Inversão de dependências)    | Injeção de dependências no Startup (`Program.cs`), Use Cases, Controllers    | Depende de abstrações (interfaces), não de implementações concretas, facilitando testes e manutenção.                                                   |

---

## Conceitos de Programação Orientada a Objetos (POO)

| Conceito                    | Onde Está Aplicado (Arquivos/Pastas/Classes)                  | Explicação / Comentário                                                                                           |
|-----------------------------|---------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------|
| **Encapsulamento**           | Propriedades privadas (`private set`), métodos para alteração de estado em entidades (`Conteudo`, `Usuario`) | Controle rigoroso do estado interno das entidades, modificações feitas por métodos específicos que validam regras. |
| **Abstração**               | Interfaces para repositórios, permissões, notificações        | Esconde detalhes de implementação, permite uso genérico e fácil substituição.                                     |
| **Herança e Polimorfismo**  | Implementações das interfaces de permissões e handlers Chain  | Classes concretas implementam interfaces, podendo ser usadas polimorficamente (ex: diferentes permissões ou handlers). |
| **Coesão**                  | Classes focadas em uma responsabilidade clara                 | Cada classe tem métodos e propriedades relacionados, evitando acoplamento interno desnecessário.                  |
| **Construtores e Métodos**  | Uso consistente para garantir objetos válidos (ex: `Conteudo` com campos obrigatórios) | Assegura objetos sempre em estado válido e consistente ao longo do tempo.                                        |

---

 # 1. Módulos Implementados e Suas Funções

### Usuário
- Entidade `Usuario` com papéis definidos (Admin, Editor, Redator).
- Cadastro, autenticação via JWT e permissões aplicadas pelo padrão **Strategy**.
- Controle de criação e edição de usuários restrito aos papéis Admin e Editor, garantindo segurança.

### Templates
- Entidade `Template` com campos obrigatórios e opcionais.
- CRUD completo para templates via API.
- Clonagem de templates utilizando o padrão **Prototype** para facilitar criação rápida de novos modelos.

### Conteúdos
- Entidade `Conteudo` com preenchimento baseado no Template associado.
- Fluxo completo contemplando criação, edição, submissão para revisão, aprovação, rejeição e devolução com comentários para feedback.
- Clonagem de conteúdos também via padrão **Prototype**.
- Uso do padrão **Chain of Responsibility** para o fluxo de aprovação editorial.
- Validação dos campos obrigatórios do conteúdo conforme o template definido.
- Controle rigoroso de acesso para que apenas o autor ou administradores possam editar ou deletar conteúdos.

### Controle de Aprovação
- Handlers específicos para as ações de Submeter, Aprovar, Rejeitar e Devolver conteúdo.
- Endpoints dedicados para gerenciar essas operações.
- Comentários são usados para fornecer feedback no processo de revisão e devolução.

### Notificações
- Evento disparado sempre que um conteúdo é aprovado.
- Observers (como `ConteudoPublicadoObserver`) notificam os usuários responsáveis painel interno.
- Arquitetura facilmente extensível para múltiplos canais de notificação.

### Autenticação e Permissões
- Login autenticado via JWT.
- Middleware para proteger rotas e validar permissões conforme papel do usuário.
- Permissões diferenciadas usando o padrão **Strategy**.

---

## Comunicação entre Camadas

1. O frontend faz uma requisição HTTP para um endpoint no Controller (API).
2. O Controller converte o JSON recebido em um DTO e chama o Use Case correspondente.
3. O Use Case acessa as entidades do domínio para aplicar as regras de negócio e realiza validações necessárias.
4. Para persistência, o Use Case utiliza os repositórios implementados via Entity Framework Core na camada de Infrastructure.
5. Caso ocorra um evento relevante (como a aprovação de um conteúdo), o Use Case dispara este evento para o sistema de Observer.
6. O Observer então envia notificações para os usuários interessados.
7. Por fim, o resultado processado é enviado de volta ao frontend no formato de DTO JSON.


## Exemplos de Requisições 

### 1. Templates: 

```json
[
  {
    "nome": "Notícia de Tecnologia",
    "campos": [
      { "nome": "Título", "tipo": 0, "obrigatorio": true },
      { "nome": "Corpo", "tipo": 2, "obrigatorio": true },
      { "nome": "Autor", "tipo": 0, "obrigatorio": false },
      { "nome": "Data de Publicação", "tipo": 3, "obrigatorio": false }
    ]
  }


  {
    "nome": "Resenha de Filme",
    "campos": [
      { "nome": "Título", "tipo": 0, "obrigatorio": true },
      { "nome": "Sinopse", "tipo": 2, "obrigatorio": true },
      { "nome": "Diretor", "tipo": 0, "obrigatorio": false },
      { "nome": "Ano de Lançamento", "tipo": 1, "obrigatorio": false },
      { "nome": "Nota", "tipo": 4, "obrigatorio": false }
    ]
  }


  {
    "nome": "Artigo Científico",
    "campos": [
      { "nome": "Título", "tipo": 0, "obrigatorio": true },
      { "nome": "Resumo", "tipo": 2, "obrigatorio": true },
      { "nome": "Autores", "tipo": 2, "obrigatorio": true },
      { "nome": "Data de Publicação", "tipo": 3, "obrigatorio": false },
      { "nome": "Referências", "tipo": 2, "obrigatorio": false }
    ]
  }


  {
    "nome": "Evento Cultural",
    "campos": [
      { "nome": "Nome do Evento", "tipo": 0, "obrigatorio": true },
      { "nome": "Descrição", "tipo": 2, "obrigatorio": true },
      { "nome": "Data do Evento", "tipo": 3, "obrigatorio": true },
      { "nome": "Local", "tipo": 0, "obrigatorio": true },
      { "nome": "Preço do Ingresso", "tipo": 1, "obrigatorio": false }
    ]
  }
]


```
### 2. Conteúdos Baseados nos Templates

```json

{
  "titulo": "Lançamento do Novo Smartphone X1000",
  "templateId": "<id_do_template_1>",
  "camposPreenchidos": [
    { "nome": "Título", "valor": "Lançamento do Novo Smartphone X1000" },
    { "nome": "Corpo", "valor": "A empresa XYZ lançou hoje o seu mais novo smartphone, o X1000, com recursos inovadores..." },
    { "nome": "Autor", "valor": "Maria Silva" }
  ]
}


{
  "titulo": "Resenha: Viagem ao Centro da Terra",
  "templateId": "<id_do_template_2>",
  "camposPreenchidos": [
    { "nome": "Título", "valor": "Resenha: Viagem ao Centro da Terra" },
    { "nome": "Sinopse", "valor": "Uma aventura emocionante onde os personagens exploram o interior do planeta..." },
    { "nome": "Diretor", "valor": "John Doe" },
    { "nome": "Ano de Lançamento", "valor": "2023" },
    { "nome": "Nota", "valor": "8.5" }
  ]
}


{
  "titulo": "Estudo sobre Energias Renováveis",
  "templateId": "<id_do_template_3>",
  "camposPreenchidos": [
    { "nome": "Título", "valor": "Estudo sobre Energias Renováveis" },
    { "nome": "Resumo", "valor": "Este estudo analisa as tendências de energias renováveis no Brasil..." },
    { "nome": "Autores", "valor": "Carlos Pereira, Ana Costa" },
    { "nome": "Referências", "valor": "Referência 1, Referência 2" }
  ]
}


{
  "titulo": "Festival de Jazz 2025",
  "templateId": "<id_do_template_5>",
  "camposPreenchidos": [
    { "nome": "Nome do Evento", "valor": "Festival de Jazz 2025" },
    { "nome": "Descrição", "valor": "Um festival anual que reúne os maiores nomes do jazz mundial." },
    { "nome": "Data do Evento", "valor": "2025-07-15" },
    { "nome": "Local", "valor": "Teatro Municipal" }
  ]
}


```
# Como subir o Backend e banco de dados via Docker

## 📦 **1. Pré-requisitos obrigatórios:**
- Docker instalado ([Docker Desktop](https://www.docker.com/products/docker-desktop/)).
- Git instalado.
- Porta **5432** (Postgres) e **8080** (API) **livres**.

---

## 🚀 **2. Passos para rodar o backend:**

### ✅ **Clonar o repositório:**

```bash
git clone <URL_DO_REPOSITORIO>
cd <PASTA_DO_PROJETO>
```

**🔔 Substituir `<URL_DO_REPOSITORIO>` e `<PASTA_DO_PROJETO>` conforme o repositório.**

---

## ✅ **3. Subir o banco de dados e a API via Docker:**

O projeto já contém o **docker-compose.yml** e o **Dockerfile** configurados.

### **Rodar o seguinte comando dentro da pasta do projeto:**

```bash
docker compose up --build
```

ou, se estiver usando versão antiga do Docker Compose:

```bash
docker-compose up --build
```

---

## ✅ **4. O que vai acontecer automaticamente:**

✅ O serviço **Postgres** vai subir na porta **5432**  
✅ A **API** será compilada, publicada e executada na porta **8080**  
✅ O backend vai:  
- O Back-End vai aplicar as **migrations** para criar as tabelas no banco  
- Criar automaticamente **usuários padrão**:  
  - `admin@cms.com / admin123`  
  - `editor@cms.com / editor123`  
  - `redator@cms.com / redator123`

---

## ✅ **5. Como testar se está funcionando:**

1. **Verificar se os containers estão rodando:**

```bash
docker ps
```

Deve aparecer algo como:  
- `cms_api` na porta `8080`  
- `postgres_db` na porta `5432`

---

2. **Testar a API:**

- Acesse no navegador:  
  http://localhost:8080/swagger  

Deve carregar a **Swagger UI** com todas as rotas documentadas.

---

## ✅ **6. Configuração da conexão do frontend:**

- O frontend deve consumir a API via:  
  **http://localhost:8080/**

- As credenciais default para testes:  
  **Usuário:** `admin@cms.com`  
  **Senha:** `admin123`

---

## ✅ **7. Sobre as portas:**

| Serviço    | Porta externa | Porta interna |
|------------|---------------|---------------|
| PostgreSQL | 5432          | 5432          |
| API        | 8080          | 8080          |

🔔 Certifique-se que **nenhum outro serviço** está usando essas portas.  
Se tiver, pode alterar no `docker-compose.yml` assim:  

```yaml
ports:
  - "8081:8080"
```

E depois acessar a API em:  
http://localhost:8081/swagger  

---

## ✅ **8. Parar os containers:**

Quando quiser **parar** a API e o banco:  

```bash
docker compose down
```

Isso vai **parar e remover** os containers, mas o volume do banco (os dados) vai continuar salvo em:  

```yaml
volumes:
  postgres_data:
```

Se quiser **apagar tudo**, incluindo dados:  

```bash
docker compose down -v
```

---

## ✅ **9. Se der algum erro:**

- **Porta em uso:**  
  - Libere a porta ou altere no `docker-compose.yml`.

- **Erro de permissão:**  
  - Rode como **administrador** ou com `sudo`.

- **Container não sobe:**  
  - Rode `docker compose logs` para ver o erro.

- **API não sobe:**  
  - Pode ser que o banco não esteja pronto. Aguarde alguns segundos e tente novamente.

---







