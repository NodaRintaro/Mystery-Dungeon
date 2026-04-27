# Design Guidelines

## 設計思想
クリーンアーキテクチャを基軸にした階層構造です。
上位の層が下位の層に依存する形で設計されており、下位の層は上位の層に依存しないようになっています。
今回はあくまでも個人製作のため、DDD(ドメイン駆動設計)のような厳密なドメインモデルの設計は行わず、
シンプルかつ分かりやすいな階層構造を目指しております。

## 階層構造

### View
 - View : ユーザーインターフェースを表すクラス

### Application
 - Controller : Viewからのリクエストを受け取り、UseCaseを呼び出す
 - Presenter : UseCaseからのレスポンスを受け取り、Viewに渡す
 - Application : UseCaseを実装するクラス

### UseCase
 - Logic : ドメインロジックを実装するクラス

### Entity
 - Data : ドメインモデルを表すクラス

### Infrastructure
 - Factory : オブジェクトの生成を行うクラス
 - Repository : データベースや外部APIとのやり取りを行うクラス
 - Initializer : アプリケーションの初期化を行うクラス

## 依存関係
 - Domain(依存先なし)
	- UseCase >> Domain
	- Application >> UseCase, Domain
	- View >> Application
	- Infrastructure >> Domain, UseCase, Application
