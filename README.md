# Azure DevOps Repo Backup Tool

![.NET](https://img.shields.io/badge/.NET-9.0-blueviolet)
![Azure DevOps](https://img.shields.io/badge/Azure%20DevOps-Backup-blue)
![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)

## 🚀 Overview

**BackUpRepos** is a robust, configurable tool for backing up your Azure DevOps Git repositories directly to your local hard drive. Designed for reliability and automation, this tool ensures you never lose your source code—even if it’s in the cloud.

- **Automated backup** of all or selected repositories from Azure DevOps
- **Configurable storage path** for local backups
- **Secure access** using Azure DevOps Personal Access Token (PAT)
- Built with **.NET 9** and modern best practices (Dependency Injection, MediatR, configuration management)

---

## 🛠️ Features

- 🔐 **Secure Authentication:** Uses your Azure DevOps PAT for API access.
- 🗂️ **Flexible Backup Location:** Store backups anywhere on your hard drive.
- ⚡ **Easy Configuration:** All settings in `appsettings.json`.
- 🏗️ **Extensible Architecture:** Powered by MediatR and Microsoft.Extensions for maintainability.

---

## 📦 Project Structure

```
BackUpRepos/
│
├── BackUpRepos.sln           # Solution file
├── LICENSE                   # License file
├── README.md                 # You're here!
├── .gitignore
└── BackUpRepos/              # Main application code
    ├── BackUpRepos.csproj    # Project file (.NET 9)
    └── appsettings.json      # Main configuration
```

---

## ⚙️ Configuration

Before running, edit `BackUpRepos/appsettings.json`:

```json
{
  "AzureDevOpsConfig": {
    "Organization": "YOUR_ORG",
    "Project": "YOUR_PROJECT",
    "PersonalAccessToken": "YOUR_PAT_AZURE_DEV_OPS"
  },
  "RepoFileStorageConfig": {
    "StoragePath": "D:\\BackUP"
  }
}
```
- Replace `YOUR_ORG`, `YOUR_PROJECT`, and `YOUR_PAT_AZURE_DEV_OPS` with your Azure DevOps details.
- Set `StoragePath` to your desired backup directory.

---

## 🚀 Getting Started

1. **Clone the repo:**
    ```sh
    git clone https://github.com/vtrsnts/BackUpRepos.git
    cd BackUpRepos
    ```

2. **Edit configuration:**
    - Update `BackUpRepos/appsettings.json` as above.

3. **Build and run:**
    ```sh
    dotnet build BackUpRepos/BackUpRepos.csproj
    dotnet run --project BackUpRepos/BackUpRepos.csproj
    ```

---

## 🤝 Contributing

Contributions and suggestions are welcome! Please open an issue or submit a pull request.

---

## 📄 License

This project is licensed under the terms of the MIT license. See [LICENSE](LICENSE) for details.

---

## ⭐️ Show your support

If you find this project useful, please star the repo!

---

*Made with ❤️ by vtrsnts*
