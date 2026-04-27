# Design Guidelines

## 設計思想
クリーンアーキテクチャ
MVC、MVP


## 階層構造

### View
 - View : ユーザーインターフェースを表すクラス

### Application
 - Controller : Viewからのリクエストを受け取り、UseCaseを呼び出す
 - Presenter : UseCaseからのレスポンスを受け取り、Viewに渡す
 - Application : UseCaseを実装するクラス

### UseCase
 - Logic : ドメインロジックを実装するクラス

### Domain
 - Data : ドメインモデルを表すクラス

### Infrastructure
 - Factory : オブジェクトの生成を行うクラス
 - Repository : データベースや外部APIとのやり取りを行うクラス
 - Initializer : アプリケーションの初期化を行うクラス

## 依存関係
 - View >> Application
 - Application >> UseCase
 - UseCase >> Domain
 - Domain(依存先なし)
