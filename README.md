# WinAppDriver-LearnKit

このリポジトリは Windows 向けの UI テスト学習用リポジトリです。サンプルの WPF アプリケーション (`src/SampleApp`) と、WinAppDriver を使った UI テスト (`tests/SampleApp.Tests.UI`) を含みます。

**Prerequisites**
- **OS**: `Windows 10` / `Windows 11`
- **Visual Studio**: `Visual Studio 2026` または `Visual Studio 2022`（`\.NET Desktop Development` ワークロードを有効にしてください）
- **.NET Framework Developer Pack**: `4.8`（プロジェクトが .NET Framework を利用している場合）
- **NuGet CLI (任意)**: `nuget.exe`（コマンドラインでパッケージ復元を行う場合）
- **WinAppDriver**: Windows Application Driver（https://github.com/microsoft/WinAppDriver）

**セットアップ手順（PowerShell）**

1. リポジトリをローカルにクローン（既に作業ディレクトリがある場合は不要）:

   `git clone <repository-url>`

2. Visual Studio でソリューションを開く（推奨）:

   - ソリューションファイルをダブルクリックして `WinAppDriver-LearnKit.slnx` を開きます。Visual Studio が起動時に NuGet パッケージの復元を実行します。

   もしくはコマンドラインで復元:

   - `nuget` を使う場合:

     `nuget restore "WinAppDriver-LearnKit.slnx"`

   - `msbuild` を使ってビルドと同時に復元する場合（開発者コマンドプロンプト/PowerShell）:

     `msbuild "WinAppDriver-LearnKit.slnx" /p:Configuration=Debug`

3. ソリューションをビルド:

   - Visual Studio のメニューから `Build -> Build Solution` を実行
   - またはコマンドラインで `msbuild`（上記）を実行

**WinAppDriver のインストールと起動**

1. WinAppDriver をダウンロードしてインストール:

   - リリースページ: https://github.com/microsoft/WinAppDriver/releases

2. WinAppDriver を起動（PowerShell 例）:

   - インストール済みの既定パスから起動:

     `Start-Process -FilePath "C:\Program Files (x86)\Windows Application Driver\WinAppDriver.exe"`

   - 直接実行する場合は管理者権限で実行してください（必要に応じて `Run as administrator`）。WinAppDriver はデフォルトで `127.0.0.1:4723` を使用します。

   - ポートを指定して起動する例:

     `& 'C:\Program Files (x86)\Windows Application Driver\WinAppDriver.exe' 4723`

**サンプルアプリ実行**

- Visual Studio から `src/SampleApp` をデバッグ実行するか、ビルド済みバイナリを直接実行します:

  `src\SampleApp\bin\Debug\SampleApp.exe`

**UI テストの実行**

1. 手順の順序:
   - まず `SampleApp` を起動（上記）。
   - 次に `WinAppDriver` を起動。
   - 最後にテストを実行します。

2. テストの実行方法:

   - Visual Studio の Test Explorer から `tests/SampleApp.Tests.UI` のテストを実行

   - コマンドラインから `vstest.console.exe` を使う（Visual Studio に同梱）:

     `vstest.console.exe "tests\SampleApp.Tests.UI\bin\Debug\SampleApp.Tests.UI.dll"`

   - または、環境が .NET SDK/`dotnet test` に対応していれば `dotnet test` を試してください（プロジェクトのターゲットに依存します）:

     `dotnet test tests\SampleApp.Tests.UI\SampleApp.Tests.UI.csproj -c Debug`

**トラブルシューティング**

- **WinAppDriver に接続できない**: WinAppDriver が起動しているか確認、`127.0.0.1:4723` にバインドされているか、管理者権限で実行しているかを確認してください。
- **ポート競合**: 4723 ポートが他プロセスで使われている場合は、WinAppDriver 起動時に別ポートを指定してください。
- **NuGet 復元エラー**: Visual Studio の `Tools -> NuGet Package Manager -> Package Manager Settings` でプロキシやパッケージソースを確認、必要であれば `nuget restore` を試してください。
- **テストが UI 要素を見つけられない**: テスト実行時に `SampleApp` が正しい状態（フォーカス、ウィンドウタイトルなど）で起動しているか確認してください。テストの `MainPage` 等の要素識別子を見直してください。
- **開発者モードが必要な場合**: Windows の「開発者モード」を有効にすると、ローカルでのアプリのデプロイや一部のデバッグ機能、サイドロード済みアプリの操作で問題が減る場合があります。設定は `設定 -> 更新とセキュリティ -> 開発者向け` から `開発者モード` をオンにしてください。管理者権限が必要な操作や署名なしアプリの扱いが関係するケースで有効です。

