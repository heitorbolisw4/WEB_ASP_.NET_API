# 🚗 Garage Manager API

API desenvolvida em .NET Minimal API para o gerenciamento de uma garagem de veículos.
O projeto faz parte de um desafio do Bootcamp DIO, e teve como foco o aprendizado de rotas, autenticação JWT, integração com MySQL e conceitos básicos de criptografia.


## ⚙️ Funcionalidades

 **1. Cadastro de administradores e editores**

        - Registro de novos administradores.

        - Login com geração de token JWT.

        - Controle de permissões baseado em perfil (Administrador / Editor).
**2.  Gerenciamento de veículos**

        - Cadastro de veículos.

        - Edição de informações (modelo, marca, ano).

        - Exclusão de veículos.

        - Listagem com paginação.


## 🔒 Segurança

**1. Autenticação via JWT Bearer.**

**2. Autorização baseada em perfis:**

        -> Administrador (Adm) → acesso total.

        -> Editor → acesso limitado.

## 🛠 Tecnologias utilizadas

| ![Static Badge](https://img.icons8.com/nolan/64/c-sharp-logo.png) | ![Static Badge](https://img.icons8.com/color/64/net-framework.png) | ![Static Badge](https://img.icons8.com/color/64/mysql--v1.png) | ![Static Badge](https://img.icons8.com/color/64/java-web-token.png) | ![Static Badge](https://img.icons8.com/color/64/cloud-function.png) |
|---------|--------|---------|--------|---------|

## 🚀 Implementações futuras

![Static Badge](https://img.icons8.com/plasticine/100/react.png)

1. Desenvolvimento de **front-end em React** para consumo da API.

2. Testes automatizados.

3. Deploy em ambiente cloud.