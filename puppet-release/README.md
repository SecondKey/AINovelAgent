# Puppet 一体化发布包（Windows）

本目录用于在一台全新的电脑上，让 Cursor 支持使用 `puppet`（你的 .NET 全局工具）替代 `dotnet run` 进行交互式流程。

## 包含内容
- `PuppetPerson.Tool.1.0.3.nupkg`：puppet 的离线安装包
- `run-puppet.ps1`：稳定调用 puppet 的启动脚本（Windows）
- `tasks.template.json`：VS Code/Cursor 任务模板（运行 puppet）
- `.cursorrules`：Cursor Agent 项目规则（提示使用 puppet）

## 一、准备 .NET SDK（必需）
- 请先安装 .NET SDK 8.0 或以上版本（可从微软官网获取）。

## 二、手动安装 puppet（离线）
在本目录打开终端（PowerShell 或 CMD），执行以下命令：

PowerShell（推荐）：
```powershell
# 安装或更新到指定版本（离线本地源）
dotnet tool install -g --add-source . PuppetPerson.Tool --version 1.0.3 --verbosity detailed
# 如已安装旧版本，可先卸载再安装
# dotnet tool uninstall -g PuppetPerson.Tool
# dotnet tool install -g --add-source . PuppetPerson.Tool --version 1.0.3 --verbosity detailed

# 检查是否安装成功
dotnet tool list -g
```

CMD：
```bat
REM 安装或更新到指定版本（离线本地源）
dotnet tool install -g --add-source . PuppetPerson.Tool --version 1.0.3 --verbosity detailed
REM 如已安装旧版本，可先卸载再安装
REM dotnet tool uninstall -g PuppetPerson.Tool
REM dotnet tool install -g --add-source . PuppetPerson.Tool --version 1.0.3 --verbosity detailed

REM 检查是否安装成功
dotnet tool list -g
```

## 三、注册命令到 PATH（如未自动生效）
- 全局工具默认安装在：`%USERPROFILE%\.dotnet\tools`
- 若当前终端执行 `puppet` 提示找不到命令，可临时加入 PATH（仅当前窗口）：

PowerShell：
```powershell
$env:PATH = "$env:PATH;$HOME\.dotnet\tools"
where.exe puppet
```

CMD：
```bat
set PATH=%PATH%;%USERPROFILE%\.dotnet\tools
where puppet
```

持久化 PATH：
- 将 `%USERPROFILE%\.dotnet\tools` 加入系统或用户 PATH 环境变量，重新打开终端生效。

## 四、让 Cursor 使用 puppet（两种方式）
方式 A：通过 VS Code/Cursor 任务运行（推荐）
1. 将 `tasks.template.json` 内容复制到项目的 `.vscode/tasks.json`。
2. 在 Cursor 中：
   - Ctrl+Shift+B → 选择 `puppet:run`，或
   - 命令面板（Ctrl+Shift+P）→ “Tasks: Run Task” → 选择 `puppet:run`。

方式 B：为 Cursor Agent 设置规则（可选）
1. 将本目录 `.cursorrules` 复制到仓库根目录。
2. Cursor Agent 在需要“运行程序”时会优先选择 `puppet`（或 `run-puppet.ps1`）而非 `dotnet run`。

## 五、交互与退出约定
- 输入阶段：可输入多行；输入 `/clear` 清空已收集内容。
- 结束方式：
  - 输入 `/success`：先原样输出收集的全部内容，再输出 `success`，以退出码 0 结束。
  - 输入 `/error`：先原样输出收集的全部内容，再输出 `error`，以非 0 退出码结束。
- Cursor/CI 应根据进程退出码与输出末行判断成功/失败，并读取全部输出继续后续任务。

## 六、故障排查
- `puppet` 仍不可用：
  - 确认 `dotnet tool list -g` 中存在 `puppetperson.tool` 且版本为 1.0.3。
  - 临时加入 PATH（见上文“三、注册命令到 PATH”），或持久化 PATH。
- 安装失败或找不到包：
  ```powershell
  dotnet nuget locals all --clear
  dotnet tool install -g --add-source . PuppetPerson.Tool --version 1.0.3 --verbosity detailed
  ```
- PowerShell 执行策略限制：
  - 运行脚本时可使用：`-ExecutionPolicy Bypass`
- 检查 puppet 可执行位置：
  ```powershell
  where.exe puppet
  ```
