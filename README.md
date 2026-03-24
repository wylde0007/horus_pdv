# Hórus PDV

[PT-BR](#pt-br) | [EN](#en)

---

## PT-BR

Projeto open source de frente de caixa (PDV) criado por **Flávio Oliveira**.

O Hórus PDV começou em 2020, passou por evoluções de arquitetura e UX ao longo dos anos, e nesta fase de 2026 está organizado para crescimento em camadas separadas de frontend e backend.

### Filosofia do Projeto

- Uso gratuito nesta versão open source.
- Reutilização e aprendizado para a comunidade.
- Evolução contínua com foco em qualidade de código, clareza e manutenção.
- Possibilidade de versões pagas no futuro com módulos adicionais.

### Estrutura do Repositório

```text
horus_pdv/
├── FRONTEND/       # Aplicação cliente (React + Vite + TypeScript)
├── API/            # Backends em desenvolvimento (NETCORE, NODE, PHP)
└── SYSTEM-LEGACY/  # Base legada para consulta histórica/migração
```

### Status Atual

- `FRONTEND`: em desenvolvimento ativo.
- `API/NETCORE`: em desenvolvimento.
- `API/NODE`: em desenvolvimento.
- `API/PHP`: em desenvolvimento.
- `SYSTEM-LEGACY`: referência da versão anterior.

### Executando o Frontend

Pré-requisitos:

- Node.js 20+
- npm 10+

Passos:

```bash
cd FRONTEND
npm install
npm run dev
```

Build de produção:

```bash
npm run build
```

### Backend (API)

A pasta `API` está preparada para organização de implementações de backend por tecnologia.

Cada subpasta possui documentação própria:

- `API/NETCORE/README.md`
- `API/NODE/README.md`
- `API/PHP/README.md`

### Open Source e Créditos

Você pode usar esta versão gratuitamente em projetos pessoais, estudos e adaptações, mantendo os devidos créditos ao autor.

Autor:

- **Flávio Oliveira**
- GitHub: https://github.com/flaviooliveira-code
- LinkedIn: https://www.linkedin.com/in/fladoliveira

### Contribuindo

Contribuições são bem-vindas.

Fluxo sugerido:

1. Faça um fork.
2. Crie uma branch (`feature/minha-melhoria`).
3. Commit suas alterações.
4. Abra um Pull Request com contexto técnico e evidências de teste.

### Roadmap (alto nível)

- Consolidação da API principal.
- Integração real entre frontend e backend.
- Módulos extras (financeiro avançado, fiscal, integrações externas).
- Melhorias contínuas de UX mobile e acessibilidade.

### Licença

Consulte o arquivo [LICENSE](./LICENSE).

---

## EN

Open-source point of sale (POS) project created by **Flávio Oliveira**.

Hórus PDV started in 2020 and has gone through architecture and UX evolution over the years. In this 2026 phase, it is organized for growth with separated frontend and backend layers.

### Project Philosophy

- Free usage in this open-source version.
- Reuse and learning for the community.
- Continuous evolution focused on code quality, clarity, and maintainability.
- Possible paid editions in the future with additional modules.

### Repository Structure

```text
horus_pdv/
├── FRONTEND/       # Client application (React + Vite + TypeScript)
├── API/            # Backends in development (NETCORE, NODE, PHP)
└── SYSTEM-LEGACY/  # Legacy base for historical reference/migration
```

### Current Status

- `FRONTEND`: actively under development.
- `API/NETCORE`: in development.
- `API/NODE`: in development.
- `API/PHP`: in development.
- `SYSTEM-LEGACY`: previous version reference.

### Running the Frontend

Requirements:

- Node.js 20+
- npm 10+

Steps:

```bash
cd FRONTEND
npm install
npm run dev
```

Production build:

```bash
npm run build
```

### Backend (API)

The `API` folder is prepared to organize backend implementations by technology.

Each subfolder has its own documentation:

- `API/NETCORE/README.md`
- `API/NODE/README.md`
- `API/PHP/README.md`

### Open Source and Credits

You can use this version for free in personal projects, studies, and adaptations, keeping proper credit to the author.

Author:

- **Flávio Oliveira**
- GitHub: https://github.com/flaviooliveira-code
- LinkedIn: https://www.linkedin.com/in/fladoliveira

### Contributing

Contributions are welcome.

Suggested flow:

1. Fork this repository.
2. Create a branch (`feature/my-improvement`).
3. Commit your changes.
4. Open a Pull Request with technical context and test evidence.

### Roadmap (high-level)

- Main API consolidation.
- Real frontend-backend integration.
- Extra modules (advanced finance, fiscal, external integrations).
- Ongoing mobile UX and accessibility improvements.

### License

See [LICENSE](./LICENSE).
